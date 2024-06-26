﻿using AutoServiceMVC.Data;
using AutoServiceMVC.Hubs;
using AutoServiceMVC.Models;
using AutoServiceMVC.Models.System;
using AutoServiceMVC.Services;
using AutoServiceMVC.Services.Implement;
using AutoServiceMVC.Services.System;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Security.Principal;

namespace AutoServiceMVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthenticateService<User> _userAuth;
        private readonly ICommonRepository<User> _userService;
        private readonly ICookieAuthentication _auth;
        private readonly ICommonRepository<Order> _orderRepo;
		private readonly ICommonRepository<Product> _productRepo;
		private readonly ICommonRepository<FavoriteProduct> _favRepo;
		private readonly ICookieService _cookie;
        private readonly ISessionCustom _session;
        private readonly AppDbContext _dbContext;
        private readonly IMailService _mail;
        private readonly IHubContext<HubServer> _hub;
        private readonly IHashPassword _hash;
		private readonly IJWTAuthentication _jwt;

        public AuthController(AppDbContext dbContext, 
                                IAuthenticateService<User> userAuth,
                                ICommonRepository<User> userService,
                                ICommonRepository<Order> orderRepo,
                                ICommonRepository<Product> productRepo,
                                ICommonRepository<FavoriteProduct> favRepo,
                                ICookieAuthentication auth,
                                ICookieService cookie,
                                ISessionCustom session,
                                IMailService mail,
                                IHubContext<HubServer> hub,
                                IHashPassword hash,
                                IJWTAuthentication jwt)
        {
            _dbContext = dbContext;
            _userAuth = userAuth;
            _userService = userService;
            _auth = auth;
            _orderRepo = orderRepo;
            _productRepo = productRepo;
            _favRepo = favRepo;
			_cookie = cookie;
            _session = session;
            _mail = mail;
            _hub = hub;
            _hash = hash;
			_jwt = jwt;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Username, Password")] Login login)
        {
            if (ModelState.IsValid)
            {
                var status = await _userAuth.ValidateLoginAsync(login);

                if (status.IsSuccess)
                {
                    await _auth.SignInAsync(status.Data, HttpContext);

                    var result = await HttpContext.AuthenticateAsync("User_Scheme");

                    await AddGuestOrder((status.Data as User));
                    await AddFavoriteProduct(status.Data as User);

					TempData["Message"] = "Login successfully";
					return RedirectToAction("Index", "Home");
                }

				TempData["Message"] = status.Message;
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Username,Password,AgainPassword,FullName,Email")]
                                Register register)
        {
            if (ModelState.IsValid)
            {
                var checkStatus = await _userAuth.CheckEmailAndUsernameAsync(register.Email, register.Username);

                if((bool) checkStatus.Data)
                {
                    return View();
                }

                var data = JsonConvert.SerializeObject(register);
                var token = _jwt.GenerateToken(data);

                _session.AddToSession(HttpContext, "RegisterMailData", data);

                var confirmEmail = Url.Action("ConfirmEmail","Auth", new { token = token }, Request.Scheme);

                var mailBody = _mail.GetMailFromContent(register.FullName, "confirm email", confirmEmail);

				MailContent content = new MailContent()
				{
					To = register.Email,
					Subject = "CONFIRM EMAIL IN AUTOSERVICE",
					Body = mailBody
				};

                _mail.SendMailAsync(content);

                TempData["SendMailAgainURL"] = Url.Action("SendRegisterMailAgain");

                return RedirectToAction("VerifyEmail");
            }
            ModelState.AddModelError(String.Empty, "Some fields was wrong");

            return View();
        }

        public IActionResult IdentityMail()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IdentityMail(string mail)
        {
            if (!mail.IsNullOrEmpty())
            {
                var identity = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == mail && x.HashPassword != null);

                if(identity != null)
                {
                    var user = new User
                    {
                        UserId = identity.UserId,
                        Username = identity.Username,
                        Email = mail
                    };

                    var data = JsonConvert.SerializeObject(user);
                    var token = _jwt.GenerateToken(data);

                    _session.AddToSession(HttpContext, "IdentityMailData", data);
 
                    var resetUrl = Url.Action("ResetPassword", "Auth", new { token = token }, Request.Scheme);

                    var mailBody = _mail.GetMailFromContent(identity.Username, "reset password", resetUrl);

                    MailContent content = new MailContent()
                    {
                        To = mail,
                        Subject = "CONFIRM EMAIL IN AUTOSERVICE",
                        Body = mailBody
					};

                    _mail.SendMailAsync(content);

                    TempData["SendMailAgainURL"] = Url.Action("SendIdentityMailAgain");

                    return RedirectToAction("VerifyEmail");
                }

                TempData["Message"] = "Email was not register or login by Google";
            }

            return View();
        }

        [HttpPost]
        public async Task<JsonResult> SendIdentityMailAgain()
        {
            var data = _session.GetSessionValue<string>(HttpContext, "IdentityMailData");
            var identity = JsonConvert.DeserializeObject<User>(data);

            var token = _jwt.GenerateToken(data);

            var resetUrl = Url.Action("ResetPassword", "Auth", new { token = token }, Request.Scheme);

            var mailBody = _mail.GetMailFromContent(identity.Username, "reset password", resetUrl);

            MailContent content = new MailContent()
            {
                To = identity.Email,
                Subject = "CONFIRM EMAIL IN AUTOSERVICE",
                Body = mailBody
            };

            await _mail.SendMailAsync(content);

            return Json(new { success = true, message = "Sent mail success" });
        }

        [HttpPost]
        public async Task<IActionResult> SendRegisterMailAgain()
        {
            var data = _session.GetSessionValue<string>(HttpContext, "RegisterData");
            var register = JsonConvert.DeserializeObject<Register>(data);

            var token = _jwt.GenerateToken(data);

            _session.AddToSession(HttpContext, "RegisterMailData", data);

            var confirmEmail = Url.Action("ConfirmEmail", "Auth", new { token = token }, Request.Scheme);

            var mailBody = _mail.GetMailFromContent(register.FullName, "confirm email", confirmEmail);

            MailContent content = new MailContent()
            {
                To = register.Email,
                Subject = "CONFIRM EMAIL IN AUTOSERVICE",
                Body = mailBody
            };

            await _mail.SendMailAsync(content);

            return Json(new { success = true, message = "Sent mail success" });
        }

        [Authorize(AuthenticationSchemes = "User_Scheme")]
        public async Task<IActionResult> ChangePassword()
        {
            var id = Convert.ToInt32(User.FindFirstValue("Id"));
            var userRs = await _userService.GetByIdAsync(id);
            var user = userRs.Data as User;
            ViewData["Id"] = user.UserId;
            ViewData["Username"] = user.Username;
            return View();
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "User_Scheme")]
        public async Task<IActionResult> ChangePassword([Bind("UserId,OldPassword,Password,AgainPassword")] ChangePassword changePassword)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.GetByIdAsync(changePassword.UserId);
                if (result.IsSuccess)
                {
                    var user = result.Data as User;

                    if (user.HashPassword != _hash.GetHashPassword(changePassword.OldPassword))
                    {
                        TempData["Message"] = "Old password is wrong";
                        return RedirectToAction("ChangePassword");
                    }

                    user.HashPassword = _hash.GetHashPassword(changePassword.Password);

                    await _userService.UpdateAsync(user);

                    TempData["Message"] = "Change password success";
                    return RedirectToAction("Index", "Profile");
                }
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> ResetPassword([FromQuery] string token)
        {
            var data = _jwt.ValidateToken(token);

            if (data == null)
            {
                return RedirectToAction("BadLink", "Auth");
            }

            _session.DeleteSession(HttpContext, "IdentityMailData");
            var user = JsonConvert.DeserializeObject<User>(data);

            if(user != null)
            {
                ViewData["UserId"] = user.UserId;
                ViewData["Username"] = user.Username;
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

                    TempData["Message"] = "Change password successful";
                    return RedirectToAction("Login");
                }
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> ConfirmEmail([FromQuery] string token)
        {
            string data = _jwt.ValidateToken(token);

            if (data == null)
            {
                return RedirectToAction("BadLink", "Auth");
            }

            _session.DeleteSession(HttpContext, "RegisterMailData");
            var register = JsonConvert.DeserializeObject<Register>(data);

            if (register != null)
            {
                var result = await _userAuth.RegisterAsync(register);

                if (result.IsSuccess)
                {
                    TempData["Message"] = "Confirm email success";
                    return View("Login");
                }
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task LoginGoogle()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
            {
                RedirectUri = Url.Action("GoogleResponse")
            });
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync("User_Scheme");
            var claims = result.Principal;

            var user = _dbContext.Users.FirstOrDefault(x => x.Email == claims.FindFirstValue(ClaimTypes.Email));

            if(user == null)
            {
                var checkStatus = await _userAuth.CheckEmailAndUsernameAsync(claims.FindFirstValue(ClaimTypes.Email), null);

                user = new User()
                {
                    FullName = claims.FindFirstValue(ClaimTypes.Name),
                    Email = claims.FindFirstValue(ClaimTypes.Email),
                    Point = 0
                };

                user = (await _userService.CreateAsync(user)).Data as User;
			}

            await _auth.SignInAsync(user, HttpContext);

            await AddGuestOrder(user);
			await AddFavoriteProduct(user);

			return RedirectToAction("Index", "Home");
		}

        public IActionResult VerifyEmail()
        {
            return View();
        }

        public IActionResult BadLink()
        {
            return View();
        }

        public async Task<IActionResult> ValidateExistEmail(string email)
        {
            var result = await _userAuth.CheckEmailAndUsernameAsync(email, null);

            if (result.IsSuccess)
            {
                return Json((bool)result.Data ? "Email address was existed." : "true");
            }

            return Json("Uncheck");
        }

        public async Task<IActionResult> ValidateExistUsername(string username)
        {
            var result = await _userAuth.CheckEmailAndUsernameAsync(username, username);

            if (result.IsSuccess)
            {
                return Json((bool)result.Data ? "Username was existed." : "true");
            }

            return Json("Uncheck");
        }

        public async Task AddGuestOrder(User user)
        {
            var guestOrderIdCookie = _cookie.GetCookie(HttpContext, "guest_order");
            if(!guestOrderIdCookie.IsNullOrEmpty())
            {
                var guestOrderIdList = guestOrderIdCookie.Split(",").ToList();


				foreach (var orderIdString in guestOrderIdList)
                {
                    int orderId = Convert.ToInt32(orderIdString ?? "0");
                    var orderRs = await _orderRepo.GetByIdAsync(orderId);

                    if (orderRs.IsSuccess)
                    {
                        var order = orderRs.Data as Order;
                        order.UserId = user.UserId;
                        await _orderRepo.UpdateAsync(order);

                        _cookie.RemoveCookie(HttpContext, "guest_order");
                    }
                }
            }
        }

        public async Task AddFavoriteProduct(User user)
        {
			var favIdCookie = _cookie.GetCookie(HttpContext, "fav_product");
			if (!favIdCookie.IsNullOrEmpty())
			{
				var favIdList = favIdCookie.Split(",").ToList();


				foreach (var favId in favIdList)
				{
					int productId = Convert.ToInt32(favId ?? "0");
					var productRs = await _productRepo.GetByIdAsync(productId);

					if (productRs.IsSuccess)
					{
						var userId = user.UserId;

                        var status = await ((FavoriteProductRepository)_favRepo).GetByConditions(fv => fv.UserId == userId && fv.ProductId == productId);

                        if (!status.IsSuccess)
                        {
                            await _favRepo.CreateAsync(new FavoriteProduct { ProductId = productId, UserId = userId });
                        }
					}
				}

				_cookie.RemoveCookie(HttpContext, "fav_product");
			}
		}
	}
}
