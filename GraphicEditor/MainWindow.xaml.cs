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

                _activeElement = GetElement();

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
        private Shape GetElement()
        {
            var element = _figureManager.GetElement(CBFill.IsChecked.Value, _selectedFillColor);
            element.Stroke = _selectedStrokeColor;
            element.StrokeThickness = _selectedStrokeThickness;
            element.Visibility = Visibility.Visible;
            return element;
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
