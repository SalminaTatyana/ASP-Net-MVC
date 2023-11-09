using IvanovaShop.Models;
using MerchShop.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.ContentModel;

namespace IvanovaShop.Controllers
{
	public class BasketController : Controller
	{
		private readonly ILogger<BasketController> _logger;
		private List<ProductModel> _product;
		private IvanovaShopContext _db;
		private Basket _basket;
		public BasketController(ILogger<BasketController> logger, IvanovaShopContext db)
		{
			_logger = logger;
			_db = db;
		}
		public IActionResult Index()
		{
			if (HttpContext.Session.Get<Basket>("Basket") == null)
				HttpContext.Session.Set<Basket>("Basket", new Basket());
			_basket = HttpContext.Session.Get<Basket>("Basket");
			return View(_basket);
		}
		public IActionResult Order()
		{
			if (HttpContext.Session.Get<Basket>("Basket") == null)
				HttpContext.Session.Set<Basket>("Basket", new Basket());
			_basket = HttpContext.Session.Get<Basket>("Basket");
			return View(_basket);
		}
		public IActionResult Buy(int id)
		{
			var product = _db.Products.Where(s => s.Id == id).SingleOrDefault();
			Basket basket = new Basket()
			{
				Items = new List<BasketItem>()
				{
					new BasketItem(){ Product=product,
					Count=1,
					Sum=product.Price}
				},
				Sum=product.Price
				
			};
			return View(_basket);
		}
		[HttpPost]
		public IActionResult Finaly(string firstName, string lastName, string patrinymic, string email, string tel)
		{
			if (HttpContext.Session.Get<Basket>("Basket") == null)
				HttpContext.Session.Set<Basket>("Basket", new Basket());
			_basket = HttpContext.Session.Get<Basket>("Basket");
			HttpContext.Session.Set<Basket>("Basket", new Basket());
			var model = new FinalViewModel()
			{
				Basket = _basket,
				FirstName = firstName,
				LastName = lastName,
				Patronymic = patrinymic,
				Email = email,
				Tel = tel
			};
			return PartialView(model);
		}
		[HttpPost]
		public async void AddInBasket(int id)
		{

			var product = _db.Products.Where(s => s.Id == id).SingleOrDefault();
			if (HttpContext.Session.Get<Basket>("Basket") == null)
				HttpContext.Session.Set<Basket>("Basket", new Basket());
			_basket = HttpContext.Session.Get<Basket>("Basket");

			if (product != null)
			{
				_basket.Add(product);
			}
			HttpContext.Session.Set<Basket>("Basket", _basket);
		}
		[HttpPost]
		public async void ChageInBasketPage(int id, int count)
		{

			var product = _db.Products.Where(s => s.Id == id).SingleOrDefault();
			if (HttpContext.Session.Get<Basket>("Basket") == null)
				HttpContext.Session.Set<Basket>("Basket", new Basket());
			_basket = HttpContext.Session.Get<Basket>("Basket");

			if (product != null)
			{
				var basketItemCount = GetBasketItemCount(id);
				var lenght = basketItemCount - count > 0 ? basketItemCount - count : count - basketItemCount;
				for (int i = 0; i < lenght; i++)
				{
					_basket.Add(product);

				}
			}
			HttpContext.Session.Set<Basket>("Basket", _basket);
		}
		[HttpPost]
		public async void RemoveOutBasket(int id)
		{
			var product = _db.Products.Where(s => s.Id == id).SingleOrDefault();
			if (HttpContext.Session.Get<Basket>("Basket") == null)
				HttpContext.Session.Set<Basket>("Basket", new Basket());
			_basket = HttpContext.Session.Get<Basket>("Basket");
			if (product != null)
			{
				_basket.Delete(product);
			}
			HttpContext.Session.Set<Basket>("Basket", _basket);

		}
		[HttpGet]
		public async void ClearBasket()
		{
			if (HttpContext.Session.Get<Basket>("Basket") == null)
				HttpContext.Session.Set<Basket>("Basket", new Basket());
			_basket = HttpContext.Session.Get<Basket>("Basket");
			_basket.Clear();
			HttpContext.Session.Set<Basket>("Basket", _basket);

		}
		[HttpGet]
		public string GetBasketCount()
		{
			if (HttpContext.Session.Get<Basket>("Basket") == null)
				HttpContext.Session.Set<Basket>("Basket", new Basket());
			_basket = HttpContext.Session.Get<Basket>("Basket");
			return
				$"<span class=\"main-counter\">{_basket.GetBasketCount()}</span> <span id=\"basketSum\">{_basket.Sum}</span>";
		}
		[HttpPost]
		public string AddInBasketView(int id)
		{

			AddInBasket(id);
			var product = _db.Products.Where(s => s.Id == id).SingleOrDefault();
			if (product != null)
			{
				var productCount = _basket.ProductCount(product);
				if (productCount > 0)
				{
					return $"<div class=\"button-wrapper-row\"><button onclick=\"removeOutBasket({id})\" class=\"primary-btn\">-</button><p class=\"button-basket-text\">Уже в корзине {productCount}</p><button  class=\"primary-btn\" onclick=\"addInBasket({id})\">+</button></div>";
				}
			}
			return $"<button class=\"primary-btn\"  type=\"button\" onclick=\"addInBasket({id})\">В корзину</button>\r\n <button class=\"primary-btn\">Купить сейчас</button>";

		}
		[HttpPost]
		public string RemoveOutBasketView(int id)
		{
			RemoveOutBasket(id);
			var product = _db.Products.Where(s => s.Id == id).SingleOrDefault();
			if (product != null)
			{
				var productCount = _basket.ProductCount(product);
				if (productCount > 0)
				{
					return $"<div class=\"button-wrapper-row\"><button onclick=\"removeOutBasket({id})\" class=\"primary-btn\">-</button><p class=\"button-basket-text\">Уже в корзине {productCount}</p><button  class=\"primary-btn\" onclick=\"addInBasket({id})\">+</button></div>";
				}
			}
			return $"<button class=\"primary-btn\"  type=\"button\" onclick=\"addInBasket({id})\">В корзину</button>\r\n <button class=\"primary-btn\">Купить сейчас</button>";


		}
		[HttpPost]
		public int GetBasketItemCount(int id)
		{
			var product = _db.Products.Where(s => s.Id == id).SingleOrDefault();
			if (product != null)
			{
				var productCount = _basket.ProductCount(product);
				return productCount;
			}
			return 0;
		}
		[HttpPost]
		public void ClearItem(int id)
		{
			if (HttpContext.Session.Get<Basket>("Basket") == null)
				HttpContext.Session.Set<Basket>("Basket", new Basket());
			_basket = HttpContext.Session.Get<Basket>("Basket");
			var product = _db.Products.Where(s => s.Id == id).SingleOrDefault();
			if (product != null)
			{
				_basket.ClearItem(product);
				HttpContext.Session.Set<Basket>("Basket", _basket);

			}
		}
		[HttpPost]
		public IActionResult ClearItemView(int id)
		{
			ClearItem(id);
			if (HttpContext.Session.Get<Basket>("Basket") == null)
				HttpContext.Session.Set<Basket>("Basket", new Basket());
			_basket = HttpContext.Session.Get<Basket>("Basket");
			return PartialView("_partialBasketItems", _basket);
		}
		[HttpGet]
		public int GetTotalSumBasket()
		{
			if (HttpContext.Session.Get<Basket>("Basket") == null)
				HttpContext.Session.Set<Basket>("Basket", new Basket());
			_basket = HttpContext.Session.Get<Basket>("Basket");
			return _basket.Sum;

		}
	}
}
