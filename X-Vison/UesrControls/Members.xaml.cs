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

namespace Center_Maneger.UesrControls
{
    /// <summary>
    /// Interaction logic for Members.xaml
    /// </summary>
    public partial class Members : UserControl
    {
        public Members()
        {
            InitializeComponent();
            
        }

        public void load_data() // display classes data in grid
        {
           
            data_grid.ItemsSource = databaseLoader.GetUserData().DefaultView;
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
        }

    }
}
