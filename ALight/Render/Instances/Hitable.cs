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

        public HitRecord()
        {
        }

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
        public abstract bool Hit(Ray ray, float t_min, float t_max, ref HitRecord rec);
        public abstract bool BoundingBox(float t0, float t1, ref AABB box);

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
            var tempBox = new AABB();
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
            => boundary.BoundingBox(t0, t1, ref box);


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
