using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALight.Render.Components;
using ALight.Render.Materials;
using ALight.Render.Mathematics;
using Random = ALight.Render.Mathematics.Random;

namespace ALight.Render.Primitives
{
    public class MovingSphere:Hitable
    {
        public Vector3 pos0, pos1;
        public float time0, time1;
        public float radius;
        public Material material;

        public MovingSphere(Vector3 p0, Vector3 p1, float t0, float t1, float r, Material m)
        {
            pos0 = p0;pos1 = p1;
            time0 = t0;time1 = t1;
            radius = r;material = m;
        }

        public Vector3 Center(float time) =>
            pos0 + (time - time0) / (time1 - time0) * (pos1 - pos0);

        public override bool Hit(Ray ray, float t_min, float t_max, ref HitRecord rec)
        {
            Vector3 oc = ray.original - Center(ray.time);

            //Console.WriteLine(Renderer.main.camera.time0 + Random.Get() * (Renderer.main.camera.time1 - Renderer.main.camera.time0) + "<<<" + Renderer.main.camera.time0 + " " + Renderer.main.camera.time1);
            var a = Vector3.Dot(ray.direction, ray.direction);
            var b = Vector3.Dot(oc, ray.direction);
            var c = Vector3.Dot(oc, oc) - radius * radius;
            var discriminant = b * b - a * c;
            if (discriminant > 0)
            {
                var temp = (-b - Mathf.Sqrt(discriminant)) / a;
                if (temp < t_max && temp > t_min)
                {
                    rec.t = temp;
                    rec.p = ray.GetPoint(rec.t);
                    rec.normal = (rec.p - Center(ray.time)) / radius;
                    rec.material = material;
                    return true;
                }
                temp = (-b + Mathf.Sqrt(discriminant)) / a;
                if (temp < t_max && temp > t_min)
                {
                    rec.t = temp;
                    rec.p = ray.GetPoint(rec.t);
                    rec.normal = (rec.p - Center(ray.time)) / radius;
                    rec.material = material;
                    return true;
                }
            }
            return false;
        }

        public override bool BoundingBox(float t0, float t1, ref AABB box)
        {
            box = GetBox(
                new AABB(Center(t0) - new Vector3(radius, radius, radius),
                    Center(t0) + new Vector3(radius, radius, radius)),
                new AABB(Center(t1) - new Vector3(radius, radius, radius),
                    Center(t1) + new Vector3(radius, radius, radius)));
            return true;
        }


    }
}
