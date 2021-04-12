using Flexx.Core.Data;
using System.IO;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;

namespace Flexx.Media.Libraries.Data
{
    public class Transcoding
    {
        private static Transcoding _singleton;
        public static Transcoding Singleton => _singleton ?? new Transcoding();
        private Transcoding()
        {
            _singleton = this;
            if (Directory.GetFiles(Values.FFMPEGDirectory, "*ffmpeg*", SearchOption.TopDirectoryOnly).Length == 0)
            {
                FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official, Values.FFMPEGDirectory).Wait();
            }

            FFmpeg.SetExecutablesPath(Values.FFMPEGDirectory);
        }

        public FileStream GetTranscodedStream(string file)
        {
            //var stream = FFmpeg.GetMediaInfo(file).Result.Streams.ToArray()[0];
            //FFmpeg.GetMediaInfo(file).Result.VideoStreams.ToArray()[0].
            return null;//new FileStream(..SetCodec(VideoCodec.mpeg4).SetBitrate(4000).SetSize(VideoSize.Pal).Split(new System.TimeSpan(0, 32, 55), new System.TimeSpan(0, 45, 55))., FileMode.Open, FileAccess.Read);
        }
    }
}
