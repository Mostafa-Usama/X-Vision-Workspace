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
        public checkPassword()
        {
            InitializeComponent();
        }

        private void check_passowrd(object sender, RoutedEventArgs e)
        {
            
                this.Close();
          
        }
    }
}
