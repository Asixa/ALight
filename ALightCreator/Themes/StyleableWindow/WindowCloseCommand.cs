using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ADK.StyleableWindow
{
    public class WindowCloseCommand :ICommand
    {     

        public bool CanExecute(object parameter)
        {
            return true;
        }

#pragma warning disable CS0067 // 从不使用事件“WindowCloseCommand.CanExecuteChanged”
        public event EventHandler CanExecuteChanged;
#pragma warning restore CS0067 // 从不使用事件“WindowCloseCommand.CanExecuteChanged”

        public void Execute(object parameter)
        {
            if (parameter is Window window) window.Close();
            
        }
    }
}
