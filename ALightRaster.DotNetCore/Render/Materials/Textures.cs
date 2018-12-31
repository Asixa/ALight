using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Numerics;
using System.Text;
using ALightRaster.DotNetCore.Render.Mathematics;

namespace ALightRaster.DotNetCore.Render.Materials
{
    public abstract class Texture
    {
        public abstract Color32 Value(float u, float v, Vector3 p);
        public static CheckerTexture checker = new CheckerTexture(new ConstantTexture(Color32.White), new ConstantTexture(new Color32(0.5f, 0.5f, 0.5f)));
        public static ConstantTexture WhiteTexture = new ConstantTexture(Color32.White);
    }

    public class ConstantTexture : Texture
    {
        private readonly Color32 color;
        public ConstantTexture(Color32 c) => color = c;
        public override Color32 Value(float u, float v, Vector3 p) => color;
    }

    public class CheckerTexture : Texture
    {
        private readonly Texture odd, even;

        public CheckerTexture(Texture t0, Texture t1)
        {
            even = t0;
            odd = t1;
        }
        public override Color32 Value(float u, float v, Vector3 p) => MathF.Sin(10 * p.X) * MathF.Sin(10 * p.Y) * MathF.Sin(10 * p.Z) <= 0 ? odd.Value(u, v, p) : even.Value(u, v, p);
    }

    public class GrayTexture : Texture
    {
        private readonly Color32 color;
        public GrayTexture(float v) => color = new Color32(v, v, v);
        public override Color32 Value(float u, float v, Vector3 p) => color;
    }

    public class ImageTexture : Texture
    {
        private readonly byte[] data;
        public readonly int w, h;
        private readonly float scale = 1;
        private readonly int dir;
        public ImageTexture(string file, float s = 1, int d = 0)
        {
            dir = d;
            scale = s;
            var bitmap = new Bitmap(Image.FromFile(file));
            data = new byte[bitmap.Width * bitmap.Height * 3];
            w = bitmap.Width;
            h = bitmap.Height;
            for (var i = 0; i < bitmap.Height; i++)
            {
                for (var j = 0; j < bitmap.Width; j++)
                {
                    var c = bitmap.GetPixel(j, i);
                    data[3 * j + 3 * w * i] = c.R;
                    data[3 * j + 3 * w * i + 1] = c.G;
                    data[3 * j + 3 * w * i + 2] = c.B;
                }
            }

            midmaps.Add(this);
            if (w == h && h == 512)for (var i = 0; i < 9; i++)midmaps.Add(new ImageTexture(KiResizeImage(KiResizeImage(bitmap, (int)Math.Pow(2, 9 - i), (int)Math.Pow(2, 9 - i)),512,512)));
            
        }
        public ImageTexture(Bitmap bitmap, float s = 1, int d = 0)
        {
            dir = d;
            scale = s;
            data = new byte[bitmap.Width * bitmap.Height * 3];
            w = bitmap.Width;
            h = bitmap.Height;
            for (var i = 0; i < bitmap.Height; i++)
            {
                for (var j = 0; j < bitmap.Width; j++)
                {
                    var c = bitmap.GetPixel(j, i);
                    data[3 * j + 3 * w * i] = c.R;
                    data[3 * j + 3 * w * i + 1] = c.G;
                    data[3 * j + 3 * w * i + 2] = c.B;
                }
            }
        }
        public static Bitmap KiResizeImage(Bitmap bmp, int newW, int newH)
        {
            try
            {
                var b = new Bitmap(newW, newH);
                var g = Graphics.FromImage(b);

                // 插值算法的质量
                g.InterpolationMode = InterpolationMode.HighQualityBilinear;

                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();

                return b;
            }
            catch
            {
                return null;
            }
        }
        public ImageTexture(byte[] p, int x, int y)
        {
            data = p;
            w = x;
            h = y;
        }

        public List<ImageTexture>midmaps=new List<ImageTexture>();

        public Color32 GetPixel(int x, int y)
        {
            if (x >= w) return Color32.Red;
            if (y >= h) return Color32.Red;
            var i = w * 3 * y + x * 3;
            return new Color32((int)(data[i] / 255f), (int)(data[i + 1] / 255f), (int)(data[i + 2] / 255f), 1);
        }


        public override Color32 Value(float u, float v, Vector3 p)
        {
            if (dir == 1)
            {
                var t = u;
                u = 1 - v;
                v = t;
            }
            u = u * scale % 1;
            v = v * scale % 1;
            var i = MathRaster.Range((int)(u * w), 0, w - 1);
            var j = MathRaster.Range((int)((1 - v) * h - 0.001f), 0, h - 1);
            return new Color32(
                data[3 * i + 3 * w * j] / 255f,
                data[3 * i + 3 * w * j + 1] / 255f,
                data[3 * i + 3 * w * j + 2] / 255f);


//            if (dir == 1)
//            {
//                var t = u;
//                u = 1 - v;
//                v = t;
//            }
//            u = u * scale % 1;
//            v = v * scale % 1;
//
//            var U = u * w;
//            var V = (1 - v) * h - 0.001f;
//            var u1 = (int)Math.Floor(U);
//            var u2 = (int)Math.Ceiling(U);
//
//            var v1 = (int)Math.Floor(V);
//            var v2 = (int)Math.Ceiling(V);
//
//            var color1 = Color32.Lerp(GetColor(u1, v1), GetColor(u2, v1), U - u1);
//            var color2 = Color32.Lerp(GetColor(u1, v2), GetColor(u2, v2), U - u1);
//            var color = Color32.Lerp(color1, color2, V - v1);
//            return color;
//            Color32 GetColor(int _u, int _v)
//            {
//                var i = MathRaster.Range(_u, 0, w - 1);
//                var j = MathRaster.Range(_v, 0, h - 1);
//                return new Color32(
//                    data[3 * i + 3 * w * j] / 255f,
//                    data[3 * i + 3 * w * j + 1] / 255f,
//                    data[3 * i + 3 * w * j + 2] / 255f);
//            }
        }

        public ImageTexture Small2()
        {
            int Get(int x, int y, int o) => data[3 * y + 3 * w * x + o];
            var _data = new byte[w/2 * h/2 * 3];
            for (var i = 0; i < h; i+=2)
            {
                for (var j = 0; j < w; j+=2)
                {
                    _data[3 * j/2 + 3 * w * i / 2] = (byte)((Get(j, i, 0) + Get(j+1, i, 0) + Get(j, i+1, 0) + Get(j+1, i+1, 0))/4);
                    _data[3 * j / 2 + 3 * w * i / 2 + 1] = (byte)((Get(j, i, 1) + Get(j + 1, i, 1) + Get(j, i + 1, 1) + Get(j + 1, i + 1, 1)) / 4);
                    _data[3 * j / 2 + 3 * w * i / 2 + 2] = (byte)((Get(j, i, 2) + Get(j + 1, i, 2) + Get(j, i + 1, 2) + Get(j + 1, i + 1, 2)) / 4);
                }
            }
            return new ImageTexture(_data,w/2,h/2);
        }
    }

}
