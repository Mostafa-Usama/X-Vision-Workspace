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
    /// Interaction logic for Edit_User.xaml
    /// </summary>
    public partial class Edit_User : Window
    {
        public bool isClicked = false;
        string name;
        string phone;
        string faculty;
        string job;
        string level;


        public Edit_User(string n,string p, string f, string j, string l)
        {
            InitializeComponent();
            name = n;
            phone = p;
            faculty = f;
            job = j;
            level = l;
        }

        private void edit_user(object sender, RoutedEventArgs e)
        {
            int jobId = Convert.ToInt32(databaseLoader.SelectData("jobs", "id", string.Format("job_name = \"{0}\" ", JobComboBox.SelectedItem.ToString()))[0]);
            int facultyId = Convert.ToInt32(databaseLoader.SelectData("faculties", "id", string.Format("faculty_name = \"{0}\" ", FacultyComboBox.SelectedItem.ToString()))[0]);
            Dictionary<string, object> data = new Dictionary<string, object>{
            {"name", userNameTextBox.Text.Trim()},
            {"phone", phoneTextBox.Text.Trim()},
            {"faculty_id", facultyId},
            {"job_id", jobId},
            {"level", levelTextBox.Text.Trim()}
            };
            databaseLoader.UpdateData("users", data, String.Format("phone = \"{0}\" ", phone));
            isClicked = true;
            this.Close();
        }

        private void load_data(object sender, RoutedEventArgs e)
        {
            List<object> faculties = databaseLoader.SelectData("faculties", "faculty_name", "");
            List<object> jobs = databaseLoader.SelectData("jobs", "job_name", "");
            FacultyComboBox.ItemsSource = faculties;
            JobComboBox.ItemsSource = jobs;
            userNameTextBox.Text = name;
            phoneTextBox.Text = phone;
            levelTextBox.Text = level;
            FacultyComboBox.SelectedItem = faculty;
            JobComboBox.SelectedItem = job;
        
        }
    }
}
