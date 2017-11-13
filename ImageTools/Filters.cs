using Images;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ImageTools
{
    public class Filters
    {
        public static void AveragingFilter(DirectBitmap bitmap, int k)
        {
            for (int i = 0; i < bitmap.Height; i += 1)
            {
                for (int j = 0; j < bitmap.Width; j += 1)
                {
                    int sumR = 0, sumG = 0, sumB = 0;
                    for (int z = k / 2; z >= -1 * k / 2; z--)
                    {
                        for (int x = k / 2; x >= -1 * k / 2; x--)
                        {
                            if (x == 0 && z == 0) continue;

                            var t = i - z;
                            var c = j - x;

                            t = Normalize(t, bitmap.Height);
                            c = Normalize(c, bitmap.Width);

                            var color = Color.FromArgb(bitmap.Bits[t * bitmap.Width + c]);

                            sumR += color.R;
                            sumG += color.G;
                            sumB += color.B;
                        }
                    }
                    var div = k * k - 1;
                    bitmap.Bits[i * bitmap.Width + j] = Color.FromArgb(sumR / div, sumG / div, sumB / div).ToArgb();
                }
            }
        }
        public static void MeanFilter(DirectBitmap bitmap, int k)
        {
            for (int i = 0; i < bitmap.Height; i += 1)
            {
                for (int j = 0; j < bitmap.Width; j += 1)
                {
                    List<int> rList = new List<int>();
                    List<int> gList = new List<int>();
                    List<int> bList = new List<int>();
                    for (int z = k / 2; z >= -1 * k / 2; z--)
                    {
                        for (int x = k / 2; x >= -1 * k / 2; x--)
                        {
                            if (x == 0 && z == 0) continue;

                            var t = i - z;
                            var c = j - x;

                            t = Normalize(t, bitmap.Height);
                            c = Normalize(c, bitmap.Width);

                            var color = Color.FromArgb(bitmap.Bits[t * bitmap.Width + c]);

                            rList.Add(color.R);
                            gList.Add(color.G);
                            bList.Add(color.B);
                        }
                    }
                    rList.Sort();
                    bList.Sort();
                    gList.Sort();

                    bitmap.Bits[i * bitmap.Width + j] = Color.FromArgb(GetMean(rList), GetMean(gList), GetMean(bList)).ToArgb();
                }
            }
        }
        private static int GetMean(List<int> list)
        {
            if (list.Count == 0) return 0;
            if (list.Count == 1) return list[0];
            if (list.Count == 2) return (list[0] + list[1]) / 2;

            if (list.Count % 2 == 0)
            {
                return (list[list.Count / 2 - 1] + list[list.Count / 2]) / 2;
            }
            return list[list.Count / 2];
        }
        public static void SobelFilter(DirectBitmap bitmap)
        {
            int k = 3;
            double[] magnitudes = new double[bitmap.Width * bitmap.Height];
            for (int i = k / 2; i < bitmap.Height - k / 2; i += 1)
            {
                for (int j = k / 2; j < bitmap.Width - k / 2; j += 1)
                {
                    double Gx = 0, Gy = 0;
                    var topLeft = Color.FromArgb(bitmap.Bits[(i - 1) * bitmap.Width + j - 1]);
                    var top = Color.FromArgb(bitmap.Bits[(i - 1) * bitmap.Width + j]);
                    var topRight = Color.FromArgb(bitmap.Bits[(i - 1) * bitmap.Width + j + 1]);
                    var bottomLeft = Color.FromArgb(bitmap.Bits[(i + 1) * bitmap.Width + j - 1]);
                    var bottom = Color.FromArgb(bitmap.Bits[(i + 1) * bitmap.Width + j]);
                    var bottomRight = Color.FromArgb(bitmap.Bits[(i + 1) * bitmap.Width + j + 1]);
                    var left = Color.FromArgb(bitmap.Bits[(i) * bitmap.Width + j - 1]);
                    var right = Color.FromArgb(bitmap.Bits[(i) * bitmap.Width + j + 1]);
                    Gx += 1 * topLeft.R + 2 * top.R + 1 * topRight.R
                        - 1 * bottomLeft.R - 1 * bottom.R - 1 * bottomRight.R;
                    Gy += 1 * topLeft.R + 2 * left.R + 1 * bottomLeft.R
                        - 1 * topRight.R - 2 * right.R - 1 * bottomRight.R;
                    var G = Math.Sqrt(Gx * Gx + Gy * Gy);
                    magnitudes[i * bitmap.Width + j] = G;
                }
            }
            var maxMagni = magnitudes.Max();
            for (int i = 0; i < bitmap.Height * bitmap.Width; i++)
            {
                var x = (int)(magnitudes[i] / maxMagni * 255);
                if (x < 100) x = 0;
                if (x > 100) x = 255;
                bitmap.Bits[i] = Color.FromArgb(x, x, x).ToArgb();
            }
        }
        public static void HighPassFilter(DirectBitmap bitmap, int k)
        {
            k = 3;
            for (int i = 1; i < bitmap.Height - 1; i += 1)
            {
                for (int j = 1; j < bitmap.Width - 1; j += 1)
                {
                    int sumR = 0, sumG = 0, sumB = 0;
                    var top = Color.FromArgb(bitmap.Bits[(i - 1) * bitmap.Width + j]);
                    var bottom = Color.FromArgb(bitmap.Bits[(i + 1) * bitmap.Width + j]);
                    var left = Color.FromArgb(bitmap.Bits[(i) * bitmap.Width + j - 1]);
                    var right = Color.FromArgb(bitmap.Bits[(i) * bitmap.Width + j + 1]);
                    var center = Color.FromArgb(bitmap.Bits[(i) * bitmap.Width + j + 1]);
                    sumR = center.R * 8 - top.R - bottom.R - left.R - right.R;
                    sumG = center.G * 8 - top.G - bottom.G - left.G - right.G;
                    sumB = center.B * 8 - top.B - bottom.B - left.B - right.B;

                    bitmap.Bits[i * bitmap.Width + j] = Color.FromArgb(Normalize(sumR), Normalize(sumG), Normalize(sumB)).ToArgb();
                }
            }
        }
        public static void GaussBlurFilter(DirectBitmap bitmap)
        {
            int k = 3;
            for (int i = k / 2; i < bitmap.Height - k / 2; i += 1)
            {
                for (int j = k / 2; j < bitmap.Width - k / 2; j += 1)
                {
                    var topLeft = Color.FromArgb(bitmap.Bits[(i - 1) * bitmap.Width + j - 1]);
                    var top = Color.FromArgb(bitmap.Bits[(i - 1) * bitmap.Width + j]);
                    var topRight = Color.FromArgb(bitmap.Bits[(i - 1) * bitmap.Width + j + 1]);
                    var bottomLeft = Color.FromArgb(bitmap.Bits[(i + 1) * bitmap.Width + j - 1]);
                    var bottom = Color.FromArgb(bitmap.Bits[(i + 1) * bitmap.Width + j]);
                    var bottomRight = Color.FromArgb(bitmap.Bits[(i + 1) * bitmap.Width + j + 1]);
                    var left = Color.FromArgb(bitmap.Bits[(i) * bitmap.Width + j - 1]);
                    var right = Color.FromArgb(bitmap.Bits[(i) * bitmap.Width + j + 1]);
                    var center = Color.FromArgb(bitmap.Bits[(i) * bitmap.Width + j + 1]);
                    var sumR = topLeft.R + 2 * top.R + topRight.R
                        + 2 * left.R + 4 * center.R + 2 * right.R
                        + bottomLeft.R + 2 * bottom.R + bottomRight.R;
                    var sumG = topLeft.G + 2 * top.G + topRight.G
                        + 2 * left.G + 4 * center.G + 2 * right.G
                        + bottomLeft.G + 2 * bottom.G + bottomRight.G;
                    var sumB = topLeft.B + 2 * top.B + topRight.B
                        + 2 * left.B + 4 * center.B + 2 * right.B
                        + bottomLeft.B + 2 * bottom.B + bottomRight.B;
                    var div = 15;
                    bitmap.Bits[i * bitmap.Width + j] = Color.FromArgb(Normalize(sumR / div), Normalize(sumG / div), Normalize(sumB / div)).ToArgb();
                }
            }
        }
        private static int Normalize(int channel)
        {
            if (channel > 255) return 255;
            if (channel < 0) return 0;
            return channel;
        }
        private static int Normalize(int value, int maxValue)
        {
            if (value >= maxValue) return maxValue - 1;
            if (value < 0) return 0;
            return value;
        }
    }
}
