using ALight.Render.Components;
using ALight.Render.Mathematics;
using Random = ALight.Render.Mathematics.Random;

namespace ALight.Render.Materials
{
    public struct ScatterRecord
    {
        public Ray specular_ray;
        public bool is_specular;
        public Color32 attenuation;
        public Pdf pdf;
    }
    public abstract class Material
    {
        protected static float Schlick(float cosine, float ref_idx)
        {
            var r0 = (1 - ref_idx) / (1 + ref_idx);
            r0 = r0 * r0;
            return r0 + (1 - r0) * Mathf.Pow((1 - cosine), 5);
        }
        public  virtual bool scatter(Ray rayIn, ref HitRecord hrec, ref ScatterRecord srec)=>false;
        public virtual Color32 emitted(Ray rayIn,HitRecord rec,float u,float v,Vector3 p)=>new Color32(0,0,0);
        protected static Vector3 Reflect(Vector3 vin, Vector3 normal) => vin - 2 * Vector3.Dot(vin, normal) * normal;
        public virtual float scattering_pdf(Ray ray_in, HitRecord record, Ray scattered) => 0;

        protected Vector3 GetRandomPointInUnitSphere()
        {
            var  p=new Vector3();
            do p = new Vector3(Random.Get(), Random.Get(), Random.Get()) * 2.0f - Vector3.one;
             while (Vector3.Dot(p, p) >= 1);
            return p.Normalized();
        }

        protected static bool Refract(Vector3 vin, Vector3 normal, float ni_no, ref Vector3 refracted)
        {
            var uvin = vin.Normalized();
            var dt = Vector3.Dot(uvin, normal);
            var discrimination = 1 - ni_no * ni_no * (1 - dt * dt);
            if (!(discrimination > 0)) return false;
            refracted = ni_no * (uvin - normal * dt) - normal * Mathf.Sqrt(discrimination);
            return true;
        }
    }

    public class Metal : Material
    {
        private readonly Texture texture;
        private readonly float fuzz;

        public Metal(Texture t, float f)
        {
            fuzz = f;
            texture = t;
        }

        public override bool scatter(Ray rayIn, ref HitRecord hrec, ref ScatterRecord srec)
        {
            var reflected = Reflect(rayIn.direction.Normalized(), hrec.normal);
            srec.specular_ray=new Ray(hrec.p,reflected+fuzz*GetRandomPointInUnitSphere());
            srec.attenuation = texture.Value(hrec.u, hrec.v, hrec.p);
            srec.is_specular = true;
            srec.pdf = null;
            return true;
        }
    }
    public class Dielectirc : Material
    {
        private readonly float ref_idx;
        public Dielectirc(float ri) => ref_idx = ri;

        public override bool scatter(Ray rayIn, ref HitRecord hrec, ref ScatterRecord srec)
        {
            srec.is_specular = true;
            srec.pdf = null;
            srec.attenuation = new Color32(1.0f, 1.0f, 1.0f);
            Vector3 outward_normal;
            var reflected = Reflect(rayIn.direction, hrec.normal);
            var refracted=new Vector3();
            float ni_over_nt;
            float cosine;
            if (Vector3.Dot(rayIn.direction, hrec.normal) > 0)
            {
                outward_normal = -hrec.normal;
                ni_over_nt = ref_idx;
                cosine = ref_idx * Vector3.Dot(rayIn.direction, hrec.normal) / rayIn.direction.length();
            }
            else
            {
                outward_normal = hrec.normal;
                ni_over_nt = 1.0f / ref_idx;
                cosine = -Vector3.Dot(rayIn.direction, hrec.normal) / rayIn.direction.length();
            }
            var reflect_prob = Refract(rayIn.direction, outward_normal, ni_over_nt,ref refracted) ? Schlick(cosine, ref_idx) : 1.0f;
            srec.specular_ray = Random.Get() < reflect_prob ? new Ray(hrec.p, reflected) : new Ray(hrec.p, refracted);
            return true;
        }
    }
    public class Lambertian : Material
    {
        private readonly Texture texture;
        public Lambertian(Texture t) => texture = t;
        public override float scattering_pdf(Ray ray_in, HitRecord record, Ray scattered)
        {
            var cos = Vector3.Dot(record.normal, scattered.direction.Normalized());
            if (cos < 0) cos = 0;
            return cos / Mathf.PI;
        }

        public override bool scatter(Ray rayIn,ref HitRecord hrec, ref ScatterRecord srec)
        {
            srec.is_specular = false;
            srec.attenuation = texture.Value(hrec.u, hrec.v, hrec.p);
            srec.pdf=new CosinePdf(hrec.normal);
            return true;
        }
    }

    public class DiffuseLight : Material
    {
        private readonly Texture texture;
        private readonly float intensity;
        public DiffuseLight(Texture t,float i)
        {
            texture = t;
            intensity = i;
        }
        public override bool scatter(Ray rayIn, ref HitRecord hrec, ref ScatterRecord srec)=>false;

        public override Color32 emitted(Ray rayIn, HitRecord rec, float u, float v, Vector3 p) =>
            texture.Value(u, v, p) * intensity;
        //public override Color32 emitted(Ray rayIn, HitRecord rec, float u, float v, Vector3 p)
        //   => Vector3.Dot(rec.normal, rayIn.direction) >0  ? texture.value(u, v, p) * intensity : new Color32(0, 0, 0, 0);

    }
}
