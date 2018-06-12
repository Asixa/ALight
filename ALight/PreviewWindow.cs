using System;
using System.Windows.Forms;
using ALight.Render;
using Timer = System.Windows.Forms.Timer;

namespace ALight
{
    public partial class PreviewWindow : Form
    {
        private Timer timer;
        public PreviewWindow()
        {
            InitializeComponent(); 
            timer = new Timer();
            timer.Interval = 500;
            timer.Tick +=(s,e)=> Timer_Elapsed();
            timer.Start();
            PreviewWindow_SizeChanged(null, null);
        }

        private void PreviewWindow_Load(object sender, EventArgs e)
        {

        }

        private void Timer_Elapsed()
        {
            if(IsHandleCreated)
             this.BeginInvoke(new Action(() => { Canvas.Image = Renderer.GetBitmap(); }));
        }

        private void PreviewWindow_SizeChanged(object sender, EventArgs e)
        {
            Canvas.Width = Width-17;
            Canvas.Height = Height-100;
            Canvas.Top = 40;
            Canvas.Left = 0;
        }

        private void PreviewWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer.Stop();
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            int bx, by;
            if (Canvas.Width <= Canvas.Height)
            {
                var f = Canvas.Width * 1f / Configuration.width;
                by = (int)(
                    (e.Location.Y - (Canvas.Height - Configuration.height*f) / 2f)*f
                    +0.5f);
                bx = (int) ((e.Location.X * 1f / Canvas.Width) * Configuration.width + 0.5f);
              
            }
            else
            {
                bx = -1;
                by = -1;
            }
           // Console.WriteLine(e.Location+" "+bx+" "+by);
        }
    }
}
