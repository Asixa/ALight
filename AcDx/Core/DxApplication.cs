using System;
using System.Drawing;
using System.Windows.Forms;
using SharpDX.Windows;

namespace AcDx.Core
{
    public abstract class DxApplication
    {
        private readonly DxTime clock = new DxTime();
        private FormWindowState _currentFormWindowState;
        private bool _disposed;
        public Form form;
        private float _frameAccumulator;
        private int _frameCount;

        /// <summary>
        ///   Performs object finalization.
        /// </summary>
        ~DxApplication()
        {
            if (_disposed) return;
            Dispose(false);
            _disposed = true;
        }

        /// <summary>
        ///   Disposes of object resources.
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                Dispose(true);
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///   Disposes of object resources.
        /// </summary>
        /// <param name = "disposeManagedResources">If true, managed resources should be
        ///   disposed of in addition to unmanaged resources.</param>
        protected virtual void Dispose(bool disposeManagedResources)
        {
            if (!disposeManagedResources) return;
            form?.Dispose();
        }

        /// <summary>
        /// Return the Handle to display to.
        /// </summary>
        protected IntPtr DisplayHandle => form.Handle;

        /// <summary>
        /// Gets the config.
        /// </summary>
        /// <value>The config.</value>
        public DxConfiguration Config { get; private set; }

        /// <summary>
        ///   Gets the number of seconds passed since the last frame.
        /// </summary>
        public float FrameDelta { get; private set; }

        /// <summary>
        ///   Gets the number of seconds passed since the last frame.
        /// </summary>
        public float FramePerSecond { get; private set; }

        /// <summary>
        /// Create Form for this demo.
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        protected virtual Form CreateForm(DxConfiguration config)
        {
            return new RenderForm(config.Title)
            {
                ClientSize = new System.Drawing.Size(config.Width, config.Height),
                Icon = null,
                FormBorderStyle = FormBorderStyle.FixedSingle,
                ShowIcon = false,
                MaximizeBox = false
                
            };
        }

        /// <summary>
        /// Runs the demo with default presentation
        /// </summary>
        public void Run()
        {
            Run(new DxConfiguration());
        }

        /// <summary>
        /// Runs the demo.
        /// </summary>
        public void Run(DxConfiguration dx_configuration)
        {
            this.Config = dx_configuration ?? new DxConfiguration();
            form = CreateForm(this.Config);
            Initialize(Config);

            var isFormClosed = false;
            var formIsResizing = false;

            form.MouseClick += HandleMouseClick;
            form.KeyDown += HandleKeyDown;
            form.KeyUp += HandleKeyUp;
            form.Resize += (o, args) =>
            {
                if (form.WindowState != _currentFormWindowState)
                {
                    HandleResize(o, args);
                }

                _currentFormWindowState = form.WindowState;
            };

            form.ResizeBegin += (o, args) => formIsResizing = true;


            form.ResizeEnd += (o, args) =>
            {
                formIsResizing = false;
                HandleResize(o, args);
            };

            form.Closed += (o, args) => isFormClosed = true;

            LoadContent();

            clock.Start();
            BeginRun();
            RenderLoop.Run(form, () =>
            {
                if (isFormClosed)
                {
                    return;
                }

                OnUpdate();
                if (!formIsResizing)
                    Render();
            });

            UnloadContent();
            EndRun();

            // Dispose explicity
            Dispose();
        }

        /// <summary>
        ///   In a derived class, implements logic to initialize the sample.
        /// </summary>
        protected abstract void Initialize(DxConfiguration dx_configuration);

        protected virtual void LoadContent()
        {
        }

        protected virtual void UnloadContent()
        {
        }

        /// <summary>
        ///   In a derived class, implements logic to update any relevant sample state.
        /// </summary>
        protected virtual void Update(DxTime time)
        {
        }

        /// <summary>
        ///   In a derived class, implements logic to render the sample.
        /// </summary>
        protected virtual void Draw(DxTime time)
        {
        }

        protected virtual void BeginRun()
        {
        }

        protected virtual void EndRun()
        {
        }

        /// <summary>
        ///   In a derived class, implements logic that should occur before all
        ///   other rendering.
        /// </summary>
        protected virtual void BeginDraw()
        {
        }

        /// <summary>
        ///   In a derived class, implements logic that should occur after all
        ///   other rendering.
        /// </summary>
        protected virtual void EndDraw()
        {
        }

        /// <summary>
        ///   Quits the sample.
        /// </summary>
        public void Exit()
        {
            form.Close();
        }

        /// <summary>
        ///   Updates sample state.
        /// </summary>
        private void OnUpdate()
        {
            FrameDelta = (float)clock.Update();
            Update(clock);
        }

        protected Size RenderingSize => form.ClientSize;

        /// <summary>
        ///   Renders the sample.
        /// </summary>
        private void Render()
        {
            _frameAccumulator += FrameDelta;
            ++_frameCount;
            if (_frameAccumulator >= 1.0f)
            {
                FramePerSecond = _frameCount / _frameAccumulator;
                _frameAccumulator = 0.0f;
                _frameCount = 0;
            }

            BeginDraw();
            Draw(clock);
            EndDraw();
        }

        protected virtual void MouseClick(MouseEventArgs e)
        {
        }

        protected virtual void KeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Exit();
        }

        protected virtual void KeyUp(KeyEventArgs e)
        {
        }

        /// <summary>
        ///   Handles a mouse click event.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The <see cref = "System.Windows.Forms.MouseEventArgs" /> instance containing the event data.</param>
        private void HandleMouseClick(object sender, MouseEventArgs e)
        {
            MouseClick(e);
        }

        /// <summary>
        ///   Handles a key down event.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The <see cref = "System.Windows.Forms.KeyEventArgs" /> instance containing the event data.</param>
        private void HandleKeyDown(object sender, KeyEventArgs e)
        {
            KeyDown(e);
        }

        /// <summary>
        ///   Handles a key up event.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The <see cref = "System.Windows.Forms.KeyEventArgs" /> instance containing the event data.</param>
        private void HandleKeyUp(object sender, KeyEventArgs e)
        {
            KeyUp(e);
        }

        private void HandleResize(object sender, EventArgs e)
        {
            if (form.WindowState == FormWindowState.Minimized)
            {
                return;
            }

            // UnloadContent();

            //_configuration.WindowWidth = form.ClientSize.Width;
            //_configuration.WindowHeight = form.ClientSize.Height;

            //if( Context9 != null ) {
            //    userInterfaceRenderer.Dispose();

            //    Context9.PresentParameters.BackBufferWidth = _configuration.WindowWidth;
            //    Context9.PresentParameters.BackBufferHeight = _configuration.WindowHeight;

            //    Context9.Device.Reset( Context9.PresentParameters );

            //    userInterfaceRenderer = new UserInterfaceRenderer9( Context9.Device, form.ClientSize.Width, form.ClientSize.Height );
            //} else if( Context10 != null ) {
            //    userInterfaceRenderer.Dispose();

            //    Context10.SwapChain.ResizeBuffers( 1, WindowWidth, WindowHeight, Context10.SwapChain.Description.ModeDescription.Format, Context10.SwapChain.Description.Flags );


            //    userInterfaceRenderer = new UserInterfaceRenderer10( Context10.Device, form.ClientSize.Width, form.ClientSize.Height );
            //}

            // LoadContent();
        }
    }
}
