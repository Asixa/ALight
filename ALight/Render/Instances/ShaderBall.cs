using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALight.Render.Components;
using ALight.Render.Materials;

namespace ALight.Render.Instances
{
    public class ShaderBall
    {
        public static Hitable Create(Shader main,Shader inside,Shader buttom)
        {
            var p = ResourceManager.ModelPath + "ShaderBall/";
            var list = new List<Hitable>
            {
                ByteModel.Load(p + "Cushion.ACM", buttom),
                ByteModel.Load(p + "GrayBackground.ACM", inside),
                ByteModel.Load(p + "GrayInlay.ACM", main),
                ByteModel.Load(p + "MaterialBase.ACM", main),
                ByteModel.Load(p + "MaterialBaseInside.ACM", main),
                ByteModel.Load(p + "MaterialSphere.ACM", main)
            };
             return new BVHNode(list.ToArray(),list.Count,0,1);
        }
    }
}
