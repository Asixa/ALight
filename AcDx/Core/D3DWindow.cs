using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace AcDx.Core
{
    public class D3DWindow : DxApplication
    {

        SharpDX.Direct3D11.Device _device;
        SwapChain _swapChain;
        Texture2D _backBuffer;
        RenderTargetView _backBufferView;

        /// <summary>
        /// Returns the device
        /// </summary>
        public SharpDX.Direct3D11.Device Device => _device;

        /// <summary> 
        /// Returns the backbuffer used by the SwapChain
        /// </summary>
        public Texture2D BackBuffer => _backBuffer;

        /// <summary>
        /// Returns the render target view on the backbuffer used by the SwapChain.
        /// </summary>
        public RenderTargetView BackBufferView => _backBufferView;

        protected override void Initialize(DxConfiguration demoConfiguration)
        {
            // SwapChain description
            var desc = new SwapChainDescription()
            {
                BufferCount = 1,
                ModeDescription =
                    new ModeDescription(demoConfiguration.Width, demoConfiguration.Height,
                        new Rational(60, 1), Format.R8G8B8A8_UNorm),
                IsWindowed = true,
                OutputHandle = DisplayHandle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput
            };

            // Create Device and SwapChain
            SharpDX.Direct3D11.Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.BgraSupport, new[] { FeatureLevel.Level_10_0 }, desc, out _device, out _swapChain);

            // Ignore all windows events
            var factory = _swapChain.GetParent<Factory>();
            factory.MakeWindowAssociation(DisplayHandle, WindowAssociationFlags.IgnoreAll);

            // New RenderTargetView from the backbuffer
            _backBuffer = Texture2D.FromSwapChain<Texture2D>(_swapChain, 0);

            _backBufferView = new RenderTargetView(_device, _backBuffer);
        }

        protected override void BeginDraw()
        {
            base.BeginDraw();
            Device.ImmediateContext.Rasterizer.SetViewport(new SharpDX.Viewport(0, 0, Config.Width, Config.Height));
            Device.ImmediateContext.OutputMerger.SetTargets(_backBufferView);
        }


        protected override void EndDraw()
        {
            _swapChain.Present(Config.WaitVerticalBlanking ? 1 : 0, PresentFlags.None);
        }
    }
}
