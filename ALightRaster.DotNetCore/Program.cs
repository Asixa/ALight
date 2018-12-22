using System;
using AcFormCore;
using ALightRaster.Engine;
using ALightRealtime.Render;
using Veldrid;

namespace ALightRaster.DotNetCore
{
    class Program
    {
        private static void Main(string[] args) => new PreviewWindow().Run( 800, 600, "ALightRaster");
    }

    public class PreviewWindow :AcApplication
    {
        public static PreviewWindow main;

        public override void Start()
        {
            main = this;
            backgroundColor = new RgbaFloat(71 / 255f, 71 / 255f, 71 / 255f,1);

            Scene.Init();
            ALightRaster.Render.Canvas.Init();
            Scene.current.StartScripts();

        }

        public override void Update()
        {
            //title = "ALightRaster FPS:" + FramePerSecond;
            ALightRaster.Render.Canvas.instance.Render();

            Time.deltatime = DeltaTime;
            Scene.current.UpdateScripts();

        }
    }
}
