using AutoServiceMVC.Models;
using AutoServiceMVC.Services;
using AutoServiceMVC.Services.Implement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AutoServiceMVC.Controllers
{
    [Authorize(AuthenticationSchemes = "User_Scheme")]
    public class BillController : Controller
    {
        private readonly ICommonRepository<Order> _orderRepo;
        private readonly ICommonRepository<OrderStatus> _orderStatusRepo;

        public BillController(ICommonRepository<Order> orderRepo,
                                ICommonRepository<OrderStatus> orderStatusRepo)
        {
            _orderRepo = orderRepo;
            _orderStatusRepo = orderStatusRepo;
        }
        public async Task<IActionResult> Index()
        {
            var userId = Convert.ToInt32(User.FindFirstValue("Id"));
            var userOrdersRs = await ((OrderRepository) _orderRepo).GetByUserIdAsync(userId);
            
            if(userOrdersRs.IsSuccess)
            {
                var userOrders = userOrdersRs.Data as List<Order>;
                var orderduserOrders = userOrders.OrderByDescending(o => o.CreatedAt);

                return View(orderduserOrders);
            }

            TempData["Message"] = "Can not get order list.";
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _orderRepo.GetByIdAsync(id);

            if(result.IsSuccess)
            {
                return View(result.Data);
            }

            return View("Index", "Home");
        }

        public async Task<IActionResult> CancelOrder(int id)
        {
            var result = await _orderRepo.GetByIdAsync(id);

            if(result.IsSuccess)
            {
                var status = (result.Data as Order).Status;

                if(status.StatusId > 2) // Can cancel if in receive
                {
                    await _orderStatusRepo.CreateAsync(new OrderStatus()
                    {
                        OrderId = id,
                        StatusId = 5
                    });

                    TempData["Message"] = "Cancel order success";
                    return View("Index");
                }
            }

            return View("Detail", id);
        }
    }
}
