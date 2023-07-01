using AutoServiceMVC.Models;
using AutoServiceMVC.Services;
using AutoServiceMVC.Services.System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoServiceMVC.ViewComponents
{
	public class CartComponent : ViewComponent
	{
		private readonly ISessionCustom _session;
		private readonly ICommonRepository<Product> _productRepo;

		public CartComponent(ISessionCustom session,
								ICommonRepository<Product> productRepo)
		{
			_session = session;
			_productRepo = productRepo;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var cart = _session.GetSessionValue<List<OrderDetail>>(HttpContext, "order_cart");

			if (cart != null)
			{
				foreach (var orderdetail in cart)
				{
					orderdetail.Product = (await _productRepo.GetByIdAsync(orderdetail.ProductId)).Data as Product;
				}
			}

			return View(cart);
		}
	}
}
