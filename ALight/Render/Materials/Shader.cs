﻿using System;
using System.Runtime.InteropServices;
using ALight.Render.Components;
using ALight.Render.Mathematics;
using Random = ALight.Render.Mathematics.Random;

namespace ALight.Render.Materials
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ScatterRecord
    {
        public Ray specular_ray;
        public bool is_specular;
        public Color32 attenuation;
        public Pdf pdf;
    }
    public abstract class Shader
    {
        public bool BackCulling = true;
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
            var  p=new Vector3(0);
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

        public static  Dielectirc Glass=new Dielectirc(1.5f,new Color32(1,1,1));
        public static Lambertian WhiteLambertion=new Lambertian(new ConstantTexture(Color32.White));
        public static Lambertian GrayLambertion = new Lambertian(new ConstantTexture(new Color32(0.2f,0.2f,0.2f)));
        public static Lambertian BlueLambertion = new Lambertian(new ConstantTexture(Color32.Blue));
        public static Metal Sliver = new Metal(new ConstantTexture(Color32.White),0);
    }
    public class StardardShader : Shader
    {
        private readonly Texture albeo;
        private readonly Texture reflective;
        private readonly Texture normal_texture;
        private readonly Texture height_texture;
        private  readonly float height_scale=-1;
        public StardardShader(Texture diff, Texture normal, Texture reflective,Texture height_texture=null)
        {
            this.normal_texture = normal;
            this.reflective = reflective;
            this.height_texture =height_texture;
            albeo = diff;
        }
//        Vector2 ParallaxMapping(float u,float v, Vector3 viewDir)
//        {
//            var height = height_texture.Value(u,v,new Vector3(0)).r;
//            var p = new Vector2(viewDir.x,viewDir.y) / viewDir.z * (height * height_scale);
//            return new Vector2(u,v) - p;
//        }

        Vector2 ParallaxMapping(float u, float v, Vector3 viewDir)
        {
            const float numLayers = 10;
            // calculate the size of each layer
            var layerDepth = 1.0f / numLayers;
            // depth of current layer
            var currentLayerDepth = 0.0f;
            // the amount to shift the texture coordinates per layer (from vector P)
            var P = new Vector2(viewDir.x,viewDir.y) * height_scale;
            var deltaTexCoords = P / numLayers;

            var currentTexCoords = new Vector2(u,v);
            var currentDepthMapValue = height_texture.Value(u, v, new Vector3(0)).r;

            while (currentLayerDepth < currentDepthMapValue)
            {
                // shift texture coordinates along direction of P
                currentTexCoords -= deltaTexCoords;
                // get depthmap value at current texture coordinates
                currentDepthMapValue = height_texture.Value(currentTexCoords.x, currentTexCoords.y, new Vector3(0)).r;
                // get depth of next layer
                currentLayerDepth += layerDepth;
            }

            var prevTexCoords = currentTexCoords + deltaTexCoords;

            // get depth after and before collision for linear interpolation
            var afterDepth = currentDepthMapValue - currentLayerDepth;
            var beforeDepth = height_texture.Value(prevTexCoords.x, prevTexCoords.y, new Vector3(0)).r - currentLayerDepth + layerDepth;

            // interpolation of texture coordinates
            var weight = afterDepth / (afterDepth - beforeDepth);
            Vector2 finalTexCoords = prevTexCoords * weight + currentTexCoords * (1.0f - weight);

            return finalTexCoords;
        }
        public override bool scatter(Ray rayIn, ref HitRecord hrec, ref ScatterRecord srec)
        {

            var rotate_maritx = new Matrix3x3(-hrec.bitangent, hrec.tangent, hrec.normal);
            var rotate_maritx2 = new Matrix3x3(hrec.bitangent, hrec.tangent, hrec.normal);
            var viewin = (Matrix3x3.FromDouble(Matrix3x3.MatrixInverse(rotate_maritx2.toDouble()))
             * -rayIn.normal_direction).Normalized();
        
            var uv=new Vector2(hrec.u, hrec.v);

//            if (height_texture!=null)
//                uv = ParallaxMapping(hrec.u, hrec.v, viewin);
            uv.Range0_1();
           
            //Vector3.Cross(hrec.tangent, hrec.normal)

            var normal = (rotate_maritx* -normal_texture.Value(uv.x, uv.y, hrec.p).ToNormal().Normalized()).Normalized();
            if(Configuration.mode==Mode.NormalMap)hrec.normal = normal;

            var reflected = Reflect(rayIn.direction.Normalized(), normal);
            var fuzz = reflective?.Value(uv.x, uv.y, hrec.p).toGray() ?? 1f;
            srec.specular_ray = new Ray(hrec.p, reflected + fuzz * GetRandomPointInUnitSphere());
            srec.attenuation = albeo.Value(uv.x, uv.y, hrec.p);
            srec.is_specular = true;
            srec.pdf = null;
            return true;
        }
    }
    public class Metal : Shader
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
    public class Dielectirc : Shader
    {
        private readonly float ref_idx;

        public Dielectirc(float ri, Color32 c)
        {
            BackCulling = false;
            ref_idx = ri;
            color = c;
        }
        public Dielectirc(float ri)
        {
            BackCulling = false;
            ref_idx = ri;
            color = Color32.White;
        }
        public Color32 color;
        public override bool scatter(Ray rayIn, ref HitRecord hrec, ref ScatterRecord srec)
        {
            srec.is_specular = true;
            srec.pdf = null;
            //var origin = new Color32(1.0f, 1.0f, 1.0f);
            //var result_c=new Color32(origin.r*color.r,,);
            //srec.attenuation = new Color32(1.0f, 1.0f, 1.0f)*(color*color.a);
            srec.attenuation = color;
            //srec.attenuation.a = 1;
            Vector3 outward_normal;
            var reflected = Reflect(rayIn.direction, hrec.normal);
            var refracted=new Vector3(0);
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
    public class Lambertian : Shader
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

    public class Subsurface : Shader
    {
#pragma warning disable CS0169 // 从不使用字段“Subsurface.density”
        private readonly float ref_idx,fuzz,density;
#pragma warning restore CS0169 // 从不使用字段“Subsurface.density”
        public Color32 color;
        public Subsurface(float ri, Color32 c, float glossy)
        {
            BackCulling = false;
            ref_idx = ri;
            color = c;
            fuzz = glossy;
        }

        public override bool scatter(Ray rayIn, ref HitRecord hrec, ref ScatterRecord srec)
        {
            srec.is_specular = true;
            srec.pdf = null;
            srec.attenuation = color;
            Vector3 outward_normal;
            var reflected = Reflect(rayIn.direction, hrec.normal);
            var refracted = new Vector3(0);
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

            var reflect_prob = Refract(rayIn.direction, outward_normal, ni_over_nt, ref refracted)
                ? Schlick(cosine, ref_idx)
                : 1.0f;


            //if (Random.Get() < reflect_prob)
            //{
            //    srec.specular_ray = new Ray(hrec.p, reflected + fuzz * GetRandomPointInUnitSphere());
            //}
            //else
            //{

            //    var rec2=new HitRecord();
            //    var refracetRay = new Ray(hrec.p, refracted);
            //    if (special.Hit(refracetRay, 0.0001f, float.MaxValue, ref rec2))
            //    {
            //        Console.WriteLine("HEY");
            //        float distance_inside_boundary = (rec2.t - hrec.t) * rayIn.direction.length();
            //        float hit_distance = -(1 / density) * Mathf.Log(Random.Get());
            //        if (hit_distance < distance_inside_boundary)
            //        {
            //            srec.specular_ray=new Ray(refracetRay.GetPoint( hit_distance / rayIn.direction.length()),GetRandomPointInUnitSphere() );
            //            return true;
            //        }
            //    }
            //}

            srec.specular_ray = Random.Get() < reflect_prob
                ? new Ray(hrec.p, reflected + fuzz * GetRandomPointInUnitSphere())
                : new Ray(hrec.p, refracted + fuzz * GetRandomPointInUnitSphere());
            return true;
        }
    }
    public class DiffuseLight : Shader
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
