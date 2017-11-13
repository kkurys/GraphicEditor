using System.ComponentModel;
using System.Windows;

namespace GraphicEditor
{
    /// <summary>
    /// Interaction logic for ColorBalanceWindow.xaml
    /// </summary>
    public partial class ColorBalanceWindow : Window, INotifyPropertyChanged
    {

        private int _r, _g, _b;
        private double _rM = 1.0, _gM = 1.0, _bM = 1.0;
        private double _redDivider = 1.0, _blueDivider = 1.0, _greenDivider = 1.0;
        public int Red
        {
            get
            {
                return _r;
            }
            set
            {
                _r = value;
                OnPropertyChanged("Red");
            }
        }
        public double RedMultiplier
        {
            get
            {
                return _rM;
            }
            set
            {
                _rM = value;
                OnPropertyChanged("RedMultiplier");
            }
        }
        public int Green
        {
            get
            {
                return _g;
            }
            set
            {
                _g = value;
                OnPropertyChanged("Green");
            }
        }
        public double GreenMultiplier
        {
            get
            {
                return _gM;
            }
            set
            {
                _gM = value;
                OnPropertyChanged("GreenMultiplier");
            }
        }
        public int Blue
        {
            get
            {
                return _b;
            }
            set
            {
                _b = value;
                OnPropertyChanged("Blue");
            }
        }
        public double BlueMultiplier
        {
            get
            {
                return _bM;
            }
            set
            {
                _bM = value;
                OnPropertyChanged("BlueMultiplier");
            }
        }
        public ColorBalanceWindow()
        {
            InitializeComponent();
            MainGrid.DataContext = this;
        }

        private void UpdateImage(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OnPropertyChanged("Update");
        }
        private void DecreaseRedMultiplier(object sender, RoutedEventArgs e)
        {
            if (RedMultiplier > 1)
            {
                RedMultiplier--;
            }
            else
            {
                _redDivider++;
                RedMultiplier = 1.0 / _redDivider;
            }
        }
        private void IncreaseRedMultiplier(object sender, RoutedEventArgs e)
        {
            if (RedMultiplier < 1)
            {
                _redDivider--;
                RedMultiplier = 1.0 / _redDivider;
            }
            else
            {
                RedMultiplier++;
            }
        }
        private void DecreaseGreenMultiplier(object sender, RoutedEventArgs e)
        {
            if (GreenMultiplier > 1)
            {
                GreenMultiplier--;
            }
            else
            {
                _greenDivider++;
                GreenMultiplier = 1.0 / _greenDivider;
            }
        }
        private void IncreaseGreenMultiplier(object sender, RoutedEventArgs e)
        {
            if (GreenMultiplier < 1)
            {
                _greenDivider--;
                GreenMultiplier = 1.0 / _greenDivider;
            }
            else
            {
                GreenMultiplier++;
            }
        }
        private void DecreaseBlueMultiplier(object sender, RoutedEventArgs e)
        {
            if (BlueMultiplier > 1)
            {
                BlueMultiplier--;
            }
            else
            {
                _blueDivider++;
                BlueMultiplier = 1.0 / _blueDivider;
            }
        }
        private void IncreaseBlueMultiplier(object sender, RoutedEventArgs e)
        {
            if (BlueMultiplier < 1)
            {
                _blueDivider--;
                BlueMultiplier = 1.0 / _blueDivider;
            }
            else
            {
                BlueMultiplier++;
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
