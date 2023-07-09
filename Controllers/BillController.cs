using AutoServiceMVC.Hubs;
using AutoServiceMVC.Models;
using AutoServiceMVC.Services;
using AutoServiceMVC.Services.Implement;
using AutoServiceMVC.Services.System;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace AutoServiceMVC.Controllers
{
    public class BillController : Controller
    {
        private readonly ICommonRepository<Order> _orderRepo;
        private readonly ICommonRepository<OrderStatus> _orderStatusRepo;
        private readonly IHubContext<HubServer> _hub;
        private readonly ICookieService _cookie;

        public BillController(ICommonRepository<Order> orderRepo,
                                ICommonRepository<OrderStatus> orderStatusRepo,
                                IHubContext<HubServer> hub,
                                ICookieService cookie)
        {
            _orderRepo = orderRepo;
            _orderStatusRepo = orderStatusRepo;
            _hub = hub;
            _cookie = cookie;
        }
        public async Task<IActionResult> Index()
        {
            var orders = new List<Order>();
            if (User.Identity.IsAuthenticated && User.Identity.AuthenticationType == "User_Scheme")
            {
                var userId = Convert.ToInt32(User.FindFirstValue("Id"));
                var userOrdersRs = await ((OrderRepository)_orderRepo).GetByUserIdAsync(userId);

                if (userOrdersRs.IsSuccess)
                {
                    orders = userOrdersRs.Data as List<Order>;
                }
            }
            else
            {
                var guestOrderIdCookie = _cookie.GetCookie(HttpContext, "guest_order");
                if (!guestOrderIdCookie.IsNullOrEmpty())
                {
                    var guestOrderIdList = guestOrderIdCookie.Split(",").ToList();
                    foreach (var orderIdString in guestOrderIdList)
                    {
                        int orderId = Convert.ToInt32(orderIdString ?? "0");
                        var order = await _orderRepo.GetByIdAsync(orderId);

                        if (order.IsSuccess)
                        {
                            orders.Add(order.Data as Order);
                        }
                    }
                }
            }

            var orderduserOrders = orders?.OrderByDescending(o => o.CreatedAt);
            return View(orderduserOrders);
        }

        public async Task<IActionResult> GetOrderData()
        {
            var orders = new List<Order>();
            if (User.Identity.IsAuthenticated && User.Identity.AuthenticationType == "User_Scheme")
            {
                var userId = Convert.ToInt32(User.FindFirstValue("Id"));
                var userOrdersRs = await ((OrderRepository)_orderRepo).GetByUserIdAsync(userId);

                if (userOrdersRs.IsSuccess)
                {
                    orders = userOrdersRs.Data as List<Order>;
                }
            }
            else
            {
                var guestOrderIdCookie = _cookie.GetCookie(HttpContext, "guest_order");
                if (!guestOrderIdCookie.IsNullOrEmpty())
                {
                    var guestOrderIdList = guestOrderIdCookie.Split(",").ToList();
                    foreach (var orderIdString in guestOrderIdList)
                    {
                        int orderId = Convert.ToInt32(orderIdString ?? "0");
                        var order = await _orderRepo.GetByIdAsync(orderId);

                        if (order.IsSuccess)
                        {
                            orders.Add(order.Data as Order);
                        }
                    }
                }
            }

            var orderduserOrders = orders?.OrderByDescending(o => o.CreatedAt);
            return PartialView("_OrderData", orderduserOrders);
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

                if(status.StatusId < 3) // Can cancel if in receive
                {
                    await _orderStatusRepo.CreateAsync(new OrderStatus()
                    {
                        OrderId = id,
                        StatusId = 5
                    });

                    _hub.Clients.Group("Employee").SendAsync("ReceiveNoti", $"Order {id} has been canceled", 5);

                    TempData["Message"] = "Cancel order success";
                    return RedirectToAction("Index");
                }

                TempData["Message"] = "Your order was prepared. Cancel fail.";
            }

            return RedirectToAction("Details", new {id = id });
        }
    }
}
