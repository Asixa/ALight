
using AcForm;
using ALight.Render.Mathematics;

namespace ALightRealtime.Render
{
    public static  class DXHelper
    {
        public static  DxColor DXColor(Color32 c) => new DxColor(c.r, c.g, c.b);
    }
}
