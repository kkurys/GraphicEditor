using Images;
using ImageTools;
using System;

namespace ImageHistogram
{
    public class EntropySelectionBinarization : ImageBinarization
    {
        private LevelBinarization _levelBinarization;
        public EntropySelectionBinarization(DirectBitmap bitmap, Histogram histogram) : base(bitmap, histogram)
        {
            _levelBinarization = new LevelBinarization(bitmap, histogram);
        }
        public override void Binarize(int level)
        {
            ResetToDefault();
            PointTransformation.ConvertToGray(_bitmap, GrayConversionMode.Colorimetric);
            _histogram.GenerateHistograms();
            int pixels = _bitmap.Height * _bitmap.Width;
            decimal Const = 255 / (decimal)pixels;

            int[] cdf = new int[256];

            Array.Copy(_histogram.RedHistogram, cdf, 255);

            for (int r = 1; r <= 255; r++)
            {
                cdf[r] = cdf[r] + cdf[r - 1];
            }
            decimal Pob = 0.0M, Pb = 0.0M;
            decimal Hob = 0.0M, Hb = 0.0M;
            for (int i = 0; i <= level; i++)
            {
                Pob += cdf[i] * Const;
            }
            for (int i = 0; i <= level; i++)
            {
                Hob += (cdf[i] * Const) / Pob * Convert.ToDecimal(Math.Log(Convert.ToDouble((cdf[i] * Const) / Pob), 2));
            }
            for (int i = level + 1; i <= 255; i++)
            {
                Pb += cdf[i] * Const;
            }
            for (int i = level + 1; i <= 255; i++)
            {
                Hb += (cdf[i] * Const) / Pb * Convert.ToDecimal(Math.Log(Convert.ToDouble((cdf[i] * Const) / Pb), 2));
            }
            level = (int)(-1 * (Hob + Hb));
            _levelBinarization.Binarize(level);
        }
    }
}
