using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using ALight.Render.Mathematics;

namespace ALight.Render.Components
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Ray
    {
        public readonly Vector3 origin;
        public readonly Vector3 direction;
        public readonly Vector3 normal_direction;
        public readonly float time;
        public readonly int id;
        public Ray(Vector3 o, Vector3 d,float t=0,int ID=0)
        {
            time = t;
            origin = o;
            direction = d;
            normal_direction = d.Normalized();
            id = ID;
        }

        public Vector3 GetPoint(float t) => origin + direction * t;
    }
}
