using System.IO;

namespace ALight.Render.Mathematics
{
    public class Sobol
    {
        private int now = 0;

        private int Next()
        {
            if (++now == int.MaxValue) now = 0;
            return now;
        }

        private static float IntegerRadicalInverse(int Base, int i)
        {
            int inverse;
            var numPoints = 1;
            for (inverse = 0; i > 0; i /= Base)
            {
                inverse = inverse * Base + (i % Base);
                numPoints = numPoints * Base;
            }
            return inverse / (float) numPoints;
        }

        public float Get() => IntegerRadicalInverse(2, Next());

    }

    public static class Random
    {
        private static readonly System.Random random = new System.Random();
        private static readonly Sobol sobol=new Sobol();

        static long seed = 1;
        public static float Get()
        {
            seed = (0x5DEECE66DL * seed + 0xB16) & 0xFFFFFFFFFFFFL;
            return (seed >> 16) / (float) 0x100000000L;
        }

        //return sobol.Get();
            //lock (random)return random.Next((int)(f * 10000), (int)(t * 10000)) / 10000f;
        }

        //public static float Get(float f, float t)=>new System.Random(Guid.NewGuid().GetHashCode()).Next((int) (f * 10000), (int) (t * 10000)) / 10000f;
    }
    


