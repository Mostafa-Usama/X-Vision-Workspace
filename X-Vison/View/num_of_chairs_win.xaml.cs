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
using System.Data.SQLite;

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
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT num_chairs FROM chairs";
                using (var command = new SQLiteCommand(query, connection))
                {
                 
                    string num = Convert.ToString(command.ExecuteScalar());
                    num_chairs_input.Text = num;

                }
            }
        }

        private void Save_num_chairs(object sender, RoutedEventArgs e)
        {
            int chairs;
            bool isNumber = int.TryParse(num_chairs_input.Text , out chairs);
            if (isNumber && chairs > 0)
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    string query = "UPDATE chairs SET num_chairs = @chairs";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@chairs", chairs);

                        command.ExecuteScalar();
                        
                    }
                }
                this.Close();

            }
            else
            {
                MessageBox.Show("برجاء ادخال ارقام صحيحة "," خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
    }
}
