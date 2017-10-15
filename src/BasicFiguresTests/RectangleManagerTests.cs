using BasicFigures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;
using System.Windows.Shapes;

namespace BasicFiguresTests
{
    [TestClass]
    public class RectangleManagerTests
    {
        [TestMethod]
        public void rectangle_width_and_height_gets_properly_adjusted()
        {
            FigureManager _manager = new RectangleManager();
            Rectangle rectangle = new Rectangle();
            Point mousePosition = new Point(3, 4);
            _manager.Set(0, 0);

            _manager.Transform(rectangle, mousePosition);
            Assert.AreEqual(4, rectangle.Height);
            Assert.AreEqual(3, rectangle.Width);
        }
        [TestMethod]
        public void rectangle_offset_left_gets_properly_adjusted_when_width_would_be_negative()
        {
            FigureManager _manager = new RectangleManager();
            Rectangle rectangle = new Rectangle();
            Point mousePosition = new Point(3, 4);
            _manager.Set(5, 5);

            _manager.Transform(rectangle, mousePosition);
            Assert.AreEqual(3, _manager.OffsetLeft);
        }
        [TestMethod]
        public void rectangle_offset_top_gets_properly_adjusted_when_height_would_be_negative()
        {
            FigureManager _manager = new RectangleManager();
            Rectangle rectangle = new Rectangle();
            Point mousePosition = new Point(3, 4);
            _manager.Set(5, 5);

            _manager.Transform(rectangle, mousePosition);
            Assert.AreEqual(4, _manager.OffsetTop);
        }
    }
}
