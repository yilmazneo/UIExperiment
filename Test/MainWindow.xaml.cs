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


        private void HandleTouchDown(Point p)
        {
            leftDown = true;
            
            foreach (Button shape in C.Children)
            {
                Rect r = new Rect(new Point(VisualTreeHelper.GetOffset(shape).X, VisualTreeHelper.GetOffset(shape).Y), new Size(shape.Width, shape.Height));
                if (r.Contains(p))
                {
                    selected = shape;
                }
            }

            if (InDragMode()) return;


            if (selected != null)
            {
                Table t = tableController.GetModel(selected.Name);
                tablewindow w = new tablewindow(t);
                w.DataContext = this;
                bool? result = w.ShowDialog();
                if (result.HasValue && result.Value)
                {
                    tableController.UpdateModel(selected.Name, TableAction.UpdateAll, args);
                }
            }
            else
            {
                tablewindow w = new tablewindow(p.X, p.Y);
                w.DataContext = this;
                bool? result = w.ShowDialog();
                if (result.HasValue && result.Value)
                {
                    tableController.UpdateModel(null, TableAction.Create, args);
                }
            }
            selected = null;
        }

        private void HandleTouchUp()
        {
            leftDown = false;
            selected = null;
        }

        private void HandleTouchMove(Point p)
        {
            if (selected != null && leftDown && InDragMode())
            {
                double newX = p.X - selected.Width / 2;
                double newY = p.Y - selected.Height / 2;
                Dictionary<UpdateKey, object> arguments = new Dictionary<UpdateKey, object>();
                arguments.Add(UpdateKey.X, newX);
                arguments.Add(UpdateKey.Y, newY);
                tableController.UpdateModel(selected.Name, TableAction.UpdateCoordinates, arguments);
            }
        }

        bool leftDown = false;

        private void Re_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HandleTouchDown(e.GetPosition(C));
        }

        Dictionary<UpdateKey, object> args = new Dictionary<UpdateKey, object>();

        public void SetArgs(Dictionary<UpdateKey, object> args)
        {
            this.args = args;
        }


        bool InDragMode()
        {
            return DragRadioButton.IsChecked.HasValue && DragRadioButton.IsChecked.Value;
        }

        private void Re_MouseMove(object sender, MouseEventArgs e)
        {
            HandleTouchMove(e.GetPosition(C));
        }

        private void C_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            HandleTouchUp();
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
                el.Template = ButtonFactory.GetCircle((SolidColorBrush)new BrushConverter().ConvertFromString(model.Color));
            }
            else
            {
                el.Template = ButtonFactory.GetRectangle((SolidColorBrush)new BrushConverter().ConvertFromString(model.Color));
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
        }


      
        private void C_TouchMove(object sender, TouchEventArgs e)
        {
            HandleTouchMove(e.GetTouchPoint(C).Position);
        }

        private void C_TouchDown(object sender, TouchEventArgs e)
        {
            HandleTouchDown(e.GetTouchPoint(C).Position);
        }

        private void C_TouchUp(object sender, TouchEventArgs e)
        {
            HandleTouchUp();
        }



        private void UpdateRotateAngle(Table model)
        {
            RotateTransform rotateTransform = new RotateTransform(model.RotateAngle);
            rotateTransform.CenterX = (model.Width + model.ScaleX) / 2;
            rotateTransform.CenterY = (model.Height + model.ScaleY) / 2;
            if (selected != null)
            {
                selected.RenderTransform = rotateTransform;
            }
        }

        private void UpdateCoordinates(Table model)
        {
            if (selected != null)
            {
                Canvas.SetLeft(selected, model.X);
                Canvas.SetTop(selected, model.Y);
            }
        }

        private void UpdateScaleFactors(Table model)
        {
            if (selected != null)
            {
                Control c = GetControl(model.Id);
                c.Width = model.Width + model.ScaleX;
                c.Height = model.Height + model.ScaleY;
            }
        }

        private void UpdateColor(Table model)
        {
            if (selected != null)
            {
                Control c = GetControl(model.Id);
                UpdateTableTemplate(c, model.Shape, model.Color);
            }
        }

        private void UpdateName(Table model)
        {
            Button b = (Button)GetControl(model.Id);
            b.Content = model.Text;
        }

        public void Update(Table model, TableLayoutUpdateMode mode)
        {
            if(mode == TableLayoutUpdateMode.All)
            {
                UpdateColor(model);
                UpdateCoordinates(model);
                UpdateName(model);
                UpdateRotateAngle(model);
                UpdateScaleFactors(model);                
            }
            else if (mode == TableLayoutUpdateMode.New)
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
                UpdateTableTemplate(c, model.Shape, selectedTableColor.Color.ToString());
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

        private void UpdateTableTemplate(Control c, TableShape shape, string color)
        {
            SolidColorBrush brush = (SolidColorBrush)new BrushConverter().ConvertFromString(color);
            if (shape == TableShape.Rectangle)
            {
                c.Template = ButtonFactory.GetRectangle(brush);
            }
            else
            {
                c.Template = ButtonFactory.GetCircle(brush);
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
