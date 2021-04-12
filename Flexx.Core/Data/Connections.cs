using System.Data.SqlClient;

namespace Flexx.Core.Data
{
    internal class Connections
    {
        public static void SaveMovie(ChaseLabs.CLConfiguration.List.ConfigManager smd, string file)
        {
            SaveMovie(smd.GetConfigByKey("Title").Value, (short)smd.GetConfigByKey("Year").ParseInt(), smd.GetConfigByKey("Summery").Value, smd.GetConfigByKey("TMDBID").ParseInt(), smd.GetConfigByKey("Cover").Value, smd.GetConfigByKey("Poster").Value, smd.GetConfigByKey("Language").Value, file, smd.PATH);
        }

        public static void SaveMovie(string title, short year, string summery, int TMDBID, string coverUrl, string posterUrl, string language, string file, string smd)
        {
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False";
            SqlConnection cnn = new SqlConnection(connectionString);
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            string cmdString = $"insert Movies (Title, Year, Summery, TMDBID, SMD, File, CoverURL, PosterURL) values ('{title}', {year}, '{summery}', {TMDBID}, '{smd}', '{file}', '{coverUrl}', '{posterUrl}')";
            cmd.CommandText = cmdString;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.ExecuteNonQuery();
            cnn.Close();
        }
    }
}
