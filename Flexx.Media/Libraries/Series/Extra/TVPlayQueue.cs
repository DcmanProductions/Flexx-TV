using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flexx.Media.Libraries.Series.Extra
{
    public class TVPlayQueue : List<Episode>
    {
        public static TVPlayQueue GenerateFromEpisode(Episode startPoint)
        {
            TVPlayQueue queue = new();
            startPoint.Series.Seasons.ForEach(item => queue.AddRange(item.Episodes));
            return queue;
        }

        public Episode GetNext(Episode current)
        {
            try
            {
                return this.ElementAt(IndexOf(current) + 1);
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }
        public Episode GetPrevious(Episode current)
        {
            try
            {
                return this.ElementAt(IndexOf(current) - 1);
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }

    }
}
