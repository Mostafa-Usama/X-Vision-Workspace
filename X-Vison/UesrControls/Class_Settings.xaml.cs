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
using System.Data.SQLite;
using System.Data;

namespace Center_Maneger.UesrControls
{
    /// <summary>
    /// Interaction logic for Class_Settings.xaml
    /// </summary>
    public partial class Class_Settings : UserControl
    {
        

        private DataRowView selectedRow; // store cuurent selected row if i wanted to delete

        public Class_Settings()
        {
            InitializeComponent();

            load_data();
           
        }

        public void load_data() // display classes data in grid
        {
            try
            {
                 data_grid.ItemsSource = databaseLoader.LoadData("classes").DefaultView;   
            }
            catch (Exception)
            {
                

            }

        }
        

        private void new_class_record(object sender, RoutedEventArgs e) // زرار جديد بينضف مكان التكست بوكس
        {
            name_of_class_input.Clear();
            cost_of_class_input.Clear();
        }

        private void save_class_record(object sender, RoutedEventArgs e)  // save new class record in database
        {
            int classCost;
            if (name_of_class_input.Text.Trim() == "" || cost_of_class_input.Text.Trim() == "")  // make sure textboxes are not empty
            {
                MessageBox.Show("برجاء ادخال جميع الحقول ", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
                return; 
            }
            bool isNumber = int.TryParse(cost_of_class_input.Text, out classCost); // make sure cost of class is a number larger than 0
            if (isNumber && classCost > 0)
            {
                Dictionary<string, object> data = new Dictionary<string, object>
                {
                    {"class_name", name_of_class_input.Text},
                    {"cost", classCost}
                };
                try
                {
                    databaseLoader.InsertRecord("classes", data);
                    load_data();
                }
                catch
                {
                    MessageBox.Show("خطأ اثناء عملية الادخال", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }
            else
            {
                MessageBox.Show("برجاء ادخال ارقام صحيحة ", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        

        private void remove_class_record(object sender, RoutedEventArgs e) // reomve class record from grid and database
        {

            if (selectedRow != null) // if a record is selected 
            {


                var result = MessageBox.Show("هل أنت متأكد من حذف هذا الصف؟", "تأكيد", MessageBoxButton.YesNo); // رسالة تأكيد
                if (result == MessageBoxResult.Yes) // if he chooses YES
                {
                    string className = Convert.ToString((selectedRow["class_name"])); // بجيب اسم العمود اللي واقف عليه تقاطعا مع الصف اللي انا مختاره عشان اعرف أجيب اسم الكلاس
                    int id = Convert.ToInt32(databaseLoader.SelectData("classes", "id", String.Format("class_name = \"{0}\" ", className))[0]);
                    List<object> class_user_id = databaseLoader.SelectData("user_class", "user_id", String.Format("class_id= {0}", id));
                    if (class_user_id.Count != 0 )
                    {
                        MessageBox.Show("لا يمكن الحذف, الغرفة محجوزة", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    databaseLoader.DeleteRecord("classes", "class_name", className);
                    load_data();
                }
            }
          
            else
            {
                MessageBox.Show("برجاء اختيار الصف الذي تريد حذفه ", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void change_selected_record(object sender, SelectionChangedEventArgs e) // this event is called whenever you select a record (row) from the grid
        {
            selectedRow = data_grid.SelectedItem as DataRowView; // store the current selected row in the variable 

        }

        private void change_header_name(object sender, DataGridAutoGeneratingColumnEventArgs e) //Becuase the grid is dynamically generated in runtime (asynchronous) مقدرش أغير اسم العمود بالكود مباشرة لكن الفنكشن دي بتشتغل لما العمود يتم انشاءه وبعدها بقدر أغير الاسم 
        {
            if (e.PropertyName == "class_name")
            {
                e.Column.Header = "اسم الغرفة";
            }
            else if (e.PropertyName == "cost")
            {
                e.Column.Header = "تكلفة الساعة";
            }
        }

    }
}
