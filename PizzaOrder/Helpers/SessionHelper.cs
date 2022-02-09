using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Linq;
using System.Text.Json;
namespace PizzaOrder.Helpers
{
    public static class SessionHelper
    {
        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, System.Text.Json.JsonSerializer.Serialize(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : System.Text.Json.JsonSerializer.Deserialize<T>(value);
        }
    }
}
