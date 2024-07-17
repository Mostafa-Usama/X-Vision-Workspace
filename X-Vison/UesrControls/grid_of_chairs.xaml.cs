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
using System.Globalization;

namespace Center_Maneger.UesrControls
{
    /// <summary>
    /// Interaction logic for grid_of_chairs.xaml
    /// </summary>
    public partial class grid_of_chairs : UserControl
    {
        int numberOfCells;

        
        public grid_of_chairs()
        {
            InitializeComponent();
            
        }


        public void CreateDynamicGrid()
        {
            numberOfCells = Convert.ToInt32(databaseLoader.SelectData("chairs", "num_chairs")[0]);
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
                mainbtn.BorderThickness = new Thickness(0);
                mainbtn.Background = Brushes.LightGray;
                mainbtn.Tag = Convert.ToString(i + 1);

                // Create a border with a TextBlock inside
                Border border = new Border
                {
                    Height = 150,
                    Width = DynamicGrid.Width * 0.23,
                    BorderBrush = Brushes.Black,
                    Background = Brushes.LightGray,
                    BorderThickness = new Thickness(1),
                    Margin = new Thickness(5),
                    CornerRadius = new CornerRadius(5),
                    HorizontalAlignment = HorizontalAlignment.Stretch,

                };
                //new
                Grid inside = new Grid();
                RowDefinition firstRow = new RowDefinition { Height = new GridLength(border.Height) };
                inside.RowDefinitions.Add(firstRow);
                border.Child = inside;
                Grid.SetRow(mainbtn, 0);
                //new
                StackPanel stackPanel = new StackPanel
                {
                   
                };
       
                TextBlock chair_ind = new TextBlock
                {
                    Text = Convert.ToString(i+1),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontWeight = FontWeights.Bold,
                    FontSize = 18,
                    Foreground = Brushes.OrangeRed,

                };
                Image chairIcon = new Image {
                    Width = border.Width * 0.3,
                    Height= border.Height * 0.3,
                    Source = new BitmapImage(new Uri("pack://application:,,,/img/grid chair icon.png")),
                };
                 
                stackPanel.Children.Add(chairIcon);

                Dictionary<int, Tuple<string, string, int>> activeUsers = databaseLoader.GetActiveUsers();

                if (activeUsers.ContainsKey(i+1))
                {
                    stackPanel.Children.Remove(chairIcon);
                    border.Background = Brushes.LightGreen;

                    firstRow.Height = new GridLength(border.Height * 0.3);
                    RowDefinition secondRow = new RowDefinition { Height = new GridLength(1, GridUnitType.Star)};
                    inside.RowDefinitions.Add(secondRow);
                    Grid.SetRow(mainbtn, 1);

                    border.MouseEnter += MouseEnter_event;
                    border.MouseLeave += MouseLeave_event;

                    TextBlock name_of_gest = new TextBlock
                    {
                        Text = activeUsers[i+1].Item1,
                        Foreground = Brushes.DarkBlue,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        FontWeight = FontWeights.Bold,
                        FontSize = 18,
                    };

                    string now = DateTime.Parse(activeUsers[i + 1].Item2).ToString("h:mm tt", CultureInfo.CreateSpecificCulture("ar-EG"));
                    
                    TextBlock time = new TextBlock
                    {
                        Text = now,
                        FontWeight = FontWeights.Bold,
                        FontSize = 16,
                        Margin = new Thickness(5),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,

                    };
                   
                    StackPanel infoStack = new StackPanel
                    {  
                        Background = new SolidColorBrush(Color.FromArgb(200, 5, 5, 5)),
                        Orientation = Orientation.Horizontal,
                        Visibility = Visibility.Hidden,
                        FlowDirection = FlowDirection.RightToLeft,
                    };

                    Grid.SetRow(infoStack, 0);


                    Image infoIcon = new Image
                    {
                       
                        Source = new BitmapImage(new Uri("pack://application:,,,/img/user info.png")),
                    };
                    Image notesIcon = new Image
                    {
                        
                        Source = new BitmapImage(new Uri("pack://application:,,,/img/user notes.png")),
                    };
                    Image kitchenIcon = new Image
                    {
                       
                        Source = new BitmapImage(new Uri("pack://application:,,,/img/kitchen icon2.png")),
                    };
                    Button info_member = new Button
                    {
                        Margin = new Thickness(5, 0, 0, 0),
                        Content = infoIcon,
                        Height = infoStack.Height * .9,
                        Background = null,
                        BorderThickness = new Thickness(0),
                    };
                    Button notes = new Button
                    {
                        Content = notesIcon,
                        Height = infoStack.Height * .9,
                        Margin = new Thickness(5, 0, 5, 0),
                        Background = null,
                        BorderThickness = new Thickness(0),
                    };
                    Button kitchen = new Button
                    {
                        Content = kitchenIcon,
                        Height = infoStack.Height * .9 ,
                        Background = null,
                        BorderThickness = new Thickness(0),
                    };

                    infoStack.Children.Add(info_member);
                    infoStack.Children.Add(notes);
                    infoStack.Children.Add(kitchen);
                    

                    stackPanel.Children.Add(name_of_gest);
                    stackPanel.Children.Add(time);

                    mainbtn.Background = Brushes.LightGreen;
                    mainbtn.Name = "user" + activeUsers[i + 1].Item3;
                    inside.Children.Add(infoStack);
                    
                }
                inside.Children.Add(mainbtn);

                stackPanel.Children.Add(chair_ind);
                mainbtn.Content = stackPanel;
              
                Grid.SetRow(border, row);
                Grid.SetColumn(border, column);
                DynamicGrid.Children.Add(border);
                
            }

        }

        private void MouseLeave_event(object sender, MouseEventArgs e)
        {

            Border border = sender as Border;
            if (border != null)
            {
                Grid grid = border.Child as Grid;
                StackPanel hoverStack = grid.Children[0] as StackPanel;
               
                hoverStack.Visibility = Visibility.Hidden;
                    
            }
         }

        private void MouseEnter_event(object sender, MouseEventArgs e)
        {
            Border border = sender as Border;

            if (border != null)
            {
                Grid grid = border.Child as Grid;
                StackPanel hoverStack = grid.Children[0] as StackPanel;
                hoverStack.Visibility = Visibility.Visible;

            }
            

        }

        private void mainbtn_click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Background == Brushes.LightGreen)
            {
                logout logoutWindow = new logout("chair");
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
                search_and_add_customer addWin = new search_and_add_customer("chair");
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
