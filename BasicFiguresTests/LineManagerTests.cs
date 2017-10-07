using BasicFigures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;
using System.Windows.Shapes;

namespace BasicFiguresTests
{
    [TestClass]
    public class LineManagerTests
    {
        [TestMethod]
        public void line_X2_and_Y2_gets_properly_set()
        {
            FigureManager _manager = new LineManager();
            Line line = new Line();
            Point mousePosition = new Point(3, 3);
            _manager.Set(0, 0);
            line.X1 = 0;
            line.Y1 = 0;

            _manager.Transform(line, mousePosition);
            Assert.AreEqual(3, line.X2);
            Assert.AreEqual(3, line.Y2);

        }
    }
}
