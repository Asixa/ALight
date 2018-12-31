using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using ALight.Render.Mathematics;

namespace ALight.Render.Materials
{
    public abstract class Texture
    {
        public abstract Color32 Value(float u, float v, Vector3 p);
        public static CheckerTexture checker=new CheckerTexture(new ConstantTexture(Color32.White), new ConstantTexture(new Color32(0.5f,0.5f,0.5f)));
        public static ConstantTexture WhiteTexture=new ConstantTexture(Color32.White);
    }

    public class ConstantTexture : Texture
    {
        private readonly Color32 color;
        public ConstantTexture(Color32 c)=>color = c;
        public override Color32 Value(float u, float v, Vector3 p)=>color;
    }

    public class CheckerTexture : Texture
    {
        private readonly Texture odd,even;

        public CheckerTexture(Texture t0, Texture t1)
        {
            even = t0;
            odd = t1;
        }
        public override Color32 Value(float u, float v, Vector3 p)=>Mathf.Sin(10 * p.x) * Mathf.Sin(10 * p.y) * Mathf.Sin(10 * p.z) <= 0?odd.Value(u,v,p):even.Value(u,v,p);
    }

    public class GrayTexture : Texture
    {
        private readonly Color32 color;
        public GrayTexture(float v) => color = new Color32(v,v,v);
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
        }
        public ImageTexture(byte[] p, int x, int y)
        {
            data = p;
            w = x;
            h = y;
        }

        public Color32 GetPixel(int x, int y)
        {
            if(x>=w)return Color32.Red;
            if(y>=h)return Color32.Red;
            //Console.WriteLine(w+"/"+h);
            var i = w * 3 * y + x * 3;
            return new Color32((int)(data[i] / 255f), (int) (data[i+1] / 255f), (int) (data[i+2] / 255f),1);
        }
        public void Save(string name)
        {

            if (!Directory.Exists("Output")) Directory.CreateDirectory("Output");

            int Get(int i) => (byte) Mathf.Range(data[i] * 255 + 0.5f, 0, 255f);
            var pic = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            for (var i = 0; i < data.Length; i += 4)
            {
                var c = Color.FromArgb(255, Get(i + 2), Get(i + 1), Get(i));
                pic.SetPixel(i % (w * 4) / 4, i / (w * 4), c);
            }

            pic.Save("Output/" + name + ".png");
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
            var i = Mathf.Range((int)(u * w), 0, w - 1);
            var j = Mathf.Range((int)((1 - v) * h - 0.001f), 0, h - 1);
            return new Color32(
                data[3 * i + 3 * w * j] / 255f,
                data[3 * i + 3 * w * j + 1] / 255f,
                data[3 * i + 3 * w * j + 2] / 255f);
        }
    }
    public class NoiseTexture : Texture
    {
        public NoiseTexture() { }
        public NoiseTexture(float s) => scale = s;
        private readonly float scale;

        public override Color32 Value(float u, float v, Vector3 p) =>
            //Color32.white * 0.5f * (1 + Perlin.Noise(scale * p));
            Color32.White * 0.5f * (1 + (Mathf.Sin(scale * p.z) + 10 * Perlin.Turb(p)));
    }
}
