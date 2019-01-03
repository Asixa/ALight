using System.Numerics;
using ALightRaster.DotNetCore.Render.Mathematics;
using ALightRealtime.Render.Structure;
using ShaderLib.Base;
using Shader= ALightRaster.DotNetCore.Render.Shaders.Shader;
// ReSharper disable InconsistentNaming
namespace ALightRaster.DotNetCore.Render.Materials
{
    public class Material
    {
       
        public VertexShader VS;
        public FragShader FS;

        public Shader shader;


        public Material(Shader s, params object[] data)
        {
            shader = s;
            shader.Init(data);
        }
        public Material(VertexShader vs, FragShader fs)
        {
            FS = fs;
            VS = vs;
        }
        public void SetM(Matrix4x4 m,Matrix4x4 tim)
        {
            VS.M = m;
            VS.tiM = tim;
        }
        public Color32 Shade(Vertex p) => shader.Shade(p);
    }
}
