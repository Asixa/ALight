using ALightRaster.DotNetCore.Render.Mathematics;
using Veldrid;

namespace ALightRaster.DotNetCore.Render
{
    public static class LowLevelHelper
    {
        public static  RgbaFloat LowLevelColor(Color32 c) => new RgbaFloat(MathRaster.Range(c.r,0,1), MathRaster.Range(c.g, 0, 1), MathRaster.Range(c.b, 0, 1), MathRaster.Range(c.a, 0, 1));
    }
}
