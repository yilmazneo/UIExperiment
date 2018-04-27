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

    public enum TableShape
    {
        Circle,
        Rectangle
    }

    public interface ITableLayoutView
    {
        void Update(Table table,TableLayoutUpdateMode mode);
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ITableLayoutView
    {
        TableController tableController;
        SolidColorBrush selectedTableColor = Brushes.LightGray;

        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        Entities en = new Entities();
        Dictionary<string, Table> tables = new Dictionary<string, Table>();

        Button s = null;
        Button selected = null;
        int id = 0;



        public MainWindow()
        {
            InitializeComponent();
            TableRepository repository = new TableRepository();
            tableController = new TableController(repository, this);

            en.Lists.Load();

            CB.DataContext = en.Lists.Local;

        }

        bool leftDown = false;

        private void Re_MouseDown(object sender, MouseButtonEventArgs e)
        {
            leftDown = true;
            //if (selected != null)
            //{
            //    Table t = GetSelectedTable();
            //    if (t.Shape == TableShape.Rectangle)
            //    {
            //        selected.Template = ButtonFactory.GetRectangle(t.Color);
            //    }
            //    else
            //    {
            //        selected.Template = ButtonFactory.GetCircle(t.Color);
            //    }
            //}
            foreach (Button shape in C.Children)
            {
                Rect r = new Rect(new Point(VisualTreeHelper.GetOffset(shape).X, VisualTreeHelper.GetOffset(shape).Y), new Size(shape.Width, shape.Height));
                if (r.Contains(e.GetPosition(C)))
                {
                    if(selected != null)
                    {
                        tableController.UpdateModel(selected.Name, TableAction.SetUnselected, null);
                    }
                    selected = shape;
                    tableController.UpdateModel(selected.Name, TableAction.SetSelected, null);
                }
            }
        }

        private void Re_MouseMove(object sender, MouseEventArgs e)
        {
            if (selected != null && leftDown)
            {
                double newX = e.GetPosition(C).X - selected.Width / 2;
                double newY = e.GetPosition(C).Y - selected.Height / 2;
                Dictionary<UpdateKey, object> arguments = new Dictionary<UpdateKey, object>();
                arguments.Add(UpdateKey.X, newX);
                arguments.Add(UpdateKey.Y, newY);
                tableController.UpdateModel(selected.Name, TableAction.UpdateCoordinates, arguments);
            }
        }

        private void CreateTable(Table model)
        {
            Button el = new Button();
            el.Content = model.Text;
            el.Name = model.Id;            
            el.Width = model.Width;
            el.Height = model.Height;
            Canvas.SetLeft(el, model.X);
            Canvas.SetTop(el, model.Y);
            el.BorderBrush = Brushes.Red;
            if (model.Shape == TableShape.Circle)
            {
                el.Template = ButtonFactory.GetCircle(Brushes.LightGreen);
            }
            else
            {
                el.Template = ButtonFactory.GetRectangle(Brushes.LightGreen);
            }

            C.Children.Add(el);           
        }

        private Table GetSelectedTable()
        {
            return tables[selected.Name];
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var b = sender as Button;
            if (b.Content.ToString() == "Update")
            {
                if(selected != null)
                {
                    Dictionary<UpdateKey, Object> arguments = new Dictionary<UpdateKey, object>();
                    arguments.Add(UpdateKey.Name, Name.Text);
                    tableController.UpdateModel(selected.Name, TableAction.UpdateName, arguments);
                }
            }
            else if (b.Content.ToString() == "Delete")
            {
                tables.Remove(selected.Name);
                C.Children.Remove(selected);
                selected = null;
            }
            else if (b.Content.ToString() == "Rotate")
            {
                if (selected != null)
                {
                    tableController.UpdateModel(selected.Name, TableAction.Rotate, null);
                }
            }
            else if (b.Content.ToString() == "ScaleX")
            {
                if (selected != null)
                {
                    tableController.UpdateModel(selected.Name, TableAction.ScaleX, null);
                }
            }
            else if (b.Content.ToString() == "ScaleY")
            {
                tableController.UpdateModel(selected.Name, TableAction.ScaleY, null);
            }
            else if (b.Content.ToString() == "Circle" || b.Content.ToString() == "Rectangle")
            {
                Dictionary<UpdateKey, Object> arguments = new Dictionary<UpdateKey, object>();
                arguments.Add(UpdateKey.Shape, b.Content.ToString() == "Circle" ? TableShape.Circle : TableShape.Rectangle);
                tableController.UpdateModel(null,TableAction.Create,arguments);
            }
            else if(b.Content.ToString() == "Dialog1")
            {
                DialogResultManager.ShowDialog("Title1","Message1",new DialogAnswer[] { DialogAnswer.Yes,DialogAnswer.No });
                Name.Text = DialogResultManager.Answer.ToString();
            }
            else if (b.Content.ToString() == "Dialog2")
            {
                DialogResultManager.ShowDialog("Title2", "Message2", new DialogAnswer[] { DialogAnswer.OK, DialogAnswer.Cancel });
                Name.Text = DialogResultManager.Answer.ToString();
            }
        }


        private void C_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
 
        }

        private void C_TouchMove(object sender, TouchEventArgs e)
        {
            if (s != null)
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
                    //if (mode == Modes.Select)
                    //{
                    //    selected = shape;
                    //    shape.Template = ButtonFactory.GetRectangle(Brushes.LightGray);
                    //}
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

        public void Update(Table model, TableLayoutUpdateMode mode)
        {
            if (mode == TableLayoutUpdateMode.New)
            {
                CreateTable(model);
            }
            else if (mode == TableLayoutUpdateMode.Delete)
            {
                Control c = GetControl(model.Id);
                C.Children.Remove(c);
            }
            else if(mode == TableLayoutUpdateMode.Coordinates)
            {
                if (selected != null)
                {
                    Canvas.SetLeft(selected, model.X);
                    Canvas.SetTop(selected, model.Y);
                }
            }
            else if (mode == TableLayoutUpdateMode.Rotate)
            {
                RotateTransform rotateTransform = new RotateTransform(model.RotateAngle);
                rotateTransform.CenterX = (model.Width + model.ScaleX)  / 2;
                rotateTransform.CenterY = (model.Height + model.ScaleY) / 2;
                selected.RenderTransform = rotateTransform;
            }
            else if (mode == TableLayoutUpdateMode.SetSelected)
            {
                Control c = GetControl(model.Id);
                UpdateTableTemplate(c, model.Shape, selectedTableColor);
            }
            else if (mode == TableLayoutUpdateMode.SetUnselected)
            {
                Control c = GetControl(model.Id);
                UpdateTableTemplate(c, model.Shape, model.Color);
            }
            else if (mode == TableLayoutUpdateMode.ScaleX)
            {
                Control c = GetControl(model.Id);
                c.Width = model.Width + model.ScaleX;
            }
            else if (mode == TableLayoutUpdateMode.ScaleY)
            {
                Control c = GetControl(model.Id);
                c.Height = model.Height + model.ScaleY;
            }
            else if (mode == TableLayoutUpdateMode.UpdateName)
            {
                Button b = (Button)GetControl(model.Id);
                b.Content = model.Text;
            }
        }

        private void UpdateTableTemplate(Control c, TableShape shape, SolidColorBrush color)
        {
            if (shape == TableShape.Rectangle)
            {
                c.Template = ButtonFactory.GetRectangle(color);
            }
            else
            {
                c.Template = ButtonFactory.GetCircle(color);
            }
        }

        private Control GetControl(string id)
        {
            Control c = null;
            foreach(Control child in C.Children)
            {
                if(child.Name == id)
                {
                    c = child;
                    break;
                }
            }
            return c;
        }

    }

}
