using Flexx.Core.Data;
using System.Collections.Generic;
using System.Linq;
using static Flexx.Media.Libraries.Movies.Extras.CastMember;

namespace Flexx.Media.Libraries.Movies.Extras
{
    public class CastMembers : List<CastMember>
    {
        public CastMembers(MediaModel media)
        {
            GenerateCastMembers(new System.Net.WebClient().DownloadString($"https://api.themoviedb.org/3/{(media.Library.Type.Equals(Values.LibraryType.Movies) ? "movie" : "tv")}/{media.TMDBID}/credits?api_key={Values.TheMovieDBAPIKey}"));
        }

        private CastMembers(List<CastMember> members)
        {
            AddRange(members);
        }

        public CastMembers GetCrewByDepartment(string department)
        {
            List<CastMember> temp = new List<CastMember>();
            ForEach(item =>
            {
                if (item.Department.ToLower().Equals(department.ToLower()))
                {
                    temp.Add(item);
                }
            });
            return new CastMembers(temp);
        }

        public string[] ListOfNames()
        {
            string[] value = new string[Count];
            for (int i = 0; i < Count; i++)
            {
                value[i] = this[i].ActorName;
            }
            return value;
        }

        public ActorModel[] ListOfActors()
        {

            ActorModel[] value = new ActorModel[Count];
            for (int i = 0; i < Count; i++)
            {
                value[i] = new ActorModel() { Name = this[i].ActorName, Character = this[i].CharacterName, ProfileURL = this[i].ProfilePictureURL };
            }
            return value;
        }

        private void GenerateCastMembers(string response)
        {
            Newtonsoft.Json.Linq.JToken obj = JSON.ParseJson(response)["cast"];
            for (int i = 0; i < obj.Count(); i++)
            {
                string name = obj[i]["name"].ToString();
                string character = obj[i]["character"].ToString();
                string department = obj[i]["known_for_department"].ToString();
                string profile = obj[i]["profile_path"].ToString();
                GenderType gender = int.Parse(obj[i]["gender"].ToString()) == 1 ? CastMember.GenderType.Female : CastMember.GenderType.Male;
                bool adult = bool.Parse(obj[i]["adult"].ToString());
                Add(new CastMember(name, profile, gender, adult, department, character));
            }
            obj = JSON.ParseJson(response)["crew"];
            for (int i = 0; i < obj.Count(); i++)
            {
                string name = obj[i]["name"].ToString();
                string job = obj[i]["job"].ToString();
                string department = obj[i]["known_for_department"].ToString();
                string profile = obj[i]["profile_path"].ToString();
                GenderType gender = int.Parse(obj[i]["gender"].ToString()) == 1 ? CastMember.GenderType.Female : CastMember.GenderType.Male;
                bool adult = bool.Parse(obj[i]["adult"].ToString());
                Add(new CastMember(name, profile, gender, adult, department, job));
            }
        }

    }
}
