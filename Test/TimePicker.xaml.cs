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

namespace Test
{
    /// <summary>
    /// Interaction logic for TimePicker.xaml
    /// </summary>
    public partial class TimePicker : UserControl
    {
        int currentTime = 11;
        string zoneString = "AM";
        public string PickedTime { get; set; }
        public TimePicker()
        {
            InitializeComponent();
            PickedTime = string.Format("{0} {1}", currentTime, zoneString);
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            if(currentTime + 1 == 13)
            {
                currentTime = 1;
                zoneString = zoneString == "AM" ? "PM" : "AM";
            }
            else
            {
                currentTime++;
            }
            PickedTime = string.Format("{0} {1}", currentTime, zoneString);
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentTime - 1 == 0)
            {
                currentTime = 12;
                zoneString = zoneString == "AM" ? "PM" : "AM";
            }
            else
            {
                currentTime--;
            }
            PickedTime = string.Format("{0} {1}", currentTime, zoneString);
        }

        public string GetTime()
        {
            return PickedTime;
        }
    }
}
