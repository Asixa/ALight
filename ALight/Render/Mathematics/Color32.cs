using System.Drawing;

namespace ALight.Render.Mathematics
{
    public class Color32
    {
        public readonly float r, b, g, a;

        public Color32(float r, float g, float b, float a = 1)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public Color32(System.Drawing.Color c)
        {
            r = Mathf.Range((float) c.R / 255, 0, 1);
            g = Mathf.Range((float) c.G / 255, 0, 1);
            b = Mathf.Range((float) c.B / 255, 0, 1);
            a = Mathf.Range((float) c.A / 255, 0, 1);
        }

        public Color32()
        {
        }

        public override string ToString() => "<" + r + "," + g + "," + b + ">";

        public Color32 ToGramma()
        {
           return new Color32(Mathf.Sqrt(r), Mathf.Sqrt(g), Mathf.Sqrt(b), 1f);
        }

        public Color ToSystemColor()
        {
            if (float.IsNaN(r) || float.IsNaN(g) || float.IsNaN(b) || float.IsNaN(a)) return Color.DeepPink;
            //return Color.FromArgb((int) (a * 255), (int) (r * 255), (int) (g * 255), (int) (b * 255));
            return Color.FromArgb((int) (a * 255+0.5f), (int) (r * 255 + 0.5f), (int) (g * 255 + 0.5f), (int) (b * 255 +0.5f));
        }

        public static Color32 operator +(Color32 a, Color32 b) =>
            new Color32(a.r + b.r, a.g + b.g, a.b + b.b, a.a + b.a);

        public static Color32 operator -(Color32 a, Color32 b) =>
            new Color32(a.r - b.r, a.g - b.g, a.b - b.b, a.a - b.a);

        public static Color32 operator *(Color32 a, Color32 b) =>
            new Color32(a.r * b.r, a.g * b.g, a.b * b.b, a.a * b.a);

        public static Color32 operator *(Color32 a, float b) => new Color32(a.r * b, a.g * b, a.b * b, a.a * b);

        public static Color32 operator *(float b, Color32 a) => new Color32(a.r * b, a.g * b, a.b * b, a.a * b);

        public static Color32 operator /(Color32 a, float b) => new Color32(a.r / b, a.g / b, a.b / b, a.a / b);

        public static Color32 Lerp(Color32 a, Color32 b, float t) => new Color32(a.r + (b.r - a.r) * t,
            a.g + (b.g - a.g) * t, a.b + (b.b - a.b) * t, a.a + (b.a - a.a) * t);

        public static Color32 
            black = new Color32(0, 0, 0),
            red = new Color32(1, 0, 0),
            green = new Color32(0, 1, 0),
            blue = new Color32(0, 0, 1),
            white = new Color32(1, 1, 1);
    }
}
