using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Images
{
    public class PPMReader : IDisposable
    {
        private int _r = -1, _g = -1, _b = -1;
        private int _elementCount = 0;
        private StreamReader _sr;
        private PPMImage _image;
        public PPMReader(string fileName)
        {
            _sr = new StreamReader(File.Open(fileName, FileMode.Open), Encoding.GetEncoding("iso-8859-1"));
            _image = new PPMImage();
        }

        public PPMImage ReadFile()
        {
            using (_sr)
            {
                string line = _sr.ReadLine();
                SetType(line);
                ReadParams();
                if (_image.Type == PPMImageType.P6)
                {
                    _image.ImageString = _sr.ReadToEnd();
                }
                else
                {
                    ReadP3();
                }

                return _image;
            }

        }
        private void ReadP3()
        {
            _image.Bits = new int[_image.Columns * _image.Rows];

            int value;
            string line;
            while ((line = _sr.ReadLine()) != null)
            {
                if (int.TryParse(line, out value))
                {
                    SetPixelCanalP3(value);
                    continue;
                }
                line = CleanInput(line).Trim();
                var lineElements = line.Split(' ');
                foreach (var el in lineElements)
                {
                    if (el.StartsWith("#"))
                    {
                        break;
                    }
                    else if (int.TryParse(el, out value))
                    {
                        SetPixelCanalP3(value);
                    }
                }
            }
        }
        private void ReadParams()
        {
            bool allParamsRead = false;
            while (!allParamsRead)
            {
                string line = CleanInput(_sr.ReadLine()).Trim();
                var lineElements = line.Split(' ');
                foreach (var el in lineElements)
                {
                    if (el.StartsWith("#"))
                    {
                        break;
                    }
                    int tmp;
                    if (int.TryParse(el, out tmp))
                    {
                        SetParam(tmp, ref allParamsRead);
                    }
                }
            }
        }
        private void SetType(string type)
        {
            if (type == "P3")
            {
                _image.Type = PPMImageType.P3;
            }
            else if (type == "P6")
            {
                _image.Type = PPMImageType.P6;
            }
            else
            {
                throw new Exception("Unsupported type!");
            }
        }
        private void SetParam(int value, ref bool allParamsRead)
        {
            if (_image.Columns == 0)
            {
                _image.Columns = value;
            }
            else if (_image.Rows == 0)
            {
                _image.Rows = value;
            }
            else if (_image.BytesPerColor == 0)
            {
                SetMaxValueBytes(value, ref allParamsRead);
            }
            else
            {
                SetPixelCanal(value);
            }
        }
        private void SetMaxValueBytes(int value, ref bool allParamsRead)
        {
            if (value > 255 && value < 65536)
            {
                _image.BytesPerColor = 2;
                _image.MaxValue = value;
                allParamsRead = true;
            }
            else if (value > 0 && value < 256)
            {
                _image.BytesPerColor = 1;
                _image.MaxValue = value;
                allParamsRead = true;
            }
            else
            {
                throw new Exception("Bad color range!");
            }
        }
        private void SetPixelCanalP3(int value)
        {
            if (_image.BytesPerColor == 2)
            {
                value = value / 256;
            }
            _image.BitsCount++;
            SetPixelCanal(value);
        }
        private void SetPixelCanal(int value)
        {
            if (_r == -1)
            {
                _r = value;
                if (_r > _image.MaxR)
                {
                    _image.MaxR = _r;
                }
            }
            else if (_g == -1)
            {
                _g = value;
                if (_g > _image.MaxG)
                {
                    _image.MaxG = _g;
                }
            }
            else if (_b == -1)
            {
                _b = value;
                if (_b > _image.MaxB)
                {
                    _image.MaxB = _b;
                }
                _image.Bits[_elementCount++] = Color.FromArgb(_r, _g, _b).ToArgb();
                _r = -1;
                _g = -1;
                _b = -1;
            }
        }
        private string CleanInput(string strIn)
        {
            return Regex.Replace(strIn, @"[^\w\.@# ]", " ", RegexOptions.None);

        }

        public void Dispose()
        {
            _r = -1;
            _g = -1;
            _b = -1;
            _elementCount = 0;
        }
    }
}
