using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
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
        public override void Update()=>Buff = Renderer.main.buff;
    }
    public class Renderer
    {
        public static Renderer main;
        private Mode mode = Mode.Diffusing;
        
        private readonly HitableList hitableList = new HitableList();
        private const int Samples = 100, MAX_SCATTER_TIME = 8;
        private int width = 512, height =512;
        private readonly Preview window=new Preview();
        public byte[] buff;

        private Camera camera;
        private float recip_width, recip_height;
        public void Init()
        {
            main = this;
            buff= new byte[width * height * 4];
            InitScene();
            Start();
            window.Run(new DxConfiguration("Preview", width, height));
        }

        public void InitScene()
        {
            camera = new Camera(new Vector3(0, 1, 1), new Vector3(0, 0, -1), new Vector3(0, 1, 0), 75, 1);
            recip_width = 1f / width;
            recip_height = 1f / height;
            hitableList.list.Add(new Sphere(new Vector3(0, 0f, -1), 0.5f, new Dielectirc(5f)));
            hitableList.list.Add(new Sphere(new Vector3(0, 0, -2), 0.5f, new Metal(new Color32(1f, 0f, 0f), 0.3f)));
            hitableList.list.Add(new Sphere(new Vector3(0, -100.5f, -1), 100f,new Metal(new Color32(0.3f, 0.3f, 0.6f), 0.2f)));
            hitableList.list.Add(new Sphere(new Vector3(1, 0, -1), 0.5f, new Metal(new Color32(1f, 1f, 0f), 0.3f)));
            hitableList.list.Add(new Sphere(new Vector3(-1, 0, -1), 0.5f, new Lambertian(new Color32(0,1,0))));
        }
        public class ScannerConfig
        {
            public int xf, xt, yf, yt;
            public bool first;

            public ScannerConfig(int a, int b, int c, int d,bool f=true)
            {
                xf = a;xt = b;yf = c;yt = d;
                first = f;
            }
        }

        public int NowSample;
        private async void Start()
        {
            //ThreadPool.SetMaxThreads(16, 16);
            //for(var i=0;1<width;i+=width/4)
            //for (var j = 0; j < height;j+=height/4)
            //{
            //    ThreadPool.QueueUserWorkItem(ThreadScanner, new ScannerConfig( i, i + width / 4, j, j + height / 4));
            //}

            //ThreadPool.QueueUserWorkItem(ThreadScanner, new ScannerConfig(0, 256, 0, 256));
            //ThreadPool.QueueUserWorkItem(ThreadScanner, new ScannerConfig(0, 256, 255, 512));
            //ThreadPool.QueueUserWorkItem(ThreadScanner, new ScannerConfig(256, 512, 0, 256));
            //ThreadPool.QueueUserWorkItem(ThreadScanner, new ScannerConfig(256, 512, 256, 512));
            for (NowSample = 1; NowSample < Samples+1; NowSample++)
            {
                Form1.main.SetSPP(NowSample);
                var t = new Task<int>(() => LinearScanner(NowSample == 1));
                t.Start();
                await t;
            }
           
        }

        private int OldScanner()
        {
            var camera = new Camera(new Vector3(0, 0, 1), new Vector3(0, 0, -1), new Vector3(0, 1, 0), 75, 1);
            var recip_width = 1f / width;
            var recip_height = 1f / height;
            for (var  j = height-1; j >=0; j--)
                for (var i = 0; i < width; i++)
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
                    SetPixel(i, height - j - 1, color);
                }
            return 0;
        }
  
        private  void ThreadScanner(object c)
        { 
            var config = c as ScannerConfig;
            for (var j = config.yt - 1; j >= config.yf; j--)
            for (var i = config.xf; i < config.xt; i++)
            {
                var color = new Color32(0, 0, 0, 0);
                color += mode == Mode.Diffusing
                    ? Diffusing(camera.CreateRay(
                        (i + Random.Range(0, 1f)) * recip_width,
                        (j + Random.Range(0, 1f)) * recip_height), hitableList, 0)
                    : NormalMap(camera.CreateRay(
                        (i + Random.Range(0, 1f)) * recip_width,
                        (j + Random.Range(0, 1f)) * recip_height), hitableList);
                SetPixel(i, height - j - 1, color,!config.first);
            }
        }

        private int LinearScanner(bool f)
        {
            var all = height * width * 4;
            for (var j = height - 1; j >= 0; j--)
            for (var i = 0; i < width; i++)
            {
                var color = new Color32(0, 0, 0, 0);
                color += mode == Mode.Diffusing
                    ? Diffusing(camera.CreateRay(
                        (i + Random.Range(0, 1f)) * recip_width,
                        (j + Random.Range(0, 1f)) * recip_height), hitableList, 0)
                    : NormalMap(camera.CreateRay(
                        (i + Random.Range(0, 1f)) * recip_width,
                        (j + Random.Range(0, 1f)) * recip_height), hitableList);
                SetPixel(i, height - j - 1, color, !f);
                Form1.main.BeginInvoke(new Action(() => { Form1.main.SetProgress(width * 4 * j + i * 4, all); }));
            }

            return 0;
        }

        private void SetPixel(int x, int y, Color32 c32,bool combine=false)
        {
            var i = width * 4 * y + x * 4;
            var color = !combine ? c32.ToSystemColor()
                : ((new Color32(buff[i] / 255f, buff[i + 1] / 255f, buff[i + 2] / 255f, buff[i + 3] / 255f)*(NowSample-1) + c32) /
                   NowSample).ToSystemColor();
            buff[i] = color.R;
            buff[i + 1] = color.G;
            buff[i + 2] = color.B;
            buff[i + 3] = color.A;
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
