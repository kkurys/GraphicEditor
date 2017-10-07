using System.Windows;
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
    }
}
