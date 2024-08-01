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
        public Product_Info(int id)
        {
            InitializeComponent();
            productId = id;
        }

        private void load_data(object sender, RoutedEventArgs e)
        {
            Tuple<string, int, double, double> data = databaseLoader.GetProductData(productId);
            product_name_label.Text = data.Item1;
            amount_label.Text = data.Item2.ToString();
            purchase_cost_label.Text = data.Item3.ToString() + "  جنيه";
            sell_cost_label.Text = data.Item4.ToString() + "  جنيه";
        }

        private void save_product(object sender, RoutedEventArgs e)
        {

        }
    }
}
