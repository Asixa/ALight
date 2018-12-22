using ShaderGen;
using System.Numerics;

[assembly: ShaderSet("FramebufferBlitter", "RayTracer.Shaders.FramebufferBlitter.VS", "RayTracer.Shaders.FramebufferBlitter.FS")]

namespace RayTracer.Shaders
{
    public class FramebufferBlitter
    {
        [VertexShader]
        public FragmentInput VS()
        {
            uint vertexID = ShaderBuiltins.VertexID;
            FragmentInput output;
            output.TexCoords = new Vector2((vertexID << 1) & 2u, vertexID & 2u);
            output.Position = new Vector4(output.TexCoords.XY() * 2.0f - new Vector2(1.0f), 0.0f, 1.0f);
            return output;
        }

        public Texture2DResource SourceTex;
        public SamplerResource SourceSampler;

        [FragmentShader]
        public Vector4 FS(FragmentInput input)
        {
            Vector4 color = ShaderBuiltins.Sample(SourceTex, SourceSampler, input.TexCoords);
            color.W = 1;
            return color;
        }
    }

    public struct FragmentInput
    {
        [SystemPositionSemantic]
        public Vector4 Position;
        [TextureCoordinateSemantic]
        public Vector2 TexCoords;
    }
}
