using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flexx.Media.Libraries.Series
{
    public class SeriesModel : MediaModel
    {
        public List<Season> Seasons { get; private set; }

        public TempEpisode TempEpisodePlacement { get; private set; }

        public SeriesObjectModel objectModel => new()
        {
            ID = TMDBID,
            Name = Title,
            Summery = Summery,
            Year = Year,
            Watched = Watched,
            CoverURL = CoverURL,
            PosterURL = PosterURL,
            Language = Language,
            Genres = Genres.ToArray(),
            Writers = Writers.ListOfNames(),
            Actors = Actors.ListOfActors().ToArray(),
        };

        public bool Watched
        {
            get
            {
                int watchedIndex = 0;
                for (int i = 0; i < Seasons.ToArray().Length; i++)
                {
                    if (Seasons[i].Watched)
                    {
                        watchedIndex++;
                    }
                }
                if (watchedIndex == Seasons.ToArray().Length)
                {
                    return true;
                }

                return false;

            }
        }

        public IEnumerable<string> GetSeasonNames()
        {
            for (int i = 0; i < Seasons.ToArray().Length; i++)
            {
                yield return Seasons[i].Name;
            }
        }

        public IEnumerable<SeasonObjectModel> GetSeasonsObjectModel()
        {
            for (int i = 0; i < Seasons.ToArray().Length; i++)
            {
                yield return Seasons[i].objectModel;
            }
        }

        public SeriesModel()
        {
            Seasons = new();
            TempEpisodePlacement = new();
        }

        public Season GetSeasonByNumber(int number)
        {
            for (int i = 0; i < Seasons.ToArray().Length; i++)
            {
                if (Seasons[i].Number == number)
                {
                    return Seasons[i];
                }
            }
            throw new NullReferenceException($"{Title} doesn't have a season number {number} loaded.  Make sure the season is properly loaded.");
        }

        public Season GetExistingSeason(Season season)
        {
            foreach (Season item in Seasons)
            {
                if (item.Number == season.Number)
                {
                    return item;
                }
            }
            return season;
        }

        public bool SeasonExists(Season season)
        {
            foreach (Season item in Seasons)
            {
                if (item.Number == season.Number)
                {
                    return true;
                }
            }
            return false;
        }

        public void SortSeasonByName()
        {
            bool success = true;
            List<Season> list = Seasons.OrderBy(o =>
            {
                try
                {
                    return o.Number;
                }
                catch { success = false; return 0; }
            }).ToList();
            if (success)
            {
                Seasons.Clear();
                Seasons.AddRange(list);
            }

        }
    }
}
