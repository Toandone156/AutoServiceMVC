using AutoServiceMVC.Models;
using AutoServiceMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace AutoServiceMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "Admin_Scheme")]
    public class FeedbackController : Controller
    {
        private readonly ICommonRepository<ServiceFeedback> _feedbackRepo;

        public FeedbackController(ICommonRepository<ServiceFeedback> feedbackRepo)
        {
            _feedbackRepo = feedbackRepo;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _feedbackRepo.GetAllAsync();
            if (result.IsSuccess)
            {
                return View(result.Data);
            }

            return View();
        }
    }
}
