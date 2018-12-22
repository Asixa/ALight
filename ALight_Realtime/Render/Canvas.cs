using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using ALightRaster.Render.Components;
using ALightRaster.Render.Mathematics;
using ALightRealtime;
using ALightRealtime.Render;
using ALightRealtime.Render.Structure;
using Vector2 = System.Numerics.Vector2;
using Vector3 =System.Numerics.Vector3;
using static ALightRaster.Render.Mathematics.MathRaster;
namespace ALightRaster.Render
{
    public class Canvas
    {
        public const bool Debug = false;
        public static Canvas instance;
        public static int Width = 800, Height = 600;
        public Matrix4x4 V => Camera.V;
        public Matrix4x4 P => Camera.P;
        private static float[,] zBuff;
        public static void Init()
        {
            zBuff=new float[Height,Width];
            instance = new Canvas();
        }

        public void Render()
        {
            Camera.Caculate();
            Array.Clear(zBuff, 0, zBuff.Length);
            foreach (var obj in Scene.current.gameObjects)
            {
                if (obj.renderer == null) continue;
                var m = obj.transform.CaculateMatrix();
                var vertices = obj.renderer.mesh_filter.mesh.vertices;
                for (var index = 0; index < vertices.Length; index += 3)
                {
                    TrianglePipline(vertices[index], vertices[index + 1], vertices[index + 2], m);
                }
            }


//            Parallel.For(0, Scene.current.gameObjects.Count, i =>
//            {
//                var obj = Scene.current.gameObjects[i];
//                if (obj.renderer == null) return;
//                var m = obj.transform.CaculateMatrix();
//                var vertices = obj.renderer.mesh_filter.mesh.vertices;
//                for (var index = 0; index < vertices.Length; index += 3)
//                {
//                    TrianglePipline(vertices[index], vertices[index + 1], vertices[index + 2], m);
//                }
//            });
        }


        public void TrianglePipline(Vertex v1, Vertex v2, Vertex v3, Matrix4x4 m)
        {
            //-------------------Geometry--------------------------
            // MV,Model-View, Camera View coordinate
            SetMvTransform(m, V, ref v1);
            SetMvTransform(m, V, ref v2);
            SetMvTransform(m, V, ref v3);
            //Culling
            if (!BackFaceCulling(v1, v2, v3)) return;
            //P, Homogenious coordinate
            SetProjectionTransform(P, ref v1);
            SetProjectionTransform(P, ref v2);
            SetProjectionTransform(P, ref v3);
            //Clipping
            if (Clip(v1) == false || Clip(v2) == false || Clip(v3) == false) return;
            //To screen coordinate
            TransformToScreen(ref v1);
            TransformToScreen(ref v2);
            TransformToScreen(ref v3);
            //-------------------Rasterization--------------------------
            var vertices = new[] {v1, v2, v3}.ToList();
            vertices.Sort((x, y) => x.point.Y < y.point.Y ? -1 : 1);
            DrawTriangle(vertices.ToArray());
        }

        #region Pipeline

        private static void SetMvTransform(Matrix4x4 m, Matrix4x4 v, ref Vertex vertex) =>vertex.point = VxM(VxM(vertex.point, m), v);

        public void SetProjectionTransform(Matrix4x4 p, ref Vertex vertex)
        {
            vertex.point = VxM(vertex.point, p);
            vertex.onePerZ = 1 / vertex.point.W;
            vertex.uv.X *= vertex.onePerZ;
            vertex.uv.Y *= vertex.onePerZ;
            // vertex.color *= vertex.onePerZ;
            vertex.light *= vertex.onePerZ;
        }

        private static void TransformToScreen(ref Vertex v)
        {
            if (v.point.W == 0) return;
            //to cvv
            v.point.X *= 1 / v.point.W;
            v.point.Y *= 1 / v.point.W;
            v.point.Z *= 1 / v.point.W;
            v.point.W = 1;
            //cvv to Screen
            v.point.X = (v.point.X + 1) * 0.5f * Width;
            v.point.Y = (1 - v.point.Y) * 0.5f * Height;
        }

        private static bool BackFaceCulling(Vertex p1, Vertex p2, Vertex p3)
        {
            var v1 = p2.point - p1.point;
            var v2 = p3.point - p2.point;
            var normal = Vector3.Cross(MathRaster.V4ToV3(v1), MathRaster.V4ToV3(v2));
            var view_dir = MathRaster.V4ToV3(p1.point) - new Vector3(0, 0, 0);
            return Vector3.Dot(normal, view_dir) > 0;
        }

        

