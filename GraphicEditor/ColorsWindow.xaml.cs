using GraphicEditor.ViewModels;
using Images;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace GraphicEditor
{
    /// <summary>
    /// Interaction logic for ColorsWindow.xaml
    /// </summary>
    public partial class ColorsWindow : Window
    {
        protected RGBViewModel _rgb = new RGBViewModel();
        protected CMYKViewModel _cmyk = new CMYKViewModel();

        RotateTransform3D xTransform, yTransform, zTransform, ctxTransform, ctyTransform, ctzTransform;
        Transform3DGroup transformGroup;
        System.Windows.Point mousePosition;
        bool rotating = false;
        public ColorsWindow()
        {
            InitializeComponent();
            _rgb.BindCMYK(_cmyk);
            _rgb.PropertyChanged += UpdateColor;
            _cmyk.BindRGB(_rgb);
            _cmyk.PropertyChanged += UpdateColor;
            GBRGB.DataContext = _rgb;
            GBCMYK.DataContext = _cmyk;
            var center = new Point3D(0.5, 0.5, 0.5);
            zTransform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), 0.2), center);
            yTransform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), 0.2), center);
            xTransform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), 0.2), center);
            ctzTransform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), -0.2), center);
            ctyTransform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), -0.2), center);
            ctxTransform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), -0.2), center);
            transformGroup = new Transform3DGroup();
            Cube.Transform = transformGroup;

            SetFloor();
            SetFrontWall();
            SetLeftWall();
            SetCeil();
            SetBackWall();
            SetRightWall();
        }

        private void StartRotation(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            mousePosition = e.GetPosition(Border);
            rotating = true;
        }
        private void StopRotation(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            rotating = false;
        }
        private void Rotate(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (rotating)
            {
                var newPos = e.GetPosition(Border);

                var xDiff = mousePosition.X - newPos.X;
                var yDiff = mousePosition.Y - newPos.Y;
                if (xDiff < 0)
                {
                    for (int i = 0; i < -1 * xDiff / 15; i++)
                    {
                        RotateLeft();
                    }
                }
                else
                {
                    for (int i = 0; i < xDiff / 15; i++)
                    {
                        RotateRight();
                    }
                }
                if (yDiff < 0)
                {
                    for (int i = 0; i < -1 * yDiff / 15; i++)
                    {
                        RotateDown();
                    }
                }
                else
                {
                    for (int i = 0; i < yDiff / 15; i++)
                    {
                        RotateUp();
                    }
                }
            }
        }


        private void UpdateColor(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ColorPreview.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb((byte)_rgb.Red, (byte)_rgb.Green, (byte)_rgb.Blue));
        }
        private void RotateDown()
        {
            if (!transformGroup.Children.Remove(ctxTransform))
            {
                transformGroup.Children.Add(xTransform);
            }
        }
        private void RotateUp()
        {
            if (!transformGroup.Children.Remove(xTransform))
            {
                transformGroup.Children.Add(ctxTransform);
            }
        }
        private void RotateLeft()
        {
            if (!transformGroup.Children.Remove(ctyTransform))
            {
                transformGroup.Children.Add(yTransform);
            }
        }

        private void RotateRight()
        {
            if (!transformGroup.Children.Remove(yTransform))
            {
                transformGroup.Children.Add(ctyTransform);
            }
        }
        private void SetFloor()
        {
            DirectBitmap bitmap = new DirectBitmap(256, 256);
            for (int i = 0; i <= 255; i++)
            {
                for (int j = 0; j <= 255; j++)
                {
                    bitmap.Bits[i * 256 + j] = System.Drawing.Color.FromArgb((byte)i, (byte)j, 0).ToArgb();
                }
            }
            ImageBrush brush = new ImageBrush(BitmapConverter.GetBitmapSource(bitmap.Bitmap));
            brush.Opacity = 1;
            CubeFloor.Material = new DiffuseMaterial(brush);
            CubeFloor.BackMaterial = new DiffuseMaterial(brush);
        }
        private void SetTop()
        {
            DirectBitmap bitmap = new DirectBitmap(256, 256);
            for (int i = 0; i <= 255; i++)
            {
                for (int j = 0; j <= 255; j++)
                {
                    bitmap.Bits[i * 256 + j] = System.Drawing.Color.FromArgb((byte)i, (byte)j, 0).ToArgb();
                }
            }
            ImageBrush brush = new ImageBrush(BitmapConverter.GetBitmapSource(bitmap.Bitmap));
            brush.Opacity = 1;
            CubeFloor.Material = new DiffuseMaterial(brush);
            CubeFloor.BackMaterial = new DiffuseMaterial(brush);
        }
        private void SetLeftWall()
        {
            DirectBitmap bitmap = new DirectBitmap(256, 256);
            for (int i = 0; i <= 255; i++)
            {
                for (int j = 0; j <= 255; j++)
                {
                    bitmap.Bits[i * 256 + j] = System.Drawing.Color.FromArgb((byte)j, 0, (byte)i).ToArgb();
                }
            }
            ImageBrush brush = new ImageBrush(BitmapConverter.GetBitmapSource(bitmap.Bitmap));
            brush.Opacity = 1;
            CubeLeftWall.Material = new DiffuseMaterial(brush);
            CubeLeftWall.BackMaterial = new DiffuseMaterial(brush);
        }
        private void SetFrontWall()
        {
            DirectBitmap bitmap = new DirectBitmap(256, 256);
            for (int i = 0; i <= 255; i++)
            {
                for (int j = 0; j <= 255; j++)
                {
                    bitmap.Bits[i * 256 + j] = System.Drawing.Color.FromArgb(0, (byte)j, (byte)i).ToArgb();
                }
            }
            ImageBrush brush = new ImageBrush(BitmapConverter.GetBitmapSource(bitmap.Bitmap));
            brush.Opacity = 1;
            CubeFrontWall.Material = new DiffuseMaterial(brush);
            CubeFrontWall.BackMaterial = new DiffuseMaterial(brush);
        }
        private void SetRightWall()
        {
            DirectBitmap bitmap = new DirectBitmap(256, 256);
            for (int i = 0; i <= 255; i++)
            {
                for (int j = 0; j <= 255; j++)
                {
                    bitmap.Bits[i * 256 + j] = System.Drawing.Color.FromArgb((byte)j, 255, (byte)i).ToArgb();
                }
            }
            ImageBrush brush = new ImageBrush(BitmapConverter.GetBitmapSource(bitmap.Bitmap));
            brush.Opacity = 1;
            CubeRightWall.Material = new DiffuseMaterial(brush);
            CubeRightWall.BackMaterial = new DiffuseMaterial(brush);
        }
        private void SetBackWall()
        {
            DirectBitmap bitmap = new DirectBitmap(256, 256);
            for (int i = 0; i <= 255; i++)
            {
                for (int j = 0; j <= 255; j++)
                {
                    bitmap.Bits[i * 256 + j] = System.Drawing.Color.FromArgb(255, (byte)j, (byte)i).ToArgb();
                }
            }
            ImageBrush brush = new ImageBrush(BitmapConverter.GetBitmapSource(bitmap.Bitmap));
            brush.Opacity = 1;
            CubeBackWall.Material = new DiffuseMaterial(brush);
            CubeBackWall.BackMaterial = new DiffuseMaterial(brush);
        }
        private void SetCeil()
        {
            DirectBitmap bitmap = new DirectBitmap(256, 256);
            for (int i = 0; i <= 255; i++)
            {
                for (int j = 0; j <= 255; j++)
                {
                    bitmap.Bits[i * 256 + j] = System.Drawing.Color.FromArgb((byte)i, (byte)j, 255).ToArgb();
                }
            }
            ImageBrush brush = new ImageBrush(BitmapConverter.GetBitmapSource(bitmap.Bitmap));
            brush.Opacity = 1;
            CubeCeil.Material = new DiffuseMaterial(brush);
            CubeCeil.BackMaterial = new DiffuseMaterial(brush);
        }

    }
}
