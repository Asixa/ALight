using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using ALight.Render.Materials;
using ALight.Render.Mathematics;

namespace ALight.Render.Components
{
    public class HitRecord
    {
        public float u, v;
        public float t;
        public Vector3 p;
        public Vector3 normal;
        public Material material;
        public HitRecord() { }

        public HitRecord(HitRecord copy)
        {
            u = copy.u;
            v = copy.v;
            p = copy.p;
            normal = copy.normal;
            material = copy.material;
        }
    }

    public abstract class Hitable
    {
        public string name;
        public abstract bool Hit(Ray ray, float t_min, float t_max, ref HitRecord rec);
        public abstract bool BoundingBox(float t0, float t1,ref AABB box);
        public AABB GetBox(AABB box0, AABB box1)
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

        public override string ToString()
        {
            return name;
        }
    }

    public class HitableList : Hitable
    {
        public readonly List<Hitable> list = new List<Hitable>();

        public override bool Hit(Ray ray, float t_min, float t_max, ref HitRecord rec)
        {
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

            return hit_anything;
        }

        public override bool BoundingBox(float t0, float t1, ref AABB box)
        {
            if (list.Count == 0) return false;
            var tempBox=new AABB();
            if (!list[0].BoundingBox(t0, t1, ref tempBox)) return false;
            box = tempBox;
            foreach (var t in list)
            {
                if (t.BoundingBox(t0, t1, ref tempBox))
                    box = GetBox(box, tempBox);
                else return false;
            }

            return true;
        }
    }

    public class BVHNode : Hitable
    {
        public Hitable left, right;
        public AABB box;
        public BVHNode() { }
        public override string ToString()
        {
            if (left.ToString() == right.ToString()) return "<" + left + ">";
            return "<"+left+ "," + right+">";
        }

        public BVHNode(Hitable[] p, int n, float time0, float time1)
        {
            int Compare(Hitable a, Hitable b,int i)
            {
                AABB l = new AABB(), r = new AABB();
                if (!a.BoundingBox(0, 0, ref l) || !b.BoundingBox(0, 0, ref r)) throw new Exception("NULL");
                return l.min[i] - r.min[i]<0 ? -1 : 1;
            }
             Hitable[] SplitArray(Hitable[] Source, int StartIndex, int EndIndex)
            {
                    Hitable[] result = new Hitable[EndIndex - StartIndex + 1];
                    for (int i = 0; i <= EndIndex - StartIndex; i++) result[i] = Source[i + StartIndex];
                    return result;
            }
         
            var pl= p.ToList();
            var method = (int) (3 * Mathematics.Random.Get());
            pl.Sort((a, b) => Compare(a, b, method));
            p = pl.ToArray();
            //Console.ReadLine();
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
                    left = new BVHNode(SplitArray(p,0,n/2-1), n / 2, time0, time1);
                    right = new BVHNode(SplitArray(p,n/2,n-1), n - n / 2, time0, time1);
                    break;
            }
            AABB box_left=new AABB(), box_right=new AABB();
            if (!left.BoundingBox(time0, time1,ref box_left) || !right.BoundingBox(time0, time1,ref box_right))
            throw new Exception("no bounding box in bvh_node constructor");
            box = GetBox(box_left, box_right);
            if(n==6)Console.WriteLine("结果" +this);
        }

        public override bool BoundingBox(float t0, float t1, ref AABB b)
        {
            b = box;
            return true;
        }

        public override bool Hit(Ray ray, float t_min, float t_max, ref HitRecord rec)
        {
            if (box.Hit(ray, t_min, t_max))
            {
                HitRecord left_rec=new HitRecord(),right_rec=new HitRecord();
                var hit_left = left.Hit(ray, t_min, t_max, ref left_rec);
                var hit_right = right.Hit(ray, t_min, t_max, ref right_rec);
                if (hit_left && hit_right)
                {
                    rec = left_rec.t < right_rec.t ? left_rec : right_rec;
                    return true;
                }
                if (hit_left)
                {
                    rec = left_rec;
                    return true;
                }
                if (hit_right)
                {
                    rec = right_rec;
                    return true;
                }
                return false;
            }
            return false;
        }

}

    public class FilpNormals : Hitable
    {
        private readonly Hitable hitable;
        public FilpNormals(Hitable p)
        {
            hitable = p;
        }

        public override bool BoundingBox(float t0, float t1, ref AABB box)
        {
            return hitable.BoundingBox(t0, t1,ref box);
        }

        public override bool Hit(Ray ray, float t_min, float t_max, ref HitRecord rec)
        {
            if (!hitable.Hit(ray, t_min, t_max, ref rec)) return false;
            rec.normal = -rec.normal;
            return true;
        }
    }

    public class Translate : Hitable
    {
        public Hitable Object;
        private Vector3 offset;

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
            var moved=new Ray(ray.original-offset,ray.direction,ray.time);
            if (!Object.Hit(moved, t_min, t_max, ref rec)) return false;
            rec.p += offset;
            return true;
        }
    }
    public class RotateY : Hitable
    {
        public AABB bbox = new AABB();
        public bool hasbox;
        public Hitable Object;
        private float sin_theta, cos_theta;
        public override bool BoundingBox(float t0, float t1, ref AABB box)
        {
            box = bbox;
            return hasbox;
        }

        public RotateY(Hitable p, float angle)
        {
            Object = p;
            float radians = (Mathf.PI / 180f) * angle;
            sin_theta = Mathf.Sin(radians);
            cos_theta = Mathf.Cos(radians);
            hasbox = Object.BoundingBox(0, 1, ref bbox);
            var min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            var max = new Vector3(-float.MaxValue, -float.MaxValue, -float.MaxValue);
            for (var i = 0; i < 2; i++)
                for (var j = 0; j < 2; j++)
                    for (var k = 0; k < 2; k++)
                    {
                        float x = i * bbox.max.x + (1 - i) * bbox.min.x;
                        float y = j * bbox.max.y + (1 - j) * bbox.min.y;
                        float z = k * bbox.max.z + (1 - k) * bbox.min.z;
                        float newx = cos_theta * x + sin_theta * z;
                        float newz = -sin_theta * x + cos_theta * z;
                        var tester = new Vector3(newx, y, newz);
                        for (int c = 0; c < 3; c++)
                        {
                            if (tester[c] > max[c]) max[c] = tester[c];
                            if (tester[c] < min[c]) min[c] = tester[c];
                        }
                    }


            bbox = new AABB(min, max);
        }

        public override bool Hit(Ray ray, float t_min, float t_max, ref HitRecord rec)
        {
            var origin = new Vector3(ray.original);
            var direction = new Vector3(ray.direction);
            origin[0] = cos_theta * ray.original[0] - sin_theta * ray.original[2];
            origin[2] = sin_theta * ray.original[0] + cos_theta * ray.original[2];
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
        private Material phase_function;

        public ConstantMedium(Hitable b, float d, Texture a)
        {
            boundary = b;
            density = d;
            phase_function = new Isotropic(a);
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
                        rec.material = phase_function;
                        return true;
                    }
                }
            }

            return false;
        }
    }

    
}
