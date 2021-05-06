using System.Collections.Generic;

namespace Flexx.Media.Libraries.Data
{
    /// <summary>
    /// Gets resolution based on width and height
    /// </summary>
    public class Resolution
    {
        public string Display { get; private set; }
        private readonly int Width, Height;
        public Resolution(int _width, int _height)
        {
            Width = _width;
            Height = _height;
            Display = $"{Width}x{Height}";
            if (Width != 0 && Height != 0)
            {
                switch (Width)
                {
                    case > 6500:
                        Display = "8K";
                        break;
                    case > 4500:
                        Display = "5K";
                        break;
                    case > 2500:
                        Display = "4K";
                        break;
                    case >= 1900:
                        Display = "1080p";
                        break;
                    case >= 1200:
                        Display = "720p";
                        break;
                    case >= 400:
                        Display = "480p";
                        break;
                    case >= 200:
                        Display = "240p";
                        break;
                    case < 200:
                        Display = "TINY";
                        break;
                }
            }
        }

    }

}
