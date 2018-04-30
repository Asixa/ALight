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
            button1.Enabled = false;
            renderer.Init();
        }

        public void SetProgress(int now,int all)
        {
            progressBar1.Maximum = all;
            progressBar1.Value = now;
        }

        public void SetSPP(int s)
        {
            SPP.Text = "第" + s + "次采样";
        }
    }
}
