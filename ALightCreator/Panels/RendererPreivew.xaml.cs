using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using PixelFormat = System.Windows.Media.PixelFormat;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace ALightCreator.Panels
{
    /// <summary>
    /// RendererPreview.xaml 的交互逻辑
    /// </summary>
    public partial class RendererPreview : UserControl
    {
        public static RendererPreview main;
        public byte[] buff;
        public RendererPreview()
        {
            main = this;
            InitializeComponent();
            buff=new byte[512*512*4];
        }

        public void Set()
        {
            Image1.Source = BitmapSource.Create(512, 512,
                    96, 96, PixelFormats.Pbgra32, null, buff, (512 * PixelFormats.Pbgra32.BitsPerPixel) / 8); ;
        }
    }
}
