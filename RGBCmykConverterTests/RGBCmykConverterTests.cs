using Microsoft.VisualStudio.TestTools.UnitTesting;
using RGBCmykConverter;
using System;

namespace RGBCmykConverterTests
{
    [TestClass]
    public class RGBCmykConverterTests
    {
        [TestMethod]
        public void RGB_gets_converted_to_CMYK()
        {
            RGB rgb = new RGB()
            {
                Red = 255,
                Green = 66,
                Blue = 100,
            };

            var result = rgb.ConvertToCMYK();

            Assert.AreEqual(0, result.Cyan);
            Assert.AreEqual(0, result.Black);
            Assert.AreEqual(0.741, Math.Round(result.Magenta, 3));
            Assert.AreEqual(0.608, Math.Round(result.Yellow, 3));

        }
        [TestMethod]
        public void CMYK_get_converted_to_RGB()
        {
            CMYK cmyk = new CMYK()
            {
                Cyan = 0,
                Black = 0,
                Magenta = 0.741,
                Yellow = 0.608
            };

            var result = cmyk.ConvertToRgb();

            Assert.AreEqual(255, result.Red);
            Assert.AreEqual(66, result.Green);
            Assert.AreEqual(100, result.Blue);
        }
    }
}