        private static bool Clip(Vertex v) => v.point.X >= -v.point.W && v.point.X <= v.point.W &&
                                       v.point.Y >= -v.point.W && v.point.Y <= v.point.W &&
                                       v.point.Z >= 0f && v.point.Z <= v.point.W;
        #endregion
        #region Rasterization

        //Please Sort Before Call : vertices.ToList().Sort((x,y)=>x.point.y<y.point.y?1:0);
        public void DrawTriangle(Vertex[] vertices)
        {
            if (vertices[1].point.Y == vertices[2].point.Y)
                FillTriangle(vertices[0], vertices[0] /*NULL*/, vertices[1], vertices[2], 1); // BottomFlat
            else if (vertices[0].point.Y == vertices[1].point.Y)
                FillTriangle(vertices[0] /*NULL*/, vertices[2], vertices[0], vertices[1], 2); // TopFlat
            else
            {
                var middle_length = (vertices[1].point.Y - vertices[0].point.Y) *
                                    (vertices[2].point.X - vertices[0].point.X) /
                                    (vertices[2].point.Y - vertices[0].point.Y) +
                                    vertices[0].point.X;
                var t = (vertices[0].point.Y - vertices[1].point.Y) * 1f / (vertices[0].point.Y - vertices[2].point.Y);
                var middle = Vertex.FastLerp(vertices[0], vertices[2], t);
                FillTriangle(vertices[0], vertices[2], middle_length > vertices[1].point.X ? vertices[1] : middle,
                    middle_length < vertices[1].point.X ? vertices[1] : middle, 0);
            }
        }

        public void FillTriangle(Vertex top, Vertex down, Vertex left, Vertex right, int mode)
        {

            if (left.point.X > right.point.X)
            {
                var t = left;
                left = right;
                right = t;
            }

            switch (mode)
            {
                case 0:
                case 1: //Bottom Flat
                    var dxy_left = (left.point.X - top.point.X) * 1f / (left.point.Y - top.point.Y);
                    var dxy_right = (right.point.X - top.point.X) * 1f / (right.point.Y - top.point.Y);
                    float xs = top.point.X, xe = top.point.X;
                    for (var y = top.point.Y; y <= right.point.Y; y++)
                    {
                        var v = (xe - xs == 0) ? 0 : (xe - xs) / (right.point.X - left.point.X);
                        xs += dxy_left;
                        xe += dxy_right;

                        ScanLine(Vertex.FastLerp(top, left, v), Vertex.FastLerp(top, right, v), (int) y, 1);
                    }

                    if (mode == 0) goto case 2;
                    break;
                case 2: //Top Flat
                    dxy_left = (down.point.X - left.point.X) * 1f / (down.point.Y - left.point.Y);
                    dxy_right = (down.point.X - right.point.X) * 1f / (down.point.Y - right.point.Y);
                    xs = left.point.X;
                    xe = right.point.X;
                    for (var y = left.point.Y; y <= down.point.Y; y++)
                    {
                        var v = (xe - xs == 0) ? 0 : (xe - xs) / (right.point.X - left.point.X);
                        ScanLine(Vertex.FastLerp(down, left, v), Vertex.FastLerp(down, right, v), (int) y, 1);
                        xs += dxy_left;
                        xe += dxy_right;
                    }

                    break;
            }
        }

        public void ScanLine(Vertex left, Vertex right, int y, int mode)
        {
            var dx = Math.Abs(left.point.X - right.point.X);
            for (var x = left.point.X; x <= right.point.X; x++)
            {
                if (x >= Width || y >= Height || x <= 0 || y <= 0) continue;
                
                var factor = Math.Abs((x - left.point.X + 1) / (dx + 1));

                var p = Vertex.FastLerp(left, right, factor);
                if (p.onePerZ < zBuff[y, (int)x])continue;
                zBuff[y, (int)x] = p.onePerZ;
                switch (mode)
                {
                    case 1:
                        PreviewWindow.main.SetPixel((int) x, y, DxHelper.DxColor(p.color));
                        break;
                    //case 2: Program.main.SetPixel(x, y, (new Color32(ReadTexture(u, v, currentMaterial.texture)) * p.light).ToDX());break;
                }
            }
        }

        #endregion
    }
}
