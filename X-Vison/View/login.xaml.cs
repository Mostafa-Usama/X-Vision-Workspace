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
using System.Data.SQLite;
using System.Net.NetworkInformation;
namespace Center_Maneger.View
{
    /// <summary>
    /// Interaction logic for login.xaml
    /// </summary>
    public partial class login : Window
    {
        private string _connectionString = "Data Source=Center_Manager.dll;Version=3;";

        public login()
        {
            InitializeComponent();
            txtUser.Focus();
        }
      
        private void Mouse_Drag(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Minimize_Win(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Close_Win(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void loginbtn_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUser.Text.Trim().ToLower();
            string password = user_password.Visibility == Visibility.Visible ? user_password.Password : user_password_tb.Text; ;

            if (ValidateLogin(username, password))
            {
                MainWindow main = new MainWindow();
                this.Close();
                main.Show();
                

            }
            else
            {
                MessageBox.Show("اسم المستخدم أو كلمة المرور خطأ","خطا", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private bool ValidateLogin(string username, string password)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(1) FROM admin WHERE username = @username AND password = @password";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count == 1;
                }
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

            if (user_password.Visibility == Visibility.Visible)
            {
                user_password_tb.Text = user_password.Password;
                user_password.Visibility = Visibility.Collapsed;
                user_password_tb.Visibility = Visibility.Visible;
                b0.Content = hide;
            }
            else
            {
                user_password.Password = user_password_tb.Text;
                user_password.Visibility = Visibility.Visible;
                user_password_tb.Visibility = Visibility.Collapsed;
                b0.Content = show;
            }



        }
    }
}
