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
    /// Interaction logic for logout.xaml
    /// </summary>
    public partial class logout : Window
    {
        public int chairNum;
        public int user_id;
        public bool isClicked = false;

        string name;
        int price;
        int kitchen_cost;
        int total_cost;
        int paid_money;
        string start_date;
        string leave_date;

        public logout()
        {
            InitializeComponent();
            
        }


        private void load_data(object sender, RoutedEventArgs e)
        {   
            Tuple<string, string, bool> data = databaseLoader.GetUserDataByChairNum(chairNum);
            name = data.Item1;
            start_date = data.Item2;
            bool hasOffer = data.Item3;
            leave_date = DateTime.Now.ToString("MM/dd/yyyy h:mm tt");
            //MessageBox.Show(start_date+"\n"+ now);
            TimeSpan duration = DateTime.Parse(leave_date) - DateTime.Parse(start_date);
            string hours = duration.Hours.ToString();
            string mintues = duration.Minutes.ToString();
           // MessageBox.Show(duration);
            price = databaseLoader.GetPriceByDuration(int.Parse(hours));
            kitchen_cost = 0;// لحد دلوقتي بس 
            total_cost = price + kitchen_cost;


            userName.Text = name;
            enterDate.Text = DateTime.Parse(start_date).ToString("h:mm tt");
            leaveDate.Text = DateTime.Parse(leave_date).ToString("h:mm tt");
            duration_stayed.Text = hours + "  ساعة    " + mintues + "  دقيقة" ;
            cost.Text = price.ToString();
            offer.Text = hasOffer ? "يوجد" : "لا يوجد";// هيرحع الاسم ولا الفترة المتبقية ولا ايه
            kitchen.Text = kitchen_cost.ToString(); // لحد دلوقتي بس
            total.Text = total_cost.ToString();

        }

        private void logout_user(object sender, RoutedEventArgs e)
        {

            if (paid.Text.Trim() == "")
            {
                MessageBox.Show("برجاء ادخال جميع الحقول ", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
                return; 
            }

            bool isNumber = int.TryParse(paid.Text, out paid_money);
            if (isNumber)
            {
                Dictionary<string,object> data = new Dictionary<string,object>{
                    {"user_id", user_id},
                    {"enter_date", start_date},
                    {"leave_date", leave_date},
                    {"type", "مقعد"},
                    {"reservation_cost", price},
                    {"kitchen", kitchen_cost},
                    {"total", total_cost},
                    {"paid", paid_money},
                };

                databaseLoader.DeleteRecord("active_users", "user_id", user_id.ToString());
                databaseLoader.InsertRecord("user_records", data);
                isClicked = true;
                // remove record from active users
                // add record to user records 
                // make chair empty
                this.Close();
            }
            else{
                 MessageBox.Show("برجاء ادخال ارقام صحيحة ", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
