using Microsoft.AspNetCore.Mvc;

namespace AutoServiceMVC.Controllers
{
    public class ChatbotController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
