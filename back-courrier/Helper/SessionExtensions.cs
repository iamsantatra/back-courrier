using Newtonsoft.Json;

namespace back_courrier.Helper
{
    public static class SessionExtensions 
    {
        // this class store and retrieve objects in Session state in ASP.NET   
        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
