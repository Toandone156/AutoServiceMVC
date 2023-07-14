using AutoServiceMVC.Models;
using AutoServiceMVC.Models.System;
using AutoServiceMVC.Services;
using AutoServiceMVC.Services.Implement;
using AutoServiceMVC.Services.System;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Security.Claims;

namespace AutoServiceMVC.Controllers
{
    public class HomeController : Controller
    {
        private ICommonRepository<FavoriteProduct> _favRepo;
        private readonly ICookieService _cookie;

        public HomeController(ICommonRepository<FavoriteProduct> favRepo,
                                ICookieService cookie
                )
        {
            _favRepo = favRepo;
            _cookie = cookie;
        }
        public async Task<IActionResult> Index()
        {
            int userId = Convert.ToInt32(User.FindFirstValue("Id") ?? "0");
            List<int> favIds = new List<int>();

            if (userId == 0)
            {
                var favProducts = _cookie.GetCookie(HttpContext, "fav_product");
                var favProductsList = favProducts.IsNullOrEmpty() ? new List<string>() : favProducts.Split(",").ToList();
                favIds = favProductsList.ConvertAll(int.Parse);
            }
            else
            {
                var status = await ((FavoriteProductRepository)_favRepo).GetByConditions(f => f.UserId == userId);

                if (status.IsSuccess)
                {
                    var favoriteList = status.Data as IEnumerable<FavoriteProduct>;
                    favIds = favoriteList.Select(fav => fav.ProductId).ToList();
                }
            }

            ViewBag.favIds = favIds;

            return View();
        }
    }
}