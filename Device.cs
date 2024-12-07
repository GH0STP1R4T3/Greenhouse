using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;

namespace Greenhouse
{
    public class Device : Border
    {
        protected TextBlock textblock;
        protected string name = "";
        public Device (Canvas canvasReference, Admin adminReference, int corner_radius, string color, string name, int width=120, int height=50) : base()
        {
            this.name = name;
            this.CornerRadius = new System.Windows.CornerRadius(corner_radius);
            this.Width = width;
            this.Height = height;
            this.Margin = new System.Windows.Thickness(10);
            this.Padding = new System.Windows.Thickness(0, 0, 0, 0);

            this.Background = color switch
            {
                "aquamarine" => new SolidColorBrush(Colors.Aquamarine),
                "red" => new SolidColorBrush(Colors.Red),
                "blue" => new SolidColorBrush(Colors.CornflowerBlue),
                "violet" => new SolidColorBrush(Colors.Violet),
                "yellow" => new SolidColorBrush(Colors.Yellow),
                _ => new SolidColorBrush(Colors.AliceBlue),
            };
            this.BorderBrush = new SolidColorBrush(Colors.Black);
            this.BorderThickness = new System.Windows.Thickness(1);
            this.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);

            this.PreviewMouseMove += adminReference.MouseMoveHandler;
            this.PreviewMouseWheel += adminReference.MouseWheelHandler;
            this.PreviewMouseRightButtonDown += adminReference.MouseRightButtonDownHandler;

            textblock = new TextBlock()
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Text = name
            };

            this.Child = textblock;

            canvasReference.Children.Add(this);
        }

        public string GetName() {
            return this.name;
        }
    }
}
