using AutoServiceMVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace AutoServiceMVC.Services.System
{
    public interface ICookieAuthentication
    {
        public Task SignInAsync(object account, HttpContext context);
        public Task SignOutAsync(HttpContext context, bool IsUser);
    }

    public class CookieAuthentication : ICookieAuthentication
    {
        public async Task SignInAsync(object account, HttpContext context)
        {
            string role;
            int id;
            string scheme;

            if(account is Employee)
            {
                role = (account as Employee).Role.RoleName;
                id = (account as Employee).EmployeeId;
                scheme = "Admin_Scheme";
            }
            else
            {
                role = "User";
                id = (account as User).UserId;
                scheme = "User_Scheme";
            }

            List<Claim> claims = new List<Claim>()
            {
                new Claim("Id", id.ToString()),
                new Claim(ClaimTypes.Name, (account as Account).FullName),
                new Claim(ClaimTypes.Role, role)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, scheme);

            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = false
            };

            await context.SignInAsync(scheme,
                new ClaimsPrincipal(claimsIdentity), properties);
        }

        public async Task SignOutAsync(HttpContext context, bool IsUser)
        {
            await context.SignOutAsync(IsUser ? "User_Scheme" : "Admin_Scheme");
        }
    }
}
