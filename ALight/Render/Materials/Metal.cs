using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALight.Render.Components;
using ALight.Render.Math;

namespace ALight.Render.Materials
{
    public class Metal : Material
    {
        public readonly Color32 albedo;
        public float fuzz;

        public Metal(Color32 a, float f)
        {
            fuzz = f;
            albedo = a;
        }

        public override bool scatter(Ray rayIn, HitRecord record, ref Color32 attenuation, ref Ray scattered)
        {
            var reflected = Reflect(rayIn.normalDirection, record.normal);
            scattered = new Ray(record.p, reflected + fuzz * GetRandomPointInUnitSphere());
            attenuation = albedo;
            return Vector3.Dot(scattered.direction, record.normal) > 0;
        }
    }
}
