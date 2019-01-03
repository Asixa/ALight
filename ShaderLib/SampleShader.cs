using System;
using System.Collections.Generic;
using System.Text;
using ShaderLib.Attributes;
using ShaderLib.Base;
using ShaderLib.DataStruture;

namespace ShaderLib
{
    public class VSampleShader:VertexShader
    {
        [Layout(0)][Input]public Vec3<float> aPos=new Vec3<float>();
        [Layout(1)][Input]public Vec3<float> aColor=new Vec3<float>(3);



        [Output] public Vec<float> vertex_color=new Vec<float>(4);
        public override void Main()
        {
            var gl_position=new Vec<float>(4,aPos.X,aPos.Y,aPos.Z,1.0f);
            vertex_color=new Vec<float>(4,0.5f,0.0f,0f,1f);
        }
    }

//    public class SampleInterData : Intermediate
//    {
////        public Vec3<>
////        public override Intermediate Lerp(Intermediate lhs, Intermediate rhs, float fater)
////        {
////            
////        }
//    }
    public class FSampleShader:FragShader
    {
        [Output] public Vec<float> frag_color;
        [Input] public Vec<float> vertex_color;
        public override void Main()
        {
            frag_color = vertex_color;
        }
    }
}
