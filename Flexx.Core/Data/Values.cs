namespace com.drewchaseproject.net.Flexx.Core.Data
{
    public class Values
    {
        #region Singleton
        private static Values _singleton;
        public static Values Singleton { get { _singleton = _singleton == null ? new Values() : _singleton; return _singleton; } }
        #endregion
        #region Static Values
        public static string ApplicationName => "Flexx";
        public static string CompanyName => "Chase Labs";
        #endregion
    }
}
