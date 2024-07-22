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
    /// Interaction logic for User_Note.xaml
    /// </summary>
    public partial class User_Note : Window
    {
        public int chairNum;
        public int userId;
        string window;

        public User_Note(string win)
        {
            InitializeComponent();
            window = win;
        }

        private void load_date(object sender, RoutedEventArgs e)
        {
            string note;
            if (window == "chair")
            {
                note = Convert.ToString(databaseLoader.SelectData("active_users", "note", String.Format("chair_num = {0}", chairNum))[0]);
            }
            else
            {
                note = Convert.ToString(databaseLoader.SelectData("user_class", "note", String.Format("user_id = {0}", userId))[0]);

            }
                NoteTextBox.Text = note;
                NoteTextBox.Focus();
                NoteTextBox.CaretIndex = note.Length;
        }

        private void save_note(object sender, RoutedEventArgs e)
        {
            string note = NoteTextBox.Text;
            Dictionary<string, object> data = new Dictionary<string,object>{
                {"note", note},
            };
            if (window == "chair")
            {
                databaseLoader.UpdateData("active_users", data, String.Format("chair_num = {0}", chairNum));
            }
            else
            {
                databaseLoader.UpdateData("user_class", data, String.Format("user_id = {0}", userId));
            }
            this.Close();
        }
    }
}
