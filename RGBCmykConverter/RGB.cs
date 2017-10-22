using System;

namespace RGBCmykConverter
{
    public class RGB
    {
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }

        public CMYK ConvertToCMYK()
        {
            double R = Red / 255.0;
            double G = Green / 255.0;
            double B = Blue / 255.0;

            double Black = Math.Min(Math.Min(1 - R, 1 - G), 1 - B);
            double Cyan = (1 - R - Black) / (1 - Black);
            double Magenta = (1 - G - Black) / (1 - Black);
            double Yellow = (1 - B - Black) / (1 - Black);
            return new CMYK()
            {
                Black = Black,
                Cyan = Cyan,
                Magenta = Magenta,
                Yellow = Yellow
            };
        }
        public void UpdateFromCMYK(CMYK cmyk)
        {
            Red = Convert.ToInt32(255 * (1 - Math.Min(1, cmyk.Cyan * (1 - cmyk.Black) + cmyk.Black)));
            Green = Convert.ToInt32(255 * (1 - Math.Min(1, cmyk.Magenta * (1 - cmyk.Black) + cmyk.Black)));
            Blue = Convert.ToInt32(255 * (1 - Math.Min(1, cmyk.Yellow * (1 - cmyk.Black) + cmyk.Black)));
        }
    }
}
