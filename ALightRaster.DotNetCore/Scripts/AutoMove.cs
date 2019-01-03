using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using ALightRaster.Engine;
using ALightRaster.Render.Components;

namespace ALightRaster.DotNetCore.Scripts
{
    public class AutoMove:Component
    {
        private Vector3 v;
        public AutoMove(Vector3 a) => v = a;
        private int dir = 1;
        public override void Update()
        {
            if ((transform.position.X >= 10&&dir==1)|| (transform.position.X <= 0&&dir==-1))
            {
                dir *= -1;
            }
            //Console.WriteLine(transform.position);
            transform.position+=dir*v*Time.deltatime;
        }
    }
}
