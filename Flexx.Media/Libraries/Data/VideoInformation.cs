using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xabe.FFmpeg;

namespace com.drewchaseproject.net.Flexx.Media.Libraries.Data
{
    public class VideoInformation
    {
        public string Duration => $"{Hours}h {Minutes}m";
        public byte Hours => (byte)info.Duration.Hours;
        public byte Minutes => (byte)info.Duration.Minutes;
        public Resolution Resolution { get; private set; }

        private IMediaInfo info;
        public VideoInformation(string MediaFile)
        {
            info = FFmpeg.GetMediaInfo(MediaFile).Result;
            var stream = info.VideoStreams.First();
            Resolution = new Resolution(stream.Width, stream.Height);
        }
    }
}
