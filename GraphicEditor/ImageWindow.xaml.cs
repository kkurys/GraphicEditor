using Images;
using ImageTools;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Media;

namespace GraphicEditor
{
    public enum OpenMode
    {
        PPM = 0,
        IMG = 1
    }
    public partial class ImageWindow : Window, INotifyPropertyChanged
    {
        private Bitmap _bitmap;
        private DirectBitmap _directBitmap;
        private JPGCompressionWindow _jpgCompressionWindow;
        private PPMImage _image;
        private double _zoomValue = 1.0;
        private int _maskSize = 3;
        private int _brightness;
        private ColorBalanceWindow _colorBalanceWindow;
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public int MaskSize
        {
            get
            {
                return _maskSize;
            }
            set
            {
                if (value % 2 == 1 && value >= 3)
                {
                    _maskSize = value;
                    OnPropertyChanged("MaskSize");
                }
            }
        }
        public int Brightness
        {
            get
            {
                return _brightness;
            }
            set
            {
                _brightness = value;
                OnPropertyChanged("Brightness");
            }
        }
        public ImageWindow()
        {
            InitializeComponent();
            MainGrid.DataContext = this;

        }
        private void SaveAsJpg(object sender, RoutedEventArgs e)
        {

            var saveDialog = new SaveFileDialog();
            saveDialog.DefaultExt = "jpg";
            saveDialog.Filter = "JPG images (*.jpg)|*.jpg";

            if (saveDialog.ShowDialog().Value)
            {
                _jpgCompressionWindow = new JPGCompressionWindow();
                if (!_jpgCompressionWindow.ShowDialog().Value) return;
                Encoder myEncoder = Encoder.Quality;
                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, int.Parse(_jpgCompressionWindow.CompressionRate));
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                var fileName = saveDialog.FileName;

                myEncoderParameters.Param[0] = myEncoderParameter;
                _directBitmap.Bitmap.Save(fileName, jpgEncoder, myEncoderParameters);
            }
        }
        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        public void Show(string filePath, OpenMode openMode)
        {
            if (openMode == OpenMode.PPM)
            {
                _bitmap = LoadPPMImage(filePath);
            }
            else
            {
                _bitmap = LoadImage(filePath);
            }
            _directBitmap = new DirectBitmap(_bitmap);
            PointTransformation.InitTransform(_directBitmap);
            Title = filePath;
            ImageCanvas.Source = BitmapConverter.GetBitmapSource(_bitmap);
            Show();
        }
        public Bitmap LoadPPMImage(string fileName)
        {
            using (var ppmReader = new PPMReader(fileName))
            {
                try
                {
                    _image = ppmReader.ReadFile();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return null;
                }
            }
            Bitmap bitmap;
            if (_image.Type == PPMImageType.P6)
            {
                int maxR = 0, maxG = 0, maxB = 0;
                bitmap = PPM_P6Parser.Parse(_image, ref maxR, ref maxG, ref maxB);
                _image.MaxR = maxR;
                _image.MaxG = maxG;
                _image.MaxB = maxB;
            }
            else
            {
                bitmap = PPM_P3Parser.Parse(_image);
            }
            return bitmap;
        }
        public Bitmap LoadImage(string fileName)
        {
            return new Bitmap(Image.FromFile(fileName));
        }
        public void OpenImage(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JPG images (*.jpg)|*.jpg| PPM Images (*.ppm)|*.ppm";
            if (openFileDialog.ShowDialog().Value)
            {
                if (System.IO.Path.GetExtension(openFileDialog.FileName) == ".jpg")
                {
                    _bitmap = LoadImage(openFileDialog.FileName);
                }
                else
                {
                    _bitmap = LoadPPMImage(openFileDialog.FileName);
                }
                _directBitmap = new DirectBitmap(_bitmap);
                PointTransformation.InitTransform(_directBitmap);
                Title = openFileDialog.FileName;
                _zoomValue = 1.0;
                ImageCanvas.Source = BitmapConverter.GetBitmapSource(_bitmap);
                SetZoom();
            }
        }

        private void Scale(object sender, RoutedEventArgs e)
        {
            if (_image != null)
            {
                Scale(_image);
            }

        }
        public void Scale(PPMImage image)
        {
            Bitmap bitmap;
            if (image.Type == PPMImageType.P3)
            {
                bitmap = PPM_P3Parser.Scale(image);
            }
            else
            {
                bitmap = PPM_P6Parser.Scale(image);
            }
            ImageCanvas.Source = BitmapConverter.GetBitmapSource(bitmap);
        }

