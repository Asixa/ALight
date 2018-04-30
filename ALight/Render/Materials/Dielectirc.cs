using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALight.Render.Components;
using ALight.Render.Math;
using Random = ALight.Render.Math.Random;

namespace ALight.Render.Materials
{
    public class Dielectirc : Material
    {
        float ref_idx;
        public Dielectirc(float ri) => ref_idx = ri;

        public override bool scatter(Ray rayIn, HitRecord record, ref Color32 attenuation, ref Ray scattered)
        {
            Vector3 outNormal;
            var reflected = Reflect(rayIn.direction, record.normal);
            attenuation = Color32.white;
            var ni_no = 1f;
            var refracted = Vector3.zero;
            float cos = 0;
            float reflect_prob = 0;
            if (Vector3.Dot(rayIn.direction, record.normal) > 0)
            {
                outNormal = -record.normal;
                ni_no = ref_idx;
                cos = ni_no * Vector3.Dot(rayIn.normalDirection, record.normal);
            }
            else
            {
                outNormal = record.normal;
                ni_no = 1f / ref_idx;
                cos = -Vector3.Dot(rayIn.normalDirection, record.normal);
            }

            reflect_prob = Refract(rayIn.direction, outNormal, ni_no, ref refracted) ? Schlick(cos, ref_idx) : 1;
            scattered = Random.Range(0f, 1f) <= reflect_prob
                ? new Ray(record.p, reflected)
                : new Ray(record.p, refracted);
            return true;
        }
    }
}
