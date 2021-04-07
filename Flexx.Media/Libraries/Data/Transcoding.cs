using Flexx.Core.Data;
using System.IO;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;

namespace Flexx.Media.Libraries.Data
{
    public class Transcoding
    {
        private static readonly Transcoding _singleton;
        public static Transcoding Singleton => _singleton ?? new Transcoding();
        private Transcoding()
        {
            if (Directory.GetFiles(Values.FFMPEGDirectory, "*ffmpeg*", SearchOption.TopDirectoryOnly).Length == 0)
            {
                FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official, Values.FFMPEGDirectory).Wait();
            }

            FFmpeg.SetExecutablesPath(Values.FFMPEGDirectory);
        }
    }
}
