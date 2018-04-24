using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Test
{
    static class ButtonFactory
    {
        public static ControlTemplate GetCircle(SolidColorBrush brush)
        {
            ControlTemplate circleButtonTemplate = new ControlTemplate(typeof(Button));
            
            // Create the circle
            FrameworkElementFactory shape = new FrameworkElementFactory(typeof(Ellipse));
            shape.SetValue(Ellipse.FillProperty, brush);
            shape.SetValue(Ellipse.StrokeProperty, Brushes.Black);
            shape.SetValue(Ellipse.StrokeThicknessProperty, 1.0);

            // Create the ContentPresenter to show the Button.Content
            FrameworkElementFactory presenter = new FrameworkElementFactory(typeof(ContentPresenter));
            presenter.SetValue(ContentPresenter.ContentProperty, new TemplateBindingExtension(Button.ContentProperty));
            presenter.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            presenter.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);

            // Create the Grid to hold both of the elements
            FrameworkElementFactory grid = new FrameworkElementFactory(typeof(Grid));
            grid.AppendChild(shape);
            grid.AppendChild(presenter);

            // Set the Grid as the ControlTemplate.VisualTree
            circleButtonTemplate.VisualTree = grid;

            return circleButtonTemplate;
        }

        public static ControlTemplate GetRectangle(SolidColorBrush brush)
        {
            ControlTemplate circleButtonTemplate = new ControlTemplate(typeof(Button));

            // Create the circle
            FrameworkElementFactory shape = new FrameworkElementFactory(typeof(Rectangle));
            shape.SetValue(Rectangle.FillProperty, brush);
            shape.SetValue(Rectangle.StrokeProperty, Brushes.Black);
            shape.SetValue(Rectangle.StrokeThicknessProperty, 1.0);

            // Create the ContentPresenter to show the Button.Content
            FrameworkElementFactory presenter = new FrameworkElementFactory(typeof(ContentPresenter));
            presenter.SetValue(ContentPresenter.ContentProperty, new TemplateBindingExtension(Button.ContentProperty));
            presenter.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            presenter.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);

            // Create the Grid to hold both of the elements
            FrameworkElementFactory grid = new FrameworkElementFactory(typeof(Grid));
            grid.AppendChild(shape);
            grid.AppendChild(presenter);

            // Set the Grid as the ControlTemplate.VisualTree
            circleButtonTemplate.VisualTree = grid;

            return circleButtonTemplate;
        }

    }
}
