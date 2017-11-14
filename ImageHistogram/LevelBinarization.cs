using Images;
using System.Drawing;

namespace ImageHistogram
{
    public class LevelBinarization : ImageBinarization
    {
        public LevelBinarization(DirectBitmap bitmap, Histogram histogram) : base(bitmap, histogram)
        {
        }

        public override void Binarize(int level)
        {
            ResetToDefault();
            for (int i = 0; i < _bitmap.Height; i++)
            {
                for (int j = 0; j < _bitmap.Width; j++)
                {
                    var color = Color.FromArgb(_bitmap.Bits[i * _bitmap.Width + j]);

                    var newR = color.R > level ? 255 : 0;
                    var newG = color.G > level ? 255 : 0;
                    var newB = color.B > level ? 255 : 0;

                    var avg = (int)(newR + newG + newB) / 3;

                    _bitmap.Bits[i * _bitmap.Width + j] = Color.FromArgb(avg, avg, avg).ToArgb();
                }
            }
            _histogram.GenerateHistograms();
        }
    }
}
