using System.Drawing;

namespace Images
{
    public class PPM_P3Parser
    {
        public static Bitmap Parse(PPMImage image)
        {

            if (image.BitsCount / 3 != image.Columns * image.Rows)
            {
                throw new System.Exception("Bytes doesn't match specified image size!");
            }
            DirectBitmap bitMap = new DirectBitmap(image.Bits, image.Columns, image.Rows);

            return bitMap.Bitmap;
        }
        public static Bitmap Scale(PPMImage image)
        {
            if (image.BitsCount / 3 != image.Columns * image.Rows)
            {
                throw new System.Exception("Bytes doesn't match specified image size!");
            }
            DirectBitmap bitMap = new DirectBitmap(image.Columns, image.Rows);
            for (int i = 0; i < image.Bits.Length; i++)
            {
                var color = Color.FromArgb(image.Bits[i]);
                var newR = color.R * image.MaxValue / image.MaxR;
                var newG = color.G * image.MaxValue / image.MaxG;
                var newB = color.B * image.MaxValue / image.MaxB;
                bitMap.Bits[i] = Color.FromArgb(newR, newG, newB).ToArgb();

            }
            return bitMap.Bitmap;
        }
    }
}
