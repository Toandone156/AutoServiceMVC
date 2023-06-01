using AutoServiceMVC.Models;
using AutoServiceMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace AutoServiceMVC.ViewComponents
{
    public class ProductComponent : ViewComponent
    {
        private readonly ICommonRepository<Product> _productRepo;

        public ProductComponent(ICommonRepository<Product> productRepo) 
        {
            _productRepo = productRepo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var rs = await _productRepo.GetAllAsync();
            var products = rs.Data as List<Product>;
            var data = products.Where(p => p.IsInStock);

            return View(data);
        }
    }
}
