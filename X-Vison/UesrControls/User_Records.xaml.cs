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
    /// Interaction logic for User_Records.xaml
    /// </summary>
    public partial class User_Records : UserControl
    {
      
        public User_Records()
        {
            InitializeComponent();
        }
        private void setButtonStyle()
        {
           
        }

        private void load_data(DataTable userTable, DataTable offers)
        {   
            data_grid.ItemsSource = userTable.DefaultView;
            data_grid.Columns[3].Width = data_grid.Columns[4].Width = data_grid.Columns[0].Width = 200;

           
            offers_grid.ItemsSource = offers.DefaultView;
            offers_grid.Columns[0].Width = offers_grid.Columns[2].Width = 200;

            calculateCosts();
        }

        private void calculateCosts()
        {
            DataView userView = data_grid.ItemsSource as DataView;
            DataTable userRecords = userView.Table;

            long reservationCost = 0;
            long kitchenCost = 0;
            long totalCost = 0;
            long paidCost = 0;
            
            foreach (DataRow row in userRecords.Rows)
            {
                reservationCost += Convert.ToInt64(row["reservation_cost"]);
                kitchenCost += Convert.ToInt64(row["kitchen"]);
                paidCost += Convert.ToInt64(row["paid"]);
                row["enter_date"] = DateTime.Parse(row["enter_date"].ToString()).ToString("MM/dd/yyyy h:mm tt");
                row["leave_date"] = DateTime.Parse(row["leave_date"].ToString()).ToString("MM/dd/yyyy h:mm tt");
            }
            totalCost = reservationCost + kitchenCost;

            totalReservationCost.Text = "اجمالي التكلفة = " + reservationCost.ToString();
            totalKitchenCost.Text = "اجمالي البوفيه = " + kitchenCost.ToString(); 
            total.Text = "الاجمالي = " + totalCost.ToString();
            totalPaid.Text = "اجمالي المدفوع = " + paidCost.ToString();


            DataView offersView = offers_grid.ItemsSource as DataView;
            DataTable offersRecords = offersView.Table;

            long offerCost = 0;
            foreach (DataRow row in offersRecords.Rows)
            {
                offerCost += Convert.ToInt64(row["cost"]);
                row["start_date"] = DateTime.Parse(row["start_date"].ToString()).ToString("MM/dd/yyyy h:mm tt");

            }
            totalOffer.Text = "اجمالي الباقات = " + offerCost.ToString();

        }
        private void change_header_name(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "name")
            {
                e.Column.Header = "اسم العضو";
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
            else if (e.PropertyName == "offer_name")
            {
                e.Column.Header = "اسم العرض";
            }
            else if (e.PropertyName == "start_date")
            {
                e.Column.Header = "تاريخ الاشتراك";
            }
            else if (e.PropertyName == "cost")
            {
                e.Column.Header = "التكلفة";
            }
           
        }

        private void showRecords(object sender, RoutedEventArgs e)
        {
            string from_date = fromDate.Text;
            string to_date = toDate.Text;
            DateTime from, to;
            if (from_date.Trim() == "" || to_date.Trim() == "")
            {
                MessageBox.Show("برجاء ادخال جميع الحقول ", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            bool isFromDate = DateTime.TryParse(from_date, out from);
            bool isToDate = DateTime.TryParse(to_date, out to);

            if (!isFromDate || !isToDate)
            {
                MessageBox.Show("برجاء ادخال تواريخ صحيحة", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (from > to)
            {
                MessageBox.Show("تاريخ البداية أكبر من تاريخ النهاية,  برجاء ادخال تواريخ صحيحة", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            to = to.AddDays(1).AddSeconds(-1);

            DataTable userRecordsTable = databaseLoader.GetUserRecords(from, to);
            DataTable offers = databaseLoader.GetOffersData(from, to);


            load_data(userRecordsTable, offers);
        }

        private void setStyle(object sender, RoutedEventArgs e)
        {
            //var datePickerButton = fromDate.Template.FindName("PART_Button", fromDate) as Button;
            //datePickerButton.Style = (Style)FindResource("DatePickerButtonStyle");
        }
    }
}
