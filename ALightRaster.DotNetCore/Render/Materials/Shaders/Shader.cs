using System;
using System.Collections.Generic;
using System.Text;
using ALightRaster.DotNetCore.Render.Mathematics;
using ALightRealtime.Render.Structure;

namespace ALightRaster.DotNetCore.Render.Shaders
{
    public class Shader
    {
        public virtual Color32 Shade(Vertex p)
        {
            return Color32.Black;
        }
        public virtual void Init(object[] data)
        {
        }
    }
}
