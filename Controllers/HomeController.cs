using AutoServiceMVC.Models;
using AutoServiceMVC.Models.System;
using AutoServiceMVC.Services;
using AutoServiceMVC.Services.System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using System.ComponentModel;
using System.Diagnostics;

namespace AutoServiceMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMailService _mail;

        public HomeController(IMailService mail)
        {
            _mail = mail;
        }

        public async Task<IActionResult> Index()
        {
            //string relativePath = $"wwwroot/mailContent/index.html";

            //string mailContent = System.IO.File.ReadAllText(relativePath);

            //MailContent mail = new MailContent()
            //{
            //    To = "phamtoan15062002@gmail.com",
            //    Subject = "Test",
            //    Body = mailContent
            //};

            //await _mail.SendMailAsync(mail);

            return View();
        }
    }
}