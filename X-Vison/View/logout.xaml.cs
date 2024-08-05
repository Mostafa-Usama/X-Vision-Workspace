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
        double price;
        double kitchen_cost = 0;
        double total_cost;
        double paid_money;
        string start_date;
        string leave_date;
        int offer_id;
        string window;
        bool expired = false;
        
        public logout(string win)
        {
            InitializeComponent();
            window = win;
        }


        private void load_data(object sender, RoutedEventArgs e)
        {
            string note;
            if (window == "chair")
            {
                Tuple<string, string> data = databaseLoader.GetUserDataByChairNum(chairNum);
                name = data.Item1;
                start_date = data.Item2;
                List <object> offers = databaseLoader.SelectData("user_offer", "offer_id", String.Format("user_id = {0} AND is_logged_out = 0",user_id));
                offer_id = offers.Count == 0 ? 0 : Convert.ToInt32(offers[0]);
                note = Convert.ToString(databaseLoader.SelectData("active_users", "note", String.Format("user_id = {0} ", user_id))[0]);
                
                start_date = Convert.ToString(databaseLoader.SelectData("active_users", "enter_date", String.Format("user_id = {0} ", user_id))[0]);

            }
            else
            {
                name = Convert.ToString(databaseLoader.SelectData("users", "name", String.Format(" id = \"{0}\" ", user_id))[0]);
                start_date = Convert.ToString(databaseLoader.SelectData("user_class", "enter_date", String.Format("user_id = {0} ", user_id))[0]);
                note = Convert.ToString(databaseLoader.SelectData("user_class", "note", String.Format("user_id = {0} ", user_id))[0]);

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
             var value = databaseLoader.SelectData("user_kitchen", "SUM(cost)", String.Format("user_id = {0} AND is_logged_out = 0", user_id))[0];
             kitchen_cost = value == DBNull.Value? 0 : Convert.ToDouble(value);
             //= true ? 0 : Convert.ToDouble(kitCost[0]); // لحد دلوقتي بس 
            total_cost = price + kitchen_cost;


            userName.Text = name;
            enterDate.Text = DateTime.Parse(start_date).ToString("h:mm tt", CultureInfo.CreateSpecificCulture("ar-EG"));
            leaveDate.Text = DateTime.Parse(leave_date).ToString("h:mm tt", CultureInfo.CreateSpecificCulture("ar-EG"));
            duration_stayed.Text = hours + "  ساعة    " + mintues + "  دقيقة" ;
            
            if (window == "chair")
            {
                if (offer_id != 0)
                {
                    int is_expired = Convert.ToInt32(databaseLoader.SelectData("user_offer", "is_expired",String.Format("user_id = {0} AND is_logged_out = 0",user_id))[0]);

                    if (is_expired == 0)
                    {
                        int left_hours = Convert.ToInt32(databaseLoader.SelectData("user_offer", "spent_hours", String.Format("user_id = {0} AND is_logged_out = 0", user_id))[0]);
                        offer.Text = "عدد الساعات المستهلكة: " + left_hours.ToString();
                        price = 0;
                    }
                    else
                    {
                        expired = true;
                        DateTime expireyDate = Convert.ToDateTime(databaseLoader.SelectData("user_offer", "expirey_date", String.Format("user_id = {0} AND is_logged_out = 0 AND is_expired = 1", user_id))[0]);
                        TimeSpan durationSinceExpirey = DateTime.Parse(leave_date) - expireyDate;
                        string hoursSinceExpirey = ((int)durationSinceExpirey.TotalHours).ToString();
                        string mintuesSinceExpirey = ((int)((durationSinceExpirey.TotalHours - (int)durationSinceExpirey.TotalHours) * 60)).ToString();

                        offer.Text = "تم استهلاك الباقة منذ  " + hoursSinceExpirey +"  ساعة  " + mintuesSinceExpirey + "  دقيقة";
                        price = databaseLoader.GetPriceByDuration(int.Parse(hoursSinceExpirey));  
                    }
                
                }
                else
                {
                    offer.Text = "لا يوجد عرض";
                }

            }
            else
            {
                offerLabel.Visibility = Visibility.Collapsed;
                offer.Visibility = Visibility.Collapsed;
            }
            cost.Text = price.ToString()+ " جنيه";
            total_cost = price + kitchen_cost ;
            kitchen.Text = kitchen_cost.ToString() + " جنيه"; // لحد دلوقتي بس
            total.Text = total_cost.ToString() + " جنيه";

            if (!string.IsNullOrEmpty(note))
            {
                note_btn.Visibility = Visibility.Visible;
            }
            //else
            //{
            //    note_btn.Visibility = Visibility.Visible;
            //}
        }

        private void logout_user(object sender, RoutedEventArgs e)
        {

            if (paid.Text.Trim() == "")
            {
                MessageBox.Show("برجاء ادخال جميع الحقول ", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
                return; 
            }

            bool isNumber = double.TryParse(paid.Text, out paid_money);
            if (isNumber)
            {
               
                if (window == "chair")
                {
                    databaseLoader.DeleteRecord("active_users", "user_id", user_id.ToString());
                    if (expired)
                    {
                        Dictionary<string, object> loggedOut = new Dictionary<string,object>{
                            {"is_logged_out", 1},
                        };
                        databaseLoader.UpdateData("user_offer", loggedOut, String.Format("user_id = {0} AND is_logged_out = 0", user_id));
                    }
                }
                else
                {
                    databaseLoader.DeleteRecord("user_class", "user_id", user_id.ToString());

                }
                Dictionary<string, object> data = new Dictionary<string, object>{
                    {"user_id", user_id},
                    {"enter_date", start_date},
                    {"leave_date", leave_date},
                    {"type", window == "chair"? "مقعد": "غرفة"},
                    {"reservation_cost", price},
                    {"kitchen", kitchen_cost},
                    {"total", total_cost},
                    {"paid", paid_money},
                };
                databaseLoader.InsertRecord("user_records", data);

                Dictionary<string, object> logged_out = new Dictionary<string, object>{
                    {"is_logged_out", 1}
                };
                databaseLoader.UpdateData("user_kitchen", logged_out, String.Format("user_id = {0} AND is_logged_out = 0", user_id));
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

        private void show_note(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            User_Note userNoteWin = new User_Note(window);
            if (window =="chair")
            {
                userNoteWin.chairNum = chairNum;
            }
            else 
            { 
                userNoteWin.userId = user_id;
            }
            
            
            userNoteWin.ShowDialog();
        }
    }
}
