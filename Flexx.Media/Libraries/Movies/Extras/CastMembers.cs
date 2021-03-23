﻿using com.drewchaseproject.net.Flexx.Core.Data;
using System.Collections.Generic;
using System.Linq;
using static com.drewchaseproject.net.Flexx.Media.Libraries.Movies.Extras.CastMember;

namespace com.drewchaseproject.net.Flexx.Media.Libraries.Movies.Extras
{
    public class CastMembers : List<CastMember>
    {
        private string JSONResponse => new System.Net.WebClient().DownloadString($"https://api.themoviedb.org/3/movie/{Movie.TMDBID}/credits?api_key={Values.TheMovieDBAPIKey}");
        private MovieModel Movie { get; set; }
        //private MediaModel Series { get; set; }

        public CastMembers(MovieModel _model)
        {
            Movie = _model;
            GenerateCastMembers(JSONResponse);
        }
        public CastMembers(MediaModel _model)
        {
            Movie = (MovieModel) _model;
            GenerateCastMembers(JSONResponse);
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
