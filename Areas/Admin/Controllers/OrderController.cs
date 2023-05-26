﻿using AutoServiceMVC.Models;
using AutoServiceMVC.Services;
using AutoServiceMVC.Services.System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace AutoServiceMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "Admin_Scheme")]
    public class OrderController : Controller
    {
        private readonly ICommonRepository<Product> _productRepo;
        private readonly ICommonRepository<Order> _orderRepo;
        private readonly ICommonRepository<OrderDetail> _detailRepo;
        private readonly ICommonRepository<OrderStatus> _orderStatusRepo;
        private readonly ICommonRepository<Status> _statusRepo;
        private readonly ISessionCustom _session;

        public OrderController(ICommonRepository<Product> productRepo,
                                ICommonRepository<Order> orderRepo,
                                ICommonRepository<OrderDetail> detailRepo,
                                ICommonRepository<OrderStatus> orderStatusRepo,
                                ICommonRepository<Status> statusRepo,
                                ISessionCustom session)
        {
            _productRepo = productRepo;
            _orderRepo = orderRepo;
            _detailRepo = detailRepo;
            _orderStatusRepo = orderStatusRepo;
            _statusRepo = statusRepo;
            _session = session;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _productRepo.GetAllAsync();
            if (result.IsSuccess)
            {
                //Load orderDetail from session
                var cart = _session.GetSessionValue<List<OrderDetail>>(HttpContext, "cart");

                if(cart != null)
                {
                    foreach(var item in cart)
                    {
                        item.Product = await LoadProduct(item.ProductId);
                    }

                    ViewBag.Cart = cart;
                }

                var data = (result.Data as List<Product>).Where(p => p.IsInStock);

                return View(data);
            }

            return View();
        }

        [HttpPost]
        public async Task<JsonResult> AddToCart(int productId, int quantity)
        {
            List<OrderDetail> cart = _session.GetSessionValue<List<OrderDetail>>(HttpContext, "cart");
            var product = (await _productRepo.GetByIdAsync(productId)).Data as Product;

            var newDetail = new OrderDetail()
            {
                ProductId = productId,
                Quantity = quantity
            };

            if (cart == null)
            {
                //If new cart => create new list order
                cart = new List<OrderDetail>()
                {
                    newDetail
                };
            }
            else
            {
                //Find product
                var result = cart.FirstOrDefault(p => p.ProductId == productId);

                //Remove if exist
                if (result != null)
                {
                    cart.Remove(result);
                }

                //If quantity != 0 => Not delete product
                if (quantity != 0)
                {
                    cart.Add(newDetail);
                }
            }

            _session.AddToSession(HttpContext, "cart", cart);

            return Json(new { success = true, id = product.ProductId, name = product.ProductName, price = product.Price, quantity = newDetail.Quantity }); ;
        }

        [HttpPost]
        public async Task<IActionResult> Payment(string Note)
        {
            int employeeId = Convert.ToInt32(User.FindFirstValue("Id"));
            var tableId = 8; //Table for employee order
            var paymentMethodId = 2; //Cash

            //Order details
            var detailsList = _session.GetSessionValue<List<OrderDetail>>(HttpContext, "cart");

            if (detailsList == null)
            {
                Console.WriteLine("Order fail");
                return RedirectToAction("Index");
            }

            //Create Order
            Order order = new Order
            {
                EmployeeId = employeeId,
                TableId = tableId,
                Note = Note,
                PaymentMethodId = paymentMethodId
            };

            var result = await _orderRepo.CreateAsync(order);
            var orderId = (result.Data as Order).OrderId;

            //Create Order Detail
            foreach(var detail in detailsList)
            {
                detail.OrderId = orderId;
                await _detailRepo.CreateAsync(detail);
            }

            //Create Order Status
            await _orderStatusRepo.CreateAsync(new OrderStatus()
            {
                OrderId = orderId,
                EmployeeId = employeeId,
                StatusId = 1 //Sended
            });
            
            _session.DeleteSession(HttpContext, "cart");

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<JsonResult> UpdateStatus(int orderId)
        {
            var orderResult = await _orderRepo.GetByIdAsync(orderId);
            var recentStatusId = (orderResult.Data as Order).Status.StatusId;

            OrderStatus newStatus = new OrderStatus()
            {
                OrderId = orderId,
                StatusId = recentStatusId + 1,
                EmployeeId = Convert.ToInt32(User.FindFirstValue("id"))
            };

            await _orderStatusRepo.CreateAsync(newStatus);

            var newStatusObject = await _statusRepo.GetByIdAsync(recentStatusId + 1);
            var status = newStatusObject.Data as Status;

            return Json(new { success = true, statusId = status.StatusId, statusName = status.StatusName });
        }

        async Task<Product> LoadProduct(int productId)
        {
            var result = await _productRepo.GetByIdAsync(productId);
            return result.Data as Product;
        }
    }
}