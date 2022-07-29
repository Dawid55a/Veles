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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Veles_Application.Components
{
    /// <summary>
    /// Interaction logic for BindablePasswordBox.xaml
    /// </summary>
    public partial class BindablePasswordBox : UserControl
    {
        private bool _isPasswordChanging;//

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(BindablePasswordBox), 
                new PropertyMetadata(string.Empty, PasswordPropertyChange));

        private static void PasswordPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is BindablePasswordBox passwordBox)
            {
                passwordBox.UpdatePassword();
            }
        }
        
        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        //Constructor
        public BindablePasswordBox()
        {
            InitializeComponent();
        }

        //Binding password
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _isPasswordChanging = true;
            Password = passwordBox.Password;
            _isPasswordChanging = false;
        }
        
        //Get value from outside
        private void UpdatePassword()
        {
            if(!_isPasswordChanging)
                passwordBox.Password = Password;
        }

    }
}
