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

        Modes mode = Modes.None;
        Button s = null;
        Button selected = null;
        int id = 0;

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

        private void Re_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Re_MouseMove(object sender, MouseEventArgs e)
        {
            if (mode == Modes.Drag)
            {
                Canvas.SetLeft(s, e.GetPosition(C).X - s.Width / 2);
                Canvas.SetTop(s, e.GetPosition(C).Y - s.Height / 2);
            }
        }

        private void CreateTable(Table type)
        {
            Button el = new Button();
            el.Content = id;
            id++;
            el.Width = 40;
            el.Height = 40;
            Canvas.SetLeft(el, 50);
            Canvas.SetTop(el, 50);
            el.BorderBrush = Brushes.Red;
            //Style s = this.FindResource(type == Table.Round ? "Circle" : "Rectangle") as Style;

            //el.Style = this.FindResource(type == Table.Round ? "Circle" : "Rectangle") as Style;
            if (type == Table.Round)
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
                selected = null;
                b.Content = "Select";
            }
            else if (b.Content.ToString() == "Rotate")
            {
                if(selected != null)
                {
                    RotateTransform rotateTransform = new RotateTransform(angle);
                    rotateTransform.CenterX = selected.Width / 2;
                    rotateTransform.CenterY = selected.Height / 2;
                    angle += 45;
                    selected.RenderTransform = rotateTransform;
                }
            }
            else if (b.Content.ToString() == "ScaleX")
            {
                if(selected != null)
                {
                    selected.Width += 10;
                    selected.UpdateLayout();
                }
            }
            else if (b.Content.ToString() == "ScaleY")
            {
                ((List)CB.SelectedItem).Name = "TEST";
                //people.Add(new Person() { Name = "J" });
                //people[0].Name = "Z";
                //CB.Items.Refresh();
            }
            else if(b.Content.ToString() == "Circle")
            {
                CreateTable(Table.Round);
            }
            else if (b.Content.ToString() == "Rectangle")
            {
                CreateTable(Table.Square);
            }
            else
            {
                mode = Modes.None;
            }
        }

        private void C_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(mode == Modes.Select)
            {
                foreach (Button shape in C.Children)
                {
                    Rect r = new Rect(new Point(Canvas.GetLeft(shape), Canvas.GetTop(shape)), new Size(shape.Width, shape.Height));
                    if (r.Contains(e.GetPosition(C)))
                    {
                        shape.BorderBrush = new SolidColorBrush(Colors.Blue);
                        s = shape;
                    }
                    else
                    {
                        shape.BorderBrush = new SolidColorBrush(Colors.Red);
                    }

                }

                //HitTestResult result = VisualTreeHelper.HitTest(Re, e.GetPosition(C));
                //((Shape)result.VisualHit).Stroke = new SolidColorBrush(Colors.Blue);
                //s = ((Shape)result.VisualHit);
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
