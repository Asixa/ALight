using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace ALightCreator.Panels
{
    public class ControlBase
    {
        public static void OnlyNumber(object sender, TextCompositionEventArgs e)=>e.Handled = new Regex("[^0-9.-]+").IsMatch(e.Text);
        
    }
}
