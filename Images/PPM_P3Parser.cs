using System.Drawing;

namespace Images
{
    public class PPM_P3Parser
    {
        public static Bitmap Parse(PPMImage image)
        {

            if (image.Bits.Length != image.Columns * image.Rows)
            {
                throw new System.Exception("Bytes doesn't match specified image size!");
            }
            DirectBitmap bitMap = new DirectBitmap(image.Bits, image.Columns, image.Rows);

            return bitMap.Bitmap;
        }
    }
}
