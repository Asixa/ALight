using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALight.Render.Components;
using ALight.Render.Materials;
using ALight.Render.Mathematics;

namespace ALight.Render.Scanners
{
    public abstract class Scanner
    {
     
    }

    public class DiffuseScanner : Scanner
    {
        public static Color32 GetColor(Ray r, HitableList hitableList, Hitable importance, int depth)
        {
            var hrec = new HitRecord();
            if (hitableList.Hit(r, 0.0001f, float.MaxValue, ref hrec))
            {
                //Console.WriteLine(hrec);
                
                var srec = new ScatterRecord();
                var emitted = hrec.shader.emitted(r, hrec, hrec.u, hrec.v, hrec.p);
                if (depth < Configuration.MAX_SCATTER_TIME && hrec.shader.scatter(r, ref hrec, ref srec))
                {
                    if (srec.is_specular) return srec.attenuation * GetColor(srec.specular_ray, Scene.main.world, importance, depth + 1);
                    var plight = new HitablePdf(importance, hrec.p);
                    var p = new MixturePdf(plight, srec.pdf);
                    var scattered = new Ray(hrec.p, p.Generate(), r.time);
                    var pdf = p.Value(scattered.direction);
                    return emitted + srec.attenuation * hrec.shader.scattering_pdf(r, hrec, scattered) *
                           GetColor(scattered, Scene.main.world, importance, depth + 1) / pdf;
                }
                else return emitted;
            }
            var t = 0.5f * r.normal_direction.y + 1f;
            return Scene.main.SkyColor ? (1 - t) * new Color32(2, 2, 2) + t * new Color32(0.5f, 0.7f, 1) : Color32.Black;
        }
    }

    public class NormalMapScanner
    {
        public static Color32 GetColor(Ray ray, HitableList hitableList)
        {
            var record = new HitRecord();
            if (hitableList.Hit(ray, 0f, float.MaxValue, ref record))
                return 0.5f * new Color32(record.normal.x + 1, record.normal.y + 1, record.normal.z + 1, 2f);
            var t = 0.5f * ray.normal_direction.y + 1f;
            return (1 - t) * new Color32(1, 1, 1) + t * new Color32(0.5f, 0.7f, 1);
        }
    }

}
