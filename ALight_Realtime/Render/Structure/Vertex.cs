using ALight.Render.Mathematics;

namespace ALightRealtime.Render.Structure
{
    public class Vertex
    {
        public Vector3 position;
        public Point point;
        public Vector2 uv;
        public Vector3 normal;
        public float depth;
        public Color32 color, light;

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
            point = Point.Lerp(a.point, b.point, t),
            //uv = Vector2.Lerp(a.uv, b.uv, t),
            color = Color32.Lerp(a.color, b.color, t),
        };
    }
}
