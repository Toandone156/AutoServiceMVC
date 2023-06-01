using AutoServiceMVC.Models;
using AutoServiceMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace AutoServiceMVC.ViewComponents
{
    public class BestSellerComponent : ViewComponent
    {
        private readonly ICommonRepository<Product> _productRepo;

        public BestSellerComponent(ICommonRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var rs = await _productRepo.GetAllAsync();
            var products = rs.Data as List<Product>;

            var data = products.Where(x => x.IsInStock)
                                .OrderByDescending(x => x.ProductRating)
                                .ThenByDescending(x => x.Price)
                                .Take(4)
                                .ToList();

            return View(data);
        }
    }
}
