﻿using AutoServiceMVC.Data;
using AutoServiceMVC.Models;
using AutoServiceMVC.Services;
using AutoServiceMVC.Services.System;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AutoServiceMVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly ICommonRepository<Order> _orderRepo;
        private readonly ICommonRepository<OrderDetail> _detailRepo;
        private readonly ICommonRepository<OrderStatus> _orderStatusRepo;
        private readonly ICommonRepository<Product> _productRepo;
        private readonly ISessionCustom _session;

        public OrderController(AppDbContext dbContext,
                                ICommonRepository<Order> orderRepo,
                                ICommonRepository<OrderDetail> detailRepo,
                                ICommonRepository<OrderStatus> orderStatusRepo,
                                ICommonRepository<Product> productRepo,
                                ISessionCustom session)
        {
            _dbContext = dbContext;
            _orderRepo = orderRepo;
            _detailRepo = detailRepo;
            _orderStatusRepo = orderStatusRepo;
            _productRepo = productRepo;
            _session = session;
        }   
        
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AccessTable([FromQuery] string tablecode)
        {
            _dbContext.ChangeTracker.LazyLoadingEnabled = false;
            var result = await _dbContext.Tables.FirstOrDefaultAsync(x => x.TableCode == tablecode);

            if (result != null)
            {
                _session.AddToSession(HttpContext, "table", result);

                return View("Index");
            }

            return View("Index", "Home");
        }

        public async Task<IActionResult> Payment()
        {
            var cart = _session.GetSessionValue<List<OrderDetail>>(HttpContext, "order_cart");

            if(cart != null)
            {
				foreach (var orderdetail in cart)
				{
					orderdetail.Product = (await _productRepo.GetByIdAsync(orderdetail.ProductId)).Data as Product;
				}
			}

            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> Payment([Bind("PaymentMethodId,Note")] Order order, string CouponCode) 
            //Check coupon using api and save in input for apply coupon
        {
            string strUserId = User.FindFirstValue("Id");
            int userId = strUserId.IsNullOrEmpty() ? 2 : Convert.ToInt32(strUserId); // 2 is Guest
            order.UserId = userId;

            var table = _session.GetSessionValue<Table>(HttpContext, "table");

            if(table == null)
            {
                TempData["Message"] = "Please enter order before payment";
                return RedirectToAction("Payment");
            }

            var tableId = table.TableId;
            order.TableId = tableId;

            //Order details
            var detailsList = _session.GetSessionValue<List<OrderDetail>>(HttpContext, "order_cart");

            if (detailsList == null)
            {
                TempData["Message"] = "Please select some product first";
                return RedirectToAction("Index");
            }

            var result = await _orderRepo.CreateAsync(order);
            var orderId = (result.Data as Order).OrderId;

            //Create Order Detail
            foreach (var detail in detailsList)
            {
                detail.OrderId = orderId;
                await _detailRepo.CreateAsync(detail);
            }

            //Create Order Status
            await _orderStatusRepo.CreateAsync(new OrderStatus()
            {
                OrderId = orderId,
                StatusId = 1 //Sended
            });

            //Payment by Momo or VNPay

            _session.DeleteSession(HttpContext, "cart");

            //Receive point

            TempData["Message"] = "Send order success";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<JsonResult> AddToCart(int productId, int quantity)
        {
            List<OrderDetail> cart = _session.GetSessionValue<List<OrderDetail>>(HttpContext, "order_cart");
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

                //If exist in cart => remove -> prepare to add new
                if (result != null)
                {
                    cart.Remove(result);
				}

                //If quantity != 0 => add new
                if(quantity != 0)
				{
					cart.Add(newDetail);
				}
            }

            _session.AddToSession(HttpContext, "order_cart", cart);

            return Json(new { success = true, id = product.ProductId, name = product.ProductName, price = product.Price, quantity = newDetail.Quantity }); ;
        }
    }
}
