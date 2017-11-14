using Images;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace ImageHistogram
{
    public class Histogram : INotifyPropertyChanged
    {
        private DirectBitmap _directBitmap;
        private int[] _originalBits;

        public event PropertyChangedEventHandler PropertyChanged;

        public int[] RedHistogram { get; set; }
        private System.Windows.Media.PointCollection _redHistogramPoints;
        public System.Windows.Media.PointCollection RedHistogramPoints
        {
            get
            {
                return _redHistogramPoints;
            }
            set
            {
                _redHistogramPoints = value;
                OnPropertyChanged("RedHistogramPoints");
            }
        }
        private System.Windows.Media.PointCollection _greenHistogramPoints;
        public System.Windows.Media.PointCollection GreenHistogramPoints
        {
            get
            {
                return _greenHistogramPoints;
            }
            set
            {
                _greenHistogramPoints = value;
                OnPropertyChanged("GreenHistogramPoints");
            }
        }
        private System.Windows.Media.PointCollection _blueHistogramPoints;
        public System.Windows.Media.PointCollection BlueHistogramPoints
        {
            get
            {
                return _blueHistogramPoints;
            }
            set
            {
                _blueHistogramPoints = value;
                OnPropertyChanged("BlueHistogramPoints");
            }
        }
        public int[] BlueHistogram { get; set; }
        public int[] GreenHistogram { get; set; }
        public int RedMin { get; set; } = 255;
        public int RedMax { get; set; } = 0;
        public int BlueMin { get; set; } = 255;
        public int BlueMax { get; set; } = 0;
        public int GreenMin { get; set; } = 255;
        public int GreenMax { get; set; } = 0;
        public Histogram(DirectBitmap bitmap)
        {
            _directBitmap = bitmap;
            _originalBits = new int[bitmap.Width * bitmap.Height];
            Array.Copy(_directBitmap.Bits, _originalBits, _directBitmap.Bits.Length);

            GenerateHistograms();
        }

        public void ResetToDefault()
        {
            for (int i = 0; i < _directBitmap.Height * _directBitmap.Width; i++)
            {
                _directBitmap.Bits[i] = _originalBits[i];
            }
            GenerateHistograms();
        }
        public void Stretch()
        {
            for (int i = 0; i < _directBitmap.Height; i++)
            {
                for (int j = 0; j < _directBitmap.Width; j++)
                {
                    var color = Color.FromArgb(_directBitmap.Bits[i * _directBitmap.Width + j]);

                    var newR = 255 * (color.R - RedMin) / (RedMax - RedMin);
                    var newG = 255 * (color.G - GreenMin) / (GreenMax - GreenMin);
                    var newB = 255 * (color.B - BlueMin) / (BlueMax - BlueMin);


                    _directBitmap.Bits[i * _directBitmap.Width + j] = Color.FromArgb(Normalize(newR), Normalize(newG), Normalize(newB)).ToArgb();
                }
            }
            GenerateHistograms();
            GenerateHistogramPoints();
        }
        public void Equalize()
        {
            int pixels = _directBitmap.Height * _directBitmap.Width;
            decimal Const = 255 / (decimal)pixels;

            int R, G, B;


            int[] cdfR = new int[256];
            int[] cdfG = new int[256];
            int[] cdfB = new int[256];

            Array.Copy(RedHistogram, cdfR, 255);
            Array.Copy(GreenHistogram, cdfG, 255);
            Array.Copy(BlueHistogram, cdfB, 255);

            for (int r = 1; r <= 255; r++)
            {
                cdfR[r] = cdfR[r] + cdfR[r - 1];
                cdfG[r] = cdfG[r] + cdfG[r - 1];
                cdfB[r] = cdfB[r] + cdfB[r - 1];
            }

            for (int i = 0; i < _directBitmap.Height; i++)
            {
                for (int j = 0; j < _directBitmap.Width; j++)
                {
                    var color = Color.FromArgb(_directBitmap.Bits[i * _directBitmap.Width + j]);

                    R = (int)(cdfR[color.R] * Const);
                    G = (int)(cdfG[color.G] * Const);
                    B = (int)(cdfB[color.B] * Const);

                    _directBitmap.Bits[i * _directBitmap.Width + j] = Color.FromArgb(R, G, B).ToArgb();
                }
            }
            GenerateHistograms();
        }
        public int Normalize(int v)
        {
            if (v > 255) return 255;
            if (v < 0) return 0;
            return v;
        }
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public void GenerateHistograms()
        {
            RedHistogram = new int[256];
            BlueHistogram = new int[256];
            GreenHistogram = new int[256];

            for (int i = 0; i < _directBitmap.Height; i++)
            {
                for (int j = 0; j < _directBitmap.Width; j++)
                {
                    var color = Color.FromArgb(_directBitmap.Bits[i * _directBitmap.Width + j]);

                    if (color.R > RedMax) RedMax = color.R;
                    if (color.R < RedMin) RedMin = color.R;

                    if (color.G > GreenMax) GreenMax = color.G;
                    if (color.G < GreenMin) GreenMin = color.G;

                    if (color.B > BlueMax) BlueMax = color.B;
                    if (color.B < BlueMin) BlueMin = color.B;

                    RedHistogram[color.R]++;
                    BlueHistogram[color.B]++;
                    GreenHistogram[color.G]++;
                }
            }
            GenerateHistogramPoints();
        }
        private void GenerateHistogramPoints()
        {
            RedHistogramPoints = new System.Windows.Media.PointCollection();
            GreenHistogramPoints = new System.Windows.Media.PointCollection();
            BlueHistogramPoints = new System.Windows.Media.PointCollection();

            int rMax = RedHistogram.Max();
            int gMax = GreenHistogram.Max();
            int bMax = BlueHistogram.Max();

            RedHistogramPoints.Add(new System.Windows.Point(0, rMax));
            GreenHistogramPoints.Add(new System.Windows.Point(0, gMax));
            BlueHistogramPoints.Add(new System.Windows.Point(0, bMax));

            for (int i = 0; i < 255; i++)
            {
                RedHistogramPoints.Add(new System.Windows.Point(i, rMax - RedHistogram[i]));
                GreenHistogramPoints.Add(new System.Windows.Point(i, gMax - GreenHistogram[i]));
                BlueHistogramPoints.Add(new System.Windows.Point(i, bMax - BlueHistogram[i]));
            }
            RedHistogramPoints.Add(new System.Windows.Point(255, rMax));
            GreenHistogramPoints.Add(new System.Windows.Point(255, gMax));
            BlueHistogramPoints.Add(new System.Windows.Point(255, bMax));

            OnPropertyChanged("RedHistogramPoints");
            OnPropertyChanged("BlueHistogramPoints");
            OnPropertyChanged("GreenHistogramPoints");
        }

    }
}
