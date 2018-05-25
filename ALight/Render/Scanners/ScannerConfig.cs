
namespace ALight.Render.Scanners
{
    public struct ScannerConfig
    {
        public readonly int w, h;
        public readonly int w2, h2;
        public readonly int ID;
        public ScannerConfig(int h, int w, int id)
        {
            this.h = h;
            this.w = w;
            w2 = w;
            h2 = w;
            ID = id;
        }
        public ScannerConfig(int h, int h2, int w, int w2,int id)
        {
            this.h = h;
            this.w = w;
            this.h2 = h2;
            this.w2 = w2;
            ID = id;
        }
    }
}
