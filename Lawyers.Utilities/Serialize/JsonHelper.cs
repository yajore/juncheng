using Newtonsoft.Json;

namespace Lawyers.Utilities
{
    public class JsonHelper
    {
        public static T Build<T>(string source)
        {
            var setting = new JsonSerializerSettings();
            setting.NullValueHandling = NullValueHandling.Ignore;
            setting.MissingMemberHandling = MissingMemberHandling.Ignore;
            T obj = JsonConvert.DeserializeObject<T>(source, setting);
            return obj;
        }

        public static string ReBuilder(object source)
        {
            var setting = new JsonSerializerSettings();
            setting.NullValueHandling = NullValueHandling.Ignore;
            setting.MissingMemberHandling = MissingMemberHandling.Ignore;
            //setting.Converters.Add(new NullIntConverter());
            return JsonConvert.SerializeObject(source, setting); ;
        }
    }

}
