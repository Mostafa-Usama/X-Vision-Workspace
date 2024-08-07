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
using Center_Maneger.View;

namespace Center_Maneger.UesrControls
{
    /// <summary>
    /// Interaction logic for grid_of_products.xaml
    /// </summary>
    public partial class grid_of_products : UserControl
    {
        int numberOfCells;

        public grid_of_products()
        {
            InitializeComponent();
        }

        private void load_grid_of_products(object sender, RoutedEventArgs e)
        {
            CreateDynamicGrid();
        }
        public void CreateDynamicGrid()
        {
            
            DynamicGrid.Children.Clear();
            DynamicGrid.RowDefinitions.Clear();
            DynamicGrid.ColumnDefinitions.Clear();

            List<Tuple<int, string, int, double, string>> products = databaseLoader.GetProductsData();
            numberOfCells = products.Count;
            int columns = 5;
            int rows = (int)Math.Ceiling((double)numberOfCells / columns);

            for (int i = 0; i < rows; i++)
            {
                DynamicGrid.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0; i < columns; i++)
            {
                DynamicGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

          

            for (int i = 0; i < numberOfCells; i++)
            {
                int row = i / columns;
                int column = i % columns;
                Button mainbtn = new Button();
                mainbtn.Click += mainbtn_click;
                mainbtn.Margin = new Thickness(1);
                mainbtn.BorderThickness = new Thickness(0);
                mainbtn.Background = products[i].Item3 != 0 ? Brushes.LightGreen : Brushes.IndianRed;
                mainbtn.Tag = Convert.ToString(products[i].Item1);

                Border border = new Border
                {
                    Height = 170,
                    Width = DynamicGrid.Width * 0.23,
                    BorderBrush = Brushes.Black,
                    Background = products[i].Item3 != 0? Brushes.LightGreen: Brushes.IndianRed,
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
                    Text ="الكمية: " + Convert.ToString(products[i].Item3),
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
                DynamicGrid.Children.Add(border);
                
                
            }
        }

        private void mainbtn_click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int id = Convert.ToInt32(btn.Tag);
            Product_Info productInfoWin = new Product_Info(id);
            productInfoWin.ShowDialog();
            if (productInfoWin.isClicked)
            {
                CreateDynamicGrid();
            }
        }
    }
}
