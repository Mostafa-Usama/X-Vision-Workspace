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
    /// Interaction logic for search_and_add_customer.xaml
    /// </summary>
    public partial class search_and_add_customer : Window
    {
        public int chairNum;
        public string className;

        public bool clickBtn =false ;
        string window;


        public search_and_add_customer(string win)
        {
            InitializeComponent();
            window = win;
            Section1.IsEnabled = true;
            Section2.IsEnabled = false;
        }

        private void search_checkbox(object sender, RoutedEventArgs e)
        {
            Section1.IsEnabled = true;
            Section2.IsEnabled = false;
        }

        private void new_checkbox(object sender, RoutedEventArgs e)
        {
            Section1.IsEnabled = false;
            Section2.IsEnabled = true;
        }

        private void UserComboBox_KeyUp(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Down || e.Key == Key.Up || e.Key == Key.Right|| e.Key == Key.Left || e.Key == Key.Back )
            {
                return;
            }
            var combo = sender as ComboBox;
            var textBox = combo.Template.FindName("PART_EditableTextBox", combo) as TextBox;

            if (textBox.Text.Trim() == "")
            {
                combo.IsDropDownOpen = false;
                return;
            }
            
                string filter = textBox.Text.ToLower();

                // Fetch filtered list from the database
                var filteredList = databaseLoader.GetUserNames(filter,combo.Tag.ToString());
                if (filteredList.Count == 0)
                {
                    combo.IsDropDownOpen = false;
                    return;
                }
                combo.ItemsSource = filteredList;
                combo.IsDropDownOpen = true;

                textBox.SelectionStart = textBox.Text.Length;
                textBox.SelectionLength = 0;
            
    }

        private void existing_user(object sender, RoutedEventArgs e)
        {
            
            string name = UserComboBox.SelectedItem == null? "" : UserComboBox.SelectedItem.ToString();
            string phone = PhoneComboBox.SelectedItem == null? "" : PhoneComboBox.SelectedItem.ToString();
            string enter_date = DateTime.Now.ToString("MM/dd/yyyy h:mm tt");
            add_active_user(name, phone, enter_date);
            
        }

        private void new_user(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;
            string phone = new_phoneTextBox.Text;
            string faculty = FacultyComboBox.SelectedItem == null ? "" : FacultyComboBox.SelectedItem.ToString(); ;
            string level = LevelTextBox.Text;
            string job = jobComboBox.SelectedItem == null ? "" : jobComboBox.SelectedItem.ToString();
            string enter_date = DateTime.Now.ToString("MM/dd/yyyy h:mm tt");

            if (name.Trim() == "" || phone.Trim() == "" || faculty.Trim() == "" || level.Trim() == "" || job.Trim() == "")
            {
                MessageBox.Show("برجاء ادخال جميع الحقول ", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int faculty_id = Convert.ToInt32(databaseLoader.SelectData("faculties", "id", String.Format("faculty_name = \"{0}\" ", faculty))[0]);
            int job_id = Convert.ToInt32(databaseLoader.SelectData("jobs", "id", String.Format("job_name = \"{0}\"", job))[0]);

            Dictionary<string, object> data = new Dictionary<string, object>
            {
                  {"name", name},
                  {"phone", phone},
                  {"faculty_id", faculty_id},
                  {"job_id", job_id},
                  {"level", level}
            };

            databaseLoader.InsertRecord("users", data);
            add_active_user(name, phone, enter_date);

        }



        private void add_active_user(string name, string phone, string enter_date)
        {
            if (phone != "")
            {
            int user_id = Convert.ToInt32(databaseLoader.SelectData("users", "id",String.Format("phone = \"{0}\" ",phone))[0]);

                if (window == "chair")
                {
                    Dictionary<string, object> data = new Dictionary<string, object>{
                        {"user_id", user_id},
                        {"enter_date", enter_date},
                        {"chair_num", chairNum}
                    };
                    try
                    {
                        databaseLoader.InsertRecord("active_users", data);
                        clickBtn = true;
                        this.Close();
                        return;
                    }
                    catch
                    {
                        MessageBox.Show("خطأ اثناء عملية الادخال", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);

                    }
                }
                else
                {
                    int classId = Convert.ToInt32(databaseLoader.SelectData("classes", "id", String.Format("class_name = \"{0}\"", className))[0]);

                    Dictionary<string, object> data = new Dictionary<string, object>{
                        {"class_id", classId},
                        {"user_id", user_id},
                        {"enter_date", enter_date}
                    };
                    try
                    {
                        databaseLoader.InsertRecord("active_users", data);
                        clickBtn = true;
                        this.Close();
                        return;
                    }
                    catch
                    {
                        MessageBox.Show("خطأ اثناء عملية الادخال", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);

                    }
                }
            }
            if (name != "")
            {
                int user_id = Convert.ToInt32(databaseLoader.SelectData("users", "id", String.Format("name = \"{0}\" ", name))[0]);
                List <object> id = databaseLoader.SelectData("users", "id", String.Format("name = \"{0}\" ",name));
                if (id.Count > 1)
                {
                    MessageBox.Show("يوجد تكرار في الاسم, الرجاء البحث برقم الهاتف", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;

                }
                if (window == "chair")
                {
                    Dictionary<string, object> data = new Dictionary<string, object>{
                        {"user_id", user_id},
                        {"enter_date", enter_date},
                        {"chair_num", chairNum}
                    };
                    try
                    {
                        databaseLoader.InsertRecord("active_users", data);
                        clickBtn = true;
                        this.Close();
                        return;
                    }
                    catch
                    {
                        MessageBox.Show("خطأ اثناء عملية الادخال", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);

                    }
                }
                else
                {
                    int classId = Convert.ToInt32(databaseLoader.SelectData("classes", "id", String.Format("class_name = \"{0}\"", className))[0]);

                    Dictionary<string, object> data = new Dictionary<string, object>{
                        {"class_id", classId},
                        {"user_id", user_id},
                        {"enter_date", enter_date}
                    };
                    try
                    {
                        databaseLoader.InsertRecord("user_class", data);
                        clickBtn = true;
                        this.Close();
                        return;
                    }
                    catch
                    {
                        MessageBox.Show("خطأ اثناء عملية الادخال", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);

                    }
                }
            }
            // add to active users;
            // reserve his chair

        }

        private void fill_combobox(object sender, RoutedEventArgs e)
        {
            List<object> faculties = databaseLoader.SelectData("faculties", "faculty_name", "");
            List<object> jobs = databaseLoader.SelectData("jobs", "job_name", "");
            FacultyComboBox.ItemsSource = faculties;
            jobComboBox.ItemsSource = jobs;
        }

        private void UserComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
