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
        public admin_setting()
        {
            InitializeComponent();
            cp_setting_grid.IsEnabled = false;
            cp_login_grid.IsEnabled = true;
            cp_setting.IsChecked = true;
           
            
        }

        private void login_or_setting(object sender, RoutedEventArgs e)
        {
            cp_setting_grid.IsEnabled = !cp_setting_grid.IsEnabled;
            cp_login_grid.IsEnabled = !cp_login_grid.IsEnabled;
            if (cp_setting_grid.IsEnabled)
            {
                cp_setting_grid.Opacity = 1 ;
                cp_login_grid.Opacity = 0.6 ;
            }
            else
            {
                cp_login_grid.Opacity = 1;
                cp_setting_grid.Opacity = 0.6;

            }
        }
    }
}
