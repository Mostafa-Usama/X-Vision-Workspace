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
    /// Interaction logic for faculty_control.xaml
    /// </summary>
    public partial class Faculty_Settings : UserControl
    {
        private DataGridColumn col = null;
        int counter = 0;
        private DataRowView selectedRow; // store cuurent selected row if i wanted to delete

        public Faculty_Settings()
        {
            InitializeComponent();
            load_data();
        }

        public void load_data() 
        {
            try
            {
                data_grid.ItemsSource = databaseLoader.LoadData("faculties").DefaultView;
            }
            catch (Exception)
            {
           
            }
            
        }
        

        private void new_faculty_record(object sender, RoutedEventArgs e)
        {
            name_of_faculty_input.Clear();
        }

        private void save_faculty_record(object sender, RoutedEventArgs e)
        {
            if (name_of_faculty_input.Text.Trim() == "")
            {
                MessageBox.Show("برجاء ادخال جميع الحقول ", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
                return; 
            }

            Dictionary<string, object> data = new Dictionary<string, object>
                {
                    {"faculty_name", name_of_faculty_input.Text}
                };

            try
            {
                databaseLoader.InsertRecord("faculties", data);
                load_data();
            }
            catch
            {
                MessageBox.Show("خطأ اثناء عملية الادخال", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        private void remove_faculty_record(object sender, RoutedEventArgs e)
        {
            if (selectedRow != null) // if a record is selected 
            {

                var result = MessageBox.Show("هل أنت متأكد من حذف هذا الصف؟", "تأكيد", MessageBoxButton.YesNo); // رسالة تأكيد
                if (result == MessageBoxResult.Yes) // if he chooses YES
                {
                    string facultyName = Convert.ToString((selectedRow["faculty_name"])); // بجيب اسم العمود اللي واقف عليه تقاطعا مع الصف اللي انا مختاره عشان اعرف أجيب اسم الكلاس
                    databaseLoader.DeleteRecord("faculties", "faculty_name", facultyName);
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
            if (e.PropertyName == "faculty_name")
            {
                e.Column.Header = "اسم الكلية";
            }
           
        }

        private void change_selected_record(object sender, SelectionChangedEventArgs e) // this event is called whenever you select a record (row) from the grid
        {
            selectedRow = data_grid.SelectedItem as DataRowView; // store the current selected row in the variable 

        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
            data_grid.HeadersVisibility = DataGridHeadersVisibility.Column;
        }

        private void grid_sort(object sender, DataGridSortingEventArgs e)
        {
            if (col == null || col != e.Column)
            {
                col = e.Column;
                counter = 1;
                return;
            }
            else
            {
                counter++;
                if (counter % 2 == 0)
                {
                    defualtSort(e.Column);
                    e.Handled = true;
                }
            }
        }

        private void defualtSort(DataGridColumn c)
        {
            c.SortDirection = null;
            System.ComponentModel.ICollectionView view = CollectionViewSource.GetDefaultView(data_grid.ItemsSource);
            if (view != null)
            {
                view.SortDescriptions.Clear();  // Clear any sort descriptions
                view.Refresh();  // Refresh the view to reset to the original order
            }
        }


    }
}
