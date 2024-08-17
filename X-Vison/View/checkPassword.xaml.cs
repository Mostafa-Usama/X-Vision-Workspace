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

namespace Center_Maneger.View
{
    /// <summary>
    /// Interaction logic for checkPassword.xaml
    /// </summary>
    public partial class checkPassword : Window
    {
        public bool correct = false;
        public checkPassword()
        {
            InitializeComponent();
            password_input.Focus();
        }

        private void check_passowrd(object sender, RoutedEventArgs e)
        {
            Image show = new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/img/showpass.png")),
            };
            string pass = databaseLoader.SelectData("admin", "password", "username = \"setting\"")[0].ToString();
            string check = password_input.Visibility == Visibility.Visible ? password_input.Password : passwordsetting.Text;
            if (check == pass)
            {
                correct = true;
                this.Close();
            }
            else{
                MessageBox.Show("كلمة المرور خطأ", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
                password_input.Clear();
                password_input.Focus();
                passwordsetting.Clear();
                passwordsetting.Visibility = Visibility.Collapsed;
                password_input.Visibility = Visibility.Visible;
                b0.Content = show;
            }
          
        }

        private void showpass(object sender, RoutedEventArgs e)
        {
            Image show = new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/img/showpass.png")),
            };
            Image hide = new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/img/blind.png")),
            };

            if (password_input.Visibility == Visibility.Visible)
            {
                passwordsetting.Text = password_input.Password;
                password_input.Visibility = Visibility.Collapsed;
                passwordsetting.Visibility = Visibility.Visible;
                b0.Content = hide;
            }
            else
            {
                password_input.Password = passwordsetting.Text;
                password_input.Visibility = Visibility.Visible;
                passwordsetting.Visibility = Visibility.Collapsed;
                b0.Content = show;
            }



        }
    }
}
