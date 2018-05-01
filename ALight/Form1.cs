using System;
using System.Windows.Forms;

namespace ALight
{
    public partial class Form1 : Form
    {
        public static Form1 main;
        private readonly Render.Renderer renderer=new Render.Renderer();
        public Form1()
        {
            main = this;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //renderer.width = renderer.height = int.Parse(Resolution_Input.Text);
            //renderer.Samples = int.Parse(SPP_Input.Text);
            button1.Enabled = false;
            renderer.Init();
        }

        public void SetProgress(int now,int all)
        {
            progressBar1.Maximum = all;
            progressBar1.Value = now;
        }

        private int samples;
        public void SetSPP()
        {
            SPP.Text = "已采样" + (++samples) + "次";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            renderer.Save();
            
        }
    }
}
