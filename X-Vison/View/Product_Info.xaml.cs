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
    /// Interaction logic for Product_Info.xaml
    /// </summary>
    public partial class Product_Info : Window
    {
        int productId;
        public bool isClicked = false;
        public Product_Info(int id)
        {
            InitializeComponent();
            productId = id;
        }

        private void load_data(object sender, RoutedEventArgs e)
        {
           
            Tuple<string, int, double, double,string> data = databaseLoader.GetProductData(productId);
            product_name_label.Text = data.Item1;
            amount_input.Text = data.Item2.ToString();
            purchase_cost_label.Text = data.Item3.ToString() + "  جنيه";
            sell_cost_label.Text = data.Item4.ToString() + "  جنيه";
            amount_input.Focus();
            amount_input.CaretIndex = amount_input.Text.Length;
        }

        private void save_product(object sender, RoutedEventArgs e)
        {
            int numOfProducts;
            bool isNumber = int.TryParse(amount_input.Text, out numOfProducts);
            if (!isNumber || numOfProducts < 0)
            {
                MessageBox.Show("برجاء ادخال ارقام صحيحة ", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Dictionary<string, object> data = new Dictionary<string, object>{
                {"amount", numOfProducts},
            };
            databaseLoader.UpdateData("kitchen", data, String.Format("id = {0} ", productId));
            isClicked = true;
            this.Close();

        }
    }
}
