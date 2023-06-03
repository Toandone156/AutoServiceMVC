using AutoServiceMVC.Models;
using AutoServiceMVC.Models.System;
using AutoServiceMVC.Services;
using AutoServiceMVC.Services.System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Policy;
using System.Security.Principal;

namespace AutoServiceMVC.Controllers
{
    [Authorize(AuthenticationSchemes = "User_Scheme")]
    public class ProfileController : Controller
    {
        private readonly ICommonRepository<User> _userRepo;
        private readonly IMailService _mail;
        private readonly IHashPassword _hash;
        private readonly ISessionCustom _session;

        public ProfileController(ICommonRepository<User> userRepo,
                                IMailService mail,
                                ISessionCustom session,
                                IHashPassword hash)
        {
            _userRepo = userRepo;
            _mail = mail;
            _hash = hash;
            _session = session;
        }
        public async Task<IActionResult> Index()
        {
            int userId = Convert.ToInt32(User.FindFirstValue("Id"));
            var user = await _userRepo.GetByIdAsync(userId);

            return View(user.Data);
        }

        public async Task<IActionResult> Update()
        {
            int userId = Convert.ToInt32(User.FindFirstValue("Id"));
            var user = await _userRepo.GetByIdAsync(userId);

            return View(user.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Update([Bind("FullName")] User user)
        {
            int userId = Convert.ToInt32(User.FindFirstValue("Id"));
            var result = await _userRepo.GetByIdAsync(userId);

            var oldUser = result.Data as User;

            oldUser.FullName = user.FullName;

            await _userRepo.UpdateAsync(oldUser);

            return View("Index", userId);
        }

        public async Task<IActionResult> ChangePassword()
        {
            int userId = Convert.ToInt32(User.FindFirstValue("Id"));
            var result = await _userRepo.GetByIdAsync(userId);
            var user = result.Data as User;

            var hashEmail = _hash.GetHashPassword(user.Email);

            //Save id in session
            _session.AddToSession(HttpContext, hashEmail, user.UserId);
            var resetUrl = Url.Action("ResetPassword", new { key = hashEmail });

            MailContent content = new MailContent()
            {
                To = user.Email,
                Subject = "RESET PASSWORD IN AUTOSERVICE",
                Body = $"Link to resetpassword: {resetUrl}"
            };

            await _mail.SendMailAsync(content);

            return View(user);
        }
    }
}
