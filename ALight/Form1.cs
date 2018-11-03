using System;
using System.Threading;
using System.Windows.Forms;
using ALight.Render;
using Timer = System.Timers.Timer;

namespace ALight
{
    public partial class Form1 : Form
    {
        public static Form1 main;
        private readonly Renderer renderer=new Renderer();
        public Timer timer=new Timer();
        public System.Windows.Forms.Timer splash = new System.Windows.Forms.Timer();
        public DateTime StartTime;
        public int seconds;
        public Form1()
        {
            //Hide();
            main = this;
            InitializeComponent();
            splash.Interval = 1000;
          
            splash.Tick += delegate (object sender, EventArgs e) {
                StartTime = DateTime.Now;
                timer.Start();
                new PreviewWindow().Show();
                Hide();
                //renderer.Init();
                splash.Stop();
                splash.Dispose();
            };
            splash.Start();

            renderer.chunk_end += (chunks) =>
            {
                main.BeginInvoke(new Action(() =>
                {
                    seconds = (int)(DateTime.Now - main.StartTime).TotalSeconds;
                    PreviewWindow.main.TimeLabel.Text = (DateTime.Now - main.StartTime).ToString();
                }));
            };
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        
        }

    }
}
