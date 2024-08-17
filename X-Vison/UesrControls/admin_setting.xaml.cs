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

namespace Center_Maneger.UesrControls
{
    /// <summary>
    /// Interaction logic for admin_setting.xaml
    /// </summary>
    public partial class admin_setting : UserControl
    {

        public string PasswordText { get; set; }

        public admin_setting()
        {
            InitializeComponent();
            cp_setting_grid.IsEnabled = false;
            cp_login_grid.IsEnabled = true;
            cp_setting.IsChecked = true;

            this.DataContext = this;

        }

        private void login_or_setting(object sender, RoutedEventArgs e)
        {
            cp_setting_grid.IsEnabled = !cp_setting_grid.IsEnabled;
            cp_login_grid.IsEnabled = !cp_login_grid.IsEnabled;
            if (cp_setting_grid.IsEnabled)
            {
                cp_setting_grid.Opacity = 1;
                cp_login_grid.Opacity = 0.6;
            }
            else
            {
                cp_login_grid.Opacity = 1;
                cp_setting_grid.Opacity = 0.6;

            }
        }

        private void change_pass(object sender, RoutedEventArgs e)
        {

            if (cp_setting_grid.IsEnabled)
            {
                string old_pass = pass0.Visibility == Visibility.Visible ? pass0.Password : box0.Text;

                if (!string.IsNullOrEmpty(old_pass))
                {
                    string pass = databaseLoader.SelectData("admin", "password", "username = \"setting\"")[0].ToString();
                    if (pass == old_pass)
                    {
                        string new_pass = pass1.Visibility == Visibility.Visible ? pass1.Password : box1.Text;
                        if (!string.IsNullOrEmpty(new_pass))
                        {
                            string confirm_pass = pass2.Visibility == Visibility.Visible ? pass2.Password : box2.Text;
                            if (!string.IsNullOrEmpty(confirm_pass))
                            {
                                if (confirm_pass == new_pass)
                                {
                                    Dictionary<string, object> data = new Dictionary<string, object>{
                                    {"password", new_pass} };
                                    databaseLoader.UpdateData("admin", data, "username = \"setting\"");
                                    MessageBox.Show("تم تغيير كلمة السر ", "", MessageBoxButton.OK, MessageBoxImage.Information);



                                    clearsetting();


                                }
                                else
                                {
                                    MessageBox.Show("كلمة السر غير متطابقة", "خظأ", MessageBoxButton.OK, MessageBoxImage.Error);
                                    clearsetting();

                                }
                            }
                            else
                            {
                                MessageBox.Show("ادخل تأكيد كلمة السر", "خظأ", MessageBoxButton.OK, MessageBoxImage.Error);
                                clearsetting();

                            }
                        }
                        else
                        {
                            MessageBox.Show("ادخل كلمة السر الجديدة", "خظأ", MessageBoxButton.OK, MessageBoxImage.Error);
                            clearsetting();

                        }
                    }
                    else
                    {
                        MessageBox.Show("كلمة السر القديمة غير صحيحة", "خظأ", MessageBoxButton.OK, MessageBoxImage.Error);
                        clearsetting();

                    }
                }
                else
                {
                    MessageBox.Show("ادخل كلمة السر القديمة", "خظأ", MessageBoxButton.OK, MessageBoxImage.Error);
                    clearsetting();

                }


            }
            if (cp_login_grid.IsEnabled)
            {
                string pass = databaseLoader.SelectData("admin", "password", "id = 1")[0].ToString();
                string old_pass = pass3.Visibility == Visibility.Visible ? pass3.Password : box3.Text;

                if (!string.IsNullOrEmpty(old_pass))
                {
                    if (pass == old_pass)
                    {
                        string new_pass = pass4.Visibility == Visibility.Visible ? pass4.Password : box4.Text;
                        if (!string.IsNullOrEmpty(new_pass))
                        {
                            string confirm_pass = pass5.Visibility == Visibility.Visible ? pass5.Password : box5.Text;
                            if (!string.IsNullOrEmpty(confirm_pass))
                            {
                                if (confirm_pass == new_pass)
                                {
                                    Dictionary<string, object> data = new Dictionary<string, object>{
                                        {"password", new_pass}
                                    };
                                    databaseLoader.UpdateData("admin", data, "id = 1");
                                    MessageBox.Show("تم تغيير كلمة السر ", "", MessageBoxButton.OK, MessageBoxImage.Information);

                                    clearlogin();



                                }
                                else
                                {
                                    MessageBox.Show("كلمة السر غير متطابقة", "خظأ", MessageBoxButton.OK, MessageBoxImage.Error);
                                    clearlogin();
                                }
                            }
                            else
                            {
                                MessageBox.Show("ادخل تأكيد كلمة السر", "خظأ", MessageBoxButton.OK, MessageBoxImage.Error);
                                clearlogin();
                            }
                        }
                        else
                        {
                            MessageBox.Show("ادخل كلمة السر الجديدة", "خظأ", MessageBoxButton.OK, MessageBoxImage.Error);
                            clearlogin();

                        }
                    }
                    else
                    {
                        MessageBox.Show("كلمة السر القديمة غير صحيحة", "خظأ", MessageBoxButton.OK, MessageBoxImage.Error);
                        clearlogin();

                    }
                }
                else
                {
                    MessageBox.Show("ادخل كلمة السر القديمة", "خظأ", MessageBoxButton.OK, MessageBoxImage.Error);
                    clearlogin();

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
            Button showbtn = sender as Button;

            string btn = showbtn.Name;
            int index = int.Parse(btn.Substring(btn.Length - 1));

            PasswordBox passBox = (PasswordBox)FindName("pass" + index);
            TextBox showBox = (TextBox)FindName("box" + index);


            if (passBox.Visibility == Visibility.Visible)
            {
                showBox.Text = passBox.Password;
                passBox.Visibility = Visibility.Collapsed;
                showBox.Visibility = Visibility.Visible;
                showbtn.Content = hide;
            }
            else
            {
                passBox.Password = showBox.Text;
                passBox.Visibility = Visibility.Visible;
                showBox.Visibility = Visibility.Collapsed;
                showbtn.Content = show;
            }



        }

        void clearsetting()
        {
            pass0.Clear();
            pass1.Clear();
            pass2.Clear();
            box0.Clear();
            box1.Clear();
            box2.Clear();
        }
        void clearlogin()
        {
            pass3.Clear();
            pass4.Clear();
            pass5.Clear();
            box3.Clear();
            box4.Clear();
            box5.Clear();
        }
    }
}
