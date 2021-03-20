using com.drewchaseproject.net.Flexx.Core.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;

namespace com.drewchaseproject.net.Flexx.Media.Libraries.Data
{
    public class Transcoding
    {
        private static Transcoding _singleton;
        public static Transcoding Singleton => _singleton ?? new Transcoding();
        private Transcoding()
        {
            if (Directory.GetFiles(Values.FFMPEGDirectory, "ffmpeg", SearchOption.TopDirectoryOnly).Length == 0)
                FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official, Values.FFMPEGDirectory).Wait();
            FFmpeg.SetExecutablesPath(Values.FFMPEGDirectory);
        }
    }
}
