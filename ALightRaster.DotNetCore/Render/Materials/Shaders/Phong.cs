using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using ALightRaster.DotNetCore.Render.Mathematics;
using ALightRaster.DotNetCore.Render.Shaders;
using ALightRaster.Render;
using ALightRaster.Render.Components;
using ALightRealtime.Render;
using ALightRealtime.Render.Structure;

namespace ALightRaster.DotNetCore.Render.Materials.Shaders
{
    public class Phong:Shader
    {
        public Color32 color;
       
        public override Color32 Shade(Vertex p)
        {
           Console.WriteLine(p.normal);

            var diff = MathF.Max(Vector3.Dot(p.normal, Scene.Sun.transform.rotation), 0);
            //Console.WriteLine(diff);
            var diffuse = Scene.Sun.color * diff*color;
            return diffuse;
        }

        public override void Init(object[] data)
        {
            color = (Color32) data[0];
        }
    }
}
