using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALight.Render.Components;
using ALight.Render.Math;

namespace ALight.Render.Materials
{
    public class Lambertian : Material
    {
        Color32 albedo;
        public Lambertian(Color32 a) => albedo = a;

        public override bool scatter(Ray rayIn, HitRecord record, ref Color32 attenuation, ref Ray scattered)
        {
            var target = record.p + record.normal + GetRandomPointInUnitSphere();
            scattered = new Ray(record.p, target - record.p);
            attenuation = albedo;
            return true;
        }
    }
}
