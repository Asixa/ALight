using System;
using System.Numerics;
using ALightRaster.Engine;
using ALightRaster.Render.Components;

namespace ALightRaster.Scripts
{
    public class AutoRotate:Component
    {
        private readonly float value;
        public AutoRotate(float v) => value = v;
        public override void Update()
        {

            transform.rotation+=new Vector3(0, value * Time.deltatime, 0);
        }
    }
}
