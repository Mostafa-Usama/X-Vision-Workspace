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
    /// Interaction logic for search_and_add_customer.xaml
    /// </summary>
    public partial class search_and_add_customer : Window
    {
        public search_and_add_customer()
        {
            InitializeComponent();

            Section1.IsEnabled = true;
            Section2.IsEnabled = false;
        }

        private void search_checkbox(object sender, RoutedEventArgs e)
        {
            Section1.IsEnabled = true;
            Section2.IsEnabled = false;
        }

        private void new_checkbox(object sender, RoutedEventArgs e)
        {
            Section1.IsEnabled = false;
            Section2.IsEnabled = true;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            string query = nameTextBox.Text.ToLower();
            var data = databaseLoader.GetUserNames(query,"name" );
            nameListBox.ItemsSource = data;

            nameListBox.Visibility = data.Any() ? Visibility.Visible : Visibility.Collapsed;
        }

        private void SuggestionListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (nameListBox.SelectedItem != null)
            {
                nameTextBox.Text = nameListBox.SelectedItem.ToString();
                nameListBox.Visibility = Visibility.Collapsed;
            }
        }
    }
}
