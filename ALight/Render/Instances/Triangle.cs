using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALight.Render.Components;
using ALight.Render.Materials;
using ALight.Render.Mathematics;

namespace ALight.Render.Instances
{
    public class Triangle:Hitable
    {
        public Vector3 v0, v1, v2;
        public Vector3 e1, e2;
        public Vector3 normal, t0, t1, t2;
        public Material material;

        public Triangle(Vector3 v0, Vector3 v1, Vector3 v2, Material material)
        {
            this.v0 = v0;
            this.v1 = v1;
            this.v2 = v2;
            e1 = v1 - v0;
            e2 = v2 - v0;
            normal=Vector3.Cross(e1,e2).Normalized();
            this.material = material;
        }

        public override bool Hit(Ray r, float t_min, float t_max, ref HitRecord rec)
        {
            Console.ReadLine();
            Console.WriteLine("----");
            var pvec = Vector3.Cross(r.direction, e2);
            var det = Vector3.Dot(e1, pvec);
            if (det == 0) return false;
            Console.WriteLine("A");
            var invDet = 1f / det;
            var tvec = r.origin- v0;
            var u = Vector3.Dot(tvec, pvec) * invDet;
            if (u < 0 || u > 1)return false;
            Console.WriteLine("B");
            var qvec = Vector3.Cross(tvec, e1);
            var v = Vector3.Dot(r.direction, qvec) * invDet;
            if (v < 0 || u + v > 1)return false;
            Console.WriteLine("C");
            var temp = Vector3.Dot(e2, qvec) * invDet;
            Console.WriteLine(temp+" "+e2+" "+qvec+" "+invDet);
            if (!(temp < t_min) || !(temp > 0)) return false;
            Console.WriteLine("D");

            
            rec.u = u;
            rec.v = v;
            rec.t = temp;
            rec.p = r.GetPoint(rec.t);
            rec.normal = normal;
            rec.material = material;
            return true;
        }

        public override bool BoundingBox(float t0, float t1, ref AABB box)
        {
            var bl = new Vector3(Mathf.Min(Mathf.Min(v0[0], v1[0]), v2[0]), Mathf.Min(Mathf.Min(v0[1], v1[1]), v2[1]), Mathf.Min(Mathf.Min(v0[2], v1[2]), v2[2]));
            var tr =new Vector3(Mathf.Max(Mathf.Max(v0[0], v1[0]), v2[0]), Mathf.Max(Mathf.Max(v0[1], v1[1]), v2[1]), Mathf.Max(Mathf.Max(v0[2], v1[2]), v2[2]));
            box =new AABB(bl, tr);
            return true;
        }
    }


