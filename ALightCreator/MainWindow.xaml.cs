using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ALight.Render;
using ALight.Render.Mathematics;
using ALightCreator.Features.Localization;
using ALightCreator.Panels;
using ModernChrome;

namespace ALightCreator
{
    public partial class MainWindow : Window
    {
        public static Renderer renderer=new Renderer();
        public static MainWindow main;
        public Timer timer = new Timer();
        public bool start;
        public MainWindow()
        {
            main = this;
            InitializeComponent();

            timer.Enabled = true;
            timer.Interval = 1000/60f;
            timer.Elapsed += (s, e) => main.Dispatcher.BeginInvoke(new Action(() =>
            {
               if(!start)return;

                for (var i = 0; i < RendererPreview.main.buff.Length; i += 4)
                {
                    RendererPreview.main.buff[i] =
                        (byte) Mathf.Range(Renderer.main.buff[i+2] * 255 / Renderer.main.Changes[i / 4] + 0.5f, 0, 255f);
                    RendererPreview.main.buff[i+1] =
                        (byte) Mathf.Range(Renderer.main.buff[i+1] * 255 / Renderer.main.Changes[i / 4] + 0.5f, 0, 255f);
                    RendererPreview.main.buff[i+2] =
                        (byte) Mathf.Range(Renderer.main.buff[i] * 255 / Renderer.main.Changes[i / 4] + 0.5f, 0, 255f);
                    RendererPreview.main.buff[i+3] =
                        (byte) Mathf.Range(Renderer.main.buff[i+3] * 255 / Renderer.main.Changes[i / 4] + 0.5f, 0, 255f);
                }

                RendererPreview.main.Set();
            }));
            timer.Start();

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
    }

}