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
    /// Interaction logic for num_of_chairs_win.xaml
    /// </summary>
    public partial class num_of_chairs_win : Window
    {
        private string _connectionString = "Data Source=database.db;Version=3;";

        public num_of_chairs_win()
        {

            InitializeComponent();
            load_old();
        }

        private void load_old()
        {
            num_chairs_input.Text = Convert.ToString(databaseLoader.SelectData("chairs", "num_chairs")[0]);
            num_chairs_input.Focus();
            num_chairs_input.CaretIndex = num_chairs_input.Text.Length;
        }

        private void Save_num_chairs(object sender, RoutedEventArgs e)
        {
            int chairs;
            bool isNumber = int.TryParse(num_chairs_input.Text , out chairs);
            if (isNumber && chairs > 0)
            {
              
                Dictionary<string, object> data = new Dictionary<string,object>{
                    {"num_chairs", chairs}
                };
                databaseLoader.UpdateData("chairs", data, "TRUE");
  
                this.Close();

            }
            else
            {
                MessageBox.Show("برجاء ادخال ارقام صحيحة "," خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
                num_chairs_input.Focus();

            }
            
        }
    }
}
