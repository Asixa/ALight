
using ALight.Render.Components;
using ALight.Render.Materials;
using ALight.Render.Mathematics;

namespace ALight.Render.Instances
{
    public class Triangle:Hitable
    {
        public Vertex v0, v1, v2;
        private Vector3 Gnormal;
        public Shader shader;

        public Triangle(Vertex a, Vertex b, Vertex c, Shader shader)
        {
            v0 = a;//a
            v1 = b;//b
            v2 =c; //c
            Gnormal =( a.normal+b.normal+c.normal)/3;
            this.shader = shader;
        }

        Vector2 GetUV(Vector3 p,out Vector3 normal)
        {
            var f1 = v0.point - p;
            var f2 = v1.point - p;
            var f3 = v2.point - p;
            //计算面积和因子（参数顺序无关紧要）：
            var a = Vector3.Cross(v0.point - v1.point, v0.point - v2.point).Magnitude(); // 主三角形面积 a
            var a1= Vector3.Cross(f2, f3).Magnitude() / a; // p1 三角形面积 / a
            var a2= Vector3.Cross(f3, f1).Magnitude() / a; // p2 三角形面积 / a 
            var a3= Vector3.Cross(f1, f2).Magnitude() / a; // p3 三角形面积 / a
            // 找到对应于点f的uv（uv1 / uv2 / uv3与p1 / p2 / p3相关）：
            var uv  = v0.uv * a1 + v1.uv * a2 + v2.uv * a3;
            // 找到对应于点f的法线（法线1 / 法线2 / 法线3与p1 / p2 / p3相关）：
            normal = v0.normal * a1 + v1.normal * a2 + v2.normal * a3;
            return uv;
        }

        public override bool Hit(Ray r, float t_min, float t_max, ref HitRecord rec)
        {
            if(shader.BackCulling&&Vector3.Dot(Gnormal,r.direction)>=0)return false;
            if (!Intersects(r.origin, r.direction.Normalized(), out var p)) return false;
            rec.t = Vector3.Distance(r.origin,p);
            rec.p = p;
            rec.shader = shader;
            var uvw = GetUV(rec.p,out p);
            rec.normal = p;
            rec.u = uvw.x;
            rec.v = uvw.y;
            return true;
        }

        public override bool BoundingBox(float t0, float t1, ref AABB box)
        {
            var bl = new Vector3(
                Mathf.Min(Mathf.Min(v0.point[0], v1.point[0]), v2.point[0]), 
                Mathf.Min(Mathf.Min(v0.point[1], v1.point[1]), v2.point[1]), 
                Mathf.Min(Mathf.Min(v0.point[2], v1.point[2]), v2.point[2]));
            var tr = new Vector3(
                Mathf.Max(Mathf.Max(v0.point[0], v1.point[0]), v2.point[0]),
                Mathf.Max(Mathf.Max(v0.point[1], v1.point[1]), v2.point[1]), 
                Mathf.Max(Mathf.Max(v0.point[2], v1.point[2]), v2.point[2]));
            box = new AABB(bl-new Vector3(0.1f, 0.1f, 0.1f), tr+new Vector3(0.1f, 0.1f, 0.1f));
            return true;
        }

        private bool Intersects(Vector3 ray_origin,Vector3 ray_dir,out Vector3 point)
        {
            point=new Vector3();
            const float EPSILON = 1e-4f;
            var edge1 = v1.point - v0.point;
            var edge2 = v2.point - v0.point;
            var h = Vector3.Cross(ray_dir,edge2); 
            var a = Vector3.Dot(edge1,h);
            if (a > -EPSILON && a < EPSILON)return false;
            var f = 1 / a;
            var s = ray_origin - v0.point;
            var u = f * (Vector3.Dot(s,h));
            if (u < 0.0 || u > 1.0)return false;
            var q = Vector3.Cross(s,edge1);
            var v = f * Vector3.Dot(ray_dir,q);
            if (v < 0.0 || u + v > 1.0)return false;
            var t = f * Vector3.Dot(edge2,q);
            if (!(t > EPSILON)) return false;
            point = ray_origin + ray_dir * t;
            return true;
        }

        private bool Intersects2(Vector3 ray_origin, Vector3 dir, out Vector3 point)
        {
            point = new Vector3();
            var v0_v1 = v1.point - v0.point;
            var v0_v2 = v2.point - v0.point;
            var ao = ray_origin - v0.point;
            var D = v0_v1[0] * v0_v2[1] * (-dir[2]) + v0_v1[1] * v0_v2[2] * (-dir[0]) + v0_v1[2] * v0_v2[0] * (-dir[1]) -
                      (-dir[0] * v0_v2[1] * v0_v1[2] - dir[1] * v0_v2[2] * v0_v1[0] - dir[2] * v0_v2[0] * v0_v1[1]);
            var D1 = ao[0] * v0_v2[1] * (-dir[2]) + ao[1] * v0_v2[2] * (-dir[0]) + ao[2] * v0_v2[0] * (-dir[1]) -
                       (-dir[0] * v0_v2[1] * ao[2] - dir[1] * v0_v2[2] * ao[0] - dir[2] * v0_v2[0] * ao[1]);
            var D2 = v0_v1[0] * ao[1] * (-dir[2]) + v0_v1[1] * ao[2] * (-dir[0]) + v0_v1[2] * ao[0] * (-dir[1]) -
                       (-dir[0] * ao[1] * v0_v1[2] - dir[1] * ao[2] * v0_v1[0] - dir[2] * ao[0] * v0_v1[1]);
            var D3 = v0_v1[0] * v0_v2[1] * ao[2] + v0_v1[1] * v0_v2[2] * ao[0] + v0_v1[2] * v0_v2[0] * ao[1] -
                       (ao[0] * v0_v2[1] * v0_v1[2] + ao[1] * v0_v2[2] * v0_v1[0] + ao[2] * v0_v2[0] * v0_v1[1]);
            var a = D1 / D;
            var b = D2 / D;
            var t = D3 / D;
            if (t < 0) return false;
            if (a < 0 || b < 0 || a + b >= 1) return false;
            point = ray_origin + dir * t;
            return true;
        }
    }
}
