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
using System.Globalization;
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
        public string class_name;

        string name;
        int price;
        int kitchen_cost;
        int total_cost;
        int paid_money;
        string start_date;
        string leave_date;
        int offer_id;
        string window;

        public logout(string win)
        {
            InitializeComponent();
            window = win;
        }


        private void load_data(object sender, RoutedEventArgs e)
        {
            if (window == "chair")
            {
                Tuple<string, string> data = databaseLoader.GetUserDataByChairNum(chairNum);
                name = data.Item1;
                start_date = data.Item2;
                List <object> offers = databaseLoader.SelectData("user_offer", "offer_id", String.Format("user_id = {0} AND is_expired = 0",user_id));
                offer_id = offers.Count == 0 ? 0 : Convert.ToInt32(offers[0]);

                start_date = Convert.ToString(databaseLoader.SelectData("active_users", "enter_date", String.Format("user_id = {0} ", user_id))[0]);

            }
            else
            {
                name = Convert.ToString(databaseLoader.SelectData("users", "name", String.Format(" id = \"{0}\" ", user_id))[0]);
                start_date = Convert.ToString(databaseLoader.SelectData("user_class", "enter_date", String.Format("user_id = {0} ", user_id))[0]);

            }
            leave_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            
            //MessageBox.Show(start_date+"\n"+ now);
            TimeSpan duration = DateTime.Parse(leave_date) - DateTime.Parse(start_date);
            string hours = ((int)duration.TotalHours).ToString();
            string mintues = ( (int)((duration.TotalHours - (int)duration.TotalHours) * 60) ).ToString();
           // MessageBox.Show(duration);
            if (window == "chair")
            {
                price = databaseLoader.GetPriceByDuration(int.Parse(hours));           

            }
            else
            {
                int total_hours = int.Parse(hours) == 0 ? 1 : int.Parse(hours);
                //int classId = Convert.ToInt32(databaseLoader.SelectData("user_class", "class_id", String.Format("user_id = \"{0}\"", user_id))[0]);
                price = total_hours* (Convert.ToInt32(databaseLoader.SelectData("classes", "cost", String.Format("class_name = \"{0}\"", class_name))[0]));
            }
            kitchen_cost = 0;// لحد دلوقتي بس 
            total_cost = price + kitchen_cost;


            userName.Text = name;
            enterDate.Text = DateTime.Parse(start_date).ToString("h:mm tt", CultureInfo.CreateSpecificCulture("ar-EG"));
            leaveDate.Text = DateTime.Parse(leave_date).ToString("h:mm tt", CultureInfo.CreateSpecificCulture("ar-EG"));
            duration_stayed.Text = hours + "  ساعة    " + mintues + "  دقيقة" ;
            
            if (window == "chair")
            {
                if (offer_id != 0)
                {
                    int left_hours = Convert.ToInt32(databaseLoader.SelectData("user_offer", "spent_hours", String.Format("user_id = {0} AND is_expired = 0",user_id))[0]);
                    offer.Text = "عدد الساعات المستهلكة: " + left_hours.ToString();
                    price = 0;
                }
                else
                {
                    offer.Text = "لا يوجد عرض";
                }

            }
            cost.Text = price.ToString();
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
                    {"type", window == "chair"? "مقعد": "غرفة"},
                    {"reservation_cost", price},
                    {"kitchen", kitchen_cost},
                    {"total", total_cost},
                    {"paid", paid_money},
                };
                if (window == "chair")
                {
                    databaseLoader.DeleteRecord("active_users", "user_id", user_id.ToString());
                }
                else
                {
                    databaseLoader.DeleteRecord("user_class", "user_id", user_id.ToString());

                }
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
