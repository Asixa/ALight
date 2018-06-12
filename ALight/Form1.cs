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
        public int seconds;
        public Form1()
        {
            main = this;
            InitializeComponent();
      
            renderer.chunk_end += (chunks) =>
            {
                main.BeginInvoke(new Action(() =>
                {
                    progressBar1.Maximum = Configuration.divide_h * Configuration.divide_w;
                    seconds = (int) (DateTime.Now - main.StartTime).TotalSeconds;
                    main.SPP.Text = TimeSpan.FromSeconds(seconds).ToString();
                    renderer.preview.TimeLabel.Text = main.SPP.Text;
                    main.progressBar1.Value = chunks;
                }));
            };

           // button1_Click(null, null);
           //this.Hide();
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
            renderer.Save("Result_"+DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")+"-T"+seconds+"s"+".png");
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            renderer.InitPreview();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var a =new PreviewWindow();
            a.Show();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            button1_Click(null, null);
            Hide();
        }
    }
}
