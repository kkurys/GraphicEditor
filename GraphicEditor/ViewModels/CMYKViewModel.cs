using RGBCmykConverter;
using System.ComponentModel;

namespace GraphicEditor.ViewModels
{
    public class CMYKViewModel : INotifyPropertyChanged
    {
        private readonly CMYK _cmyk = new CMYK();
        private RGBViewModel _rgb;

        public event PropertyChangedEventHandler PropertyChanged;

        public void BindRGB(RGBViewModel rgb)
        {
            _rgb = rgb;
        }
        public double Cyan
        {
            get
            {
                return _cmyk.Cyan;
            }
            set
            {
                double v = value;
                if (v > 1) v = 1;
                if (v < 0) v = 0;
                _cmyk.Cyan = v;
                _rgb.UpdateFromCMYK(_cmyk);
                OnPropertyChanged("Cyan");
            }
        }
        public double Black
        {
            get
            {
                return _cmyk.Black;
            }
            set
            {
                double v = value;
                if (v > 1) v = 1;
                if (v < 0) v = 0;
                _cmyk.Black = v;
                _rgb.UpdateFromCMYK(_cmyk);
                OnPropertyChanged("Black");
            }
        }
        public double Yellow
        {
            get
            {
                return _cmyk.Yellow;
            }
            set
            {
                double v = value;
                if (v > 1) v = 1;
                if (v < 0) v = 0;
                _cmyk.Yellow = v;
                _rgb.UpdateFromCMYK(_cmyk);
                OnPropertyChanged("Yellow");
            }
        }
        public double Magenta
        {
            get
            {
                return _cmyk.Magenta;
            }
            set
            {
                double v = value;
                if (v > 1) v = 1;
                if (v < 0) v = 0;
                _cmyk.Magenta = v;
                _rgb.UpdateFromCMYK(_cmyk);
                OnPropertyChanged("Magenta");
            }
        }
        public void UpdateFromRGB(RGB rgb)
        {
            _cmyk.UpdateFromRGB(rgb);
            OnPropertyChanged("Cyan");
            OnPropertyChanged("Magenta");
            OnPropertyChanged("Black");
            OnPropertyChanged("Yellow");
        }
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
