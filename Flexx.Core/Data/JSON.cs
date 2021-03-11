using Newtonsoft.Json.Linq;

namespace com.drewchaseproject.net.Flexx.Core.Data
{
    /// <summary>
    /// A Static class for ALL things JSON
    /// </summary>
    public static class JSON
    {
        /// <summary>
        /// Parses JSON String to <seealso cref="JObject"/>
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static JObject ParseJson(string json)
        {
            return JObject.Parse(json);
        }
    }
}
