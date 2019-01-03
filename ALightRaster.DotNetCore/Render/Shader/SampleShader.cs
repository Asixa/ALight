
using System;
using System.Numerics;
using ALightRaster.DotNetCore.Render.Materials;
using ALightRaster.DotNetCore.Render.Mathematics;
using ALightRaster.Render;
using ALightRaster.Render.Components;
using ALightRealtime.Render.Structure;
using ShaderLib.Base;
namespace ShaderLib
{
    public class VSampleShader:VertexShader
    {
        public override Intermediate Main(Vertex vertex,out Vector4 position)
        {
            var fragPos = vertex.point.Times(M);
            position =  fragPos.Times(Canvas.V).Times(Canvas.P);
            return new SampleInterData
            {
                z_buff = 1/position.W,
                UV = vertex.uv,
                FragPos = fragPos.V3(),
                Normal = vertex.normal.V4().Times(tiM).V3()
            };
                 
        }
    }

    public class SampleInterData : Intermediate
    {
        public Vector3 FragPos,Normal;
        public Vector2 UV;

        public override Intermediate Lerp(Intermediate rhs, float facter)
        {
            var right = (SampleInterData) rhs;
            return new SampleInterData
            {
                z_buff = MathRaster.Lerp(z_buff,right.z_buff,facter),
                UV = Vector2.Lerp(UV,right.UV,facter),
                FragPos = Vector3.Lerp(FragPos,right.FragPos,facter),
                Normal = Vector3.Lerp(Normal,right.Normal,facter)
            };
        }
    }
    public class FSampleShader:FragShader
    {
        private Vector3 lightColor, viewPos, lightPos, objectColor;
        public ImageTexture image;
        public override Vector4 Main(Intermediate d)
        {
            var data = (SampleInterData)d;
            lightPos = Light.main.transform.position;
            lightColor = Light.main.color.to_vector4().V3();
            viewPos = Camera.main.transform.position;
            objectColor=new Vector3(0,1,1);

            objectColor = image.Value(data.UV.X, data.UV.Y, Vector3.One).to_vector4().V3();

       
            float ambientStrength = 0.1f;
            var ambient = ambientStrength * lightColor;

            // diffuse 
            //var norm =new Vector3(0,1,0);
            var norm = Vector3.Normalize(data.Normal);
            var lightDir = Vector3.Normalize(lightPos - data.FragPos); //Console.WriteLine(norm);
            var diff = MathF.Max(Vector3.Dot(norm, lightDir), 0.0f);
            var diffuse = diff * lightColor;

            // specular
            float specularStrength = 0.1f;
            var viewDir = Vector3.Normalize(viewPos - data.FragPos);
            var reflectDir = Vector3.Reflect(-lightDir, norm);
            float spec = MathF.Pow(MathF.Max(Vector3.Dot(viewDir, reflectDir), 0f), 32);
            var specular = specularStrength * spec * lightColor;

            var result = (ambient + diffuse + specular) * objectColor;
            return result.V4();
        }
    }
}
