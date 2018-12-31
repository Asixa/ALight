using System;
using ALightRaster.DotNetCore.Render.Mathematics;
using Vector2 = System.Numerics.Vector2;
using Vector3 = System.Numerics.Vector3;
using Vector4 = System.Numerics.Vector4;

namespace ALightRealtime.Render.Structure
{
    public struct Vertex
    {
        public Vector4 point;
        public Vector2 uv;
        public Vector3 normal;
        public Color32 color, light;
        public float distance2Cam;
        public float onePerZ;
        
        public Vertex(Vector4 point, Vector2 uv, Vector3 normal, Color32 color)
        {
            this.distance2Cam = -1;
            this.point = point;
            this.point.W = 1;
            this.uv = uv;
            this.normal = normal;
            this.color = color;
            this.light=new Color32();
            onePerZ = 1;
        }

        public static Vertex Lerp(Vertex a, Vertex b, float t) => new Vertex
        {
            uv = Vector2.Lerp(a.uv, b.uv, t),
//
//                vcolor = Lerp(a.vcolor, b.vcolor, t),
//                u = Lerp(a.u, b.u, t),
//                v = Lerp(a.v, b.v, t),
//                point = Lerp(a.point, b.point, t),
//                lightingColor = Lerp(a.lightingColor, b.lightingColor, t)
        };
        public static Vertex FastLerp(Vertex a, Vertex b, float t) => new Vertex
        {
            point = Vector4.Lerp(a.point, b.point, t),
            onePerZ = MathRaster.Lerp(a.onePerZ,b.onePerZ,t),
            uv = Vector2.Lerp(a.uv, b.uv, t),
            normal = Vector3.Lerp(a.normal,b.normal,t),
            color = Color32.Lerp(a.color, b.color, t),
            distance2Cam = MathRaster.Lerp(a.distance2Cam,b.distance2Cam,t)
        };
    }
}
