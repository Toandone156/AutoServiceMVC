﻿using AutoServiceMVC.Data;
using AutoServiceMVC.Models;
using AutoServiceMVC.Services;
using AutoServiceMVC.Services.System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoServiceMVC.Controllers
{
    [Authorize(AuthenticationSchemes = "User_Scheme")]
    public class ProductFeedbackController : Controller
    {
        private readonly ICommonRepository<ProductFeedback> _productFeedbackRepo;
        private readonly IImageUploadService _uploadService;
        private readonly AppDbContext _context;

        public ProductFeedbackController(
            AppDbContext context,
            ICommonRepository<ProductFeedback> productFeedbackRepo, 
            IImageUploadService uploadService)
        {
            _productFeedbackRepo = productFeedbackRepo;
            _uploadService = uploadService;
            _context = context;
        }

        public async Task<IActionResult> Index(int id) //ProductId
        {
            var userId = Convert.ToInt32(User.FindFirst("Id"));

            var orderProducts = _context.Users
                .Where(x => x.UserId == userId)
                .SelectMany(u => u.Orders)
                .Where(o => o.CreatedAt < DateTime.Now.AddDays(1))
                .SelectMany(o => o.OrderDetails)
                .Select(od => od.Product)
                .ToList();

            if(orderProducts.Any(x => x.ProductId == id)) 
            {
                return View();
            }

            return RedirectToAction("Detail", "Product", id);
        }

        [HttpPost]
        public async Task<IActionResult> SendProductFeedback(
            [Bind("ProductId,Comment,Rating")] ProductFeedback feedback,
            IFormFile imageFile)
        {
            if(imageFile != null)
            {
                var imageLink = await _uploadService.UploadImageAsync(imageFile);
                feedback.Image = imageLink;
            }

            var userId = Convert.ToInt32(User.FindFirst("Id"));
            feedback.UserId = userId;

            await _productFeedbackRepo.CreateAsync(feedback);

            TempData["Message"] = "Thanks for your feedback!";
            return RedirectToAction("Detail", "Product", feedback.ProductId);
        }
    }
}
