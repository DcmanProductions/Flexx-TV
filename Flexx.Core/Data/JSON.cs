using Newtonsoft.Json.Linq;

namespace com.drewchaseproject.net.Flexx.Core.Data
{
    public class JSON
    {
        public static JObject ParseJson(string json)
        {
            return JObject.Parse(json);
        }
    }
}
