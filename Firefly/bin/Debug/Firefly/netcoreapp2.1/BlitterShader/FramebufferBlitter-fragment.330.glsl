#version 330 core

struct SamplerDummy { int _dummyValue; };
struct SamplerComparisonDummy { int _dummyValue; };

struct RayTracer_Shaders_FragmentInput
{
    vec4 Position;
    vec2 TexCoords;
};

uniform sampler2D SourceTex;

SamplerDummy SourceSampler = SamplerDummy(0);


vec4 FS( RayTracer_Shaders_FragmentInput input_)
{
    vec4 color = texture(SourceTex, input_.TexCoords);
    color.w = 1;
    return color;
}


in vec2 fsin_0;
out vec4 _outputColor_;

void main()
{
    RayTracer_Shaders_FragmentInput input_;
    input_.Position = gl_FragCoord;
    input_.TexCoords = fsin_0;
    vec4 output_ = FS(input_);
    _outputColor_ = output_;
}
