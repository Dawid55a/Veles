using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Veles_Application.Models;
using Veles_Application.ViewModels;

namespace Veles_Application.Views
{
    /// <summary>
    /// Interaction logic for tmp.xaml
    /// </summary>
    public partial class tmp : Window
    {
        public tmp()
        {
            InitializeComponent();

            DataContext = new TmpViewModel();
        }
    }
}
