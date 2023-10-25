using IvanovaShop.Models;
using MerchShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MerchShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private List<ProductModel> _product;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _product = new List<ProductModel>()
            {
                new ProductModel(1,"Футболка Белая Давид",1000,"David.jpg","Одежда","Ткань «двухнитка» — тепло в холодное время года и не жарко в теплые дни. Модель свободной посадки (оверсайз) выглядит комфортно и стильно."),
                new ProductModel(2,"Футболка Черная Давид",1500,"tshortBlack.jpg","Одежда","Черная футболка David из мягкого натурального премиального хлопка. Модель выполнена в однотонной расцветке, имеет традиционный силуэт, прямой крой и круглый эластичный ворот. На груди и спине расположены принты статуи Микеланджело - Давид в образе скейтбордиста."),
                new ProductModel(3,"Футболка Черная Логотип",1200,"logoBlack.jpg","Одежда"),
                new ProductModel(4,"Футболка Белая Логотип",1300,"LogoWhite.jpg","Одежда"),
                new ProductModel(5,"Футболка Белая MSF",1400,"1.jpg","Одежда"),
                new ProductModel(6,"Футболка Черная MSF",1100,"1.jpg","Одежда"),
                new ProductModel(7,"Брелока квадрат",200,"1.jpg","Аксессуары"),
                new ProductModel(8,"Брелока прямоугольник",300,"1.jpg","Аксессуары"),
                new ProductModel(9,"Брелока круг",320,"1.jpg","Аксессуары"),
                new ProductModel(10,"Брелока сердце",400,"1.jpg","Аксессуары"),
                new ProductModel(11,"Набор наклеек А4",150,"1.jpg","Наклейки"),
                new ProductModel(12,"Набор наклеек А5",120,"1.jpg","Наклейки"),
                new ProductModel(13,"Наклейки MSF",50,"1.jpg","Наклейки"),
                new ProductModel(14,"Набор виниловых наклеек",50,"1.jpg","Наклейки"),
                new ProductModel(15,"Музыкальный диск Гештальт",50,"1.jpg","Музыка"),
            };
        }

        public IActionResult Index()
        {
            return View(_product);
        }
        public IActionResult Product(int id)
        {
            ProductModel product = _product.FirstOrDefault(x => x.Id == id);
            if (product!=null)
            {
                product.Result =0;
                return View(product);
            }
            else
            {
                product = new ProductModel();

				product.Result = -1;
                product.Message = "Товар не найден";
                return View(product);

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