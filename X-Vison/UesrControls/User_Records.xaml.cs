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
    /// Interaction logic for User_Records.xaml
    /// </summary>
    public partial class User_Records : UserControl
    {
        public User_Records()
        {
            InitializeComponent();
        }

        private void load_data(object sender, RoutedEventArgs e)
        {
            data_grid.ItemsSource = databaseLoader.GetUserRecords().DefaultView;
            data_grid.Columns[3].Width = data_grid.Columns[4].Width = data_grid.Columns[0].Width = 200;
        }
        private void change_header_name(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "name")
            {
                e.Column.Header = "الاسم";
            }
            else if (e.PropertyName == "phone")
            {
                e.Column.Header = "رقم الهاتف";
            }
            else if (e.PropertyName == "enter_date")
            {
                e.Column.Header = "تاريخ الدخول";
            }
            else if (e.PropertyName == "leave_date")
            {
                e.Column.Header = "ناريخ الخروج";
            }
            else if (e.PropertyName == "type")
            {
                e.Column.Header = "نوع الزيارة";
            }
            else if (e.PropertyName == "reservation_cost")
            {
                e.Column.Header = "تكلفة الحجز";
            }
            else if (e.PropertyName == "kitchen")
            {
                e.Column.Header = "تكلفة البوفيه";
            }
            else if (e.PropertyName == "total")
            {
                e.Column.Header = "الاجمالي";
            }
            else if (e.PropertyName == "paid")
            {
                e.Column.Header = "المدفوع";
            }
        }
    }
}
