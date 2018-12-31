using System;
using System.Collections.Generic;
using System.Text;
using ALightRaster.DotNetCore.Render.Mathematics;
using ALightRaster.DotNetCore.Render.Shaders;
using ALightRealtime.Render.Structure;
using Shader= ALightRaster.DotNetCore.Render.Shaders.Shader;

namespace ALightRaster.DotNetCore.Render.Materials
{
    public class Material
    {
        public Shader shader;


        public Material(Shader s, params object[] data)
        {
            shader = s;
            shader.Init(data);
        }

        public Color32 Shade(Vertex p) => shader.Shade(p);
    }
}
