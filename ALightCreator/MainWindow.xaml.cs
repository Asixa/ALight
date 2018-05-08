using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using ALightCreator.Features.Localization;

namespace ALightCreator
{
    public partial class MainWindow : Window
    {
        public static MainWindow main;
        public MainWindow()
        {
            main = this;
            InitializeComponent();
            //AcLanguage.instance.Set("en-US");
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var accentBrush = TryFindResource("AccentColorBrush") as SolidColorBrush;
            accentBrush?.Color.CreateAccentColors();
        }
    }

}