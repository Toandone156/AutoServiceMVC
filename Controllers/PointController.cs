using AutoServiceMVC.Models;
using AutoServiceMVC.Services;
using AutoServiceMVC.Services.Implement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoServiceMVC.Controllers
{
    [Authorize(AuthenticationSchemes = "User_Scheme")]
    public class PointController : Controller
    {
        private readonly ICommonRepository<PointTrading> _pointTradingRepo;
        private readonly ICommonRepository<User> _userRepo;

        public PointController(
            ICommonRepository<PointTrading> pointTradingRepo,
            ICommonRepository<User> userRepo) 
        {
            _pointTradingRepo = pointTradingRepo;
            _userRepo = userRepo;
        }

        public async Task<IActionResult> Index()
        {
            var userId = Convert.ToInt32(User.FindFirst("Id"));
            var rs = await ((PointTradingRepository) _pointTradingRepo).GetByUserIdAsync(userId);

            if(rs.IsSuccess)
            {
                return View(rs.Data);
            }

            return View("Index", "Home");
        }

        [HttpPost]
        public async Task<JsonResult> ChangePoint(int amount)
        {
            var userId = Convert.ToInt32(User.FindFirst("Id"));
            var userRs = await _userRepo.GetByIdAsync(userId);
            var user = userRs.Data as User;

            user.Point += amount;

            if(user.Point < 0)
            {
                return Json(new { success = false });
            }

            await _userRepo.UpdateAsync(user);

            return Json(new { success = true });
        }
    }
}
