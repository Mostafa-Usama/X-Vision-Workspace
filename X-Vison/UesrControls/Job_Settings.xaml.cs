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
    /// Interaction logic for Job_Settings.xaml
    /// </summary>
    public partial class Job_Settings : UserControl
    {
        private DataRowView selectedRow;

        public Job_Settings()
        {
            InitializeComponent();
            load_data();

        }

        public void load_data() // display classes data in grid
        {
            try
            {
             data_grid.ItemsSource = databaseLoader.LoadData("jobs").DefaultView;

            }
            catch (Exception )
            {
                
            }
        }
        

        private void new_job_record(object sender, RoutedEventArgs e)
        {
            name_of_job_input.Clear();
        }

        private void save_job_record(object sender, RoutedEventArgs e)
        {
            if (name_of_job_input.Text.Trim() == "")
            {
                MessageBox.Show("برجاء ادخال جميع الحقول ", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Dictionary<string, object> data = new Dictionary<string, object>
                {
                    {"job_name", name_of_job_input.Text}
                };

            try
            {
                databaseLoader.InsertRecord("jobs", data);
                load_data();
            }
            catch
            {
                MessageBox.Show("خطأ اثناء عملية الادخال", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void remove_job_record(object sender, RoutedEventArgs e)
        {
            if (selectedRow != null) // if a record is selected 
            {

                var result = MessageBox.Show("هل أنت متأكد من حذف هذا الصف؟", "تأكيد", MessageBoxButton.YesNo); // رسالة تأكيد
                if (result == MessageBoxResult.Yes) // if he chooses YES
                {
                    string jobName = Convert.ToString((selectedRow["job_name"])); // بجيب اسم العمود اللي واقف عليه تقاطعا مع الصف اللي انا مختاره عشان اعرف أجيب اسم الكلاس
                    databaseLoader.DeleteRecord("jobs", "job_name", jobName);
                    load_data();
                }
            }

            else
            {
                MessageBox.Show("برجاء اختيار الصف الذي تريد حذفه ", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }




        private void change_header_name(object sender, DataGridAutoGeneratingColumnEventArgs e) //Becuase the grid is dynamically generated in runtime (asynchronous) مقدرش أغير اسم العمود بالكود مباشرة لكن الفنكشن دي بتشتغل لما العمود يتم انشاءه وبعدها بقدر أغير الاسم 
        {
            if (e.PropertyName == "job_name")
            {
                e.Column.Header = "اسم الوظيفة";
            }

        }

        private void change_selected_record(object sender, SelectionChangedEventArgs e) // this event is called whenever you select a record (row) from the grid
        {
            selectedRow = data_grid.SelectedItem as DataRowView; // store the current selected row in the variable 

        }
    }
}
