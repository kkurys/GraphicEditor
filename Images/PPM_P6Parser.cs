using System.Drawing;

namespace Images
{

    public class PPM_P6Parser
    {
        public static Bitmap Parse(PPMImage image)
        {
            DirectBitmap bitMap = new DirectBitmap(image.Columns, image.Rows);
            int stringPos = 0;
            int length = image.ImageString.Length;
            if (image.ImageString.Length / 3 != image.Columns * image.Rows)
            {
                throw new System.Exception("Bytes doesn't match specified image size!");
            }
            for (int i = 0; i < image.Rows; i++)
            {
                for (int j = 0; j < image.Columns; j++)
                {
                    int R = (int)image.ImageString[stringPos];
                    int G = (int)image.ImageString[stringPos + 1];
                    int B = (int)image.ImageString[stringPos + 2];
                    stringPos += 3;
                    bitMap.Bits[i * image.Columns + j] = Color.FromArgb(R, G, B).ToArgb();
                }
            }
            return bitMap.Bitmap;
        }

    }
}
