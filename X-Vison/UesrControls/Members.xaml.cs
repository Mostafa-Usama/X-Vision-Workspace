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
using Center_Maneger.View;

namespace Center_Maneger.UesrControls
{
    /// <summary>
    /// Interaction logic for Members.xaml
    /// </summary>
    public partial class Members : UserControl
    {
        private DataRowView selectedRow;

        public Members()
        {
            InitializeComponent();
            load_data();
        }

        public void load_data() // display classes data in grid
        {
           
            data_grid.ItemsSource = databaseLoader.GetUserData().DefaultView;
        }

        private void change_selected_record(object sender, SelectionChangedEventArgs e) // this event is called whenever you select a record (row) from the grid
        {
            selectedRow = data_grid.SelectedItem as DataRowView; // store the current selected row in the variable 

        }

        private void change_header_name(object sender, DataGridAutoGeneratingColumnEventArgs e) //Becuase the grid is dynamically generated in runtime (asynchronous) مقدرش أغير اسم العمود بالكود مباشرة لكن الفنكشن دي بتشتغل لما العمود يتم انشاءه وبعدها بقدر أغير الاسم 
        {
            if (e.PropertyName == "name")
            {
                e.Column.Header = "الاسم";
            }
            else if (e.PropertyName == "phone")
            {
                e.Column.Header = "رقم الهاتف";
            }
            else if (e.PropertyName == "faculty_name")
            {
                e.Column.Header = "الكلية";
            }
            else if (e.PropertyName == "job_name")
            {
                e.Column.Header = "الوظيفة";
            }
            else if (e.PropertyName == "level")
            {
                e.Column.Header = "السنة الدراسية";
            }
        }

        private void load(object sender, RoutedEventArgs e)
        {
            load_data();
            List<object> faculties = databaseLoader.SelectData("faculties", "faculty_name", "");
            List<object> jobs = databaseLoader.SelectData("jobs", "job_name", "");
            member_college.ItemsSource = faculties; 
            member_job.ItemsSource = jobs;
        }




        private void remove_member_record(object sender, RoutedEventArgs e)
        {
            if (selectedRow != null) // if a record is selected 
            {

                var result = MessageBox.Show("هل أنت متأكد من حذف هذا الصف؟", "تأكيد", MessageBoxButton.YesNo); // رسالة تأكيد
                if (result == MessageBoxResult.Yes) // if he chooses YES
                {
                    string phone = Convert.ToString((selectedRow["phone"])); // بجيب اسم العمود اللي واقف عليه تقاطعا مع الصف اللي انا مختاره عشان اعرف أجيب اسم الكلاس
                    int user_id = Convert.ToInt32(databaseLoader.SelectData("users", "id", String.Format("phone= \"{0}\" ", phone))[0]);

                    List<object> class_user_id = databaseLoader.SelectData("user_class", "user_id", String.Format("user_id = {0}", user_id));
                    List<object> active_user_id = databaseLoader.SelectData("active_users", "user_id", String.Format("user_id = {0}", user_id));

                    if (class_user_id.Count != 0 || active_user_id.Count != 0)
                    {
                        MessageBox.Show("هذا العضو محجوز له مقعد او غرفة بالفعل", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    databaseLoader.DeleteRecord("users", "phone", phone);
                    load_data();
                }
            }
            else
            {
                MessageBox.Show("برجاء اختيار الصف الذي تريد حذفه ", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void save_member_record(object sender, RoutedEventArgs e)
        {
            string name = member_name.Text;
            string phone = member_phone.Text;
            string faculty = member_college.SelectedItem == null ? "" : member_college.SelectedItem.ToString(); ;
            string level = member_level.Text;
            string job = member_job.SelectedItem == null ? "" : member_job.SelectedItem.ToString();
            int x;
            bool isNumber = int.TryParse(phone, out x);

            if (name.Trim() == "" || phone.Trim() == "" || faculty.Trim() == "" || level.Trim() == "" || job.Trim() == "")
            {
                MessageBox.Show("برجاء ادخال جميع الحقول ", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!isNumber || phone.Length != 11)
            {
                MessageBox.Show("برجاء ادخال رقم هاتف صحيح", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
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

            try
            {
                databaseLoader.InsertRecord("users", data);
                load_data();
            }
            catch
            {
                MessageBox.Show("خطأ اثناء عملية الادخال", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void new_member_record(object sender, RoutedEventArgs e)
        {
            member_name.Clear();
            member_college.SelectedItem= null;
            member_job.SelectedItem =null;
            member_phone.Clear();
            member_level.Clear();
        }

        private void edit_member_record(object sender, RoutedEventArgs e)
        {
            if (selectedRow != null) // if a record is selected 
            {
                
                string phone = Convert.ToString((selectedRow["phone"])); ;
                string name = Convert.ToString((selectedRow["name"]));
                string faculty = Convert.ToString((selectedRow["faculty_name"]));
                string job = Convert.ToString((selectedRow["job_name"]));
                string level = Convert.ToString((selectedRow["level"]));
                Edit_User editWin = new Edit_User(name, phone, faculty, job, level);
                editWin.ShowDialog();
                if (editWin.isClicked)
                {
                    load_data();
                }
            }
            else
            {
                MessageBox.Show("برجاء اختيار الصف الذي تريد تعديله ", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
