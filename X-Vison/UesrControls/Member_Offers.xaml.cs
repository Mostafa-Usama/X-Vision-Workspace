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
using System.Data;
namespace Center_Maneger.UesrControls
{
    /// <summary>
    /// Interaction logic for Member_Offers.xaml
    /// </summary>
    public partial class Member_Offers : UserControl
    {
        private DataRowView selectedRow;

        public Member_Offers()
        {
            InitializeComponent();
            
        }

        public void load_data(bool isChecked = false) // display classes data in grid
        {
            data_grid.ItemsSource = databaseLoader.GetOffersData(isChecked).DefaultView;
            data_grid.Columns[0].Visibility = Visibility.Collapsed;
            data_grid.Columns[8].Visibility = Visibility.Collapsed;

            DataView offersView = data_grid.ItemsSource as DataView;
            DataTable offersRecords = offersView.Table;
            foreach (DataRow row in offersRecords.Rows)
            {

                row["start_date"] = DateTime.Parse(row["start_date"].ToString()).ToString("MM/dd/yyyy h:mm tt");
                row["end_date"] = DateTime.Parse(row["end_date"].ToString()).ToString("MM/dd/yyyy h:mm tt");

            }

        }

        private void change_selected_record(object sender, SelectionChangedEventArgs e)
        {
            selectedRow = data_grid.SelectedItem as DataRowView; 

        }

        private void change_header_name(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "name")
            {
                e.Column.Header = "اسم العضو";
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

        private void loadOffers(object sender, RoutedEventArgs e)
        {
            bool isChecked = Convert.ToBoolean(expired_offers.IsChecked);
            load_data(isChecked);
        }

        private void deleteOffer(object sender, RoutedEventArgs e)
        {
            if (selectedRow != null) // if a record is selected 
            {

                var result = MessageBox.Show("هل أنت متأكد من الغاء هذا العرض؟", "تأكيد", MessageBoxButton.YesNo); // رسالة تأكيد
                if (result == MessageBoxResult.Yes) // if he chooses YES
                {
                    int is_expired= Convert.ToInt32((selectedRow["is_expired"]));
                    if (is_expired == 0) {

                        int user_id = Convert.ToInt32((selectedRow["user_id"]));
                        databaseLoader.DeleteRecord("user_offer", "user_id", user_id.ToString());
                        Dictionary<string, object> resetLastHour = new Dictionary<string,object>{
                            {"last_hour", 0}
                        };
                        databaseLoader.UpdateData("active_users", resetLastHour, String.Format("user_id = {0}",user_id));
                         Task.Run(() => MainWindow.timer_ElapsedAsync(null, null));
                        load_data(Convert.ToBoolean(expired_offers.IsChecked));
                        return;
                    }

                    else 
                    {
                        MessageBox.Show("لا يمكن الغاء عرض منتهي", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show("برجاء اختيار العرض الذي تريد حذفه ", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void SearchUser(object sender, TextChangedEventArgs e)
        {

            string searchname = searchTB.Text.Trim();
            if (string.IsNullOrEmpty(searchname))
            {
            DataView offersView = data_grid.ItemsSource as DataView;
            offersView.RowFilter = string.Empty;
            }

        }

        private void SearchUser(object sender, RoutedEventArgs e)
        {
            string searchname = searchTB.Text.Trim();
            DataView offersView = data_grid.ItemsSource as DataView;
            offersView.RowFilter = string.Format("name LIKE '{0}%'", searchname);
        }

    }
}
