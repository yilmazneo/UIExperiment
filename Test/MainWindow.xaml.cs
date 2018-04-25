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
using System.Windows.Threading;
using System.Data.Entity;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Test
{
    public enum Modes
    {
        None,
        Drag,
        Select
    }

    public enum Table
    {
        Round,
        Square,
        Diamond
    }


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        Entities en = new Entities();
        Dictionary<string, Table> tables = new Dictionary<string, Table>();

        Modes mode = Modes.None;
        Button s = null;
        Button selected = null;
        int id = 0;

        enum Shape
        {
            Circle,
            Rectangle
        }

        class Table
        {
            public string Id { get; set; }
            public Shape Shape { get; set; }
            public string Text { get; set; }
            public int RotateAngle { get; set; }
            public double Width { get; set; }
            public double Height { get; set; }
            public double X { get; set; }
            public double Y { get; set; }
            public int ScaleX { get; set; }
            public int ScaleY { get; set; }
            public SolidColorBrush color { get; set; }
        }
        

        class Person : INotifyPropertyChanged
        {
            private string name;
            public string Name { get { return name; }
                set {
                    if (name != value)
                    {
                        name = value;
                        NotifyPropertyChanged();
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }


        }

        ObservableCollection<Person> people = new ObservableCollection<Person>( new List<Person>()
        {
            new Person(){Name="A"},
            new Person(){Name="B"},
            new Person(){Name="C"},
            new Person(){Name="D"},
        });

        public MainWindow()
        {
            InitializeComponent();
            en.Lists.Load();

            CB.DataContext = en.Lists.Local;
                
        }



        private void Phone_TextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void Phone_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;

            // Preserve caret position so that we can later restore it
            int pos = tb.CaretIndex;

            tb.Text = CapVowels(tb.Text);

            tb.CaretIndex = pos;
        }

        private string CapVowels(string input)
        {
            const string vowels = "aeiou";

            StringBuilder sbInput = new StringBuilder(input);
            for (int i = 0; i < sbInput.Length; i++)
            {
                if (vowels.Contains(char.ToLowerInvariant(sbInput[i])))
                    sbInput[i] = char.ToUpper(sbInput[i]);
            }

            return sbInput.ToString();
        }

        bool leftDown = false;

        private void Re_MouseDown(object sender, MouseButtonEventArgs e)
        {
            leftDown = true;
            if (selected != null)
            {
                Table t = GetSelectedTable();
                if (t.Shape == Shape.Rectangle)
                {
                    selected.Template = ButtonFactory.GetRectangle(t.color);
                }
                else
                {
                    selected.Template = ButtonFactory.GetCircle(t.color);
                }
            }
            foreach (Button shape in C.Children)
            {
                Rect r = new Rect(new Point(VisualTreeHelper.GetOffset(shape).X, VisualTreeHelper.GetOffset(shape).Y), new Size(shape.Width, shape.Height));
                if (r.Contains(e.GetPosition(C)))
                {
                    shape.BorderBrush = Brushes.Blue;
                    shape.Background = Brushes.Blue;
                    selected = shape;

                        //selected = shape;
                        Table tb = tables[selected.Name];
                        if (tb.Shape == Shape.Rectangle)
                        {
                            shape.Template = ButtonFactory.GetRectangle(Brushes.LightGray);
                        }
                        else
                        {
                            shape.Template = ButtonFactory.GetCircle(Brushes.LightGray);
                        }
                    
                }
                else
                {
                    shape.BorderBrush = Brushes.Red;
                }

            }
        }

        private void Re_MouseMove(object sender, MouseEventArgs e)
        {
            if (selected != null && leftDown)
            {
                Table t = GetSelectedTable();
                double newX = e.GetPosition(C).X - selected.Width / 2;
                double newY = e.GetPosition(C).Y - selected.Height / 2;
                t.X = newX;
                t.Y = newY;
                Canvas.SetLeft(selected, t.X);
                Canvas.SetTop(selected, t.Y);
            }
        }

        private void CreateTable(Shape type)
        {
            Table t = new Table()
            {
                Id = "S" + id,
                Text = id.ToString(),
                Width = 40,
                Height = 40,
                X = 50,
                Y = 50,
                RotateAngle = 0,
                Shape = type,
                ScaleX = 0,
                ScaleY = 0,
                color = Brushes.LightGreen
            };

            tables.Add(t.Id, t);

            Button el = new Button();
            el.Content =t.Text;
            el.Name = t.Id;
            id++;
            el.Width = t.Width + t.ScaleX;
            el.Height = t.Height + t.ScaleY;
            Canvas.SetLeft(el, t.X);
            Canvas.SetTop(el, t.Y);
            el.BorderBrush = Brushes.Red;
            //Style s = this.FindResource(type == Table.Round ? "Circle" : "Rectangle") as Style;

            //el.Style = this.FindResource(type == Table.Round ? "Circle" : "Rectangle") as Style;
            if (type == Shape.Circle)
            {
                el.Template = ButtonFactory.GetCircle(Brushes.LightGreen);
            }
            else
            {
                el.Template = ButtonFactory.GetRectangle(Brushes.LightGreen);
            }

            C.Children.Add(el);
            
        }

        int angle = 45;

        private Table GetSelectedTable()
        {
            return tables[selected.Name];
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var b = sender as Button;
            if(b.Content.ToString() == "Drag")
            {
                mode = Modes.Drag;
            }
            else if(b.Content.ToString() == "Select")
            {
                mode = Modes.Select;
                b.Content = "Unselect";
               
            }
            else if (b.Content.ToString() == "Unselect")
            {
                mode = Modes.None;
                Table t = GetSelectedTable();
                if (t.Shape == Shape.Rectangle)
                {
                    selected.Template = ButtonFactory.GetRectangle(t.color);
                }
                else
                {
                    selected.Template = ButtonFactory.GetCircle(t.color);
                }
                selected = null;
                b.Content = "Select";
            }
            else if (b.Content.ToString() == "Rotate")
            {
                if(selected != null)
                {
                    Table t = GetSelectedTable();
                    RotateTransform rotateTransform = new RotateTransform(angle);
                    rotateTransform.CenterX = t.Width / 2;
                    rotateTransform.CenterY = t.Height / 2;
                    t.RotateAngle += 45;
                    angle = t.RotateAngle;                    
                    selected.RenderTransform = rotateTransform;
                }
            }
            else if (b.Content.ToString() == "ScaleX")
            {
                if(selected != null)
                {
                    Table t = GetSelectedTable();
                    t.ScaleX += 10;
                    selected.Width = t.Width + t.ScaleX;                    
                }
            }
            else if (b.Content.ToString() == "ScaleY")
            {
                Table t = GetSelectedTable();
                t.ScaleY += 10;
                selected.Height = t.Height + t.ScaleY;
                //((List)CB.SelectedItem).Name = "TEST";
                //people.Add(new Person() { Name = "J" });
                //people[0].Name = "Z";
                //CB.Items.Refresh();
            }
            else if(b.Content.ToString() == "Circle")
            {
                CreateTable(Shape.Circle);
            }
            else if (b.Content.ToString() == "Rectangle")
            {
                CreateTable(Shape.Rectangle);
            }
            else
            {
                mode = Modes.None;
            }
        }


        private void C_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            mode = Modes.None;
        }

        private void C_TouchMove(object sender, TouchEventArgs e)
        {
            if (s!= null)
            {
                Canvas.SetLeft(s, e.GetTouchPoint(C).Position.X - s.Width / 2);
                Canvas.SetTop(s, e.GetTouchPoint(C).Position.Y - s.Height / 2);
            }
        }

        private void C_TouchDown(object sender, TouchEventArgs e)
        {
            //if (mode == Modes.Select)
            //{
                foreach (Button shape in C.Children)
                {
                    Rect r = new Rect(new Point(VisualTreeHelper.GetOffset(shape).X, VisualTreeHelper.GetOffset(shape).Y), new Size(shape.Width, shape.Height));
                    if (r.Contains(e.GetTouchPoint(C).Position))
                    {
                        shape.BorderBrush = Brushes.Blue;
                        shape.Background = Brushes.Blue;
                        s = shape;
                        if(mode == Modes.Select)
                        {
                        selected = shape;
                        shape.Template = ButtonFactory.GetRectangle(Brushes.LightGray);
                    }
                    }
                    else
                    {
                        shape.BorderBrush = Brushes.Red;
                    }

                }
            //}
            }

        private void C_TouchUp(object sender, TouchEventArgs e)
        {
            s = null;
        }

        private void C_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            leftDown = false;
            //selected = null;
        }
    }

    public class CapVowelsConverter : IValueConverter
    {
        // From bound property TO the control -- no conversion
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        // To bound property FROM the control -- capitalize vowels
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string input = (string)value;

            const string vowels = "aeiou";

            StringBuilder sbInput = new StringBuilder(input);
            for (int i = 0; i < sbInput.Length; i++)
            {
                if (vowels.Contains(char.ToLowerInvariant(sbInput[i])))
                    sbInput[i] = char.ToUpper(sbInput[i]);
            }

            return sbInput.ToString();
        }
    }

}
