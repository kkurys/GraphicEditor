using System.Windows.Shapes;

namespace GraphicEditor.ViewModels
{
    public class ShapeListItem
    {
        public double StartX, StartY;
        public Shape Shape { get; set; }
        public ShapeListItem(Shape shape)
        {
            Shape = shape;
        }
        public override string ToString()
        {
            if (Shape is Rectangle)
            {
                return "Prostokąt";
            }
            else if (Shape is Line)
            {
                return "Linia";
            }
            else
            {
                return "Okrąg";
            }
        }
    }
}
