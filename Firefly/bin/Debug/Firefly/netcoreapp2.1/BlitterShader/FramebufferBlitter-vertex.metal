#include <metal_stdlib>
using namespace metal;
struct RayTracer_Shaders_FragmentInput
{
    float4 Position [[ position ]];
    float2 TexCoords [[ attribute(0) ]];
};

struct ShaderContainer {

ShaderContainer(

)
{}
RayTracer_Shaders_FragmentInput VS(uint _builtins_VertexID)
{
    uint vertexID = _builtins_VertexID;
    RayTracer_Shaders_FragmentInput output;
    output.TexCoords = float2((vertexID << 1) & 2u, vertexID & 2u);
    output.Position = float4(float2(output.TexCoords).xy * 2.0f - float2(1.0f, 1.0f), 0.0f, 1.0f);
    return output;
}


};

vertex RayTracer_Shaders_FragmentInput VS(uint _builtins_VertexID [[ vertex_id ]])
{
return ShaderContainer().VS(_builtins_VertexID);
}
