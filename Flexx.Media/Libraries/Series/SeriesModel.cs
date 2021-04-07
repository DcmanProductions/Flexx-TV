using Flexx.Core.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TorrentTitleParser;

namespace Flexx.Media.Libraries.Series
{
    public class SeriesModel : MediaModel
    {
        public List<Season> Seasons { get; private set; }
        public SeriesModel()
        {
            Seasons = new List<Season>();
        }

        public Season GetExistingSeason(Season season)
        {
            foreach (Season item in Seasons)
            {
                if (item.Number == season.Number) return item;
            }
            return season;
        }

        public void SortSeasonByName()
        {
            bool success = false;
            var list = Seasons.OrderBy(o =>
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
