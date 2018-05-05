using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALight.Render.Components;
using ALight.Render.Mathematics;
using Random = ALight.Render.Mathematics.Random;

namespace ALight.Render.Materials
{
    public abstract class Material
    {
        public abstract bool scatter(Ray rayIn, HitRecord record, ref Color32 attenuation, ref Ray scattered);
        public virtual Color32 emitted(float u,float v,Vector3 p)=>new Color32(0,0,0);
        public static Vector3 Reflect(Vector3 vin, Vector3 normal) => vin - 2 * Vector3.Dot(vin, normal) * normal;

        public Vector3 GetRandomPointInUnitSphere()
        {
            var p = new Vector3(Random.Get(), Random.Get(), Random.Get()) * 2f - Vector3.one;
            return p.Normalized() * Random.Get();
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

    public class Metal : Material
    {
        public Texture texture;
        public float fuzz;

        public Metal(Texture t, float f)
        {
            fuzz = f;
            texture = t;
        }

        public override bool scatter(Ray rayIn, HitRecord record, ref Color32 attenuation, ref Ray scattered)
        {
            var reflected = Reflect(rayIn.normalDirection, record.normal);
            scattered = new Ray(record.p, reflected + fuzz * GetRandomPointInUnitSphere(),rayIn.time);
            attenuation = texture.value(record.u, record.v, record.p);
            return Vector3.Dot(scattered.direction, record.normal) > 0;
        }
        
    }
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
            scattered = Random.Get() <= reflect_prob
                ? new Ray(record.p, reflected,rayIn.time)
                : new Ray(record.p, refracted,rayIn.time);
            return true;
        }
    }
    public class Lambertian : Material
    {
        public Texture texture;
        public Lambertian(Texture t) => texture = t;

        public override bool scatter(Ray rayIn, HitRecord record, ref Color32 attenuation, ref Ray scattered)
        {
            var target = record.p + record.normal + GetRandomPointInUnitSphere();
            scattered = new Ray(record.p, target - record.p,rayIn.time);

            //float phi = Mathf.Atan2(record.p.z, record.p.x);
            //float theta = Mathf.Asin(record.p.y);
            //var u = 1 - (phi + Mathf.PI) / (2 * Mathf.PI);
            //var v = (theta + Mathf.PI / 2) / Mathf.PI;
            //attenuation = texture.value(u, v, record.p);
            attenuation = texture.value(record.u, record.v, record.p);
            return true;
        }


    }

    public class DiffuseLight : Material
    {
        private readonly Texture texture;
        private float intensity;
        public DiffuseLight(Texture t,float i)
        {
            texture = t;
            intensity = i;
        }
        
        public override bool scatter(Ray rayIn, HitRecord record, ref Color32 attenuation, ref Ray scattered)=>false;
        
        public override Color32 emitted(float u, float v, Vector3 p)=>texture.value(u, v, p)*intensity;
        
    }

    public class Isotropic : Material
    {
        public Texture texture;
        public Isotropic(Texture t)
        {
            texture = t;
        }

        public override bool scatter(Ray rayIn, HitRecord record, ref Color32 attenuation, ref Ray scattered)
        {
           scattered=new Ray(record.p,GetRandomPointInUnitSphere(),rayIn.time);
            attenuation = texture.value(record.u, record.v, record.p);
            return true;
        }
    }
}
