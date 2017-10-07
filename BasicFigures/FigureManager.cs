using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BasicFigures
{
    public abstract class FigureManager
    {
        public double StartX { get; set; }
        public double StartY { get; set; }
        public double OffsetTop { get; set; }
        public double OffsetLeft { get; set; }

        public void Set(double startX, double startY)
        {
            StartX = startX;
            StartY = startY;
        }
        public abstract void Transform(Shape shape, Point mousePosition);
        public virtual Shape GetElement(bool fill, SolidColorBrush fillColor = null)
        {
            Shape element = CreateElement();
            if (fill && fillColor != null)
            {
                element.Fill = fillColor;
            }
            return element;
        }
        protected virtual Shape CreateElement()
        {
            return null;
        }
    }
}
