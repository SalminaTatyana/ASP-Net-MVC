using IvanovaShop;
using IvanovaShop.Models;
using MerchShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace MerchShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private List<ProductModel> _product;
        private IvanovaShopContext _db;
        public HomeController(ILogger<HomeController> logger, IvanovaShopContext db)
        {
            _logger = logger;
            _db = db;
           
        }

        public IActionResult Index()
        {
			if (HttpContext.Session.Get<Basket>("Basket") == null)
				HttpContext.Session.Set<Basket>("Basket", new Basket());
			var products = _db.Products.ToList();
            var categories = _db.Categories.ToList();
            AllProductsData model = new AllProductsData() { Products=products,Categories=categories,basket= HttpContext.Session.Get<Basket>("Basket") };
            return View(model);
        }
        public async Task<IActionResult> Product(int id)
        {
            var product =  await _db.Products.Where(s=>s.Id==id).SingleOrDefaultAsync();
            if (product!=null)
            {
                ProductModel model = new ProductModel() {Product= product, Result = 0 };
                return View(model);
            }
            else
            {
                ProductModel model = new ProductModel() { Result = -1, Message = "Товар не найден" };

                return View(model);

            }
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}