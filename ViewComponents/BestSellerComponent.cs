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

        public async Task<IViewComponentResult> InvokeAsync(List<int> favoriteIds)
        {
            var rs = await _productRepo.GetAllAsync();
            var products = rs.Data as List<Product>;

            var data = products.OrderByDescending(x => x.SellerQuantity)
                                .ThenByDescending(x => x.Price)
                                .Take(4)
                                .ToList();

            foreach(var product in data)
            {
                product.Favorite = favoriteIds.Any(id => id == product.ProductId);
            }

            return View(data);
        }
    }
}
