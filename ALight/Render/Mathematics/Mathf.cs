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
        public static int Floor2int(float f)=>(int) Math.Floor(f);


        public static float Abs(float f) => (float) Math.Abs(f);
        public static float Acos(float f)=>(float) Math.Acos(f);
        

        public static float Atan(float f)=>(float) Math.Atan(f);
        
        public static float Atan2(float y, float x)=> (float) Math.Atan2(y, x);

        public static float Log(float f)=>(float) Math.Log(f);
        
    }
}
