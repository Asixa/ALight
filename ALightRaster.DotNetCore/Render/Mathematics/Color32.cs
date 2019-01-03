﻿using System;
using System.Drawing;
using System.Numerics;

namespace ALightRaster.DotNetCore.Render.Mathematics
{
#pragma warning disable CS0660 // “Color32”定义运算符 == 或运算符 !=，但不重写 Object.Equals(object o)
#pragma warning disable CS0661 // “Color32”定义运算符 == 或运算符 !=，但不重写 Object.GetHashCode()
    public struct Color32
#pragma warning restore CS0661 // “Color32”定义运算符 == 或运算符 !=，但不重写 Object.GetHashCode()
#pragma warning restore CS0660 // “Color32”定义运算符 == 或运算符 !=，但不重写 Object.Equals(object o)
    {
        public  float r, b, g, a;

        public Color32(float r, float g, float b, float a = 1)
        {
            
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a; 
        }

        public Color32(System.Drawing.Color c)
        {
            r = MathRaster.Range((float) c.R / 255, 0, 1);
            g = MathRaster.Range((float) c.G / 255, 0, 1);
            b = MathRaster.Range((float) c.B / 255, 0, 1);
            a = MathRaster.Range((float) c.A / 255, 0, 1);
        }

        public Color32(Vector4 v4) {
            r = v4.X;
            g = v4.Y;
            b = v4.Z;
            a = v4.W;
        }

        public Vector4 to_vector4()=>new Vector4(r,g,b,a);

        public Color32 DeNaN()
        {
            if (float.IsNaN(r)) r = 0;
            if (float.IsNaN(g)) g = 0;
            if (float.IsNaN(b)) b = 0;
            if (float.IsNaN(a)) a = 0;
            return this;
        }

        public override string ToString() => "<" + r + "," + g + "," + b + ">";

        public Color32 ToGramma()
        {
           return new Color32(MathF.Sqrt(r), MathF.Sqrt(g), MathF.Sqrt(b), 1f);
        }

        public Color ToSystemColor()
        {
            if (float.IsNaN(r) || float.IsNaN(g) || float.IsNaN(b) || float.IsNaN(a)) return Color.DeepPink;
            return Color.FromArgb((int) (a * 255+0.5f), (int) (r * 255 + 0.5f), (int) (g * 255 + 0.5f), (int) (b * 255 +0.5f));
        }

        public Vector3 ToNormal()
        {
            var _r = r * 2 - 1;
            var _g = g * 2 - 1;
            //var _b = -(b - 0.5f) * 2;
            var _b = -(b - 0.5f) * 2;
            if (MathF.Abs(_r)< 0.01) _r = 0;
            if (MathF.Abs(_g) < 0.01) _g = 0;
            if (MathF.Abs(_b) < 0.01) _b = 0;
            return new Vector3(_g,-_r,_b);
        }

        public float toGray()
        {
            return (r + g + b) / 3f;
        }
        public static Color32 operator +(Color32 a, Color32 b) =>new Color32(a.r + b.r, a.g + b.g, a.b + b.b, a.a + b.a);

        public static Color32 operator -(Color32 a, Color32 b) =>new Color32(a.r - b.r, a.g - b.g, a.b - b.b, a.a - b.a);

        public static Color32 operator *(Color32 a, Color32 b) =>new Color32(a.r * b.r, a.g * b.g, a.b * b.b, a.a * b.a);

        public static Color32 operator *(Color32 a, float b) => new Color32(a.r * b, a.g * b, a.b * b, a.a * b);

        public static Color32 operator *(float b, Color32 a) => new Color32(a.r * b, a.g * b, a.b * b, a.a * b);

        public static Color32 operator /(Color32 a, float b) => new Color32(a.r / b, a.g / b, a.b / b, a.a / b);


        public static bool operator ==(Color32 a, Color32 b) => (a.r==b.r&&a.g==b.g&&a.b==b.b&&a.a==b.a);
        public static bool operator !=(Color32 a, Color32 b) => !(a.r == b.r && a.g == b.g && a.b == b.b && a.a == b.a);

        public static Color32 Lerp(Color32 a, Color32 b, float t)
        {
            //Console.WriteLine("ColorLerp "+a+" "+b+" "+t);
            return  new Color32(a.r + (b.r - a.r) * t,
                a.g + (b.g - a.g) * t, a.b + (b.b - a.b) * t, a.a + (b.a - a.a) * t);
        }

//        public static Color32 Lerp(Color32 a, Color32 b, float t) => new Color32(a.r + (b.r - a.r) * t,
//            a.g + (b.g - a.g) * t, a.b + (b.b - a.b) * t, a.a + (b.a - a.a) * t);
        public  Color32 Normalized()
        {
            a = 1;
            return this;
        }

        public Color32 Aravge()=>this/=a;
        
        public static readonly Color32 
            Black = new Color32(0, 0, 0),
            Red = new Color32(1, 0, 0),
            Green = new Color32(0, 1, 0),
            Blue = new Color32(0, 0, 1),
            White = new Color32(1, 1, 1),
        Transparent = new Color32(0, 0, 0,0);
    }
}
