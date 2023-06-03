using AutoServiceMVC.Models;
using AutoServiceMVC.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net.Http;
using System.Security.Claims;
using AutoServiceMVC.Services.System;
using Microsoft.AspNetCore.Identity;

namespace AutoServiceMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthController : Controller
    {
        private readonly IAuthenticateService<Employee> _emplService;
        private readonly ICookieAuthentication _auth;

        public AuthController(IAuthenticateService<Employee> emplService,
                                ICookieAuthentication auth)
        {
            _emplService = emplService;
            _auth = auth;
        }

        public ActionResult Login()
        {
            var rs = User.FindFirstValue("Id");
            return View();
        }

        [Authorize(Roles = "Admin", AuthenticationSchemes = "Admin_Scheme")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Username,Password")] Login login)
        {
            if (ModelState.IsValid)
            {
                var status = await _emplService.ValidateLoginAsync(login);

                if (status.IsSuccess)
                {
                    await _auth.SignInAsync(status.Data, HttpContext);

                    TempData["Message"] = "Login success";

                    return RedirectToAction("Index", "Home", new {area = "Admin"});
                }

                ModelState.AddModelError(String.Empty, status.Message);
            }

            return View();
        }

        [HttpPost]
        // [Authorize(Roles = "Admin", AuthenticationSchemes = "Admin_Scheme")]
        public async Task<IActionResult> Register([Bind("Username,Password,AgainPassword,FullName,Email,RoleId")]
                                Register register)
        {
            if (ModelState.IsValid)
            {
                var checkStatus = await _emplService.CheckEmailAndUsernameAsync(register.Email, register.Username);

                if ((bool)checkStatus.Data)
                {
                    ModelState.AddModelError(String.Empty, "Email was register");
                    return View();
                }

                var status = await _emplService.RegisterAsync(register);

                if (status.IsSuccess)
                {
                    return RedirectToAction("Index", "Employee", new {area = "Admin"});
                }

                ModelState.AddModelError(String.Empty, status.Message);
            }

            return View();
        }

        [Authorize(AuthenticationSchemes = "Admin_Scheme")]
        public async Task<IActionResult> Logout()
        {
            await _auth.SignOutAsync(HttpContext, false);
            return RedirectToAction("Login");
        }
    }
}
