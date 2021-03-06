﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ADK.StyleableWindow
{
    public class WindowMaximizeCommand :ICommand
    {     

        public bool CanExecute(object parameter)
        {
            return true;
        }

#pragma warning disable CS0067 // 从不使用事件“WindowMaximizeCommand.CanExecuteChanged”
        public event EventHandler CanExecuteChanged;
#pragma warning restore CS0067 // 从不使用事件“WindowMaximizeCommand.CanExecuteChanged”

        public void Execute(object parameter)
        {
            var window = parameter as Window;

            if (window != null)
            {
                if(window.WindowState == WindowState.Maximized)
                {
                    window.WindowState = WindowState.Normal;
                }
                else
                {
                    window.WindowState = WindowState.Maximized;
                }                
            }
        }
    }
}
