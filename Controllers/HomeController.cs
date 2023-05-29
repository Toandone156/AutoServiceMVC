using AutoServiceMVC.Models;
using AutoServiceMVC.Models.System;
using AutoServiceMVC.Services;
using AutoServiceMVC.Services.System;
using Microsoft.AspNetCore.Mvc;
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
            return View();
        }
    }
}