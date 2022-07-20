using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
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
            /*
            var loginView = new LoginView();
            loginView.Show();//Show LoginView window
            loginView.IsVisibleChanged += (s, ev) =>
              {
                  var mainView = new MainView();
                  mainView.Show();//Show MainView window
                  loginView.Close();//Close LoginView window
              };*/

            var mainView = new MainView();
            mainView.Show();
        }
    }
}
