using AutoServiceMVC.Models;
using AutoServiceMVC.Services;
using AutoServiceMVC.Services.Implement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoServiceMVC.Controllers
{
    [Authorize(AuthenticationSchemes = "User_Scheme")]
    public class CouponController : Controller
    {
        private readonly ICommonRepository<Coupon> _couponRepo;
        private readonly ICommonRepository<UserCoupon> _userCouponRepo;
        private readonly ICommonRepository<User> _userRepo;

        public CouponController(
            ICommonRepository<Coupon> couponRepo,
            ICommonRepository<UserCoupon> userCouponRepo,
            ICommonRepository<User> userRepo)
        {
            _couponRepo = couponRepo;
            _userCouponRepo = userCouponRepo;
            _userRepo = userRepo;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _couponRepo.GetAllAsync();
            return View(result.Data);
        }

        public async Task<IActionResult> MyCoupon()
        {
            var userId = Convert.ToInt32(User.FindFirst("Id"));
            var userCoupon = await ((UserCouponRepository)_userCouponRepo).GetByUserIdAsync(userId);

            return View(userCoupon.Data);
        }

        [HttpPost]
        public async Task<JsonResult> CheckCouponCode(string couponCode)
        {
            //Check coupon exist
            var couponResult = await ((CouponRepository) _couponRepo).GetByCodeAsync(couponCode);

            if (!couponResult.IsSuccess)
            {
                return Json(new {success = false});
            }

            var coupon = couponResult.Data as Coupon;

            //Check coupon time
            if(coupon.StartAt > DateTime.Now || coupon.EndAt < DateTime.Now)
            {
                return Json(new { success = false });
            }

            //Check coupon for user
            var userId = Convert.ToInt32(User.FindFirst("Id"));
            var userCouponRs = await ((UserCouponRepository)_userCouponRepo).GetByUserIdAsync(userId);

            var userCoupons = userCouponRs.Data as List<UserCoupon>;

            if(!userCoupons.Any(x => x.CouponId == coupon.CouponId))
            {
                return Json(new { success = false });
            }

            //return info of coupon
            return Json(new { success = true}); //Data
        }

        [HttpPost]
        public async Task<JsonResult> TradeCoupon(int id)
        {
            var couponRs = await _couponRepo.GetByIdAsync(id);

            if(couponRs.IsSuccess)
            {
                return Json(new { success = false });
            }

            var coupon = couponRs.Data as Coupon;

            if(coupon.StartAt > DateTime.Now || coupon.EndAt < DateTime.Now)
            {
                return Json(new { success = false });
            }

            //Check remain

            var userId = Convert.ToInt32(User.FindFirst("Id"));
            var userRs = await _userRepo.GetByIdAsync(userId);
            var user = userRs.Data as User;

            if(user.UserCoupons.Any(x => x.CouponId == coupon.CouponId) 
                || user.Point < coupon.PointAmount)
            {
                return Json(new { success = false });
            }

            //Spent coupon

            return Json(new { success = true });
        }
    }
}
