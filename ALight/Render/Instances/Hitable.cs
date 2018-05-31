using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using ALight.Render.Materials;
using ALight.Render.Mathematics;
using Random = ALight.Render.Mathematics.Random;

namespace ALight.Render.Components
{
    [StructLayout(LayoutKind.Sequential)]
    public struct HitRecord
    {
        public float u, v;
        public float t;
        public Vector3 p;
        public Vector3 normal;
        public Shader shader;

        public HitRecord(HitRecord copy)
        {
            t = 0;
            u = copy.u;
            v = copy.v;
            p = copy.p;
            normal = copy.normal;
            shader = copy.shader;
        }

        public override string ToString()
        {
            return " t:" + t + " u:" + u + " v:" + v + " p:" + p + " normal:" + normal;
        }
    }

    public abstract class Hitable
    {
        public abstract bool Hit(Ray ray, float t_min, float t_max, ref HitRecord rec);
        public abstract bool BoundingBox(float t0, float t1,ref AABB box);
        public virtual float PdfValue(Vector3 o, Vector3 v) => 0;
        public virtual Vector3 Random(Vector3 o)=>new Vector3(1,0,0);

        protected static AABB GetBox(AABB box0, AABB box1)
        {
            var small = new Vector3(
                Mathf.Min(box0.min.x, box1.min.x),
                Mathf.Min(box0.min.y, box1.min.y),
                Mathf.Min(box0.min.z, box1.min.z));
            var big = new Vector3(
                Mathf.Max(box0.max.x, box1.max.x),
                Mathf.Max(box0.max.y, box1.max.y),
                Mathf.Max(box0.max.z, box1.max.z));
            return new AABB(small, big);
        }
    }

    public class HitableList : Hitable
    {
        public readonly List<Hitable> list = new List<Hitable>();

        public override bool Hit(Ray ray, float t_min, float t_max, ref HitRecord rec)
        {
            //if (Renderer.main.hit[ray.id] == 1)return false;
            
            var temp_record = new HitRecord();
            var hit_anything = false;
            var closest = t_max;
            foreach (var h in list)
            {
                if (!h.Hit(ray, t_min, closest, ref temp_record)) continue;
                hit_anything = true;
                closest = temp_record.t;
                rec = temp_record;
            }
           // if (!hit_anything)Renderer.main.hit[ray.id]= 1;
            
            return hit_anything;
        }

        public override bool BoundingBox(float t0, float t1, ref AABB box)
        {
            if (list.Count == 0) return false;
            var temp_box=new AABB();
            if (!list[0].BoundingBox(t0, t1, ref temp_box)) return false;
            box = temp_box;
            foreach (var t in list)
            {
                if (t.BoundingBox(t0, t1, ref temp_box))
                    box = GetBox(box, temp_box);
                else return false;
            }
            return true;
        }

        public override float PdfValue(Vector3 o, Vector3 v)
        {
            var w = 1f / list.Count;
            float sum = 0;
            foreach (var t in list)
                sum += w * t.PdfValue(o, v);
            return sum;
        }

        public override Vector3 Random(Vector3 o)=>list[(int)(Mathematics.Random.Get() * (list.Count - 0.1f))].Random(o);
    }

    public class BVHNode : Hitable
    {
        private readonly Hitable left,right;
        private readonly AABB box;
        public BVHNode(Hitable[] p, int n, float time0, float time1)
        {
            int Compare(Hitable a, Hitable b,int i)
            {
                AABB l = new AABB(), r = new AABB();
                if (!a.BoundingBox(0, 0, ref l) || !b.BoundingBox(0, 0, ref r)) throw new Exception("NULL");
                return l.min[i] - r.min[i]<0 ? -1 : 1;
            }

            Hitable[] Split_array(Hitable[] Source, int StartIndex, int EndIndex)
            {
                var result = new Hitable[EndIndex - StartIndex + 1];
                for (var i = 0; i <= EndIndex - StartIndex; i++) result[i] = Source[i + StartIndex];
                return result;
            }

            var pl= p.ToList();
            var method = (int) (3 * Mathematics.Random.Get());
            pl.Sort((a, b) => Compare(a, b, method));
            p = pl.ToArray();
            switch (n)
            {
                case 1:
                    left = right = p[0];
                    break;
                case 2:
                    left = p[0];
                    right = p[1];
                    break;
                default:
                    left = new BVHNode(Split_array(p,0,n/2-1), n / 2, time0, time1);
                    right = new BVHNode(Split_array(p,n/2,n-1), n - n / 2, time0, time1);
                    break;
            }
            AABB box_left=new AABB(), box_right=new AABB();
            if (!left.BoundingBox(time0, time1,ref box_left) || !right.BoundingBox(time0, time1,ref box_right))
            throw new Exception("no bounding box in bvh_node constructor");
            box = GetBox(box_left, box_right);
        }

        public override bool BoundingBox(float t0, float t1, ref AABB b)
        {
            b = box;
            return true;
        }

        public override bool Hit(Ray ray, float t_min, float t_max, ref HitRecord rec)
        {
            if (!box.Hit(ray, t_min, t_max)) return false;
            HitRecord leftRec=new HitRecord(),rightRec=new HitRecord();
            var hit_left = left.Hit(ray, t_min, t_max, ref leftRec);
            var hit_right = right.Hit(ray, t_min, t_max, ref rightRec);
            if (hit_left && hit_right)
            {
                rec = leftRec.t < rightRec.t ? leftRec : rightRec;
                return true;
            }
            if (hit_left)
            {
                rec = leftRec;
                return true;
            }
            if (hit_right)
            {
                rec = rightRec;
                return true;
            }
            return false;
        }

}

