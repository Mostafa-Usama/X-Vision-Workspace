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
            
            if (password_input.Password == "osama")
            {
                correct = true;
                this.Close();
            }
            else{
                MessageBox.Show("كلمة المرور خطأ", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
                password_input.Clear();
                password_input.Focus();

            }
          
        }
    }
}
