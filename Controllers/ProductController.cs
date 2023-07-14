using AutoServiceMVC.Data;
using AutoServiceMVC.Models;
using AutoServiceMVC.Services.System;
using AutoServiceMVC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Net;
using MailKit.Search;
using Castle.Core.Internal;
using AutoServiceMVC.Services.Implement;

namespace AutoServiceMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly ICommonRepository<Product> _productRepo;
        private readonly ICommonRepository<ProductFeedback> _feedbackRepo;
        private readonly ICommonRepository<FavoriteProduct> _favProductRepo;
        private readonly ICookieService _cookie;

        [ActivatorUtilitiesConstructor]
        public ProductController(
            ICommonRepository<Product> productRepo,
            ICommonRepository<ProductFeedback> feedbackRepo,
            ICommonRepository<FavoriteProduct> favProductRepo,
            ICookieService cookie)
        {
            _productRepo = productRepo;
            _feedbackRepo = feedbackRepo;
            _favProductRepo = favProductRepo;
            _cookie = cookie;
        }

        public async Task<IActionResult> Details([FromRoute] int id)
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
                var status = await ((FavoriteProductRepository)_favProductRepo).GetByConditions(f => f.UserId == userId);

                if (status.IsSuccess)
                {
                    favIds = (status.Data as IEnumerable<FavoriteProduct>).Select(f => f.ProductId).ToList();
                }
            }

            ViewBag.favIds = favIds;

            var prostatus = await _productRepo.GetByIdAsync(id);

            if (prostatus.IsSuccess)
            {
                return View(prostatus.Data);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<JsonResult> DetailApi(int id)
        {
            var status = await _productRepo.GetByIdAsync(id);

            if (status.IsSuccess)
            {
                var product = status.Data as Product;
                return Json(new { success = true, name = product.ProductName, price = product.Price, image = product.ProductImage });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public async Task<JsonResult> ToggleFavorite(int productId, bool favorite)
        {
            var userId = Convert.ToInt32(User.FindFirstValue("Id") ?? "0");

            var productStatus = await _productRepo.GetByIdAsync(productId);
            if (!productStatus.IsSuccess)
            {
                return Json(new { success = false });
            }

            if (userId == 0)
            {
                var favProducts = _cookie.GetCookie(HttpContext, "fav_product");
                var favProductsList = favProducts.IsNullOrEmpty() ? new List<string>() : favProducts.Split(",").ToList();

                if (favorite)
                {
                    favProductsList.Add(productId.ToString());
                }
                else
                {
                    favProductsList.Remove(productId.ToString());
                }

                _cookie.AddCookie(HttpContext, 365, "fav_product", String.Join(",", favProductsList));
            }
            else
            {
                if(favorite)
                {
                    _favProductRepo.CreateAsync(new FavoriteProduct { ProductId = productId, UserId =  userId });
                }
                else
                {
                    ((FavoriteProductRepository)_favProductRepo).DeleteByEntityAsync(new FavoriteProduct { ProductId = productId, UserId = userId });
                }
            }

            return Json(new { success = true });
        }
    }
}
