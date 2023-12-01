using IvanovaShop.Areas.Identity.Data;
using IvanovaShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IvanovaShop.Controllers
{
	public class LoginController : Controller
	{
		private readonly UserManager<IvanovaIdentityShopUser> _userManager;
		private readonly SignInManager<IvanovaIdentityShopUser> _signInManager;
		public LoginController(UserManager<IvanovaIdentityShopUser> userManager, SignInManager<IvanovaIdentityShopUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
                IvanovaIdentityShopUser user = new IvanovaIdentityShopUser
                { 
					Email = model.Email, 
					UserName = model.UserName,
					PhoneNumber=model.Phone,
					FirstName = model.FirstName,
					SecondName=model.SecondName,
					Patronimyc=model.Patronimyc
				};
				//// добавляем пользователя
				var result = await _userManager.CreateAsync(user, model.Password);
				if (result.Succeeded)
				{
					//	// установка куки
					await _signInManager.SignInAsync(user, false);
					return RedirectToAction("Index", "Home");
				}
				else
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}
				}
			}
			return View(model);
		}
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }
		[HttpGet]
		public IActionResult Profile()
		{
			return View();
		}
		
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
				
				var result =
                    await _signInManager.PasswordSignInAsync(model.Name, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    // проверяем, принадлежит ли URL приложению
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return View(model);
        }

		
		public async Task<IActionResult> Logout()
        {
			// удаляем аутентификационные куки
			
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
