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
using System.Globalization;
using Center_Maneger.View;

namespace Center_Maneger.UesrControls
{
    /// <summary>
    /// Interaction logic for grid_of_classes.xaml
    /// </summary>
    public partial class grid_of_classes : UserControl
    {
        int numberOfCells;
        SolidColorBrush btnBg = new SolidColorBrush(Color.FromArgb(255, 144, 238, 144));

        public grid_of_classes()
        {
            InitializeComponent();
        }

        private void load_grid_of_classes(object sender, RoutedEventArgs e)
        {
            CreateDynamicGrid();
        }



        public void CreateDynamicGrid()
        {
                List <object> classNames = databaseLoader.SelectData("classes", "class_name", "", " ORDER BY id");
            try
            {

                numberOfCells = classNames.Count;
            }
            catch (Exception)
            {
                numberOfCells = 0;
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
                mainbtn.BorderThickness = new Thickness(0);
                mainbtn.Background = Brushes.LightGray;


                // Create a border with a TextBlock inside
                Border border = new Border
                {
                    Height = 170,
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

                TextBlock class_name = new TextBlock
                {
                    Text = Convert.ToString(classNames[i]),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontWeight = FontWeights.Bold,
                    FontSize = 18,
                    Foreground = Brushes.OrangeRed,

                };
                Image chairIcon = new Image
                {
                    Width = border.Width * 0.35,
                    Height = border.Height * 0.35,
                    Source = new BitmapImage(new Uri("pack://application:,,,/img/class chair.png")),
                };

                stackPanel.Children.Add(chairIcon);
                string ClassName = Convert.ToString(classNames[i]);
                List<Tuple<string, string, int, string>> activeUsers = databaseLoader.GetActiveClasses();
                int idx = activeUsers.FindIndex(t => t.Item4.Contains(ClassName));
                mainbtn.Tag = ClassName;

                if (idx != -1)
                {
                    stackPanel.Children.Remove(chairIcon);
                    border.Background = btnBg;

                    firstRow.Height = new GridLength(border.Height * 0.3);
                    RowDefinition secondRow = new RowDefinition { Height = new GridLength(1, GridUnitType.Star) };
                    inside.RowDefinitions.Add(secondRow);
                    Grid.SetRow(mainbtn, 1);

                    border.MouseEnter += MouseEnter_event;
                    border.MouseLeave += MouseLeave_event;

                    TextBlock name_of_gest = new TextBlock
                    {
                        Text = activeUsers[idx].Item1,
                        Foreground = new SolidColorBrush(Color.FromRgb(11, 49, 66)),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        FontWeight = FontWeights.Bold,
                        FontSize = 22,
                    };

                    string now = DateTime.Parse(activeUsers[idx].Item2).ToString("h:mm tt", CultureInfo.CreateSpecificCulture("ar-EG"));

                    TextBlock time = new TextBlock
                    {
                        Foreground = Brushes.Black,
                        Text = now,
                        FontWeight = FontWeights.Bold,
                        FontSize = 20,
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
                        Style = (Style)Application.Current.FindResource("Roundedbtn"),
                        Margin = new Thickness(5, 0, 0, 0),
                        Content = infoIcon,
                        Height = infoStack.Height * .9,
                        Background = null,
                        BorderThickness = new Thickness(0),
                        Tag = activeUsers[idx].Item3,

                    };
                    info_member.Click += info_member_Click;

                    Button notes = new Button
                    {
                        Content = notesIcon,
                        Style = (Style)Application.Current.FindResource("Roundedbtn"),
                        Height = infoStack.Height * .9,
                        Margin = new Thickness(5, 0, 5, 0),
                        Background = null,
                        BorderThickness = new Thickness(0),
                        Tag = activeUsers[idx].Item3,

                    };
                    notes.Click += notes_Click;

                    Button kitchen = new Button
                    {
                        Content = kitchenIcon,
                        Style = (Style)Application.Current.FindResource("Roundedbtn"),
                        Height = infoStack.Height * .9,
                        Background = null,
                        BorderThickness = new Thickness(0),
                        Tag = activeUsers[idx].Item4,
                        Name = "user" + activeUsers[idx].Item3.ToString(),
                    };
                    kitchen.Click += kitchen_Click;

                    infoStack.Children.Add(info_member);
                    infoStack.Children.Add(notes);
                    infoStack.Children.Add(kitchen);


                    stackPanel.Children.Add(name_of_gest);
                    stackPanel.Children.Add(time);

                    mainbtn.Background = btnBg;
                    mainbtn.Name = "user" + activeUsers[idx].Item3;
                    inside.Children.Add(infoStack);

                }
                stackPanel.Children.Add(class_name);
                inside.Children.Add(mainbtn);
                mainbtn.Content = stackPanel;

                Grid.SetRow(border, row);
                Grid.SetColumn(border, column);
                DynamicGrid.Children.Add(border);

            }

        }

        void info_member_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            User_Info userInfoWin = new User_Info();
            userInfoWin.userId = int.Parse(Convert.ToString(btn.Tag));

            userInfoWin.ShowDialog();
        }

        void notes_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            User_Note userNoteWin = new User_Note("class");
            userNoteWin.userId = int.Parse(Convert.ToString(btn.Tag));
            userNoteWin.ShowDialog();
        }

        private void kitchen_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Add_Order addOrderWin = new Add_Order("class");
            addOrderWin.user_id = int.Parse(btn.Name.Remove(0,4));
            addOrderWin.className = btn.Tag.ToString();
            addOrderWin.ShowDialog();
        }
        private void mainbtn_click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Background == btnBg)
            {
                logout logoutWindow = new logout("class");
                logoutWindow.user_id = int.Parse(btn.Name.Remove(0, 4));
                logoutWindow.class_name = btn.Tag.ToString();
                logoutWindow.ShowDialog();
                if (logoutWindow.isClicked)
                {
                    CreateDynamicGrid();
                }
            }
            else
            {
                search_and_add_customer addWin = new search_and_add_customer("class");
                addWin.className = btn.Tag.ToString();
                addWin.ShowDialog();
                if (addWin.clickBtn)
                {
                    CreateDynamicGrid();
                }
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
    }
}
