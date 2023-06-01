using AutoServiceMVC.Data;
using AutoServiceMVC.Models;
using AutoServiceMVC.Services.System;
using AutoServiceMVC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoServiceMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly ICommonRepository<Product> _productRepo;
        private readonly ICommonRepository<ProductFeedback> _feedbackRepo;

        [ActivatorUtilitiesConstructor]
        public ProductController(
            ICommonRepository<Product> productRepo,
            ICommonRepository<ProductFeedback> feedbackRepo)
        {
            _productRepo = productRepo;
            _feedbackRepo = feedbackRepo;
        }

        public async Task<IActionResult> Detail([FromRoute] int id)
        {
            var status = await _productRepo.GetByIdAsync(id);

            if (status.IsSuccess)
            {
                return View(status.Data);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
