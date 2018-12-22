using AcForm;
using ALight.Render.Mathematics;

namespace ALightRaster.Render
{
    public static class DxHelper
    {
        public static  DxColor DxColor(Color32 c) => new DxColor(Mathf.Range(c.r,0,1), Mathf.Range(c.g, 0, 1), Mathf.Range(c.b, 0, 1));
    }
}
