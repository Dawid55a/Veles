using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Veles_Application.Views;


namespace Veles_Application
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected void Main(object sender, StartupEventArgs e)//Main
        {
            var Window = new WindowView();
            Window.Show();
            
            //var mainView = new MainView();
            //mainView.Show();
        }
    }
}
