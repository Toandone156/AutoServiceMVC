namespace AutoServiceMVC.Models.System
{
    public class AppSettings
    {
       public int PageSize { get; set; }
       public string Salt { get; set; }
        public string SecretKey { get; set; }
    }
}
