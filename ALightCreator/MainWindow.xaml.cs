using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ALight.Render;
using ALight.Render.Mathematics;
using ALightCreator.Features.Localization;
using ALightCreator.Features.ResourceView;
using ALightCreator.Panels;
using ModernChrome;
using Xceed.Wpf.AvalonDock.Layout;

namespace ALightCreator
{
    public partial class MainWindow : Window
    {
        public static Renderer renderer=new Renderer();
        public static MainWindow main;

        public bool start;
        public MainWindow()
        {
            main = this;
            InitializeComponent();
            //
          
            OpenOutputWindow();
            var g=new LayoutAnchorablePane();
            g.Children.Add(new LayoutAnchorable { Title = "资源" });
            //BottomGroup.Children.Add();

        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var accentBrush = TryFindResource("AccentColorBrush") as SolidColorBrush;
            accentBrush?.Color.CreateAccentColors();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {

            renderer.Init();
            start = true;
        }

        public static void OpenOutputWindow()
        {
            var anchorable = new LayoutAnchorable
            {
                AutoHideMinHeight = 200,
                FloatingHeight = 100,
                AutoHideHeight = 20,
                Title = "资源",
                Content = new Resource()
            };
          
            anchorable.AddToLayout(MainWindow.main.DockManager, AnchorableShowStrategy.Bottom);
        }
    }

}