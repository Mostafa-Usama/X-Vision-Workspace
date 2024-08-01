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
        int user_id;
        int chair_num;

        public Add_Order(int id, int chair)
        {
            InitializeComponent();
            user_id = id;
            chair_num = chair;
        }
        private void load_data(object sender, RoutedEventArgs e)
        {
            string userName = Convert.ToString(databaseLoader.SelectData("users", "name", String.Format("id = {0}", user_id))[0]);
            username_label.Text = userName;
            chair.Text = chair_num.ToString();
            CreateProductsGrid();
        }

        private void CreateProductsGrid()
        {
            int numberOfCells = Convert.ToInt32(databaseLoader.SelectData("kitchen", "id").Count);
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

            List<Tuple<int, string, int, double>> products = databaseLoader.GetProductsData();

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

        private void add_product(object sender, RoutedEventArgs e)
        {
            
        }

        private void save_order(object sender, RoutedEventArgs e)
        {

        }

    }
}
