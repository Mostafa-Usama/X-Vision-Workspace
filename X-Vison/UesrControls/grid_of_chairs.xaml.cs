using Center_Maneger.View;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Center_Maneger.UesrControls
{
    public partial class grid_of_chairs : UserControl
    {
        int numberOfCells;
        SolidColorBrush btnBg = new SolidColorBrush(Color.FromArgb(255, 144, 238, 144));
        int x = 0;

        public grid_of_chairs()
        {
            InitializeComponent();
        }

        public void CreateDynamicGrid(string searchQuery = "")
        {
            x = 0;
            try
            {
                numberOfCells = Convert.ToInt32(databaseLoader.SelectData("chairs", "num_chairs")[0]);

            }
            catch (Exception)
            {
                numberOfCells = 0;
            }
            DynamicGrid.Children.Clear();
            DynamicGrid.RowDefinitions.Clear();
            DynamicGrid.ColumnDefinitions.Clear();

            int columns = 4;
            int rows = (int)Math.Ceiling((double)numberOfCells / columns);

            for (int i = 0; i < rows; i++)
            {
                DynamicGrid.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0; i < columns; i++)
            {
                DynamicGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            
            Dictionary<int, Tuple<string, string, int>> activeUsers = databaseLoader.GetActiveUsers();
            for (int i = 0; i < numberOfCells; i++)
            {
                int row = i / columns;
                int column = i % columns;
                int row2 = x / columns;
                int col2 = x % columns;

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    if (!activeUsers.ContainsKey(i + 1))
                    {

                        continue;
                    }
                }
                Button mainbtn = new Button();
                mainbtn.Click += mainbtn_click;
                mainbtn.Margin = new Thickness(1);
                mainbtn.BorderThickness = new Thickness(0);
                mainbtn.Background = Brushes.LightGray;
                mainbtn.Tag = Convert.ToString(i + 1);

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

                Grid inside = new Grid();
                RowDefinition firstRow = new RowDefinition { Height = new GridLength(border.Height) };
                inside.RowDefinitions.Add(firstRow);
                border.Child = inside;
                Grid.SetRow(mainbtn, 0);

                StackPanel stackPanel = new StackPanel();
                TextBlock chair_ind = new TextBlock
                {
                    Text = Convert.ToString(i + 1),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontWeight = FontWeights.Bold,
                    FontSize = 18,
                    Foreground = Brushes.OrangeRed,
                };
                
                Image chairIcon = new Image
                {
                    Width = border.Width * 0.3,
                    Height = border.Height * 0.3,
                    Source = new BitmapImage(new Uri("pack://application:,,,/img/grid chair icon.png")),
                };

                stackPanel.Children.Add(chairIcon);

                

                if (activeUsers.ContainsKey(i + 1))
                {
                    stackPanel.Children.Remove(chairIcon);
                    border.Background = btnBg;

                    firstRow.Height = new GridLength(border.Height * 0.3);
                    RowDefinition secondRow = new RowDefinition { Height = new GridLength(1, GridUnitType.Star) };
                    inside.RowDefinitions.Add(secondRow);
                    Grid.SetRow(mainbtn, 1);

                    border.MouseEnter += MouseEnter_event;
                    border.MouseLeave += MouseLeave_event;

                    string NameOfGest = activeUsers[i + 1].Item1;

                    TextBlock name_of_gest = new TextBlock
                    {
                        Text = NameOfGest,
                        Foreground = new SolidColorBrush(Color.FromRgb(11, 49, 66)),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        FontWeight = FontWeights.Bold,
                        FontSize = 22,
                    };

                    string now = DateTime.Parse(activeUsers[i + 1].Item2).ToString("h:mm tt", CultureInfo.CreateSpecificCulture("ar-EG"));

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
                        Margin = new Thickness(5, 0, 0, 0),
                        Content = infoIcon,
                        Height = infoStack.Height * .9,
                        Background = null,
                        BorderThickness = new Thickness(0),
                        Name = "user" + activeUsers[i + 1].Item3,
                    };
                    info_member.Click += info_member_Click;

                    Button notes = new Button
                    {
                        Content = notesIcon,
                        Height = infoStack.Height * .9,
                        Margin = new Thickness(5, 0, 5, 0),
                        Background = null,
                        BorderThickness = new Thickness(0),
                        Tag = Convert.ToString(i + 1),
                    };
                    notes.Click += notes_Click;

                    Button kitchen = new Button
                    {
                        Content = kitchenIcon,
                        Height = infoStack.Height * .9,
                        Background = null,
                        BorderThickness = new Thickness(0),
                        Name = "user" + activeUsers[i + 1].Item3,
                        Tag = (i+1).ToString(),
                    };
                    kitchen.Click += kitchen_Click;

                    infoStack.Children.Add(info_member);
                    infoStack.Children.Add(notes);
                    infoStack.Children.Add(kitchen);

                    stackPanel.Children.Add(name_of_gest);
                    stackPanel.Children.Add(time);

                    mainbtn.Background = btnBg;
                    mainbtn.Name = "user" + activeUsers[i + 1].Item3;
                    inside.Children.Add(infoStack);

                    if (!string.IsNullOrEmpty(searchQuery))
                    {
                        if (!NameOfGest.ToLower().StartsWith(searchQuery))
                        {
                         border.Visibility = Visibility.Collapsed;

                        continue;
                        }
                        else 
                        {
                            inside.Children.Add(mainbtn);

                            stackPanel.Children.Add(chair_ind);
                            mainbtn.Content = stackPanel;
                            Grid.SetRow(border, row2);
                            Grid.SetColumn(border, col2);
                            DynamicGrid.Children.Add(border);
                            x++;
                            continue;
                        }
                       
                    }
                    
                }
                inside.Children.Add(mainbtn);

                stackPanel.Children.Add(chair_ind);
                mainbtn.Content = stackPanel;

               

                Grid.SetRow(border, row);
                Grid.SetColumn(border, column);
                DynamicGrid.Children.Add(border);
                
               
            }
        }

        private void kitchen_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            int chair = Convert.ToInt32(btn.Tag);
            int id = int.Parse(btn.Name.Remove(0, 4));
            Add_Order addOrderWin = new Add_Order("chair");
            addOrderWin.user_id = id;
            addOrderWin.chair_num = chair;
            addOrderWin.ShowDialog();
        }

        private void notes_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            User_Note userNoteWin = new User_Note("chair");
            userNoteWin.chairNum = int.Parse(Convert.ToString(btn.Tag));
            userNoteWin.ShowDialog();
        }

        private void info_member_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            User_Info userInfoWin = new User_Info();
            userInfoWin.userId = int.Parse(btn.Name.Remove(0, 4));
            userInfoWin.ShowDialog();
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
            if (btn.Background == btnBg)
            {
                logout logoutWindow = new logout("chair");
                logoutWindow.chairNum = int.Parse(Convert.ToString(btn.Tag));
                logoutWindow.user_id = int.Parse(btn.Name.Remove(0, 4));
                logoutWindow.ShowDialog();
                if (logoutWindow.isClicked)
                {
                    string searchname = searchTB.Text;
                    CreateDynamicGrid(searchname);
                }
            }
            else
            {
                search_and_add_customer addWin = new search_and_add_customer("chair");
                addWin.chairNum = int.Parse(Convert.ToString(btn.Tag));
                addWin.ShowDialog();
                if (addWin.clickBtn)
                {
                    string searchname = searchTB.Text;
                    CreateDynamicGrid(searchname);
                }
            }
        }

        private void load_grid_of_chair(object sender, RoutedEventArgs e)
        {
            string searchname = searchTB.Text;
            CreateDynamicGrid(searchname);

        }

        

        private void SearchUser(object sender, TextChangedEventArgs e)
        {

            string searchname = searchTB.Text.Trim();
            if (string.IsNullOrEmpty(searchname))
            {
                CreateDynamicGrid(searchname);
            }
            
        }

        private void SearchUser(object sender, RoutedEventArgs e)
        {
            string searchname = searchTB.Text.Trim();
            CreateDynamicGrid(searchname);
        }

        
    }
}
