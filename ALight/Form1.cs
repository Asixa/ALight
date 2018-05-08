using System;
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
        private int seconds;
        public Form1()
        {
            main = this;
            InitializeComponent();
            timer.Enabled = true;
            timer.Interval =1000;
            progressBar1.Maximum = Configuration.SPP;
            timer.Elapsed +=(s,e)=> BeginInvoke(new Action(() => { SPP.Text = "已采样"+renderer.now_sample+"次";
                //progressBar1.Value = renderer.now_sample;
            }));
            timer.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            renderer.Init();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            renderer.Save();
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            renderer.InitPreview();
        }
    }
}
