using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALight.Render
{
    public class Configuration
    {
        public static Configuration main;
        public static int width=512, height=512;
        public static int SPP=2000, MAX_SCATTER_TIME=4;
        public static int divide = 8;
        public static  int SamplePerThread = 8;
        //public int Samples = 2000,MAX_SCATTER_TIME = 4;
        //
        //public int width =512, height =512;
        //
    }
}
