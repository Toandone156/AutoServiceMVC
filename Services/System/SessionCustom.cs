using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.CompilerServices;

namespace AutoServiceMVC.Services.System
{
    public interface ISessionCustom
    {
        T GetSessionValue<T>(HttpContext context, string sessionKey);
        void AddToSession(HttpContext context, string sessionKey, object data);
        void AddToSessionWithTimeout(HttpContext context, string sessionKey, object data, int minute);
        void DeleteSession(HttpContext context, string sessionKey);
    }

    public class SessionCustom : ISessionCustom
    {
        public T GetSessionValue<T>(HttpContext context, string sessionKey)
        {
            string value = context.Session.GetString(sessionKey);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public void AddToSession(HttpContext context, string sessionKey, object data)
        {
            context.Session.SetString(sessionKey, JsonConvert.SerializeObject(data));
        }

        public void AddToSessionWithTimeout(HttpContext context, string sessionKey, object data,int minute)
        {
            var session = context.Session;

            lock(session)
            {
                session.SetString(sessionKey, JsonConvert.SerializeObject(data));
            }

            Task.Delay(TimeSpan.FromMinutes(minute)).ContinueWith(t =>
            {
                lock(session)
                {
                    session.Remove(sessionKey);
                }
            });
        }

        public void DeleteSession(HttpContext context, string sessionKey)
        {
            context.Session.Remove(sessionKey);
        }
    }
}
