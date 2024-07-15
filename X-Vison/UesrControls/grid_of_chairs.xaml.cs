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
using Center_Maneger.View;
using System.Globalization;

namespace Center_Maneger.UesrControls
{
    /// <summary>
    /// Interaction logic for grid_of_chairs.xaml
    /// </summary>
    public partial class grid_of_chairs : UserControl
    {
        private string connectionString = "Data Source=database.db;Version=3;";
        int numberOfCells;

        public grid_of_chairs()
        {
            InitializeComponent();
            
        }


        public void CreateDynamicGrid()
        {
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
                mainbtn.Click += mainbtn_click;
                mainbtn.Margin = new Thickness(1);
                mainbtn.Width = DynamicGrid.Width * 0.23;
                mainbtn.Height = 150;
                mainbtn.BorderThickness = new Thickness(0);
                mainbtn.Background = Brushes.LightGray;
                mainbtn.Tag = Convert.ToString(i + 1);
                // Create a border with a TextBlock inside
                Border border = new Border
                {
                    BorderBrush = Brushes.Black,
                    Background = Brushes.LightGray,
                    BorderThickness = new Thickness(1),
                    Margin = new Thickness(5),
                    CornerRadius = new CornerRadius(10)
                };

                StackPanel stackPanel = new StackPanel
                {
                    //Background = Brushes.LightGreen,
                    Margin = new Thickness(5),
                    
                };

                TextBlock chair_ind = new TextBlock
                {
                    Text = Convert.ToString(i+1),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(5)
                };

                stackPanel.Children.Add(chair_ind);
                Dictionary<int, Tuple<string, string, int>> activeUsers = databaseLoader.GetActiveUsers();

                if (activeUsers.ContainsKey(i+1))
                {

                    TextBlock name_of_gest = new TextBlock
                    {
                        Text = activeUsers[i+1].Item1,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(5)
                    };

                    string now = DateTime.Parse(activeUsers[i + 1].Item2).ToString("h:mm tt", CultureInfo.CreateSpecificCulture("ar-EG"));
                    
                    TextBlock time = new TextBlock
                    {

                        Text = now,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(5)
                    };

                    stackPanel.Children.Add(name_of_gest);
                    stackPanel.Children.Add(time);
                    mainbtn.Background = Brushes.LightGreen;
                    mainbtn.Name = "user" + activeUsers[i + 1].Item3;
                    
                }
               // border.Child = stackPanel;
                mainbtn.Content = stackPanel;
                border.Child = mainbtn;
                // Add to the grid
                Grid.SetRow(border, row);
                Grid.SetColumn(border, column);
                DynamicGrid.Children.Add(border);
            }

        }




        private void mainbtn_click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Background == Brushes.LightGreen)
            {
                logout logoutWindow = new logout();
                logoutWindow.chairNum = int.Parse(Convert.ToString(btn.Tag));
                logoutWindow.user_id = int.Parse(btn.Name.Remove(0,4));
                logoutWindow.ShowDialog();
                if (logoutWindow.isClicked)
                {
                    CreateDynamicGrid();
                }
            }
            else
            {
                search_and_add_customer addWin = new search_and_add_customer();
                addWin.chairNum = int.Parse(Convert.ToString(btn.Tag));
                addWin.ShowDialog();
                if (addWin.clickBtn)
                {
                    CreateDynamicGrid();
                }
            }
        }

        private void load_grid_of_chair(object sender, RoutedEventArgs e) // called when window loads
        {
            CreateDynamicGrid();
        }

    }
}
