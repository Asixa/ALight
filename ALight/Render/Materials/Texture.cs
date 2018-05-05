using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALight.Render.Components;
using ALight.Render.Mathematics;

namespace ALight.Render.Materials
{
    public abstract class Texture
    {
        public abstract Color32 value(float u, float v, Vector3 p);
    }

    public class ConstantTexture : Texture
    {
        private Color32 color;
        public ConstantTexture(Color32 c)=>color = c;
        public override Color32 value(float u, float v, Vector3 p)=>color;
    }

    public class CheckerTexture : Texture
    {
        public Texture odd, even;

        public CheckerTexture(Texture t0, Texture t1)
        {
            even = t0;
            odd = t1;
        }
        public override Color32 value(float u, float v, Vector3 p)=>Mathf.Sin(10 * p.x) * Mathf.Sin(10 * p.y) * Mathf.Sin(10 * p.z) <= 0?odd.value(u,v,p):even.value(u,v,p);
    }

    public class ImageTexture : Texture
    {
        private byte[] data;
        private int w, h;
        private float scale = 1;
        public ImageTexture(string file,float s=1)
        {
            scale = s;
            var bitmap = new Bitmap(Image.FromFile(file));
            data=new byte[bitmap.Width*bitmap.Height*3];
            w = bitmap.Width;
            h = bitmap.Height;
            for (var i = 0; i < bitmap.Height; i++)
            {
                for (var j = 0; j < bitmap.Width; j++)
                {
                    var c = bitmap.GetPixel(j, i);
                    data[3 * j + 3 * w * i] = c.R;
                    data[3 * j + 3 * w * i+1] = c.G;
                    data[3 * j + 3 * w * i+2] = c.B;
                }
            }
        }
        public ImageTexture(byte[] p, int x, int y)
        {
            data = p;
            w = x;
            h = y;
        }
        public override Color32 value(float u, float v, Vector3 p)
        {
            u = u * scale % 1;
            v = v * scale % 1;

            var i = Mathf.Range((int) (u * w), 0, w - 1);
            var j= Mathf.Range((int) ((1 - v) * h - 0.001f), 0, h - 1);


            return new Color32(
                data[3 * i + 3 * w * j] / 255f, 
                data[3 * i + 3 * w * j+1] / 255f, 
                data[3 * i + 3 * w * j+2] / 255f);
        }


    }
    public class NoiseTexture : Texture
    {
        public NoiseTexture() { }
        public NoiseTexture(float s) => scale = s;
        public float scale;

        public override Color32 value(float u, float v, Vector3 p) =>
            //Color32.white * 0.5f * (1 + Perlin.Noise(scale * p));
            Color32.white * 0.5f * (1 + (Mathf.Sin(scale * p.z) + 10 * Perlin.Turb(p)));
    }
}
