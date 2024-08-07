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
    /// Interaction logic for Edit_Product.xaml
    /// </summary>
    public partial class Edit_Product : Window
    {
        public bool isClicked = false;
        string productName;
        string amount;
        string purchaseCost;
        string sellCost;
        string product_tpye_edit;

        List<string> type_of_product = new List<string>()
        {
            "مشروبات ساخنة",
            "مشروبات باردة",
            "باتيه و بسكوت",
            "شيبسي",
            "منتجات اخرى",
        };

        public Edit_Product(string name, string am, string pCost, string sCost ,string product_type)
        {
            InitializeComponent();
            product_type_combo.ItemsSource = type_of_product;
            productName = name;
            amount = am;
            purchaseCost = pCost;
            sellCost = sCost;
            product_tpye_edit = product_type;
        }

        private void load_data(object sender, RoutedEventArgs e)
        {
            product_name_input.Text = productName;
            amount_input.Text = amount;
            purchase_cost_input.Text = purchaseCost;
            sell_cost_input.Text = sellCost;
            product_type_combo.SelectedItem = product_tpye_edit;
        }
        private void edit_product(object sender, RoutedEventArgs e)
        {
            int am;
            double purchaseCost;
            double sellCost;

            if (product_name_input.Text.Trim() == "" || purchase_cost_input.Text.Trim() == "" || sell_cost_input.Text.Trim() == "" || amount_input.Text.Trim() == "" || product_type_combo.SelectedItem == null)
            {
                MessageBox.Show("برجاء ادخال جميع الحقول ", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            bool isNumber1 = double.TryParse(purchase_cost_input.Text, out purchaseCost);
            bool isNumber2 = double.TryParse(sell_cost_input.Text, out sellCost);
            bool isNumber3 = int.TryParse(amount_input.Text, out am);
            product_tpye_edit = product_type_combo.SelectedItem.ToString();
            if (!isNumber1 || !isNumber2 || !isNumber3)
            {
                MessageBox.Show("برجاء ادخال ارقام صحيحة ", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Dictionary<string, object> data = new Dictionary<string, object>{
                {"product_name", product_name_input.Text.Trim()},
                {"amount", am},
                {"purchase_cost", purchaseCost},
                {"sell_cost",  sellCost},
                {"product_type" , product_tpye_edit }
            };
            databaseLoader.UpdateData("kitchen", data, String.Format("product_name = \"{0}\" ", productName));
            isClicked = true;
            this.Close();
        }

    }
}
