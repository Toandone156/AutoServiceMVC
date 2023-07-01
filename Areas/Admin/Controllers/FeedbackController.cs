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
                var sfeedbacks = (result.Data as List<ServiceFeedback>).OrderByDescending(f => f.CreatedAt);

                return View(sfeedbacks);
            }

            TempData["Message"] = "Get data fail";
            return View();
        }
    }
}
