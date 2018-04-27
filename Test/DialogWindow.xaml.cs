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

namespace Test
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        Dictionary<Button, DialogAnswer> mappings = new Dictionary<Button, DialogAnswer>();
        Dictionary<DialogAnswer, Button> reverseMappings = new Dictionary<DialogAnswer, Button>();

        public DialogWindow()
        {
            InitializeComponent();
        }

        public DialogWindow(string title,string message,DialogAnswer[] buttons)
        {
            InitializeComponent();
            SetMappings();
            SetReverseMappings();
            SetButtons(buttons);
            Title.Content = title;
            Message.Text = message;
        }

        private void SetButtons(DialogAnswer[] buttons)
        {
            foreach(var button in buttons)
            {
                Button b = reverseMappings[button];
                b.Visibility = Visibility.Visible;
            }
        }

        private void SetMappings()
        {
            mappings.Add(Yes, DialogAnswer.Yes);
            mappings.Add(No, DialogAnswer.No);
            mappings.Add(OK, DialogAnswer.OK);
            mappings.Add(Cancel, DialogAnswer.Cancel);
        }

        private void SetReverseMappings()
        {
            reverseMappings.Add(DialogAnswer.Yes,Yes);
            reverseMappings.Add(DialogAnswer.No,No);
            reverseMappings.Add(DialogAnswer.OK,OK);
            reverseMappings.Add(DialogAnswer.Cancel,Cancel);
        }

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button b = e.OriginalSource as Button;
            DialogResultManager.SetResult(mappings[b]);
            this.Close();
        }
    }
}
