using AutoServiceMVC.Models;
using AutoServiceMVC.Services;
using AutoServiceMVC.Services.System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoServiceMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ICommonRepository<Product> _productRepo;
        private readonly ICommonRepository<Category> _categoryRepo;
        private readonly IImageUploadService _imageUpload;

        public ProductController(
            ICommonRepository<Product> productRepo,
            ICommonRepository<Category> categoryRepo,
            IImageUploadService imageUpload)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _imageUpload = imageUpload;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _productRepo.GetAllAsync();
            return View(result.Data);
        }

        public async Task<IActionResult> Details([FromRoute]int id)
        {
            var result = await _productRepo.GetByIdAsync(id);
            if(result.IsSuccess)
            {
                var categories = await _categoryRepo.GetAllAsync();
                ViewBag.CategoryList = categories.Data;
                return View(result.Data);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _categoryRepo.GetAllAsync();
            ViewBag.CategoryList = categories.Data;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("ProductName,Price,ProductDescription,CategoryId")] Product product,
            IFormFile imageFile)
        {
            if(imageFile != null)
            {
                var imageLink = await _imageUpload.UploadImageAsync(imageFile);
                product.ProductImage = imageLink;
            }

            var result = await _productRepo.CreateAsync(product);
            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            [Bind("ProductId,ProductName,Price,ProductDescription,CategoryId,ProductImage")] Product product,
            IFormFile imageFile)
        {
            if (imageFile != null)
            {
                var imageLink = await _imageUpload.UploadImageAsync(imageFile);
                product.ProductImage = imageLink;
            }

            var result = await _productRepo.UpdateAsync(product);
            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Details",product.ProductId);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productRepo.DeleteByIdAsync(id);

            return Json(result);
        }
    }
}
