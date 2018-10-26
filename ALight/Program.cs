using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Threading.Tasks;
using System.Windows.Forms;
using ALight.Render.Components;
using ALight.Render.Mathematics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

            //var json =(JObject)JsonConvert.DeserializeObject(File.ReadAllText("Render/Scene.json"));
            //var camera = (JObject)JsonConvert.DeserializeObject(json["Camera"].ToString());
            //var lookFrom = JsonConvert.DeserializeObject<List<float>>(camera["lookFrom"].ToString());;
            //Console.WriteLine(lookFrom[0]);


            //Console.ReadLine();

        }
    }
}
