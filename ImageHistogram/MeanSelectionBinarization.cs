using Images;
using ImageTools;

namespace ImageHistogram
{
    public class MeanSelectionBinarization : ImageBinarization
    {
        private LevelBinarization _levelBinarization;
        public MeanSelectionBinarization(DirectBitmap bitmap, Histogram histogram) : base(bitmap, histogram)
        {
            _levelBinarization = new LevelBinarization(bitmap, histogram);
        }
        public override void Binarize(int level)
        {
            ResetToDefault();
            PointTransformation.ConvertToGray(_bitmap, GrayConversionMode.Colorimetric);
            _histogram.GenerateHistograms();
            int Tk = 100;
            while (true)
            {
                int leftSum = 0;
                int leftBottomSum = 0;
                int rightSum = 0;
                int rightBottomSum = 0;

                for (int j = 0; j <= Tk; j++)
                {
                    leftSum += _histogram.RedHistogram[j] * j;
                    leftBottomSum += _histogram.RedHistogram[j];
                }

                for (int j = Tk + 1; j <= 255; j++)
                {
                    rightSum += _histogram.RedHistogram[j] * j;
                    rightBottomSum += _histogram.RedHistogram[j];
                }
                if (rightBottomSum == 0) rightBottomSum = 255;
                if (leftBottomSum == 0) leftBottomSum = 1;

                if (leftSum / (2 * leftBottomSum) == rightSum / (2 * rightBottomSum)) break;
                if (Tk == (leftSum / (2 * leftBottomSum)) + (rightSum / (2 * rightBottomSum))) break;
                Tk = (leftSum / (2 * leftBottomSum)) + (rightSum / (2 * rightBottomSum));
            }
            _levelBinarization.Binarize(Tk);

        }
    }
}
