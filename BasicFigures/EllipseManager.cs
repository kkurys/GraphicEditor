using System;
using System.Windows;
using System.Windows.Shapes;

namespace BasicFigures
{
    public class EllipseManager : FigureManager
    {
        public override void Transform(Shape ellipse, Point mousePosition)
        {
            ellipse.Width = 2 * (Math.Abs(StartX - mousePosition.X) + Math.Abs(StartY - mousePosition.Y));
            ellipse.Height = ellipse.Width;
            OffsetLeft = StartX - (ellipse.Width / 2);
            OffsetTop = StartY - (ellipse.Height / 2);
        }
    }
}
