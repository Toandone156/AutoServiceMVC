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
        private readonly IImageUploadService _imageUpload;

        public ProductController(
            AppDbContext context,
            ICommonRepository<Product> productRepo,
            IImageUploadService imageUpload)
        {
            _context = context;
            _productRepo = productRepo;
            _imageUpload = imageUpload;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _productRepo.GetAllAsync();
            return View(result.Data);
        }

        public async Task<IActionResult> Details([FromRoute]int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductFeedbacks)
                .ThenInclude(f => f.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if(product != null)
            {
                return View(product);
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

            var result = await _productRepo.CreateAsync(product);
            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin", AuthenticationSchemes = "Admin_Scheme")]
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

            return View("Details",product.ProductId);
        }

        [HttpPost]
        [Authorize(Roles = "Admin", AuthenticationSchemes = "Admin_Scheme")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            //var result = await _productRepo.DeleteByIdAsync(id);

            return Json("hello");
        }
    }
}
