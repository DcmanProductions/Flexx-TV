using Flexx.Core.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Flexx.Core.Data.Utilities.MathUtil;

namespace Flexx.Media.Libraries.Movies
{
    /// <summary>
    /// A List of Movies.<br />
    /// <i>Usually from a library, but doesn't have to be.</i>
    /// </summary>
    public class MovieListModel : List<MediaModel>
    {
        public LibraryModel library;
        /// <summary>
        /// Gets the <seealso cref="MovieModel"/> Based on the Movie Name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public MediaModel GetByName(string name)
        {
            MediaModel model = null;
            ForEach(e => { if (e.Title.ToLower().Equals(name.ToLower())) { model = e; } });
            if (model == null)
            {
                throw new NullReferenceException($"No Movie Named {name} exists. Make sure that its been loaded!");
            }

            return model;
        }
        public MediaModel GetByID(int ID)
        {
            MediaModel model = null;
            ForEach(e => { if (e.TMDBID.Equals(ID)) { model = e; } });
            if (model == null)
            {
                throw new NullReferenceException($"No Movie with ID of {ID} exists. Make sure that its been loaded!");
            }

            return model;
        }

        public MovieObjectModel[] GetMovieListAsJsonObject()
        {
            List<MovieObjectModel> models = new();
            foreach (MediaModel movie in this)
            {
                MovieObjectModel model;
                try
                {
                    model = new()
                    {
                        ID = movie.TMDBID,
                        Name = movie.Title,
                        Summery = movie.Summery,
                        Year = movie.Year,
                        CoverURL = movie.CoverURL,
                        PosterURL = movie.PosterURL,
                        MPAA = movie.MPAARating,
                        Duration = movie.MediaDetails.Duration,
                        Language = movie.Language,
                        Resolution = movie.MediaDetails.Resolution.Display,
                        Writers = movie.Writers.ListOfNames(),
                        Genres = movie.Genres.ToArray(),
                        Actors = movie.Actors.ListOfActors(),
                        Watched = movie.Watched,
                        WatchedDuration = movie.WatchedDuration,
                        WatchedPercentage = (int)Math.Floor(movie.WatchPercentage * 100),
                    };
                    models.Add(model);
                }
                catch { continue; }
            }
            return models.ToArray();
        }

        public MovieObjectModel GetMovieAsJsonObject(int id)
        {
            foreach (MovieObjectModel movie in GetMovieListAsJsonObject())
            {
                if (movie.ID == id)
                {
                    return movie;
                }
            }
            return null;
        }

        public void SortByName()
        {
            List<MediaModel> list = this.OrderBy(o =>
            {
                string s = o.Title;
                s = Regex.Replace(s, $@"\ba\b", "", RegexOptions.IgnoreCase);
                s = Regex.Replace(s, $@"\bthe\b", "", RegexOptions.IgnoreCase);
                return s;
            }).ToList();
            Clear();
            AddRange(list);
        }

        public bool Contains(MovieModel movie)
        {
            foreach (MovieModel item in this)
            {
                if (item.Path.Equals(movie.Path))
                {
                    return true;
                }

                if (item.Title.Equals(movie.Title) && item.Year.Equals(movie.Year))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Creates a <seealso cref="MovieListModel"/> based on <seealso cref="LibraryModel"/> object.
        /// </summary>
        /// <param name="_lib"></param>
        /// <returns></returns>
        public static MovieListModel GenerateListFromLibrary(LibraryModel _lib)
        {
            string[] MediaExtensions = new string[] { "mpegg", "mpeg", "mp4", "mkv", "m4a", "m4v", "f4v", "f4a", "m4b", "m4r", "f4b", "mov", "3gp", "3gp2", "3g2", "3gpp", "3gpp2", "ogg", "oga", "ogv", "ogx", "wmv", "wma", "flv", "avi" };
            MovieListModel model = new()
            {
                library = _lib
            };
            Values.Singleton.ScanningMovies = true;
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
                    model.RunChunksAsync(chunks.ToArray()[i]).ContinueWith(t => t.Dispose());
                }


            });
            //search = Task.Run(() =>
            //{
            //    string[] files = Directory.GetFiles(_lib.Path, "*", SearchOption.AllDirectories).Where(item => MediaExtensions.Any(item.ToLower().EndsWith)).ToArray();
            //    int chunkSize = (int)Math.Round((double)files.Length / 100);
            //    bool[] scanners = new bool[chunkSize];
            //    for (int i = 0; i < scanners.Length; i++)
            //    {
            //        scanners[i] = false;
            //    }
            //    IEnumerable<string[]> chunks = GetChunk(files, chunkSize);
            //    for (int i = 0; i < chunks.Count(); i++)
            //    {
            //        model.RunChunk(chunks.ToArray()[i]).ContinueWith(t =>
            //        {
            //            scanners[i] = true;
            //            int count = 0;
            //            for (int i = 0; i < scanners.Length; i++)
            //            {
            //                if (scanners[i] == true) count++;
            //            }
            //            if (count == scanners.Length)
            //            {
            //                Console.WriteLine("Done Scanning Movies");
            //                model.SortByName();
            //                Values.Singleton.ScanningMovies = false;
            //            }
            //            else
            //            {
            //                Values.Singleton.ScanningMovies = true;
            //            }
            //        });
            //    }
            //});
            return model;
        }

        private Task RunChunksAsync(string[] files)
        {
            return Task.Run(() =>
            {
                foreach (string file in files)
                {
                    RunChunk(file);
                }
                SortByName();
            });
        }

        private void RunChunk(string file, int attempt = 0)
        {
            if (new FileInfo(file).Length > 1000)
            {
                try
                {
                    MovieModel value = new() { Path = file, Library = library };
                    string smd = value.GenerateDetails();
                    if (!string.IsNullOrWhiteSpace(value.PosterURL) && !string.IsNullOrWhiteSpace(value.CoverURL))
                    {
                        try
                        {
                            Add(value);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"\n\nERROR MESSAGE: {e.Message}\n{e.StackTrace}\n\n");
                            Console.WriteLine($"Couldn't Load {file}");
                            if (attempt < 5)
                            {
                                Console.WriteLine($"Trying to Load {file} again");
                                System.Threading.Thread.Sleep(1000);
                                RunChunk(file, attempt + 1);
                                //Environment.Exit(0);
                            }
                            else
                            {
                                Console.WriteLine($"{file} will be disgarded.");
                            }
                        }
                    }
                }
                catch
                {
                }
            }
            else if (new FileInfo(file).Name.ToLower().Contains("sample"))
            {
            }
            else if (new FileInfo(file).Length < 1000)
            {
            }
        }

    }
}
