
using System.Runtime.InteropServices;
using ALight.Render.Mathematics;

namespace ALight.Render.Scanners
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ScannerConfig
    {
        public readonly int w, h;
        public readonly int ID;
        public readonly Point point;
        public ScannerConfig(int w, int h, int id,Point p)
        {
            this.h = h;
            this.w = w;
            ID = id;
            point = p;
        }
    }
}
