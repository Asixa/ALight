using System;
using System.Drawing;
using System.Threading;
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
            for (var i = 0; i < Buff.Length; i++)
            Buff[i] = (byte) (Renderer.main.buff[i] * 255 + 0.5);
        }
    }
    public class Renderer
    {
        public static Renderer main;
        private readonly Mode mode = Mode.Diffusing;

        private readonly HitableList hitableList = new HitableList();
        private readonly int Samples = 10000,MAX_SCATTER_TIME = 8;
        private int width = 512, height = 512;
        private readonly Preview preview = new Preview();
        public float[] buff;

        private Camera camera;
        private float recip_width, recip_height;
        private int NowSample = 0;

        public void Init()
        {
            main = this;
            buff = new float[width * height * 4];
            InitScene();
            Start();
            preview.Run(new DxConfiguration("Preview", width, height));
        }

        private void InitScene()
        {
            camera = new Camera(new Vector3(0, 1, 1), new Vector3(0, 0, -1), new Vector3(0, 1, 0), 75, 1);
            recip_width = 1f / width;
            recip_height = 1f / height;
            hitableList.list.Add(new Sphere(new Vector3(0, -100.5f, -1), 100f,
                new Metal(new Color32(0.3f, 0.3f, 0.6f), 0.2f)));
            hitableList.list.Add(new Sphere(new Vector3(0, 0f, -1), 0.5f, new Dielectirc(5f)));
            hitableList.list.Add(new Sphere(new Vector3(0, 0, -2), 0.5f, new Metal(new Color32(1f, 0f, 0f), 0.3f)));
            hitableList.list.Add(new Sphere(new Vector3(1, 0, -1), 0.5f, new Metal(new Color32(1f, 1f, 0f), 0.3f)));
            hitableList.list.Add(new Sphere(new Vector3(-1, 0, -1), 0.5f, new Lambertian(new Color32(0, 1, 0))));
            //hitableList.list.Add(new Sphere(new Vector3(0, 0, -2), 0.5f, new Dielectirc(5f)));
            //hitableList.list.Add(new Sphere(new Vector3(1, 0, -1), 0.5f, new Dielectirc(5f)));
            //hitableList.list.Add(new Sphere(new Vector3(-1, 0, -1), 0.5f, new Dielectirc(5f)));
        }

        private class ScannerConfig
        {
            public readonly int w,h;

            public ScannerConfig( int h, int w)
            {
                this.h = h;
                this.w = w;
            }
        }

        private async void Start()
        {
            ThreadPool.SetMaxThreads(16, 16);
            await Task.Factory.StartNew(delegate { LinearScanner(new ScannerConfig(height, width)); });
            for (var i = 1; i < Samples; i++)
                ThreadPool.QueueUserWorkItem(LinearScanner, new ScannerConfig(height, width));
        }

        private void LinearScanner(object o)
        {
            var config = (ScannerConfig)o;
            int n;
            lock (buff) n = ++NowSample;
            for (var j = config.h - 1; j >= 0; j--)
                for (var i = 0; i < config.w; i++)
                {
                    var color = mode == Mode.Diffusing
                        ? Diffusing(camera.CreateRay(
                            (i + Random.Range(0, 1f)) * recip_width,
                            (j + Random.Range(0, 1f)) * recip_height), hitableList, 0)
                        : NormalMap(camera.CreateRay(
                            (i + Random.Range(0, 1f)) * recip_width,
                            (j + Random.Range(0, 1f)) * recip_height), hitableList);
                    SetPixel(i, config.h - j-1,color,n);
                }
            Form1.main.BeginInvoke(new Action(() => { Form1.main.SetSPP();}));
        }

        private void SetPixel(int x, int y, Color32 c32,int n)
        { 
            var i = width * 4 * y + x * 4;
            var color = (new Color32(buff[i], buff[i + 1] , buff[i + 2] , buff[i + 3]) *(n - 1) + c32) /n;
            buff[i] = color.r;
            buff[i + 1] = color.g;
            buff[i + 2] = color.b;
            buff[i + 3] = color.a;
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
                if (depth >= MAX_SCATTER_TIME || !record.material.scatter(ray, record, ref attenuation, ref r))
                    return Color32.black;
                var c = Diffusing(r, hitableList, depth + 1);
                return new Color32(c.r * attenuation.r, c.g * attenuation.g, c.b * attenuation.b);
            }
            var t = 0.5f * ray.normalDirection.y + 1f;
            return (1 - t) * new Color32(1, 1, 1) + t * new Color32(0.5f, 0.7f, 1);
        }

        private enum Mode
        {
            NormalMap,
            Diffusing
        };

        public void Save()
        {
            var pic_buff = preview.Buff;
            var pic = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            for (var i = 0; i < pic_buff.Length; i+=4)
            {
                var c = Color.FromArgb(pic_buff[i+3], pic_buff[i], pic_buff[i+1], pic_buff[i+2]);
                pic.SetPixel(i % (width*4)/4, i / (width*4), c);
            }
            pic.Save("a.png");
        }
    }
}
