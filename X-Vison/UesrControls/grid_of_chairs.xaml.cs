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
using System.Data.SQLite;
namespace Center_Maneger.UesrControls
{
    /// <summary>
    /// Interaction logic for grid_of_chairs.xaml
    /// </summary>
    public partial class grid_of_chairs : UserControl
    {
        private DateTime currenttime = DateTime.Now;
        public int numberOfCells;
        public grid_of_chairs()
        {
            InitializeComponent();
            
        }


        public void CreateDynamicGrid()
        {
            string connectionString = "Data Source=database.db;Version=3;";
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();
                        string query = String.Format("SELECT num_chairs FROM chairs");
                        SQLiteCommand command = new SQLiteCommand(query, connection);
                        SQLiteDataReader reader = command.ExecuteReader();
                        if (reader.Read()) 
                        {
                            numberOfCells = reader.GetInt32(0);
                        }
                        else 
                        {
                            numberOfCells = 0;
                        }
                        reader.Close();
                    
                    }
            }
            catch 
            {

                MessageBox.Show("An error occurred");
            }
            
            
            int hours = currenttime.Hour;
            int min = currenttime.Minute;
            int sec = currenttime.Second;
            string date = String.Format("{0}:{1}:{2}", hours, min, sec);
            // Clear any existing children
            DynamicGrid.Children.Clear();
            DynamicGrid.RowDefinitions.Clear();
            DynamicGrid.ColumnDefinitions.Clear();

            // Calculate the number of rows and columns
            int columns = 4; // Set fixed columns for simplicity
            int rows = (int)Math.Ceiling((double)numberOfCells / columns);

            // Create Rows
            for (int i = 0; i < rows; i++)
            {
                DynamicGrid.RowDefinitions.Add(new RowDefinition());
            }

            // Create Columns
            for (int i = 0; i < columns; i++)
            {
                DynamicGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            // Create Cells
            for (int i = 0; i < numberOfCells; i++)
            {
                int row = i / columns;
                int column = i % columns;

                Button mainbtn = new Button();
                mainbtn.Content = "clickme";
                mainbtn.Click += mainbtn_click;
                mainbtn.Margin = new Thickness(5);
                mainbtn.Width = DynamicGrid.Width * 0.23;
                mainbtn.Height = 150;

                // Create a border with a TextBlock inside
                Border border = new Border
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1),
                    Margin = new Thickness(5)
                };

                StackPanel stackPanel = new StackPanel
                {
                    Background = Brushes.LightGreen,
                    Margin = new Thickness(5)
                };

                TextBlock chair_ind = new TextBlock
                {
                    Text = String.Format("Cell {0}", i+1),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(5)
                };
                TextBlock name_of_gest = new TextBlock
                {
                    Text = "شوكت مدحت محسن",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(5)
                };
                TextBlock time = new TextBlock
                {

                    Text = date,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(5)
                };

                stackPanel.Children.Add(chair_ind);
                stackPanel.Children.Add(name_of_gest);
                stackPanel.Children.Add(time);
                border.Child = stackPanel;
                mainbtn.Content = border;

                // Add to the grid
                Grid.SetRow(mainbtn, row);
                Grid.SetColumn(mainbtn, column);
                DynamicGrid.Children.Add(mainbtn);
            }

        }
        private void mainbtn_click(object sender, RoutedEventArgs e)
        {
            // Handle the button click event
            MessageBox.Show("Button Clicked!");
        }

        private void load_grid_of_chair(object sender, RoutedEventArgs e)
        {
            CreateDynamicGrid();
        }
    }
}