    public class FilpNormals : Hitable
    {
        private readonly Hitable hitable;
        public FilpNormals(Hitable p)=>hitable = p;
        public override bool BoundingBox(float t0, float t1, ref AABB box)=>hitable.BoundingBox(t0, t1,ref box);
        public override bool Hit(Ray ray, float t_min, float t_max, ref HitRecord rec)
        {
            if (!hitable.Hit(ray, t_min, t_max, ref rec)) return false;
            rec.normal = -rec.normal;
            return true;
        }
    }

    public class Translate : Hitable
    {
        private readonly Hitable Object;
        private readonly Vector3 offset;

        public Translate(Hitable p, Vector3 displace)
        {
            offset = displace;
            Object = p;
        }

        public override bool BoundingBox(float t0, float t1, ref AABB box)
        {
            if (!Object.BoundingBox(t0, t1, ref box)) return false;
            box = new AABB(box.min + offset, box.max + offset);
            return true;
        }

        public override bool Hit(Ray ray, float t_min, float t_max, ref HitRecord rec)
        {
            var moved=new Ray(ray.origin-offset,ray.direction,ray.time);
            if (!Object.Hit(moved, t_min, t_max, ref rec)) return false;
            rec.p += offset;
            return true;
        }
    }
    public class RotateY : Hitable
    {
        private readonly AABB bbox = new AABB();
        private readonly bool hasbox;
        private readonly Hitable Object;
        private readonly float sin_theta,cos_theta;

        public override bool BoundingBox(float t0, float t1, ref AABB box)
        {
            box = bbox;
            return hasbox;
        }

        public RotateY(Hitable p, float angle)
        {
            Object = p;
            var radians = (Mathf.PI / 180f) * angle;
            sin_theta = Mathf.Sin(radians);
            cos_theta = Mathf.Cos(radians);
            hasbox = Object.BoundingBox(0, 1, ref bbox);
            var min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            var max = new Vector3(-float.MaxValue, -float.MaxValue, -float.MaxValue);
            for (var i = 0; i < 2; i++)
                for (var j = 0; j < 2; j++)
                    for (var k = 0; k < 2; k++)
                    {
                        var x = i * bbox.max.x + (1 - i) * bbox.min.x;
                        var y = j * bbox.max.y + (1 - j) * bbox.min.y;
                        var z = k * bbox.max.z + (1 - k) * bbox.min.z;
                        var newx = cos_theta * x + sin_theta * z;
                        var newz = -sin_theta * x + cos_theta * z;
                        var tester = new Vector3(newx, y, newz);
                        for (var c = 0; c < 3; c++)
                        {
                            if (tester[c] > max[c]) max[c] = tester[c];
                            if (tester[c] < min[c]) min[c] = tester[c];
                        }
                    }


            bbox = new AABB(min, max);
        }

        public override bool Hit(Ray ray, float t_min, float t_max, ref HitRecord rec)
        {
            var origin = new Vector3(ray.origin);
            var direction = new Vector3(ray.direction);
            origin[0] = cos_theta * ray.origin[0] - sin_theta * ray.origin[2];
            origin[2] = sin_theta * ray.origin[0] + cos_theta * ray.origin[2];
            direction[0] = cos_theta * ray.direction[0] - sin_theta * ray.direction[2];
            direction[2] = sin_theta * ray.direction[0] + cos_theta * ray.direction[2];
            var rotatedR = new Ray(origin, direction, ray.time);
            var r = new HitRecord(rec);
            if (Object.Hit(rotatedR, t_min, t_max, ref rec))
            {
                var p = new Vector3(rec.p);
                var normal = new Vector3(rec.normal);
                p[0] = cos_theta * rec.p[0] + sin_theta * rec.p[2];
                p[2] = -sin_theta * rec.p[0] + cos_theta * rec.p[2];
                normal[0] = cos_theta * rec.normal[0] + sin_theta * rec.normal[2];
                normal[2] = -sin_theta * rec.normal[0] + cos_theta * rec.normal[2];
                rec.p = p;
                rec.normal = normal;
                return true;
            }
            rec = r;
                return false;
        }
    }

    public class ConstantMedium : Hitable
    {
        private Hitable boundary;
        private float density;
        private Shader phase_function;

        public ConstantMedium(Hitable b, float d, Texture a)
        {
            boundary = b;
            density = d;
           // phase_function = new Isotropic(a); //FIX ME 
        }

        public override bool BoundingBox(float t0, float t1, ref AABB box)
        {
            return boundary.BoundingBox(t0, t1, ref box);
        }

        public override bool Hit(Ray ray, float t_min, float t_max, ref HitRecord rec)
        {
            HitRecord rec1 = new HitRecord(), rec2 = new HitRecord();
            if (boundary.Hit(ray, -float.MaxValue, float.MaxValue, ref rec1))
            {
                if (boundary.Hit(ray, rec1.t + 0.0001f, float.MaxValue, ref rec2))
                {
                    rec1.t = Mathf.Range(rec1.t, t_min, t_max);
                    if (rec1.t < t_min) rec1.t = t_min;
                    if (rec2.t > t_max) rec2.t = t_max;
                    if (rec1.t >= rec2.t) return false;
                    if (rec1.t < 0) rec1.t = 0;
                    float distance_inside_boundary = (rec2.t - rec1.t) * ray.direction.length();

                    float hit_distance = -(1 / density) * Mathf.Log(Mathematics.Random.Get());
                    if (hit_distance < distance_inside_boundary)
                    {
                        rec.t = rec1.t + hit_distance / ray.direction.length();
                        rec.p = ray.GetPoint(rec.t);
                        rec.normal = new Vector3(1, 0, 0); // arbitrary
                        rec.shader = phase_function;
                        return true;
                    }
                }
            }

            return false;
        }
    }

    
}
