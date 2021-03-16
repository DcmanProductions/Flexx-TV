namespace com.drewchaseproject.net.Flexx.Media.Libraries.Movies.Extras
{
    public class CastMember
    {
        public enum GenderType
        {
            Male,
            Female
        }
        public bool IsAdult { get; private set; }
        public GenderType Gender { get; private set; }
        public string ActorName { get; private set; }
        public string CharacterName { get; private set; }
        public string Department { get; private set; }
        public string ProfilePictureURL => $"http://image.tmdb.org/t/p/original{ProfilePicturePath}";

        private string ProfilePicturePath { get; set; }

        public CastMember(string _actorName, string _profilePath, GenderType _gender, bool _isAdult = true, string _department = "Acting", string _characterName = "N/A")
        {
            IsAdult = _isAdult;
            Gender = _gender;
            ActorName = _actorName;
            CharacterName = _characterName;
            Department = _department;
            ProfilePicturePath = _profilePath;
        }


    }
}
