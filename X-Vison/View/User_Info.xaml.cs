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
    /// Interaction logic for User_Info.xaml
    /// </summary>
    public partial class User_Info : Window
    {
        public int userId;

        public User_Info()
        {
            InitializeComponent();
        }

      
        private void load_data(object sender, RoutedEventArgs e)
        {
            string name = Convert.ToString(databaseLoader.SelectData("users", "name", String.Format("id = {0}", userId))[0]);
            string phone = Convert.ToString(databaseLoader.SelectData("users", "phone", String.Format("id = {0}", userId))[0]);
            int facultyId = Convert.ToInt32(databaseLoader.SelectData("users", "faculty_id", String.Format("id = {0}", userId))[0]);
            int jobId = Convert.ToInt32(databaseLoader.SelectData("users", "job_id", String.Format("id = {0}", userId))[0]);
            string level = Convert.ToString(databaseLoader.SelectData("users", "level", String.Format("id = {0}", userId))[0]);
            string faculty = Convert.ToString(databaseLoader.SelectData("faculties", "faculty_name", String.Format("id = {0}", facultyId))[0]);
            string job = Convert.ToString(databaseLoader.SelectData("jobs", "job_name", String.Format("id = {0}", jobId))[0]);

            userName.Text = name;
            phoneText.Text = phone;
            facutlyText.Text = faculty;
            jobText.Text = job;
            levelText.Text = level;

        }
    }
}
