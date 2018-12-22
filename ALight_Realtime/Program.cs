using System;
using AcForm;
using ALightRealtime.Render;
using SharpDX.DirectInput;
using ALightRaster.Engine;

namespace ALightRealtime
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)=>new PreviewWindow().Run(new DxConfiguration("ALightRaster", 800, 600));
    }

    
    public class PreviewWindow : DxWindow
    {
        public static PreviewWindow main;
     
        public override void Start()
        {
            main = this;
            backgroundColor = new DxColor(71 / 255f, 71 / 255f, 71 / 255f);

            Scene.Init();
            ALightRaster.Render.Canvas.Init();
            Scene.current.StartScripts();
            
        }

        public override void Update()
        {
            form.Text = "ALightRaster FPS:" + FramePerSecond;
            ALightRaster.Render.Canvas.instance.Render();
            
            Time.deltatime = FrameDelta;
            Scene.current.UpdateScripts();
            if(Input.GetKey(Key.LeftControl) &&Input.GetKeyDown(Key.S))
            {
                
            }
        }
    }
}
