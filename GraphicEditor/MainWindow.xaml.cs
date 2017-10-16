using BasicFigures;
using GraphicEditor.ViewModels;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private bool _currentlyDrawing = false;
        private Shape _activeElement;
        private ShapeListItem _activeMyShapeElement;
        private double _selectedStrokeThickness = 4;
        private SolidColorBrush _selectedStrokeColor = new SolidColorBrush(Colors.Black);
        private SolidColorBrush _selectedFillColor = new SolidColorBrush(Colors.Black);
        private FigureManager _figureManager;
        private ObservableCollection<ShapeListItem> _shapes;
        private ImageWindow _imageWindow;
        public event PropertyChangedEventHandler PropertyChanged;
        public string StrokeThicknessText
        {
            get
            {
                return _selectedStrokeThickness.ToString();
            }
            set
            {
                double result;
                if (double.TryParse(value, out result))
                {
                    _selectedStrokeThickness = result;
                }
            }
        }
        public bool CurrentlyDrawing
        {
            get
            {
                return _currentlyDrawing;
            }
            set
            {
                _currentlyDrawing = value;
                OnPropertyChanged("SettingsActive");
            }
        }
        public bool SettingsActive
        {
            get
            {
                return !_currentlyDrawing;
            }
        }
        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            _shapes = new ObservableCollection<ShapeListItem>();
            _imageWindow = new ImageWindow();
            LBItems.ItemsSource = _shapes;
            Canvas.MouseLeftButtonDown += StartStopDrawing;
            Canvas.MouseMove += Draw;
            Canvas.MouseRightButtonDown += CancelDrawing;
        }

        private void CancelDrawing(object sender, MouseButtonEventArgs e)
        {
            if (_currentlyDrawing)
            {
                Canvas.Children.Remove(_activeElement);
                CurrentlyDrawing = false;
                _shapes.Remove(_activeMyShapeElement);
            }
        }

        private void Draw(object sender, MouseEventArgs e)
        {
            if (_currentlyDrawing)
            {
                var mousePosition = e.GetPosition(Canvas);

                _figureManager.Transform(_activeElement, mousePosition);

                Canvas.SetTop(_activeElement, _figureManager.OffsetTop);
                Canvas.SetLeft(_activeElement, _figureManager.OffsetLeft);
            }
        }

        private void StartStopDrawing(object sender, MouseButtonEventArgs e)
        {
            if (_currentlyDrawing)
            {
                CurrentlyDrawing = false;
            }
            else
            {
                CurrentlyDrawing = true;

                var mousePosition = e.GetPosition(Canvas);

                _figureManager = GetFigureManager(mousePosition.X, mousePosition.Y);

                _activeElement = GetElement();


                Canvas.Children.Add(_activeElement);
                _activeMyShapeElement = new ShapeListItem(_activeElement);
                _activeMyShapeElement.StartX = mousePosition.X;
                _activeMyShapeElement.StartY = mousePosition.Y;
                _shapes.Add(_activeMyShapeElement);
            }
        }
        private FigureManager GetFigureManager(double x, double y)
        {
            FigureManager figureManager = null;
            if (RBLine.IsChecked.Value)
            {
                figureManager = new LineManager();
            }
            else if (RBCircle.IsChecked.Value)
            {
                figureManager = new EllipseManager();
            }
            else if (RBRectangle.IsChecked.Value)
            {
                figureManager = new RectangleManager();
            }
            figureManager.Set(x, y);
            return figureManager;
        }
        private FigureManager GetFigureManagerForEdit(ShapeListItem shape)
        {
            FigureManager figureManager = null;
            if (shape.Shape is Line)
            {
                figureManager = new LineManager();
            }
            else if (shape.Shape is Ellipse)
            {
                figureManager = new EllipseManager();
            }
            else if (shape.Shape is System.Windows.Shapes.Rectangle)
            {
                figureManager = new RectangleManager();
            }
            figureManager.Set(shape.StartX, shape.StartY);
            return figureManager;
        }
        private Shape GetElement()
        {
            var element = _figureManager.GetElement(CBFill.IsChecked.Value, _selectedFillColor);
            element.Stroke = _selectedStrokeColor;
            element.StrokeThickness = _selectedStrokeThickness;
            element.Visibility = Visibility.Visible;

            return element;
        }
        private void SelectedStrokeColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
        {
            var color = StrokeColorPicker.SelectedColor.Value;
            _selectedStrokeColor = new SolidColorBrush(System.Windows.Media.Color.FromRgb(color.R, color.G, color.B));
        }
        private void SelectedFillColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
        {
            var color = FillColorPicker.SelectedColor.Value;
            _selectedFillColor = new SolidColorBrush(System.Windows.Media.Color.FromRgb(color.R, color.G, color.B));
        }
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private void ClearCanvas(object sender, RoutedEventArgs e)
        {
            Canvas.Children.Clear();
        }

        private void EditItem(object sender, MouseButtonEventArgs e)
        {
            var activeListItem = LBItems.SelectedItem as ShapeListItem;
            _activeElement = activeListItem.Shape;
            _activeMyShapeElement = activeListItem;
            _figureManager = GetFigureManagerForEdit(activeListItem);
            _currentlyDrawing = true;
        }
        private void LoadImage(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JPG images (*.jpg)|*.jpg| PPM Images (*.ppm)|*.ppm";
            if (openFileDialog.ShowDialog().Value)
            {
                var extension = System.IO.Path.GetExtension(openFileDialog.FileName);
                if (extension == ".jpg")
                {
                    _imageWindow.Show(openFileDialog.FileName, OpenMode.IMG);
                }
                else
                {
                    _imageWindow.Show(openFileDialog.FileName, OpenMode.PPM);
                }

            }
        }
    }
}

