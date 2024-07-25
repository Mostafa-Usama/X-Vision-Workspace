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
using System.Timers;

namespace Center_Maneger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UserControl[] userControls;
       

        public MainWindow()
        {
            InitializeComponent();
            
            userControls = new UserControl [] {
                new Class_Settings(),
                new Faculty_Settings(),
                new Job_Settings(),
                new Prices_Settings(),
                new Offer_Settings(),
                new Members(),
                new grid_of_chairs(),
                new grid_of_classes(),
                null, 
                new Member_Offers(),
                new User_Records(),
            };
            Timer timer = new Timer(500000);
            timer.Elapsed += timer_Elapsed;
            timer.AutoReset = true;
            timer.Enabled = true;
            timer_Elapsed(null, null);   
        }

       public static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {

            List<Tuple<int, string, int, int, int>> userIds = databaseLoader.GetUserIdWithOffers();

            foreach (var userId in userIds)
            {
                updateOffers(userId.Item1, userId.Item2, userId.Item3, userId.Item4, userId.Item5);
            }
        }

        public static void updateOffers(int user_id, string enter_date, int last_hour, int left_hours, int spent_hours)
        {

            // ميحسبش من التكلفة طول ما الباقة شغال
            // الباقة تخلص تاريخ

            DateTime now = DateTime.Now;
            int hours = Convert.ToInt32((now - DateTime.Parse(enter_date)).TotalHours);
            int duration = hours - last_hour;
            if (hours > last_hour)
            {

                Dictionary<string, object> newLastHour = new Dictionary<string,object>{
                    {"last_hour", hours}
                };
                databaseLoader.UpdateData("active_users", newLastHour, String.Format("user_id = {0}", user_id));



                Dictionary<string, object> newLeftHours = new Dictionary<string,object>{
                    {"left_hours", left_hours - duration}
                };
                databaseLoader.UpdateData("user_offer", newLeftHours, String.Format("user_id = {0} AND is_expired = 0", user_id));



                Dictionary<string, object> newSpentHours = new Dictionary<string, object>{
                    {"spent_hours", spent_hours + duration}
                };
                databaseLoader.UpdateData("user_offer", newSpentHours, String.Format("user_id = {0} AND is_expired = 0", user_id));

            }
        }

        private void num_chairs_btn(object sender, RoutedEventArgs e)
        {
           num_of_chairs_win Chair =   new num_of_chairs_win(); 
            Chair.ShowDialog(); // so user cant interact with the app unless he closes this window
        }



        private void openUserControl (object sender, RoutedEventArgs e)
        {
            Button clickedBtn = sender as Button;

            int idx = int.Parse(clickedBtn.Name.Remove(0, 3));
            string header = Convert.ToString(clickedBtn.Tag);
            TabItem newTab = new TabItem
            {
                Header = header,
                Content = userControls[idx],
                Style = (Style)Resources["dynamic_tabs"]
            };
     
            settings_controls.Items.Add(newTab);
            settings_controls.SelectedItem = newTab;
        }

        private void settings_clicked(object sender, MouseButtonEventArgs e)
        {


            checkPassword checkPass = new checkPassword();
            checkPass.ShowDialog();

            if (checkPass.correct)
            {
                settings_tab.SelectedIndex = 0;
            }
            else
            {
                settings_tab.SelectedIndex = 1;
            }

        }

        private void close_tab(object sender, RoutedEventArgs e)
        {
            Button clickedBtn = sender as Button; // pressed btn
            StackPanel stack = clickedBtn.Parent as StackPanel; // parent stackpanel
            TabItem tab = null;
            foreach (TabItem item in settings_controls.Items) // loop over all taps 
            {
                if (Convert.ToString(item.Header)== ((TextBlock)stack.Children[0]).Text) // if the header is the same as the textbox then this is the tab we want to close
                {
                    tab = item;
                    break;
                }
            }
            settings_controls.Items.Remove(tab);
      
        }

        private void offer_window(object sender, RoutedEventArgs e)
        {
            search_and_add_customer win = new search_and_add_customer("offer");
            win.ShowDialog();
        }

       
    }
}
