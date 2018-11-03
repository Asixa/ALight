using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using ALight.Render.Denoise;
using ALight.Render.Mathematics;
using ALight.Render.Scanners;
using static  ALight.Render.Configuration;
using Point = ALight.Render.Mathematics.Point;
using Random = ALight.Render.Mathematics.Random;

namespace ALight.Render
{
    public enum Mode
    {
        NormalMap,
        Shaded
    }

    public class Renderer
    {
        public static Renderer main;
        public delegate void ChunkEnd(int i);
        public ChunkEnd chunk_end;
        public float[] buff;
        public int[] Changes;
        public int FinishedChunks;
        public int[] scaned;
        public int[] hit;
        public List<Point> aliveBlocks;
        public byte[] previewUI;
        private static PointBitmap PointBitmap;
        public bool FirstRound;
        private float recip_width, recip_height;
        public bool STOP;


        public void Init()
        {
            main = this;
            recip_width = 1f / width;
            recip_height = 1f / height;
            divide_w = width / block_size + 1;
            divide_h = height / block_size + 1;
            buff = new float[width * height * 4];
            Changes = new int[width * height];
            previewUI = new byte [width * height * 4];
            hit = new int[width * height];
            scaned = new int[divide_w * divide_h];
            FinishedChunks = 0;
            FirstRound = false;
            STOP = false;
            aliveBlocks = new List<Point>();
            PointBitmap = new PointBitmap(new Bitmap(width, height));
            for (var h = 0; h < divide_h; h++)
            for (var w = 0; w < divide_w; w++)
                aliveBlocks.Add(new Point(w, h));
            for (var index = 0; index < scaned.Length; index++) scaned[index] = SPP;
            StartRender();
        }

        private void StartRender()
        {
            ThreadPool.SetMaxThreads(divide_w * divide_h, divide_w * divide_h);
            var seq = Sequence(divide_w, divide_h);
            seq.Reverse();
            foreach (var t in seq)
            {
                ThreadPool.QueueUserWorkItem(Scanner,
                   new ScannerConfig(t.x * block_size, t.y * block_size, t.y * divide_w + t.x, aliveBlocks[t.y * divide_w + t.x]));
            }
        }

        private void Scanner(object o)
        {
            var config = (ScannerConfig) o;
            for (; 0 < scaned[config.ID];)
            {
                if(STOP)return;
                if (scaned[config.ID] == SPP) SetPixelPriview(config, Color32.White);
                for (var h = config.h; h < config.h + block_size; h++)
                {
                    if (h >= height) continue;
                    for (var w = config.w; w < config.w + block_size; w++)
                    {
                        if (w >= width) continue;
                        var id = h * width + w;
                        var color = mode == Mode.Shaded
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
                    if (config.ID == divide_h * divide_h - 1) FirstRound = true;
                    SetPixelPriview(config, Color32.Transparent);
                    chunk_end?.Invoke(FinishedChunks);
                    aliveBlocks.Remove(config.point);
                    if (FinishedChunks == divide_w * divide_h) MessageBox.Show("渲染完成", "渲染器");
                    else if (FirstRound)
                    {
                        var h = aliveBlocks[0].y;
                        var w = aliveBlocks[0].x;
                        ThreadPool.QueueUserWorkItem(Scanner,
                            new ScannerConfig(w * block_size, h * block_size, h * divide_w + w, aliveBlocks[0]));
                    }
                }
            }
        }

        private void SetPixelPriview(ScannerConfig config, Color32 c32)
        {
            void Set(int _x, int _y, Color32 c)
            {
                if (_x >= width || _y >= height) return;
                var i = width * 4 * _y + _x * 4;
                previewUI[i] = (byte) (c.r * 255);
                previewUI[i + 1] = (byte) (c.g * 255);
                previewUI[i + 2] = (byte) (c.b * 255);
                previewUI[i + 3] = (byte) (c.a * 255);

            }

            for (var i = 0; i < block_size; i++)
            {
                for (var j = 0; j < 2; j++)
                {
                    Set(config.w + i, config.h + j, c32); //横着
                    Set(config.w + i, config.h + block_size - j - 1, c32); //横着
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
            buff[i] += c32.b;
            buff[i + 1] += c32.g;
            buff[i + 2] += c32.r;
            buff[i + 3] += c32.a;
        }

        public void Save(string path = "Result" + ".png")
        {
            if (!Directory.Exists("Output")) Directory.CreateDirectory("Output");

            int Get(int i) => (byte) Mathf.Range(main.buff[i] * 255 / main.Changes[i / 4] + 0.5f, 0, 255f);
            var pic = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            for (var i = 0; i < main.buff.Length; i += 4)
            {
                var c = Color.FromArgb(Get(i + 3), Get(i+2), Get(i + 1), Get(i));
                pic.SetPixel(i % (width * 4) / 4, i / (width * 4), c);
            }

            pic.Save("Output/" + path);
            MessageBox.Show("Output/" + path, "保存完成");
        }



        public static Bitmap GetBitmap()
        {
            if(PointBitmap==null)return new Bitmap(width,height);
            PointBitmap.LockBits();
            for (var i = 0; i < main.buff.Length; i++)
                if (main.previewUI[i] != 0)PointBitmap.SetColor(i,main.previewUI[i]);
                else  PointBitmap.SetColor(i, (byte) Mathf.Range(main.buff[i] * 255 / main.Changes[i / 4] + 0.5f, 0, 255));
            return PointBitmap.UnlockBits();
        }

        public static List<Point> Sequence(int w, int h)
        {
            byte state = 0;
            var points = new List<Point>();
            int rl = 0, ul = 0, dl = 1, ll = 0, x = 0, y = h - 1;
            for (var i = 0; i < w * h; i++)
            {
                points.Add(new Point(x, y));
                if (state == 0)
                {
                    if (x == w - rl - 1)
                    {
                        rl++;
                        y--;
                        state = 1;
                    }
                    else
                        x++;
                }
                else if (state == 1)
                {
                    if (y == ul)
                    {
                        ul++;
                        x--;
                        state = 2;
                    }
                    else
                        y--;
                }
                else if (state == 2)
                {
                    if (x == ll)
                    {
                        ll++;
                        y++;
                        state = 3;
                    }
                    else
                        x--;
                }
                else if (state == 3)
                {
                    if (y == h - dl - 1)
                    {
                        dl++;
                        x++;
                        state = 0;
                    }
                    else y++;
                }
            }

            return points;

        }
        #region Denoise


        #endregion
    }
}




