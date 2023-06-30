using AutoServiceMVC.Models;
using AutoServiceMVC.Services;
using AutoServiceMVC.Services.System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Cryptography.X509Certificates;

namespace AutoServiceMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "Admin_Scheme")]
    public class HomeController : Controller
    {
        private readonly ICommonRepository<Order> _orderRepo;
        private readonly ICommonRepository<Product> _productRepo;
        private readonly IMemoryCache _cache;

        public HomeController(
            ICommonRepository<Order> orderRepo,
            ICommonRepository<Product> productRepo,
            IMemoryCache cache
            )
        {
            _orderRepo = orderRepo;
            _productRepo = productRepo;
            _cache = cache;
        }

        public async Task<IActionResult> Index()
        {
            dynamic statistic = await GetStatistic();
            ViewData["amount"] = statistic.Item1;
            ViewData["totalOrder"] = statistic.Item2;
            ViewData["doneRadio"] = statistic.Item3;
            ViewData["processOrder"] = statistic.Item4;
            return View();
        }

        public async Task<JsonResult> GetOrderStatisticByMonth([FromQuery] int month)
        {
            List<string> labels = new List<string>();
            List<int> dataNum = new List<int>();
            List<int> cancelNum = new List<int>();

            var orderRs = await _orderRepo.GetAllAsync();
            if (orderRs.IsSuccess)
            {
                var orders = orderRs.Data as IEnumerable<Order>;

                var ordersByMoth = orders.Where(o => o.CreatedAt.Month == month);

                var numOfDayPerMonth = DateTime.DaysInMonth(DateTime.Now.Year, month);

                for(int day = 1; day <= numOfDayPerMonth; day++)
                {
                    var ordersByDay = ordersByMoth.Where(o => o.CreatedAt.Day == day);
                    var cancelByDay = ordersByDay.Where(o => o.Status.StatusId == 5);
                    labels.Add($"{day}/{month}");
                    dataNum.Add(ordersByDay.Count());
                    cancelNum.Add(cancelByDay.Count());
                }
            }

                var data = new
            {
                labels = labels,
                datasets = new List<dynamic>()
                {
                    new
                    {
                        label = "Number of cancel",
                        data = cancelNum,
                        borderColor = "rgb(255, 99, 132)",
                        tension = 0.5
                    },
                    new
                    {
                        label = "Number of order",
                        data = dataNum,
                        borderColor = "rgb(75, 192, 192)",
                        tension = 0.5
                    }
                }
            };

            return Json(new
            {
                success = true,
                data = data
            });
        }

        public async Task<JsonResult> GetTop10Products()
        {
            List<string> labels = new List<string>();
            List<int> productsData = new List<int>();

            var productRs = await _productRepo.GetAllAsync();

            if (productRs.IsSuccess)
            {
                var products = productRs.Data as IEnumerable<Product>;
                var top10products = products.OrderByDescending(p => p.SellerQuantity)
                                    .Take(10);
                foreach(var product in top10products)
                {
                    labels.Add(product.ProductName);
                    productsData.Add(product.SellerQuantity);
                }
            }

            var data = new
            {
                labels = labels,
                datasets = new List<dynamic>()
                {
                    new
                    {
                        label = "Number of order",
                        data = productsData,
                        backgroundColor = new List<string>()
                        {
                            "#DCE429","#63C97A","#FA2E5F","#7ED117","#C4BD30","#1CC3CB","#A967DD","#0B9A1E","#5C3D32","#9B2591"
                        }
                    }
                }
            };

            return Json(new
            {
                success = true,
                data = data
            });
        }

        public async Task<JsonResult> GetOrderStatusStatistic()
        {
            List<int> statusData = new List<int>();

            var orderRs = await _orderRepo.GetAllAsync();
            if (orderRs.IsSuccess)
            {
                var orders = orderRs.Data as IEnumerable<Order>;

                for(int statusId = 1; statusId <= 5; statusId++)
                {
                    statusData.Add(orders.Where(o => o.Status.StatusId == statusId).Count());
                }
            }

            var data = new
            {
                labels = new List<string>()
                        {
                            "Sent","Received","Processing","Done","Cancel"
                        },
                datasets = new List<dynamic>()
                {
                    new
                    {
                        label = "Number of order",
                        data = statusData,
                        backgroundColor = new List<string>()
                        {
                            "#FFC107","#4CAF50","#2196F3","#8BC34A","#FF5722"
                        }
                    }
                }
            };

            return Json(new
            {
                success = true,
                data = data
            });
        }

        public async Task<dynamic> GetStatistic()
        {
            int amount = 0, totalOrder = 0, processOrder = 0;
            double doneRadio = 0;

            var orderRs = await _orderRepo.GetAllAsync();
            if (orderRs.IsSuccess)
            {
                var orders = orderRs.Data as IEnumerable<Order>;
                amount = orders.Sum(o => o.Amount);
                totalOrder = orders.Count();
                processOrder = orders.Count(o => o.Status.StatusId < 4);
                var doneOrder = orders.Count(o => o.Status.StatusId == 4);
                var cancelOrder = orders.Count(o => o.Status.StatusId == 5);
                doneRadio = (doneOrder*1.0)/((doneOrder + cancelOrder) *1.0) * 100.0 ;
                doneRadio = Math.Round(doneRadio);
            }

            return (amount, totalOrder, doneRadio, processOrder);
        }
    }
}
