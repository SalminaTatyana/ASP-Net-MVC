using IvanovaShop;
using IvanovaShop.Areas.Identity.Data;
using IvanovaShop.Models;
using MerchShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace MerchShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private List<ProductModel> _product;
        private IvanovaShopContext _db;
        private readonly UserManager<IvanovaIdentityShopUser> _userManager;
        private readonly SignInManager<IvanovaIdentityShopUser> _signInManager;
        public HomeController(ILogger<HomeController> logger, IvanovaShopContext db, UserManager<IvanovaIdentityShopUser> userManager, SignInManager<IvanovaIdentityShopUser> signInManager)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]

        public IActionResult Index(string sort, string search, string filter,int category,string color,int minPrice, int maxPrice,int size, string material,bool? gender, double? weight, string productType,string lockType,string genre,string appointment, DateTime? year)
        {
            //sort = "price";
            // sort = "priceDesc";
            // sort = "name";
            // sort = "nameDesc";

            if (HttpContext.Session.Get<Basket>("Basket") == null)
                HttpContext.Session.Set<Basket>("Basket", new Basket());
            AllProductsData model = new AllProductsData();
            
           
            model.ChooseFilters = new List<ChooseFilter>();
			var products = _db.Products.AsQueryable();//select * from dbo.Products
            if (!string.IsNullOrWhiteSpace(sort))
            {
                switch (sort)
                {
                    case "price":
                        products = products.OrderBy(column => column.Price);//select * from dbo.Products order by price
                        model.SortPrice = 1;
                        break;
                    case "priceDesc":
                        products = products.OrderByDescending(column => column.Price);//select * from dbo.Products order by price desc
                        model.SortPrice = -1;
                        break;
                    case "name":
                        products = products.OrderBy(column => column.Name);//select * from dbo.Products order by price
                        model.SortName = 1;
                        break;
                    case "nameDesc":
                        products = products.OrderByDescending(column => column.Name);//select * from dbo.Products order by price desc
                        model.SortName = -1;
                        break;
                    default:
                        break;
                }
            }
			if (category > 0)
			{
						products = products.Where(s => s.ProductCtegoryId== category);//select * from dbo.Products order by price
						model.ChooseCategory = category;
			}
			if (!string.IsNullOrWhiteSpace(color))
			{
				products = products.Where(s => s.Color == color);
                model.ChooseFilters.Add(new ChooseFilter { Name="Цвет",Value= color ,ValueType="string"});
			}
			if (minPrice>0)
			{
				products = products.Where(s => s.Price > minPrice);
				model.ChooseFilters.Add(new ChooseFilter { Name = "Минимальная цена", ValueInt = minPrice, ValueType = "int" });
			}
			if (maxPrice > 0)
			{
				products = products.Where(s => s.Price < maxPrice);
				model.ChooseFilters.Add(new ChooseFilter { Name = "Максимальная цена", ValueInt = maxPrice, ValueType = "int" });
			}
			if (size > 0)
			{
				products = products.Where(s => s.Size == size);
				model.ChooseFilters.Add(new ChooseFilter { Name = "Размер", ValueInt = size, ValueType = "int" });
			}
			
			if (!string.IsNullOrWhiteSpace(material))
			{
				products = products.Where(s => s.Material == material);
				model.ChooseFilters.Add(new ChooseFilter { Name = "Материал", Value = material, ValueType = "string" });
			}
			if (gender!=null)
			{
				products = products.Where(s => s.Gender == gender);
				model.ChooseFilters.Add(new ChooseFilter { Name = "Пол", ValueBool = gender.Value, ValueType = "bool" });
			}
			if (weight!=null&&weight > 0)
			{
				products = products.Where(s => s.Weight == weight);
				model.ChooseFilters.Add(new ChooseFilter { Name = "Вес", ValueDouble = weight.Value, ValueType = "double" });
			}
			if (!string.IsNullOrWhiteSpace(productType))
			{
				products = products.Where(s => s.Type == productType);
				model.ChooseFilters.Add(new ChooseFilter { Name = "Тип", Value = productType, ValueType = "string" });
			}
			if (!string.IsNullOrWhiteSpace(lockType))
			{
				products = products.Where(s => s.LockType == lockType);
				model.ChooseFilters.Add(new ChooseFilter { Name = "Тип замка", Value = lockType, ValueType = "string" });
			}
			if (!string.IsNullOrWhiteSpace(genre))
			{
				products = products.Where(s => s.Genre == genre);
				model.ChooseFilters.Add(new ChooseFilter { Name = "Жанр", Value = genre, ValueType = "string" });
			}
			if (!string.IsNullOrWhiteSpace(appointment))
			{
				products = products.Where(s => s.Appointment == appointment);
				model.ChooseFilters.Add(new ChooseFilter { Name = "Назначение", Value = appointment, ValueType = "string" });
			}
			if (year!=null)
			{
				products = products.Where(s => s.Year.Value.ToString().Contains(year.Value.Year.ToString()));
				model.ChooseFilters.Add(new ChooseFilter { Name = "Год", ValueDate = year.Value, ValueType = "DateTime" });
			}
			if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();//set @search = '%Давид%'
                if (search.Contains("  "))
                {
                    search = search.Replace("  ", " ");
                }
                products = products.Where(item => item.Name.Contains(search));//select * from dbo.Products where Name like @search
                model.Search = search;                                                            //select * from dbo.Products where Name = @search order by price
            }
            var categories = _db.Categories.ToList();
            model.Products = products.ToList();
            List<int> categoriesId = new List<int>();
            foreach (var item in model.Products) { 
                categoriesId.Add(item.ProductCtegoryId);
            }
            model.Categories = categories.Where(s=> categoriesId.Contains(s.Id)).ToList();
            model.Filters = new List<Filter>();


            if (model.Products.Where(s => s.Color != null).FirstOrDefault() != null)
			{

                model.Filters.Add(new Filter() { Name = "Цвет", Value = model.Products.Where(s => s.Color!=null).OrderBy(s => s.Color).Select(s => s.Color).ToList(), ValueType = "string" });
            }
            if (model.Products.Where(s => s.Price != null).FirstOrDefault() != null)
            {
                model.Filters.Add(new Filter() { Name = "Цена", ValueInt = model.Products.Where(s => s.Price != null).OrderBy(s => s.Price).Select(s => s.Price).ToList(), ValueType = "int" });
            }
            if (model.Products.Where(s => s.Size != null).FirstOrDefault() != null)
            {
                model.Filters.Add(new Filter() { Name = "Размер", ValueInt = model.Products.Where(s => s.Size != null).OrderBy(s => s.Size).Select(s=>s.Size.Value).ToList(), ValueType = "int" });
            }
            if (model.Products.Where(s => !string.IsNullOrWhiteSpace(s.Material)).FirstOrDefault() != null)
            {
                model.Filters.Add(new Filter() { Name = "Материал", Value = model.Products.Where(s => !string.IsNullOrWhiteSpace(s.Material)).OrderBy(s => s.Material).Select(s => s.Material).ToList(), ValueType = "string" });
            }
            if (model.Products.Where(s => s.Gender != null).FirstOrDefault() != null)
            {
                model.Filters.Add(new Filter() { Name = "Пол", ValueBool = model.Products.Where(s => s.Gender != null).OrderBy(s => s.Gender).Select(s => s.Gender.Value).ToList(), ValueType = "int" });
            }
            if (model.Products.Where(s => s.Weight != null).FirstOrDefault() != null)
            {
                model.Filters.Add(new Filter() { Name = "Вес", ValueDouble = model.Products.Where(s => s.Weight != null).OrderBy(s => s.Weight).Select(s => s.Weight.Value).ToList(), ValueType = "float" });
            }
            if (model.Products.Where(s => s.Type != null).FirstOrDefault() != null)
            {
                model.Filters.Add(new Filter() { Name = "Тип", Value = model.Products.Where(s => s.Type != null).OrderBy(s => s.Type).Select(s => s.Type).ToList(), ValueType = "string" });
            }
            if (model.Products.Where(s => s.LockType != null).FirstOrDefault() != null)
            {
                model.Filters.Add(new Filter() { Name = "Тип замка", Value = model.Products.Where(s => s.LockType != null).OrderBy(s => s.LockType).Select(s=>s.LockType).ToList(), ValueType = "string" });
            }
            if (model.Products.Where(s => s.Genre != null).FirstOrDefault() != null)
            {
                model.Filters.Add(new Filter() { Name = "Жанр", Value = model.Products.Where(s => s.Genre != null).OrderBy(s => s.Genre).Select(s => s.Genre).ToList(), ValueType = "string" });
            }
            if (model.Products.Where(s => s.Appointment != null).FirstOrDefault() != null)
            {
                model.Filters.Add(new Filter() { Name = "Назначение", Value = model.Products.Where(s => s.Appointment != null).OrderBy(s => s.Appointment).Select(s => s.Appointment).ToList(), ValueType = "string" });
            }
            if (model.Products.Where(s => s.Year != null).FirstOrDefault() != null)
            {
                model.Filters.Add(new Filter() { Name = "Год", ValueDate = model.Products.Where(s => s.Year != null).OrderBy(s => s.Year).Select(s => s.Year.Value).ToList(), ValueType = "Data" });
            }
            model.basket = HttpContext.Session.Get<Basket>("Basket");
            return View(model);
        }
        public async Task<IActionResult> Product(int id)
        {
            if (HttpContext.Session.Get<Basket>("Basket") == null)
                HttpContext.Session.Set<Basket>("Basket", new Basket());
            var product = await _db.Products.Where(s => s.Id == id).SingleOrDefaultAsync();
            if (product != null)
            {
                ProductModel model = new ProductModel() { Product = product, Result = 0, basket = HttpContext.Session.Get<Basket>("Basket") };
                return View(model);
            }
            else
            {
                ProductModel model = new ProductModel() { Result = -1, Message = "Товар не найден" };

                return View(model);

            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}