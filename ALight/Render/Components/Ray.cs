using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALight.Render.Mathematics;

namespace ALight.Render.Components
{
    public class Ray
    {
        public readonly Vector3 original;
        public readonly Vector3 direction;
        public readonly Vector3 normalDirection;

        public float time;
        public Ray(Vector3 o, Vector3 d,float t=0)
        {
            time = t;
            original = o;
            direction = d;
            normalDirection = d.Normalized();
        }

        public Vector3 GetPoint(float t) => original + direction * t;
    }
}
