using System;
using System.Numerics;
using ALightRaster.DotNetCore.Render.Mathematics;
using ALightRealtime.Render.Structure;

namespace ALightRaster.Render.Structure
{
    public class Mesh
    {
        public Vertex[] vertices;
        public Mesh(Model model)
        {
            vertices = new Vertex[model.indexs.Count];
            for (var i = 0; i < model.indexs.Count; i++)
            {
                var index = model.indexs[i];
                var point = model.points[index];
                var c = model.vertColors[index];
                vertices[i] = new Vertex(new Vector4(point.X,point.Y,point.Z,0), model.uvs[i], model.norlmas[i],new Color32(c.X,c.Y,c.Z));
            }
        }
    }
}
