using System.Windows;

namespace GraphicEditor
{
    /// <summary>
    /// Interaction logic for JPGCompressionWindow.xaml
    /// </summary>
    public partial class JPGCompressionWindow : Window
    {
        private int _compressionRate;
        public string CompressionRate
        {
            get
            {
                return _compressionRate.ToString();
            }
            set
            {
                int tmp;
                if (int.TryParse(value, out tmp))
                {
                    if (tmp >= 0 && tmp <= 100)
                    {
                        _compressionRate = tmp;
                    }
                }
            }
        }
        public JPGCompressionWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
        private void Cancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
