using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcDx;
using ALight.Render.Components;
using ALight.Render.Materials;
using ALight.Render.Math;
using ALight.Render.Primitives;
using Random = ALight.Render.Math.Random;

namespace ALight.Render
{
    public class Preview : DxWindow
    {
        public override void Update()
        {
            Buff = Renderer.main.Buff;
        }
    }
    public class Renderer
    {
        public static Renderer main;
        private Mode mode = Mode.NormalMap;
        
        private readonly HitableList hitableList = new HitableList();
        private const int Samples = 1, MAX_SCATTER_TIME = 8;

        private int j, i;
        private int width = 1024, height = 1024;

        public Preview window=new Preview();
        public byte[] Buff;

        public void Init()
        {
            main = this;
            Buff= new byte[width * height * 4];
            InitScene();
            Start();
            window.Run(new DxConfiguration("Preview", width, height));
        }

        public void InitScene()
        {
            // hitableList.list.Add(new Sphere(new Vector3(0, 0, -1), 0.5f, new Lambertian(new Color32(1f, 0f, 0f))));
            hitableList.list.Add(new Sphere(new Vector3(0, 0, -1), 0.5f, new Metal(new Color32(1f, 0f, 0f), 0.3f)));
            hitableList.list.Add(new Sphere(new Vector3(0, -100.5f, -1), 100f,new Metal(new Color32(0.3f, 0.3f, 0.6f), 0.2f)));
            hitableList.list.Add(new Sphere(new Vector3(1, 0, -1), 0.5f, new Metal(new Color32(1f, 1f, 0f), 0.3f)));
            hitableList.list.Add(new Sphere(new Vector3(0, 1, -1), 0.5f, new Metal(new Color32(1f, 1f, 1f), 0f)));
            hitableList.list.Add(new Sphere(new Vector3(-1, 0, -1), 0.5f, new Dielectirc(20f)));
        }

        private async void Start()
        {
            var t = new Task<int>(Scan);
            t.Start();
            await t;
        }

        private int Scan()
        {
            var camera = new Camera(new Vector3(0, 0, 1), new Vector3(0, 0, -1), new Vector3(0, 1, 0), 75, 1);
            var recip_width = 1f / width;
            var recip_height = 1f / height;
            for (j = height-1; j >=0; j--)
                for (i = 0; i < width; i++)
                {
                    var color = new Color32(0, 0, 0, 0);
                    for (var s = 0; s < Samples; s++)
                        color += mode == Mode.Diffusing
                            ? Diffusing(camera.CreateRay(
                                (i + Random.Range(0, 1f)) * recip_width,
                                (j + Random.Range(0, 1f)) * recip_height), hitableList, 0)
                            : NormalMap(camera.CreateRay(
                                (i + Random.Range(0, 1f)) * recip_width,
                                (j + Random.Range(0, 1f)) * recip_height), hitableList);
                    color /= Samples;
                    color *= 1f;
                    SetPixel(i, height - j - 1, color.ToSystemColor());
                }
            return 0;
        }

        public void SetPixel(int x, int y, Color color)
        {
            var i = width * 4 * y + x * 4;
            Buff[i] = color.R;
            Buff[i + 1] = color.G;
            Buff[i + 2] = color.B;
            Buff[i + 3] = color.A;
        }

        private Color32 NormalMap(Ray ray, HitableList hitableList)
        {
            var record = new HitRecord();
            if (hitableList.Hit(ray, 0f, float.MaxValue, ref record))
                return 0.5f * new Color32(record.normal.x + 1, record.normal.y + 1, record.normal.z + 1, 2f);
            var t = 0.5f * ray.normalDirection.y + 1f;
            return (1 - t) * new Color32(1, 1, 1) + t * new Color32(0.5f, 0.7f, 1);
        }

        private Color32 Diffusing(Ray ray, HitableList hitableList, int depth)
        {
            var record = new HitRecord();
            if (hitableList.Hit(ray, 0.0001f, float.MaxValue, ref record))
            {
                var r = new Ray(Vector3.zero, Vector3.zero);
                var attenuation = Color32.black;
                if (depth < MAX_SCATTER_TIME && record.material.scatter(ray, record, ref attenuation, ref r))
                {
                    var c = Diffusing(r, hitableList, depth + 1);
                    return new Color32(c.r * attenuation.r, c.g * attenuation.g, c.b * attenuation.b);
                }
                else
                {
                    return Color32.black;
                }
            }

            var t = 0.5f * ray.normalDirection.y + 1f;
            return (1 - t) * new Color32(1, 1, 1) + t * new Color32(0.5f, 0.7f, 1);
        }

        enum Mode
        {
            NormalMap,
            Diffusing
        };
    }
}
