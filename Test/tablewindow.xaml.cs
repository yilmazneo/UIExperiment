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
    /// Interaction logic for tablewindow.xaml
    /// </summary>
    public partial class tablewindow : Window
    {
        Table table = null;

        public tablewindow()
        {
            InitializeComponent();
        }

        public tablewindow(Table t)
        {
            InitializeComponent();
            table = t;
            SetUI();
        }

        public tablewindow(double x,double y)
        {
            InitializeComponent();            
            SetUI(x,y);
        }

        private void SetUI(double x, double y)
        {
            X.Text = x.ToString();
            Y.Text = y.ToString();
            ScaleX.Text = "0";
            ScaleY.Text = "0";
            Angle.Text = "0";
            Color.Text = "LightGreen";
            Name.Text = "";
            Width.Text = "50";
            Height.Text = "50";
            Circle.IsChecked = true;
        }

        private void SetUI()
        {
            if (table != null)
            {
                X.Text = table.X.ToString();
                Y.Text = table.Y.ToString();
                ScaleX.Text = table.ScaleX.ToString();
                ScaleY.Text = table.ScaleY.ToString();
                Angle.Text = table.RotateAngle.ToString();
                Color.Text = table.Color;
                Name.Text = table.Text;
                Width.Text = table.Width.ToString();
                Height.Text = table.Height.ToString();
                bool isCircle = table.Shape == TableShape.Circle;
                Circle.IsChecked = isCircle;
                Rectangle.IsChecked = !isCircle;
            }
        }
        
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            MainWindow parent = (MainWindow)this.DataContext;
            parent.SetArgs(GenerateArgs());
            DialogResult = true;
            this.Close();
        }

        private Dictionary<UpdateKey, object> GenerateArgs()
        {
            Dictionary<UpdateKey, object> args = new Dictionary<UpdateKey, object>();
            args.Add(UpdateKey.Angle, int.Parse(Angle.Text));
            args.Add(UpdateKey.X, double.Parse(X.Text));
            args.Add(UpdateKey.Y, double.Parse(Y.Text));
            args.Add(UpdateKey.Width, double.Parse(Width.Text));
            args.Add(UpdateKey.Height, double.Parse(Height.Text));
            args.Add(UpdateKey.ScaleX, int.Parse(ScaleX.Text));
            args.Add(UpdateKey.ScaleY, int.Parse(ScaleY.Text));
            args.Add(UpdateKey.Name, Name.Text);
            args.Add(UpdateKey.Color, Color.Text);
            args.Add(UpdateKey.Shape, Circle.IsChecked.HasValue && Circle.IsChecked.Value ? TableShape.Circle : TableShape.Rectangle);
            return args;
        }


    }
}
