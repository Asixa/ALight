using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using ALightRaster.DotNetCore.Render.Mathematics;
using ALightRaster.DotNetCore.Render.Shaders;
using ALightRealtime.Render.Structure;

namespace ALightRaster.DotNetCore.Render.Materials.Shaders
{
    public class Gouraud:Shader
    {
        public override Color32 Shade(Vertex p) => p.color;
    }

    public class Gouraud_Image : Shader
    {
        public ImageTexture texture;

        public override Color32 Shade(Vertex p)
        {
  
            var d = (p.distance2Cam/2f);
            var d1 = (int)Math.Floor(d);
            var d2 = (int)Math.Ceiling(d);
            if (d2 > 8) d2 = 8;
            if (d1 > 8) d1 = 8;

            var gray = MathRaster.Lerp(d1 / 9f, d2 / 9f, d - d1);
            var c= Color32.Lerp(texture.midmaps[d1].Value(p.uv.X, p.uv.Y, Vector3.One), texture.midmaps[d2].Value(p.uv.X, p.uv.Y, Vector3.One), d - d1);
            
            return new Color32(0, gray, 0, 1)*c;
            return Color32.Lerp(texture.midmaps[d1].Value(p.uv.X, p.uv.Y, Vector3.One), texture.midmaps[d2].Value(p.uv.X, p.uv.Y, Vector3.One),d-d1);
        } 
        public override void Init(object[] data)
        {
            texture = (ImageTexture)data[0];

        }
    }
}
