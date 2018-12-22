#include <metal_stdlib>
using namespace metal;
struct RayTracer_Shaders_FragmentInput
{
    float4 Position [[ position ]];
    float2 TexCoords [[ attribute(0) ]];
};

struct ShaderContainer {
thread texture2d<float> SourceTex;
thread sampler SourceSampler;

ShaderContainer(
thread texture2d<float> SourceTex_param, thread sampler SourceSampler_param
)
:
SourceTex(SourceTex_param), SourceSampler(SourceSampler_param)
{}
float4 FS( RayTracer_Shaders_FragmentInput input)
{
    float4 color = SourceTex.sample(SourceSampler, input.TexCoords);
    color[3] = 1;
    return color;
}


};

fragment float4 FS(RayTracer_Shaders_FragmentInput input [[ stage_in ]], texture2d<float> SourceTex [[ texture(0) ]], sampler SourceSampler [[ sampler(0) ]])
{
return ShaderContainer(SourceTex, SourceSampler).FS(input);
}
