using AVS.Models.UserModels;
using AVS.Services;
using Microsoft.AspNetCore.Mvc;

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
                return RedirectToAction(nameof(Index), "Authorization");

            HttpContext.Response.Cookies.Append("something", token);

            return RedirectToAction(nameof(Index), "Auth");
        }

        public async Task<IActionResult> Authorizatuion()
        {
            return RedirectToAction(nameof(Index), "PersonalAccount");
        }
    }
}
