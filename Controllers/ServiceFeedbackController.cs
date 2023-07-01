using AutoServiceMVC.Models;
using AutoServiceMVC.Services;
using AutoServiceMVC.Services.System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AutoServiceMVC.Controllers
{
    [Authorize(AuthenticationSchemes = "User_Scheme")]
    public class ServiceFeedbackController : Controller
    {
        private readonly ICommonRepository<ServiceFeedback> _serviceFeedbackRepo;
        private readonly IImageUploadService _uploadService;

        public ServiceFeedbackController(
            ICommonRepository<ServiceFeedback> serviceFeedbackRepo, 
            IImageUploadService uploadService)
        {
            _serviceFeedbackRepo = serviceFeedbackRepo;
            _uploadService = uploadService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendServiceFeedback(
            [Bind("Comment")] ServiceFeedback feedback,
            IFormFile imageFile)
        {
            if(imageFile != null)
            {
                var imageLink = await _uploadService.UploadImageAsync(imageFile);
                feedback.Image = imageLink;
            }

            var userId = Convert.ToInt32(User.FindFirstValue("Id"));
            feedback.UserId = userId;

            await _serviceFeedbackRepo.CreateAsync(feedback);

            TempData["Message"] = "Thanks for your feedback!";
            return RedirectToAction("Index", "Home");
        }
    }
}