    public class Tri:Hitable
    {
        public Vector3 v0, v1, v2;
        public Vector2 uv0, uv1, uv2;
        public Vector3 normal;
        public Material material;
        public Tri(Vector3 v0, Vector3 v1, Vector3 v2, Material material)
        {
            this.v2= v0;
            this.v1 = v1;
            this.v0= v2;
            this.uv0 = new Vector2(0,0);
            this.uv1 = new Vector2(0.2f,0);
            this.uv2 = new Vector2(0.5f,0.2f);
            normal = Vector3.Cross(v1-v0, v2-v0);
            this.material = material;
        }
        Vector2 barycentric(Vector3 p)
        {
            var f1 = v0 - p;
            var f2 = v1- p;
            var f3 = v2 - p;
            // calculate the areas (parameters order is essential in this case):
            var va= Vector3.Cross(v0 - v1, v0 - v2); // main triangle cross product
            var va1 = Vector3.Cross(f2, f3); // p1's triangle cross product
            var va2 = Vector3.Cross(f3, f1); // p2's triangle cross product
            var va3= Vector3.Cross(f1, f2); // p3's triangle cross product
            var a = va.Magnitude(); // main triangle area
            // calculate barycentric coordinates with sign:
            var a1= va1.Magnitude() / a * Mathf.Sin(Vector3.Dot(va, va1));
            var a2 = va2.Magnitude() / a * Mathf.Sin(Vector3.Dot(va, va2));
            var a3 = va3.Magnitude() / a * Mathf.Sin(Vector3.Dot(va, va3));
            // find the uv corresponding to point f (uv1/uv2/uv3 are associated to p1/p2/p3):
            return uv0 * a1 + uv1 * a2 + uv2 * a3;
            //var e1 = v1 - v0;
            //var e2 = v2 - v0;
            //var p2 = p - v0;
            //float d00 = Vector3.Dot(e1, e1);
            //float d01 = Vector3.Dot(e1, e2);
            //float d11 = Vector3.Dot(e2, e2);
            //float d20 = Vector3.Dot(p2, e1);
            //float d21 = Vector3.Dot(p2, e2);
            //float d = d00 * d01 - d01 * d11;
            //float v = (d11 * d20 - d01 * d21) / d;
            //float w = (d00 * d21 - d01 * d20) / d;
            //float u = 1 - v - w;
            //return new Vector3(u, v, w);
        }


public override bool Hit(Ray r, float t_min, float t_max, ref HitRecord rec)
        {
            
            var v0v1 = v1 - v0;
            var v0v2 = v2 - v0;
            var ao = r.origin - v0;
            var D = v0v1[0] * v0v2[1] * (-r.direction[2]) + v0v1[1] * v0v2[2] * (-r.direction[0]) + v0v1[2] * v0v2[0] * (-r.direction[1]) -
                      (-r.direction[0] * v0v2[1] * v0v1[2] - r.direction[1] * v0v2[2] * v0v1[0] - r.direction[2] * v0v2[0] * v0v1[1]);
            var D1 = ao[0] * v0v2[1] * (-r.direction[2]) + ao[1] * v0v2[2] * (-r.direction[0]) + ao[2] * v0v2[0] * (-r.direction[1]) -
                       (-r.direction[0] * v0v2[1] * ao[2] - r.direction[1] * v0v2[2] * ao[0] - r.direction[2] * v0v2[0] * ao[1]);

            var D2 = v0v1[0] * ao[1] * (-r.direction[2]) + v0v1[1] * ao[2] * (-r.direction[0]) + v0v1[2] * ao[0] * (-r.direction[1]) -
                       (-r.direction[0] * ao[1] * v0v1[2] - r.direction[1] * ao[2] * v0v1[0] - r.direction[2] * ao[0] * v0v1[1]);

            var D3 = v0v1[0] * v0v2[1] * ao[2] + v0v1[1] * v0v2[2] * ao[0] + v0v1[2] * v0v2[0] * ao[1] -
                       (ao[0] * v0v2[1] * v0v1[2] + ao[1] * v0v2[2] * v0v1[0] + ao[2] * v0v2[0] * v0v1[1]);

            var a = D1 / D;
            var b = D2 / D;
            var temp = D3 / D;

            if (temp < 0)return false;
            if (!(a >= 0) || !(b >= 0) || !(a + b < 1)) return false;

            rec.p = r.GetPoint(rec.t);
            //var f1 = v0 - rec.p;
            //var f2 =v1 - rec.p;
            //var f3 = v2- rec.p;
            //var a0 = Vector3.Cross(v0 - v1, v0 - v2).Magnitude(); // main triangle area a
            //var a1 = Vector3.Cross(f2, f3).Magnitude() / a0; // p1's triangle area / a
            //var a2= Vector3.Cross(f3, f1).Magnitude() / a0; // p2's triangle area / a 
            //var a3 = Vector3.Cross(f1, f2).Magnitude() / a0; // p3's triangle area / a
            //// find the uv corresponding to point f (uv1/uv2/uv3 are associated to p1/p2/p3):
            //var uv = uv0 * a1 + uv1 * a2 + uv2 * a3;
            //rec.u = uv.x;
            //rec.v = uv.y;
            var uvw = barycentric(rec.p);
            //Console.WriteLine(uvw);
            rec.u = uvw.x;
            rec.v = uvw.y;
            //Console.ReadLine();
            //Console.WriteLine(uvw.x+" "+uvw.y);
            rec.t = temp;
             
            rec.normal = normal;
            rec.material = material;
            return true;
        }

        public override bool BoundingBox(float t0, float t1, ref AABB box)
        {
            var bl = new Vector3(Mathf.Min(Mathf.Min(v0[0], v1[0]), v2[0]), Mathf.Min(Mathf.Min(v0[1], v1[1]), v2[1]), Mathf.Min(Mathf.Min(v0[2], v1[2]), v2[2]));
            var tr = new Vector3(Mathf.Max(Mathf.Max(v0[0], v1[0]), v2[0]), Mathf.Max(Mathf.Max(v0[1], v1[1]), v2[1]), Mathf.Max(Mathf.Max(v0[2], v1[2]), v2[2]));
            box = new AABB(bl, tr);
            return true;
        }
    }
}
