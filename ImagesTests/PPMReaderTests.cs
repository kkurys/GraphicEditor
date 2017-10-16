using Images;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImagesTests
{
    [TestClass]
    public class PPMReaderTests
    {
        [TestMethod]
        public void PPMImageReader_correctly_parses_P6_file()
        {
            var reader = new PPMReader("test1.ppm");
            var file = reader.ReadFile();

            Assert.AreEqual(1, file.BytesPerColor);
            Assert.AreEqual(PPMImageType.P6, file.Type);
            Assert.AreEqual(610, file.Columns);
            Assert.AreEqual(460, file.Rows);
        }
        [TestMethod]
        public void PPMImageReader_correctly_parses_P3_file()
        {
            var reader = new PPMReader("test2.ppm");
            var file = reader.ReadFile();
            Assert.AreEqual(1, file.BytesPerColor);
            Assert.AreEqual(PPMImageType.P3, file.Type);
            Assert.AreEqual(610, file.Columns);
            Assert.AreEqual(460, file.Rows);
        }
    }
}
