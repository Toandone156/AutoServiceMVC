using AutoServiceMVC.Data;
using AutoServiceMVC.Models;
using AutoServiceMVC.Models.System;
using AutoServiceMVC.Services;
using AutoServiceMVC.Services.System;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoServiceMVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthenticateService<User> _userAuth;
        private readonly ICommonRepository<User> _userService;
        private readonly ICookieAuthentication _auth;
        private readonly AppDbContext _dbContext;
        private readonly IMailService _mail;
        private readonly IHashPassword _hash;
        private readonly ISessionCustom _session;

        public AuthController(AppDbContext dbContext, 
                                IAuthenticateService<User> userAuth,
                                ICommonRepository<User> userService,
                                ICookieAuthentication auth,
                                IMailService mail,
                                ISessionCustom session,
                                IHashPassword hash)
        {
            _dbContext = dbContext;
            _userAuth = userAuth;
            _userService = userService;
            _auth = auth;
            _mail = mail;
            _hash = hash;
            _session = session;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind("Username,Password")] Login login)
        {
            if (ModelState.IsValid)
            {
                var status = await _userAuth.ValidateLoginAsync(login);

                if (status.IsSuccess)
                {
                    await _auth.SignInAsync(status.Data, HttpContext);
                    TempData["Message"] = status.Message;

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(String.Empty, status.Message);
            }
            else
            {
                ModelState.AddModelError(String.Empty, "Some fields is invalid");
            }

            return View();
        }

        [Authorize(AuthenticationSchemes = "User_Scheme")]
        public async Task<IActionResult> Logout()
        {
            await _auth.SignOutAsync(HttpContext, true);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([Bind("Username,Password,AgainPassword,FullName,Email")]
                                Register register)
        {
            if (ModelState.IsValid)
            {
                var status = await _userAuth.RegisterAsync(register);

                if (status.IsSuccess)
                {
                    TempData["Message"] = status.Message;
                    //2FA
                    return RedirectToAction("Login");
                }

                ModelState.AddModelError(String.Empty, status.Message);
            }
            else
            {
                ModelState.AddModelError(String.Empty, "Some fields is invalid");
            }

            return View();
        }

        public IActionResult IdentityMail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IdentityMail(string mail)
        {
            if (!mail.IsNullOrEmpty())
            {
                var identity = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == mail);

                if(identity != null)
                {
                    var hashEmail = _hash.GetHashPassword(mail);

                    //Save id in session
                    _session.AddToSessionWithTimeout(HttpContext, hashEmail, identity.UserId, 5);
                    var resetUrl = Url.Action("ResetPassword", new { key = hashEmail });

                    MailContent content = new MailContent()
                    {
                        To = mail,
                        Subject = "RESET PASSWORD IN AUTOSERVICE",
                        Body = $"Link to resetpassword: {resetUrl}"
                    };

                    await _mail.SendMailAsync(content);
                }
            }

            return View();
        }

        public async Task<IActionResult> ResetPassword([FromQuery] string key)
        {
            var id = _session.GetSessionValue<int>(HttpContext, key);
            if(id != 0)
            {
                var result = await _userService.GetByIdAsync(id);

                if (result.IsSuccess)
                {
                    return View(result.Data);
                }
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([Bind("UserId,Password,AgainPassword")] ResetPassword resetPassword)
        {
            if(ModelState.IsValid)
            {
                var result = await _userService.GetByIdAsync(resetPassword.UserId);
                if (result.IsSuccess)
                {
                    var user = result.Data as User;
                    user.HashPassword = _hash.GetHashPassword(resetPassword.Password);

                    await _userService.UpdateAsync(user);

                    return RedirectToAction("Login");
                }
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<JsonResult> SendOTP(string mail)
        {
            if(mail != null)
            {
                var hashEmail = _hash.GetHashPassword(mail);
                var otp = GenerateOTP();

                _session.AddToSessionWithTimeout(HttpContext, hashEmail, otp, 5);

                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public async Task<JsonResult> CheckOTP(string mail, string otp)
        {
            if (!(mail.IsNullOrEmpty() && otp.IsNullOrEmpty()))
            {
                var hashEmail = _hash.GetHashPassword(mail);

                var correctOTP = _session.GetSessionValue<string>(HttpContext, hashEmail);
                if (correctOTP != null && otp == correctOTP)
                {
                    return Json(new {success = true});
                }
            }

            return Json(new { success = false });
        }

        public string GenerateOTP()
        {
            const string chars = "0123456789";
            var random = new Random();
            var otp = new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            return otp;
        }
    }
}
