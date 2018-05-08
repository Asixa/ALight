
namespace ALight.Render.Scanners
{
    public struct ScannerConfig
    {
        public readonly int w, h;
        public readonly int w2, h2;
        public ScannerConfig(int h, int w)
        {
            this.h = h;
            this.w = w;
            w2 = w;
            h2 = w;
        }
        public ScannerConfig(int h, int h2, int w, int w2)
        {
            this.h = h;
            this.w = w;
            this.h2 = h2;
            this.w2 = w2;
        }
    }
}
