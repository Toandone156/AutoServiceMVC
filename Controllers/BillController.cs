using AutoServiceMVC.Models;
using AutoServiceMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(int id)
        {
            var result = await _orderRepo.GetByIdAsync(id);

            if(result.IsSuccess)
            {
                return View(result);
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