        private void ZoomIn(object sender, RoutedEventArgs e)
        {
            _zoomValue += _zoomValue / 10;
            SetZoom();
        }
        private void ZoomOut(object sender, RoutedEventArgs e)
        {
            _zoomValue -= _zoomValue / 10;
            SetZoom();
        }
        private void SetZoom()
        {
            ScaleTransform scale = new ScaleTransform(_zoomValue, _zoomValue);
            ImageCanvas.LayoutTransform = scale;
        }

        private void ConvertToGrayAverage(object sender, RoutedEventArgs e)
        {
            PointTransformation.ConvertToGray(_directBitmap, GrayConversionMode.Average);
            ImageCanvas.Source = BitmapConverter.GetBitmapSource(_directBitmap.Bitmap);
        }
        private void ConvertToGrayColorimetric(object sender, RoutedEventArgs e)
        {
            PointTransformation.ConvertToGray(_directBitmap, GrayConversionMode.Colorimetric);
            ImageCanvas.Source = BitmapConverter.GetBitmapSource(_directBitmap.Bitmap);
        }
        private void RevertChanges(object sender, RoutedEventArgs e)
        {
            ImageCanvas.Source = BitmapConverter.GetBitmapSource(_bitmap);
        }

        private void ChangeBrightness(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            PointTransformation.ModifyBrightness(_directBitmap, _brightness);
            ImageCanvas.Source = BitmapConverter.GetBitmapSource(_directBitmap.Bitmap);
        }
        private void OpenColorBalanceWindow(object sender, RoutedEventArgs e)
        {
            _colorBalanceWindow = new ColorBalanceWindow();
            _colorBalanceWindow.Show();
            _colorBalanceWindow.PropertyChanged += UpdateColor;
        }

        private void UpdateColor(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Update")
            {
                PointTransformation.AddValue(_directBitmap, Channel.Red, _colorBalanceWindow.Red);
                PointTransformation.AddValue(_directBitmap, Channel.Green, _colorBalanceWindow.Green);
                PointTransformation.AddValue(_directBitmap, Channel.Blue, _colorBalanceWindow.Blue);
                ImageCanvas.Source = BitmapConverter.GetBitmapSource(_directBitmap.Bitmap);
            }
            else if (e.PropertyName == "RedMultiplier" || e.PropertyName == "GreenMultiplier" || e.PropertyName == "BlueMultiplier")
            {
                PointTransformation.MultiplyByValue(_directBitmap, Channel.Red, _colorBalanceWindow.RedMultiplier);
                PointTransformation.MultiplyByValue(_directBitmap, Channel.Green, _colorBalanceWindow.GreenMultiplier);
                PointTransformation.MultiplyByValue(_directBitmap, Channel.Blue, _colorBalanceWindow.BlueMultiplier);
                ImageCanvas.Source = BitmapConverter.GetBitmapSource(_directBitmap.Bitmap);
            }
        }
        private void AverageFilter(object sender, RoutedEventArgs e)
        {
            Filters.AveragingFilter(_directBitmap, MaskSize);
            ImageCanvas.Source = BitmapConverter.GetBitmapSource(_directBitmap.Bitmap);
        }
        private void MeanFilter(object sender, RoutedEventArgs e)
        {
            Filters.MeanFilter(_directBitmap, MaskSize);
            ImageCanvas.Source = BitmapConverter.GetBitmapSource(_directBitmap.Bitmap);
        }
        private void SobelFilter(object sender, RoutedEventArgs e)
        {
            PointTransformation.ConvertToGray(_directBitmap, GrayConversionMode.Colorimetric);
            Filters.SobelFilter(_directBitmap);
            ImageCanvas.Source = BitmapConverter.GetBitmapSource(_directBitmap.Bitmap);
        }
        private void HighPassFilter(object sender, RoutedEventArgs e)
        {
            Filters.HighPassFilter(_directBitmap, MaskSize);
            ImageCanvas.Source = BitmapConverter.GetBitmapSource(_directBitmap.Bitmap);
        }
        private void GaussianBlurFilter(object sender, RoutedEventArgs e)
        {
            Filters.GaussBlurFilter(_directBitmap);
            ImageCanvas.Source = BitmapConverter.GetBitmapSource(_directBitmap.Bitmap);
        }
    }
}
