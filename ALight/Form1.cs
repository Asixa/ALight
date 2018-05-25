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
        public DateTime StartTime;
        private int seconds;
        public Form1()
        {
            main = this;
            InitializeComponent();
            progressBar1.Maximum = Configuration.divide*Configuration.divide;
            renderer.chunk_end += (chunks) =>
            {
                main.BeginInvoke(new Action(() =>
                {
                    main.SPP.Text = TimeSpan.FromSeconds((int) (DateTime.Now - main.StartTime).TotalSeconds).ToString();
                    main.progressBar1.Value = chunks;
                }));
            };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StartTime=DateTime.Now;
            timer.Start();
            button1.Enabled = false;
            renderer.Init();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            renderer.Save("Result_"+DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")+".png");
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            renderer.InitPreview();
        }
    }
}
