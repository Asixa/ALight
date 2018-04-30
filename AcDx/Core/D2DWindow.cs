
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DXGI;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Factory = SharpDX.Direct2D1.Factory;
namespace AcDx.Core
{
    public class D2DWindow : D3DWindow
    {
        public Factory Factory2D { get; private set; }
        public SharpDX.DirectWrite.Factory FactoryDWrite { get; private set; }
        public RenderTarget RenderTarget2D { get; private set; }
        public SolidColorBrush SceneColorBrush { get; private set; }

        protected override void Initialize(DxConfiguration demoConfiguration)
        {
            base.Initialize(demoConfiguration);
            Factory2D = new Factory();
            using (var surface = BackBuffer.QueryInterface<Surface>())
            {
                RenderTarget2D = new RenderTarget(Factory2D, surface,
                    new RenderTargetProperties(new PixelFormat(Format.Unknown, AlphaMode.Premultiplied)));
            }
            RenderTarget2D.AntialiasMode = AntialiasMode.PerPrimitive;

            FactoryDWrite = new SharpDX.DirectWrite.Factory();

            SceneColorBrush = new SolidColorBrush(RenderTarget2D, Color.White);
        }

        protected override void BeginDraw()
        {
            base.BeginDraw();
            RenderTarget2D.BeginDraw();
        }

        protected override void EndDraw()
        {
            RenderTarget2D.EndDraw();
            base.EndDraw();
        }
    }
}
