using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using ALight.Editor;
using ALight.Render;
using ALight.Render.Denoise;
using Timer = System.Windows.Forms.Timer;

namespace ALight
{
    public partial class PreviewWindow : Form
    {
        public static PreviewWindow main;
        private bool renderering;
        private bool MenuOpend=true;
        private MyProgressBar bar= new MyProgressBar { Top = 0, Height = 40, Width = 300, Left = 40 };
        private readonly Timer timer;
        public PreviewWindow()
        {
            main = this;
            InitializeComponent();
            renderermode.SelectedItem = "Shaded";
            timer = new Timer {Interval = 500};
            timer.Tick +=(s,e)=> Timer_Elapsed();
            timer.Start();
            PreviewWindow_SizeChanged(null, null);
            bar.BackColor=Color.FromArgb(30,30,30);
            bar.Value = 50;
        }


        private void Timer_Elapsed()
        {
            if(IsHandleCreated)
             BeginInvoke(new Action(() =>
             {
                 var img = Render.Renderer.GetBitmap();
                 if(flip.Checked)
                     img.RotateFlip(RotateFlipType.Rotate180FlipY);
                 Canvas.Image = img;
             }));
        }

        private void PreviewWindow_SizeChanged(object sender, EventArgs e)
        {
            bar.Top = 0;
            bar.Height = 40;
            bar.Width = 300;
            bar.Left = 40;

            topanel.Top = 0;
            topanel.Left = 0;
            topanel.Height = 40;
            topanel.Width = Width;

            MenuPanel.Visible = MenuOpend;
            MenuPanel.Left=0;
            MenuPanel.Top = 40;
            MenuPanel.Width = 200;
            MenuPanel.Height = Height - 100;

            Canvas_denoise.Width=Canvas.Width = Width-17-(MenuOpend?200:0);
            Canvas_denoise.Height=Canvas.Height = Height-100;
            Canvas_denoise.Top=Canvas.Top = 40;
            Canvas_denoise.Left=Canvas.Left = MenuOpend ? 200 : 0;


            TimeLabel.Location=new Point(2,Height-60);
        }

        private void PreviewWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer.Stop();
            Form1.main.Close();//
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {

        }
        public int DrawToPictureBox(IntPtr data, int width, int height)
        {
            
            var pBuffer = new Bitmap(width, height, width * 4, PixelFormat.Format32bppArgb, data);
            if (pBuffer == null)
                return -1;
            pBuffer.RotateFlip(RotateFlipType.Rotate180FlipX);
            Canvas_denoise.Image = pBuffer;
            Canvas.Visible = false;
            return 0;
        }
        private void Save_Click(object sender, EventArgs e)=> Render.Renderer.main.Save("Result_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "-T" + Form1.main.seconds + "s" +".png");

        private void Menu_Click(object sender, EventArgs e)
        {
            MenuOpend = !MenuOpend;
            PreviewWindow_SizeChanged(null, null);
        }

        private void DragScene(object sender, DragEventArgs e)
        {
            Loading.Visible = true;
            Loading.Top = Loading.Left = 0;
            Loading.Width = Width;
            Loading.Height = Height;
            new Scene(((Array) e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString());
        }

        private void Canvas_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.All : DragDropEffects.None;
        }

        private void Renderer_Click(object sender, EventArgs e)
        {
            if (!renderering)
            {
                renderering = true;
                Configuration.mode = (Mode)Enum.Parse(typeof(Mode), renderermode.SelectedItem.ToString());
                Configuration.MAX_SCATTER_TIME = int.Parse(MST_input.Text);
                Configuration.SPP = int.Parse(spp_input.Text);
                Configuration.height = int.Parse(height_input.Text);
                Configuration.width = int.Parse(width_input.Text);
                new Renderer().Init();
                Renderer.Text = "Stop";
            }
            else
            {
                Render.Renderer.main.STOP = true;
                renderering = false;
                Renderer.Text = "Renderer";
                //Renderer_Click(null,null);
            }
        }

        private void DenoiseButton_Click(object sender, EventArgs e)
        {
            return;
          Denoise();
        }



        public void Denoise()
        {
            //this.progressBar1.Value = 0;
            //this.lock_Buttons();

            //if (!Directory.Exists("Cache")) Directory.CreateDirectory("Cache");
            //int Get(int i) => (byte)Mathf.Range(main.buff[i] * 255 / main.Changes[i / 4] + 0.5f, 0, 255f);
            //var pic = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            //for (var i = 0; i < main.buff.Length; i += 4)
            //{
            //    var c = Color.FromArgb(Get(i + 3), Get(i + 2), Get(i + 1), Get(i));
            //    pic.SetPixel(i % (width * 4) / 4, i / (width * 4), c);
            //}
            //pic.Save("Cache/img.png");
            Denoiser.input_image = "Cache/img.png";
            Denoiser.output_image = "Cache/de_img.png";
            Denoiser.blend = (float)10 / 100f;
            Denoiser.Denoise(ImageCallBack, ProgressCallBack, DenoiseFinishedCallBack);
        }

        public int DenoiseFinishedCallBack()
        {
            //this.unlock_Buttons();
            return 0;
        }

        public int ProgressCallBack(float progress)
        {
            //this.progressBar1.Value = (int)(progress * 100f);
            //this.pictureBox1.Image = this.pBuffer;
            return 0;
        }

        public int ImageCallBack(IntPtr data, int w, int h, int size)
        {
            PreviewWindow.main.DrawToPictureBox(data, w, h);
            //this.pBuffer = new Bitmap(w, h, size / h, PixelFormat.Format32bppArgb, data);
            //this.pBuffer.RotateFlip(RotateFlipType.Rotate180FlipX);
            //this.DoEvents();
            return 0;
        }
    }
}
