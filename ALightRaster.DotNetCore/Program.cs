using System;
using AcFormCore;
using ALightRaster.Engine;
using ALightRaster.Render;
using ALightRealtime.Render;
using Veldrid;
using Vulkan.Xlib;

namespace ALightRaster.DotNetCore
{
    class Program
    {
        private static void Main(string[] args) => new PreviewWindow().Run( PreviewWindow.width, PreviewWindow.height, "ALightRaster");
    }

    public class PreviewWindow :AcApplication
    {
        public static PreviewWindow main;
        public static uint height = 512, width = 512;
        public override void Start()
        {
            main = this;
            backgroundColor = new RgbaFloat(71 / 255f, 71 / 255f, 71 / 255f,1);

            Scene.Init();
            ALightRaster.Render.Canvas.Init();
            Scene.current.StartScripts();
            _window.KeyDown += _window_KeyDown;
        }

        public override void Update()
        {
          
            //title = "ALightRaster FPS:" + FramePerSecond;
            ALightRaster.Render.Canvas.instance.Render();

            Time.deltatime = DeltaTime;
            Scene.current.UpdateScripts();

        }

        private void _window_KeyDown(KeyEvent obj)
        {
            if (obj.Key == Key.F1)
            {
                Canvas.instance.Save();
            }
        }
    }
}
