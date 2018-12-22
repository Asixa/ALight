#version 450
#extension GL_ARB_separate_shader_objects : enable
#extension GL_ARB_shading_language_420pack : enable
struct RayTracer_Shaders_FragmentInput
{
    vec4 Position;
    vec2 TexCoords;
};


RayTracer_Shaders_FragmentInput VS()
{
    uint vertexID = gl_VertexIndex;
    RayTracer_Shaders_FragmentInput output_;
    output_.TexCoords = vec2((vertexID << 1) & 2u, vertexID & 2u);
    output_.Position = vec4(output_.TexCoords.xy * 2.0f - vec2(1.0f, 1.0f), 0.0f, 1.0f);
    return output_;
}


layout(location = 0) out vec2 fsin_0;

void main()
{
    RayTracer_Shaders_FragmentInput output_ = VS();
    fsin_0 = output_.TexCoords;
    gl_Position = output_.Position;
        gl_Position.y = -gl_Position.y; // Correct for Vulkan clip coordinates
}
