using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
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
    }

    public abstract class Hitable
    {
        public abstract bool Hit(Ray ray, float t_min, float t_max, ref HitRecord rec);
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
    }

    public class FilpNormals : Hitable
    {
        private readonly Hitable hitable;
        public FilpNormals(Hitable p)
        {
            hitable = p;
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

        public override bool Hit(Ray ray, float t_min, float t_max, ref HitRecord rec)
        {
            var moved=new Ray(ray.original-offset,ray.direction);
            if (!Object.Hit(moved, t_min, t_max, ref rec)) return false;
            rec.p += offset;
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
