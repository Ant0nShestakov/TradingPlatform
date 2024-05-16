using AVS.Models.UserModels;
using AVS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AVS.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserService _userService = null!;

        public AuthController(UserService userService) 
        {
            this._userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Authentification(User user) 
        {
                
            var token = await _userService.Login(user);
            if (token == null)
            {
                TempData["Error"] = "Не верный логин/пароль";
                return RedirectToAction(nameof(Index));
            }

            HttpContext.Response.Cookies.Append("something", token);

            return RedirectToAction(nameof(Index), "PersonalAccount");
        }

    }
}
