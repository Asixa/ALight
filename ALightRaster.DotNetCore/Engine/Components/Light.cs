using ALightRaster.DotNetCore.Render.Mathematics;

namespace ALightRaster.Render.Components
{
    public enum LightType { Point,Directional,Spot}
    public class Light:Component
    {
        public static Light main;
        public Color32 color;
        public float intensity;
        public LightType type;

        public Light() => main = this;
    }
}
