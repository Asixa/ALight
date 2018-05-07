using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using ALight.Render;

namespace WpfOfficeTheme
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    ///
   
    public partial class MainWindow : Window
    {
        public  Renderer renderer=new Renderer();
        public MainWindow()
        {
            InitializeComponent();
            

        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            renderer.Init();
        }
    }

}