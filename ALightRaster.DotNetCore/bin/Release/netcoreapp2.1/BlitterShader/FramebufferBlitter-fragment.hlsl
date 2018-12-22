struct RayTracer_Shaders_FragmentInput
{
    float4 Position : SV_Position;
    float2 TexCoords : TEXCOORD0;
};

Texture2D SourceTex : register(t0);

SamplerState SourceSampler : register(s0);


float4 FS( RayTracer_Shaders_FragmentInput input) : SV_Target
{
    float4 color = SourceTex.Sample(SourceSampler, input.TexCoords);
    color.w = 1;
    return color;
}


