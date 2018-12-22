using System;
using System.Diagnostics;
using System.IO;
using System.Timers;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;

namespace AcFormCore
{
    public unsafe class AcApplication
    {
        public  uint Width = 512;
        public  uint Height = 512; 
        public const uint ViewScale = 1;
        public string title;

        private Sdl2Window _window;
        private GraphicsDevice _graphicsDevice;
        private CommandList _commandList;
        private Texture _transferTex;
        private TextureView _texView;
        private RgbaFloat[] _buff;
        private ResourceSet _graphicsSet;
        private Pipeline _graphicsPipeline;

        public int FramePerSecond;
        public float DeltaTime;
        public RgbaFloat backgroundColor=new RgbaFloat(1,1,1,1);
        public virtual void Start()
        {

        }

        public virtual void Update()
        {
            
        }
        public void Run(uint w, uint h, string title)
        {
            Width = w;
            Height = h;
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            //Shader
            stopwatch.Stop();

            var backend = VeldridStartup.GetPlatformDefaultBackend();

            VeldridStartup.CreateWindowAndGraphicsDevice(
                new WindowCreateInfo(100, 100, (int)(Width * ViewScale), (int)(Height * ViewScale), WindowState.Normal, title),
                new GraphicsDeviceOptions(debug: false, swapchainDepthFormat: null, syncToVerticalBlank: false),
                backend,
                out _window,
                out _graphicsDevice);
            CreateDeviceResources();

            _buff = new RgbaFloat[Width * Height];
           
            var timer1 = new Timer
            {
                Interval = 1000,
                Enabled = true
            };
            timer1.Elapsed += Timer1_Elapsed;
            Start();
            while (_window.Exists)
            {
                _window.PumpEvents();
                if (!_window.Exists) { break; }
                for (var i = 0; i < _buff.Length; i++)
                {
                    _buff[i] = backgroundColor;
                }
                //System.Threading.Thread.Sleep(10);
                RenderFrame();
            }

            _graphicsDevice.Dispose();



        }

        private void Timer1_Elapsed(object sender, ElapsedEventArgs e)
        {
            _window.Title  = title + " FPS:" + FramePerSecond;
            DeltaTime = 1f / FramePerSecond;
            FramePerSecond = 0;
          
        }

        private void RenderFrame()
        {
            FramePerSecond++;
            _commandList.Begin();

            Update();

            fixed (RgbaFloat* pixel_data_ptr = _buff) _graphicsDevice.UpdateTexture(_transferTex, (IntPtr)pixel_data_ptr, Width * Height * (uint)sizeof(RgbaFloat), 0, 0, 0, Width, Height, 1, 0, 0);
            
            _commandList.SetFramebuffer(_graphicsDevice.MainSwapchain.Framebuffer);
            _commandList.SetPipeline(_graphicsPipeline);
            _commandList.SetGraphicsResourceSet(0, _graphicsSet);
            _commandList.Draw(3);
            _commandList.End();
            _graphicsDevice.SubmitCommands(_commandList);
            _graphicsDevice.SwapBuffers();
        }

        public void SetPixel(int x, int y,RgbaFloat c)
        {
            if (x < 0 || y < 0) return;
            if (x >= Width || y >= Height) return;
            _buff[(Height-y) * Width + x] =c;
        }
        private void CreateDeviceResources()
        {
            var factory = _graphicsDevice.ResourceFactory;
            _commandList = factory.CreateCommandList();
            _transferTex = factory.CreateTexture(
                TextureDescription.Texture2D(Width, Height, 1, 1, PixelFormat.R32_G32_B32_A32_Float, TextureUsage.Sampled | TextureUsage.Storage));
            _texView = factory.CreateTextureView(_transferTex);

            var graphics_layout = factory.CreateResourceLayout(new ResourceLayoutDescription(
                new ResourceLayoutElementDescription("SourceTex", ResourceKind.TextureReadOnly, ShaderStages.Fragment),
                new ResourceLayoutElementDescription("SourceSampler", ResourceKind.Sampler, ShaderStages.Fragment)));

            _graphicsSet = factory.CreateResourceSet(new ResourceSetDescription(graphics_layout, _texView, _graphicsDevice.LinearSampler));

            _graphicsPipeline = factory.CreateGraphicsPipeline(new GraphicsPipelineDescription(
                BlendStateDescription.SingleOverrideBlend,
                DepthStencilStateDescription.Disabled,
                RasterizerStateDescription.CullNone,
                PrimitiveTopology.TriangleList,
                new ShaderSetDescription(
                    Array.Empty<VertexLayoutDescription>(),
                    new[]
                    {
                        factory.CreateShader(new ShaderDescription(ShaderStages.Vertex, LoadShaderBytes("FramebufferBlitter-vertex"), "VS")),
                        factory.CreateShader(new ShaderDescription(ShaderStages.Fragment, LoadShaderBytes("FramebufferBlitter-fragment"), "FS"))
                    }),
                graphics_layout,
                _graphicsDevice.MainSwapchain.Framebuffer.OutputDescription));
        }

        private byte[] LoadShaderBytes(string name)
        {
            string extension;
            switch (_graphicsDevice.BackendType)
            {
                case GraphicsBackend.Direct3D11:
                    extension = "hlsl.bytes";
                    break;
                case GraphicsBackend.Vulkan:
                    extension = "450.glsl.spv";
                    break;
                case GraphicsBackend.OpenGL:
                    extension = "330.glsl";
                    break;
                case GraphicsBackend.Metal:
                    extension = "metallib";
                    break;
                case GraphicsBackend.OpenGLES:
                    extension = "300.glsles";
                    break;
                default: throw new InvalidOperationException();
            }

            return File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "BlitterShader", $"{name}.{extension}"));
        }
    }
}
