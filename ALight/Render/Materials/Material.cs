using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALight.Render.Components;
using ALight.Render.Math;
using Random = ALight.Render.Math.Random;

namespace ALight.Render.Materials
{
    public abstract class Material
    {
        public abstract bool scatter(Ray rayIn, HitRecord record, ref Color32 attenuation, ref Ray scattered);
        public static Vector3 Reflect(Vector3 vin, Vector3 normal) => vin - 2 * Vector3.Dot(vin, normal) * normal;

        public Vector3 GetRandomPointInUnitSphere()
        {
            var p = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)) * 2f - Vector3.one;
            return p.Normalized() * Random.Range(0f, 1f);
        }

        public static bool Refract(Vector3 vin, Vector3 normal, float ni_no, ref Vector3 refracted)
        {
            Vector3 uvin = vin.Normalized();
            float dt = Vector3.Dot(uvin, normal);
            float discrimination = 1 - ni_no * ni_no * (1 - dt * dt);
            if (discrimination > 0)
            {
                refracted = ni_no * (uvin - normal * dt) - normal * Mathf.Sqrt(discrimination);
                return true;
            }

            return false;
        }

        public static float Schlick(float cos, float ref_idx)
        {
            var r0 = (1 - ref_idx) / (1 + ref_idx);
            r0 *= r0;
            return r0 + (1 - r0) * Mathf.Pow((1 - cos), 5);
        }
    }
}
