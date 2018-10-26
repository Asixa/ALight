using System;

namespace ALight.Render.Mathematics
{
    public class Mathf
    {
        public const float PI = 3.14159274f;
        public static float Sqrt(float v) => (float) Math.Sqrt(Convert.ToDouble(v));

        public static float Range(float v, float min, float max) => (v <= min) ? min :
            v >= max ? max : v;

        public static int Range(int v, int min, int max) => (v <= min) ? min :
            v >= max ? max : v;
        public static float Tan(float f) => (float) Math.Tan(f);
        public static float Min(float a, float b) { return a < b ? a : b; }
        public static float Max(float a, float b) { return a > b ? a : b; }
        public static float Lerp(float a, float b, float t)
        {
            if (t <= 0)return a;
            if (t >= 1)return b;
            return b * t + (1 - t) * a;
        }
        public static int Lerp(int a, int b, float t)
        {
            if (t <= 0) return a;
            if (t >= 1) return b;
            return (int)(b * t + (1 - t) * a+0.5f);
        }
        public static Point Lerp(Point a, Point b, float t)
        {
            return new Point(Lerp(a.x,b.x,t),Lerp(a.y,b.y,t));
        }
        public static Vector3 RandomCosineDirection()
        {
            var r2 = Random.Get();
            var phi = 2 * PI * Random.Get();
            return new Vector3(Cos(phi) * 2 * Sqrt(r2), Sin(phi) * 2 * Sqrt(r2), Sqrt(1 - r2));
        }
        public static void Swap(ref float a, ref float b)
        {
            var c = a;
            a = b;
            b = c;
        }
        public static float Pow(float f, float p) => (float) Math.Pow(f, p);
        public static float Sin(float f)=>(float) Math.Sin(f);
        public static float Cos(float f)=>(float) Math.Cos(f);
        public static float Asin(float f)=>(float) Math.Asin(f);
        public static float Floor(float f)=>(float) Math.Floor(f);
        public static int Floor2Int(float f)=>(int) Math.Floor(f);
        public static float Abs(float f) => Math.Abs(f);
        public static float Acos(float f)=>(float) Math.Acos(f);
        public static float Atan(float f)=>(float) Math.Atan(f);
        public static float Atan2(float y, float x)=> (float) Math.Atan2(y, x);
        public static float Log(float f)=>(float) Math.Log(f);

        public static float DegreeToRadian(float angle)
        {
            return PI * angle / 180f;
        }


        public static Vector3 GetRelativePosition(Vector3 t, Vector3 zR)
        {
            var rot = zR / 57.29578f;
            float x = t.x, y = t.y, z = t.z;
            float y0 = y, x0 = x, z0 = z;
            //y
            var a = rot.y;
            z0 = z * Cos(a) - x * Sin(a);
            x0 = z * Sin(a) + x * Cos(a);
            y = y0;
            z = z0;
            //x
            a = rot.x;
            y0 = y * Cos(a) - z * Sin(a);
            z0 = y * Sin(a) + z * Cos(a);
            x = x0;
            y = y0;
            //z
            a = rot.z;
            x0 = x * Cos(a) - y * Sin(a);
            y0 = x * Sin(a) + y * Cos(a);
            return new Vector3(x0, y0, z0);
        }
    }
}
