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
using AutoServiceMVC.Models.System;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Castle.Core.Internal;
using AutoServiceMVC.Data;
using System.Security.Policy;

namespace AutoServiceMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthController : Controller
    {
        private readonly IAuthenticateService<Employee> _emplService;
        private readonly ICommonRepository<Employee> _emplRepo;
        private readonly ICookieAuthentication _auth;
        private readonly AppDbContext _dbContext;
        private readonly IJWTAuthentication _jwt;
        private readonly IMailService _mail;
        private readonly IHashPassword _hash;

        public AuthController(
            IAuthenticateService<Employee> emplService,
            ICommonRepository<Employee> emplRepo,
            ICookieAuthentication auth,
            AppDbContext dbContext,
            IJWTAuthentication jwt,
            IMailService mail,
            IHashPassword hash
            )
        {
            _emplService = emplService;
            _emplRepo = emplRepo;
            _auth = auth;
            _dbContext = dbContext;
            _jwt = jwt;
            _mail = mail;
            _hash = hash;
        }

        public ActionResult Login()
        {
            var rs = User.FindFirstValue("Id");
            return View();
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ForgotPassword(string email)
        {
            if (!email.IsNullOrEmpty())
            {
                var identity = await _dbContext.Employees.FirstOrDefaultAsync(x => x.Email == email && x.HashPassword != null);

                if (identity != null)
                {
                    var employee = new Employee
                    {
                        EmployeeId = identity.EmployeeId,
                        Username = identity.Username,
                        Email = email
                    };

                    var data = JsonConvert.SerializeObject(employee);
                    var token = _jwt.GenerateToken(data);

                    var resetUrl = Url.Action("ResetPassword", "Auth", new { token = token }, Request.Scheme);

                    var mailBody = _mail.GetMailFromContent(identity.Username, "reset password", resetUrl);

                    MailContent content = new MailContent()
                    {
                        To = email,
                        Subject = "RESET ADMIN PASSWORD IN AUTOSERVICE",
                        Body = mailBody
                    };

                    _mail.SendMailAsync(content);

                    TempData["Message"] = "Reset link was sent in email";
                    return RedirectToAction("Login");
                }

                TempData["Message"] = "Email was not register";
            }

            return View();
        }

        public async Task<IActionResult> ResetPassword([FromQuery] string token)
        {
            var data = _jwt.ValidateToken(token);

            if (data == null)
            {
                TempData["Message"] = "Your link is expired";
                return RedirectToAction("Login");
            }

            var employee = JsonConvert.DeserializeObject<Employee>(data);

            if (employee != null)
            {
                ViewData["EmployeeId"] = employee.EmployeeId;
                ViewData["Username"] = employee.Username;

                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([Bind("UserId,Password,AgainPassword")] ResetPassword resetPassword)
        {
            if (ModelState.IsValid)
            {
                var result = await _emplRepo.GetByIdAsync(resetPassword.UserId);
                if (result.IsSuccess)
                {
                    var empl = result.Data as Employee;
                    empl.HashPassword = _hash.GetHashPassword(resetPassword.Password);

                    await _emplRepo.UpdateAsync(empl);

                    TempData["Message"] = "Change password success";
                    return RedirectToAction("Login");
                }
            }

            return RedirectToAction("Index", "Home");
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

                TempData["Message"] = status.Message;
            }

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin", AuthenticationSchemes = "Admin_Scheme")]
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

                TempData["Message"] = status.Message;
            }

            return View();
        }

        [Authorize(AuthenticationSchemes = "Admin_Scheme")]
        public async Task<IActionResult> Logout()
        {
            await _auth.SignOutAsync(HttpContext, false);
            return RedirectToAction("Login");
        }

        [Authorize(AuthenticationSchemes = "Admin_Scheme")]
        public async Task<IActionResult> Profile()
        {
            var id = Convert.ToInt32(User.FindFirstValue("Id"));
            var emplRs = await _emplRepo.GetByIdAsync(id);
            return View(emplRs.Data as Employee);
        }

        [Authorize(AuthenticationSchemes = "Admin_Scheme")]
        public async Task<IActionResult> ChangePassword()
        {
            var id = Convert.ToInt32(User.FindFirstValue("Id"));
            var emplRs = await _emplRepo.GetByIdAsync(id);
            var empl = emplRs.Data as Employee;
            ViewData["Id"] = empl.EmployeeId;
            ViewData["Username"] = empl.Username;
            return View();
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Admin_Scheme")]
        public async Task<IActionResult> ChangePassword([Bind("UserId,OldPassword,Password,AgainPassword")] ChangePassword changePassword)
        {
            if (ModelState.IsValid)
            {
                var result = await _emplRepo.GetByIdAsync(changePassword.UserId);
                if (result.IsSuccess)
                {
                    var empl = result.Data as Employee;

                    if(empl.HashPassword != _hash.GetHashPassword(changePassword.OldPassword))
                    {
                        TempData["Message"] = "Old password is wrong";
                        return RedirectToAction("ChangePassword");
                    }

                    empl.HashPassword = _hash.GetHashPassword(changePassword.Password);

                    await _emplRepo.UpdateAsync(empl);

                    TempData["Message"] = "Change password success";
                    return RedirectToAction("Profile");
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
