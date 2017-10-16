using Images;
using System.Drawing;
using System.Windows;

namespace GraphicEditor
{
    /// <summary>
    /// Interaction logic for ImageWindow.xaml
    /// </summary>
    public partial class ImageWindow : Window
    {
        public ImageWindow(Bitmap bitmap)
        {
            InitializeComponent();
            ImageCanvas.Source = BitmapConverter.GetBitmapSource(bitmap);
        }
    }
}
