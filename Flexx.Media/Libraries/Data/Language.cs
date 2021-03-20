namespace com.drewchaseproject.net.Flexx.Media.Libraries.Data
{
    public class Language
    {
        public string Abbreviation { get; private set; }
        public string Name { get; private set; }
        public Language(string abbv, string lang)
        {
            Abbreviation = abbv;
            Name = lang;
        }
    }
}
