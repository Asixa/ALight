using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AcDx;
using ALight.Render.Denoise;
using ALight.Render.Mathematics;
using ALight.Render.Scanners;
using static  ALight.Render.Configuration;
using Random = ALight.Render.Mathematics.Random;
using Point=ALight.Render.Mathematics.Point;
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
        public delegate void ChunkEnd(int i);

        public ChunkEnd chunk_end;
        private Preview preview = new Preview();
        public float[] buff;
        public int[] Changes;
        public int FinishedChunks;
        public int[] scaned;
        public int[] hit;
        public List<Point> aliveBlocks=new List<Point>();
        public byte[] previewUI;

        public bool First;
        private float recip_width, recip_height;
        public void InitPreview()
        {
            preview = new Preview();
            preview.Run(new DxConfiguration("Preview", width, height));
        }

        public void Init()
        {
            main = this;
            recip_width = 1f / width;
            recip_height = 1f / height;
            divide_w = width / block_size + 1;
            divide_h = height / block_size + 1;
            buff = new float[width * height * 4];
            Changes = new int[width * height];
            previewUI = new byte [width * height*4];
            hit=new int[width*height];
            scaned = new int[divide_w * divide_h];
            for (var h = 0; h <divide_h; h++)
            for (var w = 0; w < divide_w; w++)
            aliveBlocks.Add(new Point(w,h));
            for (var index = 0; index < scaned.Length; index++)scaned[index]= SPP;
            Scene.main.Init();
            Start(); 
        }
     
        private async void Start()
        {

            ThreadPool.SetMaxThreads(divide_w * divide_h, divide_w * divide_h);
            for (var h = 0; h < divide_h; h++)
            for (var w = 0; w < divide_w; w++)
                ThreadPool.QueueUserWorkItem(Scanner,
                    new ScannerConfig(w * block_size, h * block_size, h * divide_w + w, aliveBlocks[h * divide_w + w]));
            InitPreview();
        }
        
        private void Scanner(object o)
        {
            var config = (ScannerConfig) o;
            for(;0< scaned[config.ID] ;)
            {
                if (scaned[config.ID] == SPP) SetPixelPriview(config, Color32.White);
                for (var h = config.h; h < config.h + block_size; h++)
                {
                    if (h >= height) continue;
                    for (var w = config.w; w < config.w + block_size; w++)
                    {
                        if (w >= width) continue;
                        var id = h * width + w;
                        var color = mode == Mode.Diffusing
                            ? DiffuseScanner.GetColor(Scene.main.camera.CreateRay(
                                    (width - w + Random.Get()) * recip_width,
                                    (height - h + Random.Get()) * recip_height, id), Scene.main.world,
                                Scene.main.Important, 0).DeNaN()
                            : NormalMapScanner.GetColor(Scene.main.camera.CreateRay(
                                (width - w + Random.Get()) * recip_width,
                                (height - h + Random.Get()) * recip_height, h * width + w), Scene.main.world);
                        SetPixel(w, h, color);
                    }
                }
                scaned[config.ID]--;
                if (scaned[config.ID] == 0)
                {
                    FinishedChunks++;
                    if (config.ID == divide_h * divide_h - 1)First = true;
                    SetPixelPriview(config, Color32.Transparent);
                    chunk_end?.Invoke(FinishedChunks);
                    aliveBlocks.Remove(config.point);
                    if (FinishedChunks == divide_w * divide_h)MessageBox.Show("渲染完成", "渲染器");
                    else if (First)
                    {
                        var h = aliveBlocks[0].y;
                        var w = aliveBlocks[0].x;
                        ThreadPool.QueueUserWorkItem(Scanner, new ScannerConfig(w * block_size, h * block_size, h * divide_w + w, aliveBlocks[0]));
                    }
                }
            } 
        }

        private void Scanner2(object o)
        {
            var config = (ScannerConfig)o;
            Parallel.For(0, SPP, a =>
            {
                if (a == 0)
                {
                    SetPixelPriview(config, Color32.White);
                }
                for (var h = config.h; h < config.h + block_size; h++)
                {
                    if (h >= height) continue;
                    for (var w = config.w; w < config.w + block_size; w++)
                    {
                        if (w >= width) continue;
                        var id = h * width + w;
                        var color = mode == Mode.Diffusing
                            ? DiffuseScanner.GetColor(Scene.main.camera.CreateRay(
                                    (width - w + Random.Get()) * recip_width,
                                    (height - h + Random.Get()) * recip_height, id), Scene.main.world,
                                Scene.main.Important, 0).DeNaN()
                            : NormalMapScanner.GetColor(Scene.main.camera.CreateRay(
                                (width - w + Random.Get()) * recip_width,
                                (height - h + Random.Get()) * recip_height, h * width + w), Scene.main.world);
                        SetPixel(w, h, color);
                    }
                }


                if (a == SPP - 1)
                {
                    FinishedChunks++;
                    SetPixelPriview(config, Color32.Transparent);
                    chunk_end?.Invoke(FinishedChunks);
                    if (FinishedChunks == divide_w * divide_h)
                        MessageBox.Show("渲染完成", "渲染器");
                }
            });
        }
        private void SetPixelPriview(ScannerConfig config, Color32 c32)
        {
            void Set(int _x, int _y, Color32 c)
            {
                if (_x >= width || _y >= height) return;
                var i = width * 4 * _y + _x * 4;
                previewUI[i] = (byte)(c.r * 255);
                previewUI[i + 1] = (byte)(c.g * 255);
                previewUI[i + 2] = (byte)(c.b * 255);
                previewUI[i + 3] = (byte)(c.a * 255);

            }
            for (var i = 0; i < block_size; i++)
            {
                for (var j = 0; j < 2; j++)
                {
                    Set(config.w + i, config.h + j, c32); //横着
                    Set(config.w + i, config.h + block_size - j-1, c32); //横着
                    if (i >= 10 && i <= block_size - 10) continue;
                    Set(config.w + j, config.h + i, c32);
                    Set(config.w + block_size - j, config.h + i, c32);
                }
            }
            
        }

        private void SetPixel(int x, int y, Color32 c32)
        { 
            var i = width * 4 * y + x * 4;
            Changes[width * y + x]++;
            buff[i] += c32.r;
            buff[i + 1] += c32.g;
            buff[i + 2] += c32.b;
            buff[i + 3] += c32.a;
        }

        public void Save(string path="Result"+".png")
        {
            if (!Directory.Exists("Output")) Directory.CreateDirectory("Output");

            int Get(int i)=> (byte)Mathf.Range(main.buff[i] * 255 / main.Changes[i / 4] + 0.5f, 0, 255f);
            var pic = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            for (var i = 0; i < main.buff.Length; i+=4)
            {
                var c = Color.FromArgb(Get(i+3), Get(i), Get(i + 1), Get(i + 2));
                pic.SetPixel(i % (width*4)/4, i / (width*4), c);
            }
            pic.Save("Output/"+path);
            MessageBox.Show( "Output/" + path, "保存完成");
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
