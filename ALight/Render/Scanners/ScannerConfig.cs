using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALight.Render.Scanners
{
    public class ScannerConfig
    {
        public readonly int w, h;
        public readonly int w2, h2;
        public ScannerConfig(int h, int w)
        {
            this.h = h;
            this.w = w;
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
