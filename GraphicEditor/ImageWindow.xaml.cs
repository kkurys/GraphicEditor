using Images;
using Microsoft.Win32;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;

namespace GraphicEditor
{
    public enum OpenMode
    {
        PPM = 0,
        IMG = 1
    }
    public partial class ImageWindow : Window
    {
        private Bitmap _bitmap;
        private JPGCompressionWindow _jpgCompressionWindow;
        public ImageWindow()
        {
            InitializeComponent();

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
                _bitmap.Save(fileName, jpgEncoder, myEncoderParameters);
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
            Title = filePath;
            ImageCanvas.Source = BitmapConverter.GetBitmapSource(_bitmap);
            Show();
        }
        public Bitmap LoadPPMImage(string fileName)
        {
            PPMImage image;

            using (var ppmReader = new PPMReader(fileName))
            {
                try
                {
                    image = ppmReader.ReadFile();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return null;
                }
            }
            Bitmap bitmap;
            if (image.Type == PPMImageType.P6)
            {
                bitmap = PPM_P6Parser.Parse(image);
            }
            else
            {
                bitmap = PPM_P3Parser.Parse(image);
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
                if (System.IO.Path.GetExtension(openFileDialog.FileName) == "jpg")
                {
                    _bitmap = LoadImage(openFileDialog.FileName);
                }
                else
                {
                    _bitmap = LoadPPMImage(openFileDialog.FileName);
                }
                Title = openFileDialog.FileName;
                ImageCanvas.Source = BitmapConverter.GetBitmapSource(_bitmap);
            }
        }
    }
}
