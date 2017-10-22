using System;

namespace RGBCmykConverter
{
    public class CMYK
    {
        public double Cyan { get; set; } = 0;
        public double Magenta { get; set; } = 0;
        public double Yellow { get; set; } = 0;
        public double Black { get; set; } = 1;
        public RGB ConvertToRgb()
        {
            int Red = Convert.ToInt32(255 * (1 - Math.Min(1, Cyan * (1 - Black) + Black)));
            int Green = Convert.ToInt32(255 * (1 - Math.Min(1, Magenta * (1 - Black) + Black)));
            int Blue = Convert.ToInt32(255 * (1 - Math.Min(1, Yellow * (1 - Black) + Black)));
            return new RGB()
            {
                Red = Red,
                Green = Green,
                Blue = Blue
            };
        }
        public void UpdateFromRGB(RGB rgb)
        {
            double R = rgb.Red / 255.0;
            double G = rgb.Green / 255.0;
            double B = rgb.Blue / 255.0;
            if (R == 0 && G == 0 && B == 0)
            {
                Black = 1;
                Cyan = 0;
                Magenta = 0;
                Yellow = 0;
                return;
            }
            Black = Math.Min(Math.Min(1 - R, 1 - G), 1 - B);
            Cyan = (1 - R - Black) / (1 - Black);
            Magenta = (1 - G - Black) / (1 - Black);
            Yellow = (1 - B - Black) / (1 - Black);
        }
    }
}
