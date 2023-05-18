using AutoServiceBE.Models;

namespace AutoServiceMVC.Models
{
    public class Login
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class Register
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string AgainPassword { get; set; }
        public string FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNum { get; set; }
        public Role? Role { get; set; }
    }
}
