using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using ALightRaster.DotNetCore.Render.Mathematics;
using ALightRaster.DotNetCore.Render.Shaders;
using ALightRealtime.Render.Structure;
using ShaderGen;

namespace ALightRaster.DotNetCore.Render.Materials.Shaders
{
    class Lighting:Shader
    {
        public float specular_strength = 0.5f;
        public Vector4 ambient, object_color;

        //Update
        public Matrix4x4 M;
        public Vector3 frag_pos, light_pos, view_pos;
        public Vector4 light_color;

        public override Color32 Shade(Vertex p)
        {
            return Color32.Red;
//            p.normal=new Vector3(0,1,0);
//            //Console.WriteLine(p.normal);
//            frag_pos=
//            var normal = new Vector4(0, 1, 0,1);//p.normal.V4().Times(M);
//            var norm = Vector4.Normalize(normal);
//            var light_dir = Vector3.Normalize(light_pos - frag_pos);
//            var diff = MathF.Max(Vector4.Dot(norm, light_dir.V4()), 0);
//            var diffuse = diff * light_color;
//
//            var view_dir = Vector3.Normalize(view_pos - frag_pos);
//            var reflect_dir = Vector3.Reflect(-light_dir, norm.V3());
//
//            var spec = MathF.Pow(MathF.Max(Vector3.Dot(view_dir, reflect_dir), 0.0f), 32);
//
//            var specular = specular_strength * spec * light_color;
//            return new Color32((ambient+diffuse + specular) * object_color);
        }
    }
}
