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

        public override float PdfValue(Vector3 o, Vector3 v)
        {
            HitRecord rec=new HitRecord();
            if (Hit(new Ray(o, v), 0.001f, float.MaxValue, ref rec))
            {
                float cos_theta_max = Mathf.Sqrt(1 - radius * radius / (center - o).SqrtMagnitude);
                float solid_angle = 2 * Mathf.PI * (1 - cos_theta_max);
                return 1 / solid_angle;
            }
            else return 0;
        }

        public override Vector3 Random(Vector3 o)
        {
            Vector3 direction = center - o;
            float ds = direction.SqrtMagnitude;
            Onb uvw=new Onb(direction);
            return uvw.Local(RandomToSphere(radius,ds));
        }

        public Vector3 RandomToSphere(float radius, float ds)
        {
            float r1 = Mathematics.Random.Get();
            float r2 = Mathematics.Random.Get();
            float z = 1 + r2 * (Mathf.Sqrt(1 - radius * radius / ds) - 1);
            float phi = 2 * Mathf.PI * r1;
            float x = Mathf.Cos(phi) * Mathf.Sqrt(1 - z * z);
            float y = Mathf.Sin(phi) * Mathf.Sqrt(1 - z * z);
            return new Vector3(x,y,z);
        }

        public override bool BoundingBox(float t0, float t1, ref AABB box)
        {
            box=new AABB(center-new Vector3(radius,radius,radius),center+new Vector3(radius,radius,radius));
            return true;
        }

        public override bool Hit(Ray ray, float t_min, float t_max, ref HitRecord rec)
        {
            void GetSphereUV(ref  HitRecord record)
            {
                float phi = Mathf.Atan2(record.p.z, record.p.x);
                float theta = Mathf.Asin(record.p.y);
                record.u = 1 - (phi + Mathf.PI) / (2 * Mathf.PI);
                record.v = (theta + Mathf.PI / 2) / Mathf.PI;
            }
            var oc = ray.origin - center;
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
                GetSphereUV(ref rec);
                return true;
            }
            temp = (-b + Mathf.Sqrt(discriminant)) / a * 0.5f;
            if (!(temp < t_max) || !(temp > t_min)) return false;
            rec.material = material;
            rec.t = temp;
            rec.p = ray.GetPoint(rec.t);
            rec.normal = (rec.p - center).Normalized();
            GetSphereUV(ref rec);
            return true;
        }
    }
}
