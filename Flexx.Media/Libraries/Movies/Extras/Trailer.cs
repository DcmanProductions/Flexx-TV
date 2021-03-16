using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace com.drewchaseproject.net.Flexx.Media.Libraries.Movies.Extras
{
    public class Trailer
    {
        public string URL { get; private set; }
        public Trailer(MovieModel movie)
        {
            YoutubeClient youtube = new YoutubeClient();
            StreamManifest streamManifest = youtube.Videos.Streams.GetManifestAsync(movie.TrailerVideoID).Result;
            IVideoStreamInfo streamInfo = streamManifest.GetMuxed().WithHighestVideoQuality();
            if (streamInfo != null)
            {
                URL = streamInfo.Url;
            }
        }
    }
}
