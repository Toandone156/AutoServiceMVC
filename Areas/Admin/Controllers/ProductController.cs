using AutoServiceMVC.Data;
using AutoServiceMVC.Models;
using AutoServiceMVC.Services;
using AutoServiceMVC.Services.System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoServiceMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "Admin_Scheme")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ICommonRepository<Product> _productRepo;
        private readonly ICommonRepository<ProductFeedback> _feedbackRepo;
        private readonly IImageUploadService _imageUpload;

        [ActivatorUtilitiesConstructor]
        public ProductController(
            AppDbContext context,
            ICommonRepository<Product> productRepo,
            ICommonRepository<ProductFeedback> feedbackRepo,
            IImageUploadService imageUpload)
        {
            _context = context;
            _productRepo = productRepo;
            _feedbackRepo = feedbackRepo;
            _imageUpload = imageUpload;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _productRepo.GetAllAsync();

            if (result.IsSuccess)
            {
                var products = (result.Data as List<Product>).OrderByDescending(o => o.ProductId);
                return View(products);
            }

            TempData["Message"] = "Get data fail";
            return View();
        }

        public async Task<IActionResult> Details([FromRoute]int id)
        {
            var status = await _productRepo.GetByIdAsync(id);

            if(status.IsSuccess)
            {
                return View(status.Data);
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin", AuthenticationSchemes = "Admin_Scheme")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin", AuthenticationSchemes = "Admin_Scheme")]
        public async Task<IActionResult> Create(
            [Bind("ProductName,Price,ProductDescription,CategoryId")] Product product,
            IFormFile imageFile)
        {
            if(imageFile != null)
            {
                var imageLink = await _imageUpload.UploadImageAsync(imageFile);
                product.ProductImage = imageLink;
            }

            if(ModelState.IsValid)
            {
                var result = await _productRepo.CreateAsync(product);
                if (result.IsSuccess)
                {
                    TempData["Message"] = "Create product success";
                    return RedirectToAction("Index");
                }

                TempData["Message"] = result.Message;
            }
            else
            {
                TempData["Message"] = "Some fields is invalid";
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin", AuthenticationSchemes = "Admin_Scheme")]
        public async Task<IActionResult> Edit(
            [Bind("ProductId,ProductName,Price,ProductDescription,CategoryId,ProductImage")] Product product,
            IFormFile? imageFile)
        {
            if (imageFile != null)
            {
                var imageLink = await _imageUpload.UploadImageAsync(imageFile);
                product.ProductImage = imageLink;
            }

            if (ModelState.IsValid)
            {
                var result = await _productRepo.UpdateAsync(product);
                if (result.IsSuccess)
                {
                    TempData["Message"] = "Edit product success";
                    return RedirectToAction("Index");
                }

                TempData["Message"] = result.Message;
            }
            else
            {
                TempData["Message"] = "Some fields is invalid";
            }

            return View("Details",product.ProductId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productRepo.DeleteByIdAsync(id);
            TempData["Message"] = "Delete product success";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Admin", AuthenticationSchemes = "Admin_Scheme")]
        public async Task<JsonResult> DeleteFeedback(int feedbackId)
        {
            var result = await _feedbackRepo.DeleteByIdAsync(feedbackId);

            return Json(new {success = result.IsSuccess});
        }

        [HttpPost]
        public async Task<JsonResult> UpdateStock(int productId)
        {
            var result = await _productRepo.GetByIdAsync(productId);
            if(result.IsSuccess)
            {
                var product = result.Data as Product;

                product.IsInStock = !product.IsInStock;
                await _productRepo.UpdateAsync(product);

                return Json(new { success = true, stockValue = product.IsInStock});
            }

            return Json(new { error = "Update fail" });
        }
    }
}
