using AVS.Models.UserModels;
using AVS.Services;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace AVS.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly UserService _userService = null!;

        public RegistrationController(UserService service) 
        {
            this._userService = service;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(User newUser)
        {
            if(!ModelState.IsValid)
                return RedirectToAction(nameof(Index));

            newUser.Email = newUser.Email.ToLower();

            var user = await _userService.GetUserByEmail(newUser.Email);
            if (user is not null) 
                return RedirectToAction(nameof(Index));

            await _userService.Registration(newUser);
            return RedirectToAction(nameof(AuthController.Index), "Auth");
        }
    }
}
