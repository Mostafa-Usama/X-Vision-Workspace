using Center_Maneger.UesrControls;
using Center_Maneger.View;
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

namespace Center_Maneger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UserControl[] userControls;
        private Button[] buttons;

        public MainWindow()
        {
            InitializeComponent();
            
            userControls = new UserControl [] {
                new Class_Settings(),
                new Faculty_Settings(),
                new Job_Settings(),
                new Prices_Settings(),
                new Offer_Settings(),
                new Members(),
                new grid_of_chairs(),
                new grid_of_classes(),
            };
            buttons = new Button[] {
            btn0, btn1, btn2, btn3, btn4, btn5,btn6,btn7,btn8};
        }

        private void num_chairs_btn(object sender, RoutedEventArgs e)
        {
           num_of_chairs_win Chair =   new num_of_chairs_win(); 
            Chair.ShowDialog(); // so user cant interact with the app unless he closes this window
        }



        private void openUserControl (object sender, RoutedEventArgs e)
        {
            Button clickedBtn = sender as Button;
            

            int idx = int.Parse(clickedBtn.Name.Remove(0, 3));
            string header = Convert.ToString(clickedBtn.Tag);
            TabItem newTab = new TabItem
            {
                Header = header,
                Content = userControls[idx],
              
            };
            foreach (var btn in buttons)
            {
                if (clickedBtn == btn)
                {
                    clickedBtn.Background = new SolidColorBrush(Colors.LightGreen);
                }
                else
                {
                    clickedBtn.Background = new SolidColorBrush(Colors.White);

                }
            }
            


            newTab.Style = (Style)Resources["dynamic_tabs"];
            
            settings_controls.Items.Add(newTab);
            settings_controls.SelectedItem = newTab;
        }

        private void settings_clicked(object sender, MouseButtonEventArgs e)
        {


            checkPassword checkPass = new checkPassword();
            checkPass.ShowDialog();

            if (checkPass.correct)
            {
                settings_tab.SelectedIndex = 0;
            }
            else
            {
                settings_tab.SelectedIndex = 1;
            }

        }

        private void close_tab(object sender, RoutedEventArgs e)
        {
            Button clickedBtn = sender as Button; // pressed btn
            StackPanel stack = clickedBtn.Parent as StackPanel; // parent stackpanel
            TabItem tab = null;
            foreach (TabItem item in settings_controls.Items) // loop over all taps 
            {
                if (Convert.ToString(item.Header)== ((TextBlock)stack.Children[0]).Text) // if the header is the same as the textbox then this is the tab we want to close
                {
                    tab = item;
                    break;
                }
            }
            settings_controls.Items.Remove(tab);
      
        }
        
    }
}
