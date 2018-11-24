using System;
using System.Linq;
using ALight.Render.Mathematics;
using ALightRealtime;
using ALightRealtime.Render;
using ALightRealtime.Render.Structure;

namespace ALightRaster.Render
{
    public class Canvas
    {
        public static int Width = 512,Height =512;
        public byte[] buffer;

        public void Render()
        {
            var vertices = new[]
            {
                new Vertex {point = new Point(100, 512-100), color = Color32.Red},
                new Vertex {point = new Point(100, 100), color = Color32.Green},
                new Vertex {point = new Point(512-100, 512-100), color = Color32.Green}
            }.ToList();
            vertices.Sort((x, y) => x.point.y < y.point.y ? -1 : 1);
            DrawTriangle(vertices.ToArray());
            var vertices2 = new[]
            {
                new Vertex {point = new Point(512-100, 100), color = Color32.Blue},
                new Vertex {point = new Point(100, 100), color = Color32.Green},
                new Vertex {point = new Point(512-100, 512 - 100), color = Color32.Green}
            }.ToList();
            vertices2.Sort((x, y) => x.point.y < y.point.y ? -1 : 1);
            DrawTriangle(vertices2.ToArray());
        }


        public void DrawMesh(Mesh mesh, Vector3 position)
        {

        }


        //Please Sort Before Call : vertices.ToList().Sort((x,y)=>x.point.y<y.point.y?1:0);
        public void DrawTriangle(Vertex[] vertices)
        {
            if (vertices[1].point.y == vertices[2].point.y) FillTriangle(vertices[0], null, vertices[1], vertices[2], 1);  // BottomFlat
            else if (vertices[0].point.y == vertices[1].point.y) FillTriangle(null, vertices[2], vertices[0], vertices[1], 2); // TopFlat
            else
            {
                var middle_length =(vertices[1].point.y - vertices[0].point.y) * (vertices[2].point.x - vertices[0].point.x) / (vertices[2].point.y - vertices[0].point.y) +
                    vertices[0].point.x;
                var t = (vertices[0].point.y - vertices[1].point.y)*1f / (vertices[0].point.y - vertices[2].point.y);
                var middle = Vertex.FastLerp(vertices[0], vertices[2],t);
                FillTriangle(vertices[0],vertices[2], middle_length > vertices[1].point.x ? vertices[1] : middle, middle_length < vertices[1].point.x ? vertices[1] : middle,0);
            }
        }

        public void FillTriangle(Vertex top,Vertex down, Vertex left, Vertex right,int mode)
        {
            if (left.point.x > right.point.x)
            {
                var t = left;
                left = right;
                right = t;
            }
            switch (mode)
            {
                case 0:
                case 1: //Bottom Flat
                    var dxy_left = (left.point.x - top.point.x)*1f/(left.point.y - top.point.y);
                    var dxy_right = (right.point.x - top.point.x) * 1f / (right.point.y - top.point.y);
                    float xs = top.point.x, xe = top.point.x;
                    for (var y = top.point.y; y <= right.point.y; y++)
                    {
                        var v = (xe - xs == 0) ? 0 : (xe - xs) / (right.point.x - left.point.x);
                        xs += dxy_left;
                        xe += dxy_right;
                        ScanLine(Vertex.FastLerp(top, left, v), Vertex.FastLerp(top, right, v), y, 1);
                    }
                    if (mode == 0) goto case 2;
                    break;
                case 2: //Top Flat
                    dxy_left = (down.point.x - left.point.x) * 1f / (down.point.y - left.point.y);
                    dxy_right = (down.point.x - right.point.x) * 1f / (down.point.y - right.point.y);
                    xs = left.point.x;
                    xe = right.point.x;
                    for (var y = left.point.y; y <= down.point.y; y++)
                    {
                        var v = (xe - xs == 0) ? 0 : (xe - xs) / (right.point.x - left.point.x);
                        ScanLine(Vertex.FastLerp(down, left, v), Vertex.FastLerp(down, right, v),y, 1);
                        xs += dxy_left;
                        xe += dxy_right;
                    }
                    break;
            }
        }

        public void ScanLine(Vertex left, Vertex right, int y,int mode)
        {
            float dx = Math.Abs(left.point.x - right.point.x);
            for (var x = left.point.x; x <= right.point.x; x++)
            {
                if (x >= Width || y >= Height || x <= 0 || y <= 0)continue;
                var factor = Math.Abs((x - left.point.x + 1) / (dx + 1));
                var p = Vertex.FastLerp(left, right, factor);
                switch (mode)
                {
                    case 1: PreviewWindow.main.SetPixel(x, y, DXHelper.DXColor(p.color));break;
                    //case 2: Program.main.SetPixel(x, y, (new Color32(ReadTexture(u, v, currentMaterial.texture)) * p.light).ToDX());break;
                }
            }
        }

    }
}
