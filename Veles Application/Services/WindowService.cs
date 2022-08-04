using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Veles_Application.Services
{
    class WindowService
    {
        public void ShowWindow(object viewModel)
        {
            var win = new Window();
            win.Content = viewModel;
            win.Show();
        }
    }
}
