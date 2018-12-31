using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using ALightRaster.DotNetCore.Render.Mathematics;
using ALightRaster.DotNetCore.Render.Shaders;
using ALightRealtime.Render.Structure;

namespace ALightRaster.DotNetCore.Render.Materials.Shaders
{
    class Unlit
    {
        public class Texture : Shader
        {
            public ImageTexture texture;
            public override Color32 Shade(Vertex p)
            {

                var d = (p.distance2Cam / 2f);
                var d1 = (int)Math.Floor(d);
                var d2 = (int)Math.Ceiling(d);
                if (d2 > 8) d2 = 8;
                if (d1 > 8) d1 = 8;

                return Color32.Lerp(texture.midmaps[d1].Value(p.uv.X, p.uv.Y, Vector3.One), texture.midmaps[d2].Value(p.uv.X, p.uv.Y, Vector3.One), d - d1);
            }
            public override void Init(object[] data)
            {
                texture = (ImageTexture)data[0];
            }
        }
        public class RawTexture : Shader
        {
            public ImageTexture texture;
            public override Color32 Shade(Vertex p) => texture.Value(p.uv.X, p.uv.Y, Vector3.One);
            public override void Init(object[] data)
            {
                texture = (ImageTexture)data[0];
            }
        }
        public class Color : Shader
        {
            public Color32 color;
            public override Color32 Shade(Vertex p) => color;
            public override void Init(object[] data)
            {
                color = (Color32)data[0];
            }
        }
    }
}
