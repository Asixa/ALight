using System;
using System.Collections.Generic;
using ALight.Render.Materials;
using ALight.Render.Math;

namespace ALight.Render.Components
{
    public class HitRecord
    {
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
}
