using System.Windows;
using System.Windows.Shapes;

namespace BasicFigures
{
    public class LineManager : FigureManager
    {
        protected override Shape CreateElement()
        {
            Line line = new Line()
            {
                X1 = StartX,
                Y1 = StartY
            };

            return line;
        }

        public override void Transform(Shape shape, Point mousePosition)
        {
            var line = (Line)shape;
            line.X2 = mousePosition.X;
            line.Y2 = mousePosition.Y;
        }
    }
}
