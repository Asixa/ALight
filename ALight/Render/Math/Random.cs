using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALight.Render.Math
{
    public static class Random
    {
        public static float Range(float f, float t)
        {
            var random = new System.Random(Guid.NewGuid().GetHashCode());
            return random.Next((int) (f * 10000), (int) (t * 10000)) / 10000f;
        }
    }
}
