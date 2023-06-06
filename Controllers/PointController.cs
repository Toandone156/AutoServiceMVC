using AutoServiceMVC.Models;
using AutoServiceMVC.Services;
using AutoServiceMVC.Services.Implement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            var userId = Convert.ToInt32(User.FindFirstValue("Id"));
            var user = await _userRepo.GetByIdAsync(userId);
            var rs = await ((PointTradingRepository) _pointTradingRepo).GetByUserIdAsync(userId);

            if(rs.IsSuccess)
            {
                var tradeList = (rs.Data as List<PointTrading>);

                if(tradeList != null)
                {
                    tradeList = tradeList.OrderByDescending(pt => pt.TradedAt).ToList();
                }

                ViewData["userPoint"] = (user.Data as User).Point;

                return View(tradeList);
            }

            return View("Index", "Home");
        }
    }
}
