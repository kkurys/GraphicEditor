using Images;
using ImageTools;
using System;

namespace ImageHistogram
{
    public class BlackSelectionBinarization : ImageBinarization
    {
        private LevelBinarization _levelBinarization;
        public BlackSelectionBinarization(DirectBitmap bitmap, Histogram histogram) : base(bitmap, histogram)
        {
            _levelBinarization = new LevelBinarization(bitmap, histogram);
        }
        public override void Binarize(int level)
        {
            ResetToDefault();
            int desiredPixels = Convert.ToInt32(_bitmap.Width * _bitmap.Height * ScaleLevelToPercentage(level));
            PointTransformation.ConvertToGray(_bitmap, GrayConversionMode.Colorimetric);
            _histogram.GenerateHistograms();
            int sum = _bitmap.Width * _bitmap.Height;

            for (int i = 255; i >= 0; i--)
            {
                sum -= _histogram.RedHistogram[i];
                if (sum < desiredPixels)
                {
                    level = i + 1;
                    break;
                }
            }
            _levelBinarization.Binarize(level);
        }
        private double ScaleLevelToPercentage(int level)
        {
            return level / 255.0;
        }
    }
}
