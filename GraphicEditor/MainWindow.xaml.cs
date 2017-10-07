using BasicFigures;
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

        private double _selectedStrokeThickness = 4;
        private SolidColorBrush _selectedStrokeColor = new SolidColorBrush(Colors.Black);
        private SolidColorBrush _selectedFillColor = new SolidColorBrush(Colors.Black);

        private FigureManager _figureManager;

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

                _activeElement = GetElement(mousePosition.X, mousePosition.Y);

                Canvas.Children.Add(_activeElement);
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
        private Shape GetElement(double x, double y)
        {
            Shape element = null;
            if (RBLine.IsChecked.Value)
            {
                element = CreateLine(x, y);
            }
            else if (RBCircle.IsChecked.Value)
            {
                element = CreateEllipse(x, y);
            }
            else if (RBRectangle.IsChecked.Value)
            {
                element = CreateRectangle(x, y);
            }
            element.Stroke = _selectedStrokeColor;
            element.StrokeThickness = _selectedStrokeThickness;
            element.Visibility = Visibility.Visible;
            return element;
        }
        private Line CreateLine(double x, double y)
        {
            var line = new Line()
            {
                X1 = x,
                X2 = x,
                Y1 = y,
                Y2 = y
            };
            return line;
        }
        private Ellipse CreateEllipse(double desiredCenterX, double desiredCenterY)
        {
            Ellipse ellipse = new Ellipse { Width = 1, Height = 1 };

            ellipse.Stretch = Stretch.Uniform;
            if (CBFill.IsChecked.Value)
            {
                ellipse.Fill = _selectedFillColor;
            }

            return ellipse;
        }
        private Rectangle CreateRectangle(double leftX, double topY)
        {
            Rectangle rect = new Rectangle();
            if (CBFill.IsChecked.Value)
            {
                rect.Fill = _selectedFillColor;
            }
            return rect;
        }
        private void SelectedStrokeColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            var color = StrokeColorPicker.SelectedColor.Value;
            _selectedStrokeColor = new SolidColorBrush(Color.FromRgb(color.R, color.G, color.B));
        }
        private void SelectedFillColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            var color = FillColorPicker.SelectedColor.Value;
            _selectedFillColor = new SolidColorBrush(Color.FromRgb(color.R, color.G, color.B));
        }
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
