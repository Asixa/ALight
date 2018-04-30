using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALight.Render.Math
{
    public class Mathf
    {
        public static float Sqrt(float v) => (float) System.Math.Sqrt(Convert.ToDouble(v));

        public static float Range(float v, float min, float max) => (v <= min) ? min :
            v >= max ? max : v;

        public static float Tan(float f) => (float) System.Math.Tan((double) f);

        public static float Pow(float f, float p) => (float) System.Math.Pow(f, p);
        public const float PI = 3.14159274f;
    }
}
