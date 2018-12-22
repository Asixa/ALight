struct RayTracer_Shaders_FragmentInput
{
    float4 Position : SV_Position;
    float2 TexCoords : TEXCOORD0;
};


RayTracer_Shaders_FragmentInput VS(uint _builtins_VertexID : SV_VertexID)
{
    uint vertexID = _builtins_VertexID;
    RayTracer_Shaders_FragmentInput output;
    output.TexCoords = float2((vertexID << 1) & 2u, vertexID & 2u);
    output.Position = float4(output.TexCoords.xy * 2.0f - float2(1.0f, 1.0f), 0.0f, 1.0f);
    return output;
}


