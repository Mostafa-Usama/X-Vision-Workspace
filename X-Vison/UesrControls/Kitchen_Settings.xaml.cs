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
using Center_Maneger.View;
namespace Center_Maneger.UesrControls
{
    /// <summary>
    /// Interaction logic for Kitchen_Settings.xaml
    /// </summary>
    public partial class Kitchen_Settings : UserControl
    {
        private DataRowView selectedRow;
       
       
        public Kitchen_Settings()
        {
            InitializeComponent();
            load_data();
        }

        private void load_data()
        {
            try
            {
                data_grid.ItemsSource = databaseLoader.LoadData("kitchen").DefaultView;
                List<string> type_of_product = new List<string>() 
                    {
                        "مشروبات ساخنة",
                        "مشروبات باردة",
                        "باتيه و بسكوت",
                        "شيبسي",
                        "منتجات اخرى",
                    };
                product_type_combo.ItemsSource = type_of_product;
            }
            catch (Exception)
            {
              
            }

        }
        private void change_header_name(object sender, DataGridAutoGeneratingColumnEventArgs e) //Becuase the grid is dynamically generated in runtime (asynchronous) مقدرش أغير اسم العمود بالكود مباشرة لكن الفنكشن دي بتشتغل لما العمود يتم انشاءه وبعدها بقدر أغير الاسم 
        {
            if (e.PropertyName == "product_name")
            {
                e.Column.Header = "اسم المنتج";
            }
            else if (e.PropertyName == "amount")
            {
                e.Column.Header = "الكمية";
            }
            else if (e.PropertyName == "purchase_cost")
            {
                e.Column.Header = "سعر الشراء";
            }
            else if (e.PropertyName == "sell_cost")
            {
                e.Column.Header = "سعر البيع";
            }
            else if (e.PropertyName == "product_type")
            {
                e.Column.Header = "نوع المنتج";
            }

        }
        private void change_selected_record(object sender, SelectionChangedEventArgs e) // this event is called whenever you select a record (row) from the grid
        {
            selectedRow = data_grid.SelectedItem as DataRowView; // store the current selected row in the variable 

        }


        private void new_product_record(object sender, RoutedEventArgs e)
        {
            product_name_input.Clear();
            purchase_cost_input.Clear();
            sell_cost_input.Clear();
        }

        private void save_product_record(object sender, RoutedEventArgs e)
        {
            string productName = product_name_input.Text;
            double purchaseCost;
            double sellCost;
            string product_type;

            if (product_name_input.Text.Trim() == "" || purchase_cost_input.Text.Trim() == "" || sell_cost_input.Text.Trim() == "" || product_type_combo.SelectedItem == null)
            {
                MessageBox.Show("برجاء ادخال جميع الحقول ", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
                return; 
            }
            bool isNumber1 = double.TryParse(purchase_cost_input.Text, out purchaseCost);
            bool isNumber2 = double.TryParse(sell_cost_input.Text, out sellCost);
            product_type = product_type_combo.SelectedItem.ToString();

            if (!isNumber1 || !isNumber2)
            {
                MessageBox.Show("برجاء ادخال ارقام صحيحة ", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Dictionary<string, object> data = new Dictionary<string, object>
                {
                    {"product_name", product_name_input.Text},
                    {"purchase_cost", purchaseCost},
                    {"sell_cost", sellCost},
                    {"product_type",product_type }
                };
            try
            {
                databaseLoader.InsertRecord("kitchen", data);
                load_data();
            }
            catch
            {
                MessageBox.Show("خطأ اثناء عملية الادخال", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void remove_product_record(object sender, RoutedEventArgs e)
        {
            if (selectedRow != null) // if a record is selected 
            {
                
                var result = MessageBox.Show("هل أنت متأكد من حذف هذا الصف؟", "تأكيد", MessageBoxButton.YesNo); // رسالة تأكيد
                if (result == MessageBoxResult.Yes) // if he chooses YES
                {


                    string productName = Convert.ToString((selectedRow["product_name"])); // بجيب اسم العمود اللي واقف عليه تقاطعا مع الصف اللي انا مختاره عشان اعرف أجيب اسم الكلاس
                    int productId = Convert.ToInt32(databaseLoader.SelectData("kitchen", "id", String.Format("product_name = \"{0}\" ", productName))[0]);
                    List<object> orders = databaseLoader.SelectData("user_kitchen", "product_id", String.Format("product_id = {0} AND is_logged_out = 0", productId));
                    if (orders.Count != 0)
                    {
                        MessageBox.Show("لا يمكن حذف المنتج, يوجد طلبات بهذا المنتج  ", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    Dictionary<string, object> data = databaseLoader.AddDeletedRecord(productId, "kitchen");


                    databaseLoader.InsertRecord("deleted_kitchen", data);
                    databaseLoader.DeleteRecord("kitchen", "product_name", productName);
                    load_data();
                }
            }

            else
            {
                MessageBox.Show("برجاء اختيار الصف الذي تريد حذفه ", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void edit_product_record(object sender, RoutedEventArgs e)
        {
            if (selectedRow != null) // if a record is selected 
            {

                string productName = Convert.ToString((selectedRow["product_name"])); ;
                string amount = Convert.ToString((selectedRow["amount"]));
                string purchaseCost = Convert.ToString((selectedRow["purchase_cost"]));
                string sellCost = Convert.ToString((selectedRow["sell_cost"]));
                string product_type = Convert.ToString((selectedRow["product_type"]));

                Edit_Product editWin = new Edit_Product(productName, amount, purchaseCost, sellCost, product_type);
                editWin.ShowDialog();
                if (editWin.isClicked)
                {
                    load_data();
                }
            }
            else
            {
                MessageBox.Show("برجاء اختيار الصف الذي تريد تعديله ", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void SearchUser(object sender, TextChangedEventArgs e)
        {

            string searchname = searchTB.Text.Trim();
            if (string.IsNullOrEmpty(searchname))
            {
                DataView prodcutView = data_grid.ItemsSource as DataView;
                prodcutView.RowFilter = string.Empty;
            }

        }

        private void SearchUser(object sender, RoutedEventArgs e)
        {
            string searchname = searchTB.Text.Trim();
            DataView prodcutView = data_grid.ItemsSource as DataView;
            prodcutView.RowFilter = string.Format("product_name LIKE '{0}%'", searchname);
            
        }
    }
}
