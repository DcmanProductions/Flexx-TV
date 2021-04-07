using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flexx.Core.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using TorrentTitleParser;
using static Flexx.Core.Data.Utilities.MathUtil;

namespace Flexx.Media.Libraries.Series
{

    /// <summary>
    /// A List of Movies.<br />
    /// <i>Usually from a library, but doesn't have to be.</i>
    /// </summary>
    public class SeriesListModel : List<SeriesModel>
    {
        public LibraryModel library;
        /// <summary>
        /// Gets the <seealso cref="MovieModel"/> Based on the Movie Name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SeriesModel GetByName(string name)
        {
            SeriesModel model = null;
            ForEach(e => { if (e.Title.ToLower().Equals(name.ToLower())) { model = e; } });
            if (model == null)
            {
                throw new NullReferenceException($"No TV Show Named {name} exists. Make sure that its been loaded!");
            }

            return model;
        }

        public void SortByName()
        {
            try
            {
                bool success = false;
                var list = this.OrderBy(o =>
                {
                    string s = o.Title;
                    s = Regex.Replace(s, $@"\ba\b", "", RegexOptions.IgnoreCase);
                    s = Regex.Replace(s, $@"\bthe\b", "", RegexOptions.IgnoreCase);
                    success = true;
                    return s;
                }).ToList();
                if (success)
                {
                    Clear();
                    AddRange(list);
                }
            }
            catch { }
        }

        public new bool Contains(SeriesModel model)
        {
            foreach (SeriesModel item in this)
            {
                if (item.Path.Equals(model.Path)) return true;
                if (item.Title.Equals(model.Title) && item.Year.Equals(model.Year)) return true;
            }
            return false;
        }

        public SeriesModel GetExisting(SeriesModel model)
        {
            foreach (SeriesModel item in this)
            {
                if (model.Title.ToLower().Equals(item.Title.ToLower())) return item;
            }

            return model;
        }

        /// <summary>
        /// Creates a <seealso cref="MovieListModel"/> based on <seealso cref="LibraryModel"/> object.
        /// </summary>
        /// <param name="_lib"></param>
        /// <returns></returns>
        public static SeriesListModel GenerateListFromLibrary(LibraryModel _lib)
        {
            string[] MediaExtensions = new string[] { "mpegg", "mpeg", "mp4", "mkv", "m4a", "m4v", "f4v", "f4a", "m4b", "m4r", "f4b", "mov", "3gp", "3gp2", "3g2", "3gpp", "3gpp2", "ogg", "oga", "ogv", "ogx", "wmv", "wma", "flv", "avi" };
            SeriesListModel model = new()
            {
                library = _lib
            };
            Values.Singleton.ScanningTV = true;
            Task search;
            search = Task.Run(() =>
            {
                string[] files = Directory.GetFiles(_lib.Path, "*", SearchOption.AllDirectories).Where(item => MediaExtensions.Any(item.ToLower().EndsWith)).ToArray();
                int chunkSize = (int)Math.Round((double)files.Length / 100);
                chunkSize = (chunkSize == 0 || chunkSize == 1) ? 2 : chunkSize;
                bool[] scanners = new bool[chunkSize];
                for (int i = 0; i < scanners.Length; i++)
                {
                    scanners[i] = false;
                }
                IEnumerable<string[]> chunks = GetChunk(files, chunkSize);
                for (int i = 0; i < chunks.Count(); i++)
                {
                    model.RunChunk(chunks.ToArray()[i]).ContinueWith(t => t.Dispose());
                }


            });
            return model;
        }

        private Task RunChunk(string[] files)
        {
            return Task.Run(() =>
            {
                foreach (string file in files)
                {
                    if (new FileInfo(file).Length > 1000)
                    {
                        try
                        {
                            SeriesModel value = new() { Path = file, Library = library };
                            string smd = value.GenerateDetails();
                            value = GetExisting(value);

                            var torrent = new Torrent(new FileInfo(file).Name);
                            Season season = new(value, (short)torrent.Season, Directory.GetParent(file).FullName);
                            bool hasSeason = false;
                            value.Seasons.ForEach(s =>
                            {
                                if (s.SeasonPath.ToLower().Equals(season.SeasonPath.ToLower()))
                                {
                                    hasSeason = true;
                                    return;
                                }
                            });
                            season = value.GetExistingSeason(season);
                            Episode episode = new(file, torrent.Episode, library, value, season);
                            season.Episodes.Add(episode);

                            if (!hasSeason)
                                value.Seasons.Add(season);
                            if (!Contains(value))
                                Add(value);
                            //value.SortSeasonByName();
                            //season.SortEpisodeByName();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Couldn't Load {file}");
                            Console.WriteLine($"ERROR MESSAGE: {e.Message}");
                            Console.WriteLine(e.StackTrace);
                        }
                    }
                    else if (new FileInfo(file).Name.ToLower().Contains("sample"))
                    {
                    }
                    else if (new FileInfo(file).Length < 1000)
                    {
                    }
                }
                Console.WriteLine("Done Processing Chunk");
            });
        }
    }
}

