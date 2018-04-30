using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALight.Render.Components;
using ALight.Render.Materials;
using ALight.Render.Math;

namespace ALight.Render.Primitives
{
    public class Sphere : Hitable
    {
        public Vector3 center;
        public float radius;
        public Material material;

        public Sphere(Vector3 cen, float rad, Material m)
        {
            center = cen;
            radius = rad;
            material = m;
        }

        public override bool Hit(Ray ray, float t_min, float t_max, ref HitRecord rec)
        {
            var oc = ray.original - center;
            var a = Vector3.Dot(ray.direction, ray.direction);
            var b = 2f * Vector3.Dot(oc, ray.direction);
            var c = Vector3.Dot(oc, oc) - radius * radius;
            var discriminant = b * b - 4 * a * c;
            if (!(discriminant > 0)) return false;
            var temp = (-b - Mathf.Sqrt(discriminant)) / a * 0.5f;

            if (temp < t_max && temp > t_min)
            {
                rec.material = material;
                rec.t = temp;
                rec.p = ray.GetPoint(rec.t);
                rec.normal = (rec.p - center).Normalized();
                return true;
            }

            temp = (-b + Mathf.Sqrt(discriminant)) / a * 0.5f;
            if (!(temp < t_max) || !(temp > t_min)) return false;
            rec.material = material;
            rec.t = temp;
            rec.p = ray.GetPoint(rec.t);
            rec.normal = (rec.p - center).Normalized();
            return true;
        }
    }
}
