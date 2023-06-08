using AutoServiceMVC.Data;
using AutoServiceMVC.Hubs;
using AutoServiceMVC.Models;
using AutoServiceMVC.Services;
using AutoServiceMVC.Services.Implement;
using AutoServiceMVC.Services.System;
using AutoServiceMVC.Utils;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Net;
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
		private readonly ICommonRepository<User> _userRepo;
        private readonly ICommonRepository<UserCoupon> _userCouponRepo;
        private readonly IHubContext<HubServer> _hub;
        private readonly IPointService _pointService;
        private readonly ISessionCustom _session;
        private readonly ICookieService _cookie;
        private readonly IPaymentService _payment;

		public OrderController(AppDbContext dbContext,
                                ICommonRepository<Order> orderRepo,
                                ICommonRepository<OrderDetail> detailRepo,
                                ICommonRepository<OrderStatus> orderStatusRepo,
                                ICommonRepository<Product> productRepo,
                                ICommonRepository<User> userRepo,
                                ICommonRepository<UserCoupon> userCouponRepo,
                                IHubContext<HubServer> hub,
                                ISessionCustom session,
                                ICookieService cookie,
                                IPaymentService payment)
        {
            _dbContext = dbContext;
            _orderRepo = orderRepo;
            _detailRepo = detailRepo;
            _orderStatusRepo = orderStatusRepo;
            _productRepo = productRepo;
            _userRepo = userRepo;
            _userCouponRepo = userCouponRepo;
            _hub = hub;
            _session = session;
            _cookie = cookie;
            _payment = payment;

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

                TempData["Message"] = "Add table success";
                return View("Index");
            }

			TempData["Message"] = "Table code is wrong";
			return View("Index", "Home");
        }

        [HttpPost]
        public async Task<JsonResult> AccessTableApi(string tablecode)
        {
            _dbContext.ChangeTracker.LazyLoadingEnabled = false;
            var result = await _dbContext.Tables.FirstOrDefaultAsync(x => x.TableCode == tablecode);

            if (result != null)
            {
                _session.AddToSession(HttpContext, "table", result);

                return Json(new {success = true, message = "Add table success", name = result.TableName });
            }

            return Json(new { success = false, message = "Table code is wrong" });
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
        public async Task<IActionResult> Payment([Bind("PaymentMethodId,Note,ApplyCouponId")] Order order, string Total)
        {
            string strUserId = User.FindFirstValue("Id");
            int userId = strUserId.IsNullOrEmpty() ? 2 : Convert.ToInt32(strUserId); // 2 is Guest
            order.UserId = userId;

            var table = _session.GetSessionValue<Table>(HttpContext, "table");

            if(table == null)
            {
                TempData["Message"] = "Please enter TABLECODE before payment";
                return RedirectToAction("Payment");
            }

            var tableId = table.TableId;
            order.TableId = tableId;

            //Order details
            var detailsList = _session.GetSessionValue<List<OrderDetail>>(HttpContext, "order_cart");

            if (detailsList == null)
            {
                TempData["Message"] = "Please select some product";
                return RedirectToAction("Index");
            }

            _session.AddToSession(HttpContext, "doing_order", order);

            string redirectUrl = _payment.GetVnpayPaymentUrl(HttpContext, Convert.ToInt32(Total));
            return Redirect(redirectUrl);
        }

        public async Task<IActionResult> ConfirmVnpayPayment()
        {
            var paymentResult = _payment.CheckResponseVnpayPayment(HttpContext);

            if(paymentResult.status == true && paymentResult.responseCode == "00")
            {
                var order = _session.GetSessionValue<Order>(HttpContext, "doing_order");
                var detailsList = _session.GetSessionValue<List<OrderDetail>>(HttpContext, "order_cart");

				var result = await _orderRepo.CreateAsync(order);
                var orderid = (result.Data as Order).OrderId;

                //create order detail
                foreach (var detail in detailsList)
                {
                    detail.OrderId = orderid;
                    await _detailRepo.CreateAsync(detail);
                }

                //create order status
                await _orderStatusRepo.CreateAsync(new OrderStatus()
                {
                    OrderId = orderid,
                    StatusId = 1 //sended
                });

                var applyCouponId = order.ApplyCouponId;

                if(applyCouponId != null)
                {
                    var userId = Convert.ToInt32(User.FindFirstValue("Id"));
                    var userCoupons = (await ((UserCouponRepository)_userCouponRepo).GetByUserIdAsync(userId)).Data as List<UserCoupon>;
                    var usedCoupon = userCoupons.Find(c => c.CouponId == applyCouponId);

                    usedCoupon.IsUsed = true;
                    await _userCouponRepo.UpdateAsync(usedCoupon);
                }

                if(order.UserId == 2) //Guest
                {
                    var guestOrder = _cookie.GetCookie(HttpContext, "guest_order");
                    var orderIdList = guestOrder.IsNullOrEmpty() ? new List<string>() : guestOrder.Split(",").ToList();

                    orderIdList.Add(orderid.ToString());
                    _cookie.AddCookie(HttpContext, 1, "guest_order", String.Join(",", orderIdList));
                }

                _session.DeleteSession(HttpContext, "order_cart");
                _session.DeleteSession(HttpContext, "doing_cart");

                await _hub.Clients.Group("Employee").SendAsync("ReceiveOrder", $"New order at table {order.TableId}");

                TempData["Message"] = "Order successfully";
				return RedirectToAction("Index");
			}
            else
            {
				_session.DeleteSession(HttpContext, "doing_cart");

				TempData["Message"] = $"Payment fail. ERROR: {paymentResult.status} {paymentResult.responseCode}";
				return RedirectToAction("Payment");
			}
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
