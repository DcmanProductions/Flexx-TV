using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace com.drewchaseproject.net.Flexx.Media.Libraries.Movies.Extras
{
    public class Trailer
    {
        public string URL { get; private set; }
        public Trailer(MovieModel movie)
        {
            YoutubeClient youtube = new YoutubeClient();
            var streamManifest = youtube.Videos.Streams.GetManifestAsync(movie.TrailerVideoID).Result;
            var streamInfo = streamManifest.GetMuxed().WithHighestVideoQuality();
            if (streamInfo != null)
            {
                URL = streamInfo.Url;
            }
        }
    }
}
