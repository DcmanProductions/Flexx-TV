using System.Linq;
using Xabe.FFmpeg;

namespace Flexx.Media.Libraries.Data
{
    /// <summary>
    /// Gets Important information from the video file
    /// </summary>
    public class VideoInformation
    {
        public string Duration
        {
            get
            {
                string tmp = "";
                if (Hours > 0)
                {
                    tmp = $"{Hours}h {Minutes}m";
                }
                else if (Hours == 0 && Minutes == 0)
                {
                    tmp = $"{Seconds}s";
                }
                else if (Hours == 0 && Minutes > 0)
                {
                    tmp = $"{Minutes} min";
                }

                return tmp;
            }
        }
        public sbyte Hours => (sbyte)info.Duration.Hours;
        public sbyte Minutes => (sbyte)info.Duration.Minutes;
        public ushort Seconds => (ushort)((Minutes * 60) + (Hours * 60 * 60));
        public Resolution Resolution { get; private set; }
        private readonly IMediaInfo info;
        public VideoInformation(string MediaFile)
        {
            info = FFmpeg.GetMediaInfo(MediaFile).Result;
            IVideoStream stream = info.VideoStreams.First();
            Resolution = new Resolution(stream.Width, stream.Height);
        }
    }
}
