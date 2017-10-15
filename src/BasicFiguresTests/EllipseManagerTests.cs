using BasicFigures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;
using System.Windows.Shapes;

namespace BasicFiguresTests
{
    [TestClass]
    public class EllipseManagerTests
    {
        [TestMethod]
        public void ellipse_width_is_properly_calculated()
        {
            FigureManager manager = new EllipseManager();
            Ellipse ellipse = new Ellipse();
            Point mousePosition = new Point(4, 3);
            manager.Set(3, 3);
            manager.Transform(ellipse, mousePosition);

            Assert.AreEqual(2, ellipse.Width);
        }
        [TestMethod]
        public void ellipse_height_is_properly_calculated()
        {
            FigureManager manager = new EllipseManager();
            Ellipse ellipse = new Ellipse();
            Point mousePosition = new Point(4, 3);
            manager.Set(3, 3);
            manager.Transform(ellipse, mousePosition);

            Assert.AreEqual(2, ellipse.Width);
        }
        [TestMethod]
        public void ellipse_offset_left_is_properly_generated()
        {
            FigureManager manager = new EllipseManager();
            Ellipse ellipse = new Ellipse();
            Point mousePosition = new Point(4, 3);
            manager.Set(5, 5);
            manager.Transform(ellipse, mousePosition);

            Assert.AreEqual(2, manager.OffsetLeft);
        }
        [TestMethod]
        public void ellipse_offset_top_is_properly_generated()
        {
            FigureManager manager = new EllipseManager();
            Ellipse ellipse = new Ellipse();
            Point mousePosition = new Point(4, 3);
            manager.Set(5, 5);
            manager.Transform(ellipse, mousePosition);

            Assert.AreEqual(2, manager.OffsetTop);
        }
    }
}
