using System.Collections.Generic;

namespace Flexx.Media.Libraries.Data
{
    /// <summary>
    /// Gets resolution based on width and height
    /// </summary>
    public class Resolution
    {
        public string Display { get; private set; }
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
                //for (int i = 0; i < Names.Count(); i++)
                //{
                //    if (Width == Widths.ElementAt(i) && Height == Heights.ElementAt(i))
                //    {
                //        Display = Names.ElementAt(i);
                //    }
                //}
            }
        }

        private readonly int Width, Height;
        private IEnumerable<int> Widths => new List<int>
            {
                10240,
                8192,
                7680,
                7680,
                6400,
                6400,
                5120,
                5120,
                5120,
                5120,
                4096,
                4096,
                3840,
                3840,
                3840,
                3440,
                3200,
                3200,
                3200,
                3000,
                2960,
                2880,
                2800,
                2560,
                2560,
                2560,
                2560,
                2048,
                2048,
                2048,
                1920,
                1920,
                1920,
                1920,
                1680,
                1600,
                1600,
                1440,
                1440,
                1400,
                1280,
                1280,
                1280,
                1280,
                1280,
                1024,
                1024,
                960,
                960,
                854,
                848,
                800,
                800,
                768,
                640,
                640,
                640,
                640,
                480,
                480,
                432,
                400,
                400,
                320,
                320,
                240,
                160,
                1920
            };
        private IEnumerable<int> Heights => new List<int>
            {
                4320,
                4320,
                4320,
                4800,
                4096,
                4800,
                2880,
                2160,
                4096,
                3200,
                2160,
                3072,
                2160,
                1600,
                2400,
                1440,
                1800,
                2048,
                2400,
                2000,
                1440,
                900,
                2100,
                1440,
                1080,
                2048,
                1600,
                1080,
                1152,
                1536,
                1080,
                1280,
                1400,
                1200,
                1050,
                900,
                1200,
                900,
                900,
                1050,
                720,
                960,
                768,
                1024,
                800,
                600,
                768,
                540,
                640,
                480,
                480,
                600,
                480,
                480,
                240,
                480,
                350,
                200,
                234,
                320,
                240,
                300,
                240,
                240,
                200,
                160,
                120,
                800
            };
        private IEnumerable<string> Names =>
             new List<string>
            {
                "Ultra-Wide 10K (UW10K)",
                "DCI 8K (8K Full Format)",
                "8K Ultra HD 2 (8K UHD-2)",
                "Wide HUXGA (WHUXGA)",
                "Wide HSXGA (WHSXGA)",
                "Hex UXGA (HUXGA)",
                "5K",
                "Ultra-Wide 5K (UW5K)",
                "Hex SXGA (HSXGA)",
                "Wide HXGA (WHXGA)",
                "DCI 4K (DCI 4K)",
                "Hex XGA (HXGA)",
                "4K Ultra HD 1 (4K UHD-1)",
                "Ultra-Wide 4K (UW4K)",
                "Wide QUXGA (WQUXGA)",
                "Ultra-Wide QHD",
                "Wide QXGA+ (WQXGA+)",
                "Wide QSXGA (WQSXGA)",
                "Quad UXGA (QUXGA)",
                "3K",
                "Infinity Display",
                "CWSXGA",
                "Quad SXGA+ (QSXGA+)",
                "Quad HD (QHD)",
                "UltraWide FHD (UW-FHD)",
                "Quad SXGA (QSXGA)",
                "Wide QXGA (WQXGA)",
                "DCI 2K (DCI 2K)",
                "QWXGA",
                "Quad XGA (QXGA)",
                "Full HD (FHD)",
                "Full HD Plus (FHD+)",
                "Tesselar XGA (TXGA)",
                "Wide UXGA (WUXGA)",
                "Wide SXGA+ (WSXGA+)",
                "HD+",
                "Ultra XGA (UXGA)",
                "Wide XGA+ (WXGA+)",
                "Wide SXGA (WSXGA)",
                "Super XGA Plus (SXGA+)",
                "Wide XGA (WXGA-H)",
                "Super XGA \"Minus\" (SXGA−)",
                "Wide XGA (WXGA)",
                "Super XGA (SXGA)",
                "Wide XGA (WXGA)",
                "Wide SVGA (WSVGA)",
                "Extended Graphics Array (XGA)",
                "Quarter FHD (qHD)",
                "Double VGA (DVGA)",
                "FWVGA",
                "Wide PAL (W-PAL)",
                "Super VGA (SVGA)",
                "Wide VGA (WGA)",
                "Wide VGA (WVGA)",
                "HVGA",
                "Video Graphics Array (VGA)",
                "Enhanced Graphics Adapter (EGA)",
                "Color Graphics Adapter (CGA)",
                "WQVGA*",
                "HVGA",
                "Wide QVGA (WQVGA)",
                "Quarter SVGA (qSVGA)",
                "Wide QVGA (WQVGA)",
                "Quarter VGA (QVGA)",
                "Color Graphics Adapter (CGA)",
                "Half QVGA (HQVGA)",
                "Quarter Quarter VGA (QQVGA)",
                "1080p Widescreen"
            };
        private IEnumerable<string> AspectRatios => new List<string>
            {
                "21∶9",
                "1.90∶1",
                "16∶9",
                "8∶5",
                "25∶16",
                "4∶3",
                "16∶9",
                "21∶9",
                "5∶4",
                "8∶5",
                "1.90∶1",
                "4∶3",
                "16∶9",
                "2.35∶1",
                "8∶5",
                "43:18:00",
                "16∶9",
                "25∶16",
                "4∶3",
                "3∶2",
                "2.056",
                "16:05",
                "4∶3",
                "16∶9",
                "21∶9",
                "5∶4",
                "8∶5",
                "1.90∶1",
                "16∶9",
                "4∶3",
                "16∶9",
                "3∶2",
                "7∶5",
                "8∶5",
                "8∶5",
                "16∶9",
                "4∶3",
                "8∶5",
                "8∶5",
                "4∶3",
                "16∶9",
                "4∶3",
                "5∶3",
                "5∶4",
                "8∶5",
                "16∶9",
                "4∶3",
                "16∶9",
                "3∶2",
                "16∶9",
                "16∶9",
                "4∶3",
                "5∶3",
                "8∶5",
                "8:03",
                "4∶3",
                "4∶3",
                "4∶3",
                "16∶9",
                "3∶2",
                "9∶5",
                "4∶3",
                "5∶3",
                "4∶3",
                "4∶3",
                "3∶2",
                "4∶3",
                "12:5"
            };
    }

}
