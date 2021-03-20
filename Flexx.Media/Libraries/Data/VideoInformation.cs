using System.Linq;
using Xabe.FFmpeg;

namespace com.drewchaseproject.net.Flexx.Media.Libraries.Data
{
    public class VideoInformation
    {
        public string Duration => $"{Hours}h {Minutes}m";
        public byte Hours => (byte)info.Duration.Hours;
        public byte Minutes => (byte)info.Duration.Minutes;
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
