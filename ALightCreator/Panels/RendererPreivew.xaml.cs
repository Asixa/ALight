using System;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ALightCreator.Panels
{
    /// <summary>
    /// RendererPreview.xaml 的交互逻辑
    /// </summary>
    public partial class RendererPreview : UserControl
    {
        public static RendererPreview main;
        public byte[] buff;
        public Timer timer = new Timer();
        public bool start=>MainWindow.main.start;
        public RendererPreview()
        {
            main = this;
            InitializeComponent();
            buff=new byte[512*512*4];

            timer.Enabled = true;
            timer.Interval = 1000f;
            timer.Elapsed += (s, e) => main.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (!start) return;

                for (var i = 0; i < main.buff.Length; i += 4)
                {
//                    buff[i] = (byte)Mathf.Range(Renderer.main.buff[i + 2] * 255 / Renderer.main.Changes[i / 4] + 0.5f, 0, 255f);//b
//                    buff[i + 1] =(byte)Mathf.Range(Renderer.main.buff[i + 1] * 255 / Renderer.main.Changes[i / 4] + 0.5f, 0, 255f);//g
//                    buff[i + 2] = (byte)Mathf.Range(Renderer.main.buff[i] * 255 / Renderer.main.Changes[i / 4] + 0.5f, 0, 255f);//r
//                    buff[i + 3] =(byte)Mathf.Range(Renderer.main.buff[i + 3] * 255 / Renderer.main.Changes[i / 4] + 0.5f, 0, 255f);
                }
                main.Set();
            }));
            timer.Start();
        }

        public void Set()
        {
            Image1.Source = BitmapSource.Create(512, 512,
                    96, 96, PixelFormats.Pbgra32, null, buff, (512 * PixelFormats.Pbgra32.BitsPerPixel) / 8); ;
        }
    }
}
