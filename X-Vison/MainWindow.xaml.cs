using Center_Maneger.UesrControls;
using Center_Maneger.View;
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

namespace Center_Maneger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void num_chairs_btn(object sender, RoutedEventArgs e)
        {
           num_of_chairs_win Chair =   new num_of_chairs_win(); 
            Chair.Show();
        }

        private void class_setting_btn(object sender, RoutedEventArgs e)
        {
            TabItem newTab = new TabItem
            {// افتكر الفنكشن الواحدة للكل الزراير
                Header = "اعدادات الغرفة",
                Content = new Class_Settings()
            };
            settings_controls.Items.Add(newTab);
            settings_controls.SelectedItem = newTab;
        }

        private void Faculty_Settings(object sender, RoutedEventArgs e)
        {
            TabItem newTab = new TabItem
            {// افتكر الفنكشن الواحدة للكل الزراير
                // كملن افتكر الاراي بتاعت اليوزر كونترول
                Header = "الكليات",
                Content = new Faculty_Settings()
            };
            settings_controls.Items.Add(newTab);
            settings_controls.SelectedItem = newTab;
        }
    }
}
