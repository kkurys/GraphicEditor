using RGBCmykConverter;
using System.ComponentModel;

namespace GraphicEditor.ViewModels
{
    public class RGBViewModel : INotifyPropertyChanged
    {
        private readonly RGB _rgb = new RGB();
        private CMYKViewModel _cmyk;

        public event PropertyChangedEventHandler PropertyChanged;

        public void BindCMYK(CMYKViewModel cmyk)
        {
            _cmyk = cmyk;
        }
        public int Red
        {
            get
            {
                return _rgb.Red;
            }
            set
            {
                int v = value;
                if (v < 0) v = 0;
                if (v > 255) v = 255;
                _rgb.Red = v;
                OnPropertyChanged("Red");
                if (_cmyk != null)
                {
                    _cmyk.UpdateFromRGB(_rgb);
                }
            }
        }
        public int Green
        {
            get
            {
                return _rgb.Green;
            }
            set
            {
                int v = value;
                if (v < 0) v = 0;
                if (v > 255) v = 255;
                _rgb.Green = v;
                OnPropertyChanged("Green");
                if (_cmyk != null)
                {
                    _cmyk.UpdateFromRGB(_rgb);
                }
            }
        }
        public int Blue
        {
            get
            {
                return _rgb.Blue;
            }
            set
            {
                int v = value;
                if (v < 0) v = 0;
                if (v > 255) v = 255;
                _rgb.Blue = v;
                OnPropertyChanged("Blue");
                if (_cmyk != null)
                {
                    _cmyk.UpdateFromRGB(_rgb);
                }
            }
        }
        public void UpdateFromCMYK(CMYK cmyk)
        {
            _rgb.UpdateFromCMYK(cmyk);
            OnPropertyChanged("Red");
            OnPropertyChanged("Green");
            OnPropertyChanged("Blue");
        }
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
