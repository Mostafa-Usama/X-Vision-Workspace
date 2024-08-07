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
    /// Interaction logic for Add_Order.xaml
    /// </summary>
    public partial class Add_Order : Window
    {
        public int user_id;
        public string className;
        public int chair_num;
        Dictionary<int, int> basket = new Dictionary<int, int>();
        List<Tuple<int, string, int, double,string>> products = databaseLoader.GetProductsData("");
        Dictionary<int, Tuple<int, string, int, double,string>> productDict;
        Dictionary<int, int> lastOrder = null;
        string window;
            
        public Add_Order(string win)
        {
            InitializeComponent();
            window = win;
            productDict = products.ToDictionary(p => p.Item1);

        }
        private void load_data(object sender, RoutedEventArgs e)
        {
            string userName = Convert.ToString(databaseLoader.SelectData("users", "name", String.Format("id = {0}", user_id))[0]);
            username_label.Text = userName;
            if ( window == "chair") {
                chair.Text = chair_num.ToString();
                chair_class.Text = "رقم المقعد: ";
            } 
            else {
                chair.Text = className;
                chair_class.Text = "اسم الغرفة: ";
            }
            CreateProductsGrid();
            lastOrder = databaseLoader.GetProductsDictionary(user_id);
            foreach (var item in lastOrder)
            {
                createBoughtGrid(item.Key, item.Value);
            }
        }


        private void CreateProductsGrid(string product_type="")
        {
            string fliter = product_type == "" ? "" : String.Format("product_type = \"{0}\" ", product_type);

            int numberOfCells = Convert.ToInt32(databaseLoader.SelectData("kitchen", "id" ,fliter).Count);
            products_grid.Children.Clear();
            products_grid.RowDefinitions.Clear();
            products_grid.ColumnDefinitions.Clear();

            int columns = 4;
            int rows = (int)Math.Ceiling((double)numberOfCells / columns);

            for (int i = 0; i < rows; i++)
            {
                products_grid.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0; i < columns; i++)
            {
                products_grid.ColumnDefinitions.Add(new ColumnDefinition());
            }


            for (int i = 0; i < numberOfCells; i++)
            {
                int row = i / columns;
                int column = i % columns;
                Button mainbtn = new Button();
                mainbtn.Click += add_product;
                mainbtn.Margin = new Thickness(1);
                mainbtn.BorderThickness = new Thickness(0);
                mainbtn.Background = products[i].Item3 != 0 ? Brushes.LightGreen : Brushes.IndianRed;
                mainbtn.Tag = Convert.ToString(products[i].Item1);

                Border border = new Border
                {
                    Height = 150,
                    Width = products_grid.Width * 0.23,
                    BorderBrush = Brushes.Black,
                    Background = products[i].Item3 != 0 ? Brushes.LightGreen : Brushes.IndianRed,
                    BorderThickness = new Thickness(1),
                    Margin = new Thickness(5),
                    CornerRadius = new CornerRadius(5),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                };

                StackPanel stackPanel = new StackPanel();

                TextBlock product_name = new TextBlock
                {
                    Text = products[i].Item2,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontWeight = FontWeights.Bold,
                    FontSize = 18,
                    Foreground = Brushes.Black,
                };
                TextBlock product_cost = new TextBlock
                {
                    Text = Convert.ToString(products[i].Item4) + " جنيه",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontWeight = FontWeights.Bold,
                    FontSize = 18,
                    Foreground = Brushes.Black,
                };
                TextBlock product_amount = new TextBlock
                {
                    Text = "الكمية: " + Convert.ToString(products[i].Item3),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontWeight = FontWeights.Bold,
                    FontSize = 18,
                    Foreground = Brushes.Black,
                };
                Image product_icon = new Image
                {
                    Width = border.Width * 0.3,
                    Height = border.Height * 0.3,
                    Source = new BitmapImage(new Uri("pack://application:,,,/img/kitchen icon2.png")),
                };

                stackPanel.Children.Add(product_icon);
                stackPanel.Children.Add(product_name);
                stackPanel.Children.Add(product_amount);
                stackPanel.Children.Add(product_cost);
                mainbtn.Content = stackPanel;
                border.Child = mainbtn;

                Grid.SetRow(border, row);
                Grid.SetColumn(border, column);
                products_grid.Children.Add(border);
               

            }
        }

        private void createBoughtGrid(int productId, int am = 1)
        {
            if (!basket.ContainsKey(productId))
            {
                basket.Add(productId, am);

                Tuple<string, int, double, double,string> products = databaseLoader.GetProductData(productId);
                Button mainbtn = new Button();
                mainbtn.Margin = new Thickness(1);
                mainbtn.BorderThickness = new Thickness(0);
                mainbtn.Background = Brushes.LightGreen;

                Border border = new Border
                {
                    Height = 150,
                    BorderBrush = Brushes.Black,
                    Background = Brushes.LightGreen,
                    BorderThickness = new Thickness(1),
                    Margin = new Thickness(5),
                    CornerRadius = new CornerRadius(5),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Name = "btn" + productId.ToString(),
                };
                border.MouseEnter += MouseEnter_event;
                border.MouseLeave += MouseLeave_event;
                ////////////////////////////////////////////////////
                Grid inside = new Grid();
                RowDefinition firstRow = new RowDefinition { Height = new GridLength(border.Height * 0.22) };
                RowDefinition secondRow = new RowDefinition { Height = new GridLength(1, GridUnitType.Star) };
                inside.RowDefinitions.Add(firstRow);
                inside.RowDefinitions.Add(secondRow);
                border.Child = inside;
                Grid.SetRow(mainbtn, 1);
                /////////////////////////////////////////////////////
                DockPanel dock = new DockPanel
                {
                    Background = new SolidColorBrush(Color.FromArgb(200, 5, 5, 5)),
                    Visibility = Visibility.Hidden,
                    FlowDirection = FlowDirection.LeftToRight,

                };
                StackPanel btnStack = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(5, 0, 5, 0),
                };
                Button add = new Button
                {
                    Content = "+",
                    VerticalAlignment = VerticalAlignment.Center,
                    FontWeight = FontWeights.Bold,
                    Background = Brushes.Transparent,
                    BorderBrush = Brushes.Transparent,
                    Foreground = Brushes.White,
                    Tag = productId.ToString(),
                };
                add.Click += add_product;


                Button subtract = new Button
                {
                    Content = "-",
                    VerticalAlignment = VerticalAlignment.Center,
                    FontWeight = FontWeights.Bold,
                    Background = Brushes.Transparent,
                    BorderBrush = Brushes.Transparent,
                    Foreground = Brushes.White,
                    Tag = productId.ToString(),
                };
                subtract.Click += subtract_product;

                btnStack.Children.Add(add);
                btnStack.Children.Add(subtract);
                ///////////////////////////////////////////////////////
                Button remove = new Button
                {
                    Content = "x",
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontWeight = FontWeights.Bold,
                    Background = Brushes.IndianRed,
                    BorderBrush = Brushes.Transparent,
                    Padding = new Thickness(5),
                    Margin = new Thickness(5, 0, 5, 0),
                    Foreground = Brushes.White,
                    Tag = productId.ToString(),
                };
                remove.Click += remove_product;

                dock.Children.Add(btnStack);
                dock.Children.Add(remove);
                DockPanel.SetDock(btnStack, Dock.Left);
                DockPanel.SetDock(remove, Dock.Right);
                ///////////////////////////////////////////////////////
                Grid.SetRow(dock, 0);
                inside.Children.Add(dock);
                inside.Children.Add(mainbtn);
                ///////////////////////////////////////////////////////
                StackPanel stackPanel = new StackPanel();

                TextBlock product_name = new TextBlock
                {
                    Text = products.Item1,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontWeight = FontWeights.Bold,
                    FontSize = 18,
                    Foreground = Brushes.Black,
                };

                TextBlock product_amount = new TextBlock
                {
                    Text = "الكمية: " + Convert.ToString(am),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontWeight = FontWeights.Bold,
                    FontSize = 18,
                    Foreground = Brushes.Black,

                };
                TextBlock product_cost = new TextBlock
                {
                    Text = Convert.ToString(products.Item4) + " جنيه",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontWeight = FontWeights.Bold,
                    FontSize = 18,
                    Foreground = Brushes.Black,
                };
                Image product_icon = new Image
                {
                    Width = border.Width * 0.25,
                    Height = border.Height * 0.25,
                    Source = new BitmapImage(new Uri("pack://application:,,,/img/kitchen icon2.png")),
                };
                stackPanel.Children.Add(product_icon);
                stackPanel.Children.Add(product_name);
                stackPanel.Children.Add(product_amount);
                stackPanel.Children.Add(product_cost);
                ///////////////////////////////////////////////////////
                mainbtn.Content = stackPanel;
                bought_stack.Children.Add(border);
            }
            else
            {

                basket[productId]++;
                Border border = findChildBorder(productId);
                Grid inside = border.Child as Grid;
                Button clickedBtn = inside.Children[1] as Button;
                StackPanel stack = clickedBtn.Content as StackPanel;
                TextBlock amount = stack.Children[2] as TextBlock;
                amount.Text = "الكمية: " + basket[productId].ToString();
            }
        }

        private void add_product(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int productId = Convert.ToInt32(btn.Tag);
            createBoughtGrid(productId);
        }

        private void remove_product(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int productId = Convert.ToInt32(btn.Tag);
            Border border = findChildBorder(productId);
            bought_stack.Children.Remove(border);
            basket.Remove(productId);
        }

        private void subtract_product(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int productId = Convert.ToInt32(btn.Tag);
            basket[productId]--;
            Border border = findChildBorder(productId);
            if (basket[productId] <= 0)
            {
                bought_stack.Children.Remove(border);
                basket.Remove(productId);
                return;
            }
            Grid inside = border.Child as Grid;
            Button clickedBtn = inside.Children[1] as Button;
            StackPanel stack = clickedBtn.Content as StackPanel;
            TextBlock amount = stack.Children[2] as TextBlock;
            amount.Text = "الكمية: " + basket[productId].ToString();
        }

        private Border findChildBorder(int productId)
        {
             foreach (var child in bought_stack.Children)
            {
                Border x = child as Border;
                if (x.Name == "btn"+productId.ToString())
                {
                    return x;
                }
            }
            return null;
        }
        //private async void save_order(object sender, RoutedEventArgs e)
        //{
        //    await save(sender, e);
        //}

        private void save_order(object sender, RoutedEventArgs e)
        {
    
        foreach (var item in basket)
        {
            Tuple<int, string, int, double,string> product = productDict[item.Key];
            if (item.Value > product.Item3)
            {
                MessageBox.Show(String.Format(" لا يوجد كمية كافية من ({0})  ", product.Item2), "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }

        foreach (var item in lastOrder)
        {
            if (!basket.ContainsKey(item.Key))
            {
                int amount = productDict[item.Key].Item3;
                databaseLoader.DeleteRecord("user_kitchen", String.Format("is_logged_out  = 0 AND user_id = {0} AND product_id", user_id), item.Key.ToString());
                Dictionary<string, object> newAmount = new Dictionary<string, object>{
                            {"amount", amount + item.Value},
                        };
                databaseLoader.UpdateData("kitchen", newAmount, String.Format("id = {0}", item.Key));
            }
            else if (basket[item.Key] != item.Value)
            {
                int amount = productDict[item.Key].Item3;
                double cost = productDict[item.Key].Item4;
                Dictionary<string, object> updatedAmount = new Dictionary<string, object>{
                            {"amount", basket[item.Key]} ,
                            {"cost", basket[item.Key] * cost},
                        };
                databaseLoader.UpdateData("user_kitchen", updatedAmount, String.Format("product_id = {0} AND user_id = {1} AND is_logged_out = 0", item.Key, user_id));

                Dictionary<string, object> newAmount = new Dictionary<string, object>{
                            {"amount", amount + (item.Value - basket[item.Key])},
                        };
                databaseLoader.UpdateData("kitchen", newAmount, String.Format("id = {0}", item.Key));
            }
        }
        foreach (var item in basket)
        {
            if (!lastOrder.ContainsKey(item.Key))
            {
                double cost = productDict[item.Key].Item4;
                int amount = productDict[item.Key].Item3;
                Dictionary<string, object> order = new Dictionary<string, object>{
                            {"user_id", user_id},
                            {"product_id", item.Key},
                            {"amount", item.Value},
                            {"cost", cost * item.Value}
                        };
                databaseLoader.InsertRecord("user_kitchen", order);
                Dictionary<string, object> newAmount = new Dictionary<string, object>{
                            {"amount", amount - item.Value}
                        };
                databaseLoader.UpdateData("kitchen", newAmount, String.Format("id = {0}", item.Key));
            }
        }
        this.Close();
   
           
        }

        private void MouseLeave_event(object sender, MouseEventArgs e)
        {
            Border border = sender as Border;
            if (border != null)
            {
                Grid grid = border.Child as Grid;
                DockPanel hoverStack = grid.Children[0] as DockPanel;
                hoverStack.Visibility = Visibility.Hidden;
            }
        }

        private void MouseEnter_event(object sender, MouseEventArgs e)
        {
            Border border = sender as Border;
            if (border != null)
            {
                Grid grid = border.Child as Grid;
                DockPanel hoverStack = grid.Children[0] as DockPanel;
                hoverStack.Visibility = Visibility.Visible;
            }
        }

        private void all_products(object sender, RoutedEventArgs e)
        {
            CreateProductsGrid("");
        }

        private void hot_drink(object sender, RoutedEventArgs e)
        {
            products = databaseLoader.GetProductsData("مشروبات ساخنة");
            CreateProductsGrid("مشروبات ساخنة");

        }

        private void cold_drink(object sender, RoutedEventArgs e)
        {
            products = databaseLoader.GetProductsData("مشروبات باردة");
            CreateProductsGrid("مشروبات باردة");

        }

        private void snacks(object sender, RoutedEventArgs e)
        {
            products = databaseLoader.GetProductsData("باتيه و بسكوت");
            CreateProductsGrid("باتيه و بسكوت");

        }

        private void chips(object sender, RoutedEventArgs e)
        {
            products = databaseLoader.GetProductsData("شيبسي");
            CreateProductsGrid("شيبسي");

        }

        private void other_product(object sender, RoutedEventArgs e)
        {
            products = databaseLoader.GetProductsData("منتجات اخرى");
            CreateProductsGrid("منتجات اخرى");

        }
    }
}
