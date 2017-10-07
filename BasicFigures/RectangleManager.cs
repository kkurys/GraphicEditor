using System;
using System.Windows;
using System.Windows.Shapes;

namespace BasicFigures
{
    public class RectangleManager : FigureManager
    {
        public override void Transform(Shape rectangle, Point mousePosition)
        {
            OffsetLeft = rectangle.Margin.Left;
            OffsetTop = rectangle.Margin.Top;
            rectangle.Width = Math.Abs(mousePosition.X - StartX);
            if (mousePosition.X - StartX > 0)
            {
                OffsetLeft = StartX;
            }
            else
            {
                OffsetLeft = mousePosition.X - OffsetLeft;
            }
            rectangle.Height = Math.Abs(mousePosition.Y - StartY);
            if (mousePosition.Y - StartY > 0)
            {
                OffsetTop = StartY;
            }
            else
            {
                OffsetTop = mousePosition.Y - OffsetTop;
            }
        }
    }
}
