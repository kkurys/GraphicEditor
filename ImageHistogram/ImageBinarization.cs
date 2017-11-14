using Images;
using System;

namespace ImageHistogram
{
    public abstract class ImageBinarization
    {
        protected DirectBitmap _bitmap;
        protected Histogram _histogram;
        protected int[] _originalBits;
        public ImageBinarization(DirectBitmap bitmap, Histogram histogram)
        {
            _bitmap = bitmap;
            _histogram = histogram;
            _originalBits = new int[bitmap.Width * bitmap.Height];
            Array.Copy(_bitmap.Bits, _originalBits, _bitmap.Bits.Length);
        }
        public void ResetToDefault()
        {
            for (int i = 0; i < _bitmap.Height * _bitmap.Width; i++)
            {
                _bitmap.Bits[i] = _originalBits[i];
            }
        }

        public virtual void Binarize(int level)
        {

        }
    }
}
