using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ALight
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            
            //GCSettings.LatencyMode = GCLatencyMode.NoGCRegion;

            GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
           // GC.TryStartNoGCRegion(10000000);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

        }
    }
}
