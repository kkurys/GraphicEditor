using Images;
using System;
using System.Drawing;

namespace ImageBinarization
{
    public class ImageBinarization
    {
        private DirectBitmap _bitmap;
        private int[] _originalBits;
        public ImageBinarization(DirectBitmap bitmap)
        {
            _bitmap = bitmap;
            _originalBits = new int[bitmap.Width * bitmap.Height];
            Array.Copy(_bitmap.Bits, _originalBits, _bitmap.Bits.Length);
        }
        public void LevelBinarization(int level)
        {
            for (int i = 0; i < _bitmap.Height; i++)
            {
                for (int j = 0; j < _bitmap.Width; j++)
                {
                    var color = Color.FromArgb(_bitmap.Bits[i * _bitmap.Width + j]);

                    var newR = color.R > level ? 255 : 0;
                    var newG = color.G > level ? 255 : 0;
                    var newB = color.B > level ? 255 : 0;

                    _bitmap.Bits[i * _bitmap.Width + j] = Color.FromArgb(newR, newG, newB).ToArgb();
                }
            }
        }
    }
}
