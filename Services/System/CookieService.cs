namespace AutoServiceMVC.Services.System
{
    public interface ICookieService
    {
        public void AddCookie(HttpContext context, int storeDays, string key, string value);
        public string GetCookie(HttpContext context, string key);
        public void RemoveCookie(HttpContext context, string key);
    }

    public class CookieService : ICookieService
    {
        public void AddCookie(HttpContext context, int storeDays, string key, string value)
        {
            CookieOptions options = new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(storeDays)
            };
            context.Response.Cookies.Append(key, value, options);
        }

        public string? GetCookie(HttpContext context, string key)
        {
            return context.Request.Cookies.FirstOrDefault(p => p.Key == key).Value;
        }

        public void RemoveCookie(HttpContext context, string key)
        {
            context.Response.Cookies.Delete(key);
        }
    }
}
