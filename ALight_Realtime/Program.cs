using System;
using System.IO;
using AcForm;
using ALight.Render.Mathematics;
using ALightRaster.Render;
using ALightRealtime.Render;
using SharpDX;
using SharpDX.DirectInput;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
namespace ALightRealtime
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)=>new PreviewWindow().Run(new DxConfiguration("ALightRaster", 512, 512));
    }

    
    public class PreviewWindow : DxWindow
    {
        public static PreviewWindow main;
        public Canvas canvas;
     
        public override void Start()
        {
            main = this;
            backgroundColor = new DxColor(71 / 255f, 71 / 255f, 71 / 255f);
            canvas = new Canvas();
        }

        public override void Update()
        {
            canvas.Render();
            if(Input.GetKey(Key.LeftControl) &&Input.GetKeyDown(Key.S))
            {
                
            }
        }

//        public void Save(string path = "Result" + ".png")
//        {
//            if (!Directory.Exists("Output")) Directory.CreateDirectory("Output");
//            
//            int Get(int i) => (byte)Mathf.Range(buff[i] * 255 / main.Changes[i / 4] + 0.5f, 0, 255f);
//            var pic = new Bitmap(512,512, PixelFormat.Format32bppArgb);
//            for (var i = 0; i < main.buff.Length; i += 4)
//            {
//                var c = Color.FromArgb(Get(i + 3), Get(i + 2), Get(i + 1), Get(i));
//                pic.SetPixel(i % (width * 4) / 4, i / (width * 4), c);
//            }
//
//            pic.Save("Output/" + path);
//            MessageBox.Show("Output/" + path, "保存完成");
//        }
    }
}
