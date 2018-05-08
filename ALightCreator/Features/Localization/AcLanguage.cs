using System;
using System.Linq;
using System.Windows;
using ALightCreator;

namespace ALightCreator.Features.Localization
{
    public class AcLanguage
    {
        public string Current = "zh-CN";
        public static AcLanguage instance=new AcLanguage();

        public string Get(string tag)=>MainWindow.main.TryFindResource(tag).ToString();
        
        public void Set(string file)
        {
            var newLanguage = new ResourceDictionary {Source = new Uri(@"Features/Localization/Packages/" + file + ".xaml",UriKind.Relative)};
            ResourceDictionary resourceDictionary = Application.Current.Resources.MergedDictionaries.
                FirstOrDefault(d => d.Source.OriginalString.Equals(@"Features/Localization/Packages/" + Current + ".xaml"));
            Application.Current.Resources.MergedDictionaries.Remove(resourceDictionary);
            Application.Current.Resources.MergedDictionaries.Add(newLanguage);
            Current = file;
        }
    }
}
