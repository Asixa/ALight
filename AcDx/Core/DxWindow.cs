using System;
using System.Data;
using AcDx.Core;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace AcDx
{
    public class DxWindow:D2DWindow
    {
        public byte[] Buff;
        private Bitmap BufferBmp;

        public int Width = 512;
        public int Height = 512;

        public delegate void OnUpdate();

        public OnUpdate onUpdate;

        public RenderTarget Canvas => RenderTarget2D;
        protected override void Initialize(DxConfiguration Config)
        {
            base.Initialize(Config);
            Width = Config.BuffWidth;
            Height = Config.BuffHeight;
            Buff = new byte[Width * Height * 4];
            BufferBmp = new Bitmap(RenderTarget2D, new Size2(Width, Height), new BitmapProperties(RenderTarget2D.PixelFormat));
            Start();
        }

        protected override void Draw(DxTime time)
        {
            base.Draw(time);
            Update();
            BufferBmp.CopyFromMemory(Buff, Width * 4);
            RenderTarget2D.DrawBitmap(BufferBmp, 1f, BitmapInterpolationMode.Linear);
        }

        public virtual void Update()
        {
           onUpdate?.Invoke();
        }

        public virtual void Start()
        {

        }

        private void SetPixel(int x, int y, Color color)
        {
            var i = Width * 4 * y + x * 4;
            Buff[i] += color.R;
            Buff[i + 1] += color.G;
            Buff[i + 2] += color.B;
            Buff[i + 3] += color.A;
        }

    }
}
