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
    /// Interaction logic for Member_Offers.xaml
    /// </summary>
    public partial class Member_Offers : UserControl
    {
        public Member_Offers()
        {
            InitializeComponent();
            load_data();
        }

        public void load_data() // display classes data in grid
        {
            data_grid.ItemsSource = databaseLoader.GetOffersData().DefaultView;
        }
        private void change_selected_record(object sender, SelectionChangedEventArgs e)
        {

        }

        private void change_header_name(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "name")
            {
                e.Column.Header = "الاسم";
            }
            else if (e.PropertyName == "offer_name")
            {
                e.Column.Header = "اسم العرض";
            }
            else if (e.PropertyName == "start_date")
            {
                e.Column.Header = "تاريخ الاشتراك";
            }
            else if (e.PropertyName == "end_date")
            {
                e.Column.Header = "ناريخ الانتهاء";
            }
            else if (e.PropertyName == "hours")
            {
                e.Column.Header = "ساعات العرض";
            }
            else if (e.PropertyName == "left_hours")
            {
                e.Column.Header = "الساعات المتبقية ";
            }
            else if (e.PropertyName == "cost")
            {
                e.Column.Header = "التكلفة";
            }
        }

    }
}
