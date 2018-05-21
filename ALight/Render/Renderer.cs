using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Threading.Tasks;
using AcDx;
using ALight.Render.Components;
using ALight.Render.Denoise;
using ALight.Render.Instances;
using ALight.Render.Materials;
using ALight.Render.Mathematics;
using ALight.Render.Primitives;
using ALight.Render.Scanners;
using Random = ALight.Render.Mathematics.Random;

namespace ALight.Render
{
    public enum Mode
    {
        NormalMap,
        Diffusing
    }
    public class Renderer
    {
        public static Renderer main;
        private const Mode mode = Mode.Diffusing;

        private Preview preview = new Preview();
        public float[] buff;
        public int[] Changes;

        
        private float recip_width, recip_height;
        public int now_sample = 0;

        public void InitPreview()
        {
            preview = new Preview();
            preview.Run(new DxConfiguration("Preview", Configuration.width, Configuration.height));
        }

        public void Init()
        {
            main = this;
            recip_width = 1f / Configuration.width;
            recip_height = 1f / Configuration.height;
            buff = new float[Configuration.width * Configuration.height * 4];
            Changes = new int[Configuration.width * Configuration.height];
            Scene.main.Init();
            Start();
            InitPreview();
        }

        private async void Start()
        {
            ThreadPool.SetMaxThreads(32, 32);
            //await Task.Factory.StartNew(delegate { LinearScanner(new ScannerConfig(height, width)); });
            //for (var i = 0; i < Configuration.SPP; i += Configuration.SamplePerThread)
            //    ThreadPool.QueueUserWorkItem(LinearScanner, new ScannerConfig(Configuration.height, Configuration.width));

            var wp = Configuration.width / Configuration.divide;
            var hp = Configuration.height / Configuration.divide;
            for (var i = Configuration.divide - 1; i >= 0; i--)
            for (var j = Configuration.divide - 1; j >= 0; j--)
                ThreadPool.QueueUserWorkItem(LinearScanner2,
                    new ScannerConfig(hp * i, hp * (1 + i), wp * j, wp * (1 + j)));

        }

        private void LinearScanner2(object o)
        {
            var config = (ScannerConfig) o;

            Parallel.For(0, Configuration.SPP, a => {
                for (var j = config.h; j < config.h2; j++)
                for (var i = config.w; i < config.w2; i++)
                {
                    var color = mode == Mode.Diffusing
                        ? DiffuseScanner.GetColor(Scene.main.camera.CreateRay(
                            (i + Random.Get()) * recip_width,
                            (j + Random.Get()) * recip_height), Scene.main.world, Scene.main.Important, 0).DeNaN()
                        : NormalMapScanner.GetColor(Scene.main.camera.CreateRay(
                            (i + Random.Get()) * recip_width,
                            (j + Random.Get()) * recip_height), Scene.main.world);
                    SetPixel(Configuration.width - i - 1, Configuration.height - j - 1, color);
                }
            });
            
            //for (var a = 0; a < Configuration.SPP; a++)
            //{
            //    for (var j = config.h; j < config.h2; j++)
            //    for (var i = config.w; i < config.w2; i++)
            //    {
            //        var color = mode == Mode.Diffusing
            //            ? DiffuseScanner.GetColor(Scene.main.camera.CreateRay(
            //                (i + Random.Get()) * recip_width,
            //                (j + Random.Get()) * recip_height), Scene.main.world, Scene.main.Important, 0).DeNaN()
            //            : NormalMapScanner.GetColor(Scene.main.camera.CreateRay(
            //                (i + Random.Get()) * recip_width,
            //                (j + Random.Get()) * recip_height), Scene.main.world);
            //        SetPixel(Configuration.width - i - 1, Configuration.height - j - 1, color);
            //    }
            //}
            now_sample += Configuration.SamplePerThread;
        }

        private void LinearScanner(object o)
        {
            var config = (ScannerConfig)o;
            for (var j = config.h - 1; j >= 0; j--)
                for (var i = 0; i < config.w; i++)
                {
                    for (var a= 0; a < Configuration.SamplePerThread;a++)
                    {
                        var color = mode == Mode.Diffusing
                            ? DiffuseScanner.GetColor(Scene.main.camera.CreateRay(
                                (i + Random.Get()) * recip_width,
                                (j + Random.Get()) * recip_height), Scene.main.world, Scene.main.Important, 0).DeNaN()
                            : NormalMapScanner.GetColor(Scene.main.camera.CreateRay(
                                (i + Random.Get()) * recip_width,
                                (j + Random.Get()) * recip_height), Scene.main.world);
                        SetPixel(config.w - i - 1, config.h - j - 1, color);
                    }
                }
            now_sample += Configuration.SamplePerThread;
        }

        private void SetPixel(int x, int y, Color32 c32)
        { 
            var i = Configuration.width * 4 * y + x * 4;
            Changes[Configuration.width * y + x]++;
            buff[i] += c32.r;
            buff[i + 1] += c32.g;
            buff[i + 2] += c32.b;
            buff[i + 3] += c32.a;
        }

        public void Save(string path="a.png")
        {
            int Get(int i)=> (byte)Mathf.Range(main.buff[i] * 255 / main.Changes[i / 4] + 0.5f, 0, 255f);
            var pic = new Bitmap(Configuration.width, Configuration.height, PixelFormat.Format32bppArgb);
            for (var i = 0; i < main.buff.Length; i+=4)
            {
                var c = Color.FromArgb(Get(i+3), Get(i), Get(i + 1), Get(i + 2));
                pic.SetPixel(i % (Configuration.width*4)/4, i / (Configuration.width*4), c);
            }
            pic.Save(path);
        }

        #region Denoise
        public void Denoise()
        {
            //this.progressBar1.Value = 0;
            //this.lock_Buttons();
            //Denoiser.input_image = this.textBox1.Text;
            //Denoiser.output_image = this.textBox2.Text;
            //Denoiser.blend = (float)this.trackBar1.Value / 100f;
            Denoiser.Denoise(ImageCallBack, ProgressCallBack, DenoiseFinishedCallBack);
        }

        public int DenoiseFinishedCallBack()
        {
            //this.unlock_Buttons();
            return 0;
        }
        public int ProgressCallBack(float progress)
        {
            //this.progressBar1.Value = (int)(progress * 100f);
            //this.pictureBox1.Image = this.pBuffer;
            return 0;
        }
        public int ImageCallBack(IntPtr data, int w, int h, int size)
        {
            //this.pBuffer = new Bitmap(w, h, size / h, PixelFormat.Format32bppArgb, data);
            //this.pBuffer.RotateFlip(RotateFlipType.Rotate180FlipX);
            //this.DoEvents();
            return 0;
        }
        #endregion
    }
}
