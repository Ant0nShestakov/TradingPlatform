using AVS.Models.UserModels;
using AVS.Services;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace AVS.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserService _userService = null!;

        public AuthController(UserService userService) 
        {
            this._userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Request.Cookies.TryGetValue("something", out var jwtToken))
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(jwtToken);

                var userClaims = token.Claims.FirstOrDefault(user => user.Type == "user_id");
                if (userClaims != null)
                {
                    var user = await _userService.GetUserById(Guid.Parse(userClaims.Value));
                    if (user != null)
                        return RedirectToAction(nameof(PersonalAccountController.Index), "PersonalAccount");
                }
            }

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
