using AVS.Models.AddressModels;
using AVS.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace AVS.Controllers
{
    //[Authorize(Policy = "UserPolicy")]
    public class PersonalAccountController : Controller
    {
        private UserRepository _userRepository;

        public PersonalAccountController(UserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public async Task <IActionResult> Index()
        {
            if(HttpContext.Request.Cookies.TryGetValue("something", out var jwtToken))
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(jwtToken);

                var userClaims = token.Claims.FirstOrDefault(user => user.Type == "user_id");
                if (userClaims == null)
                    return RedirectToAction(nameof(Index), "Auth");

                var user = await _userRepository.GetById(Guid.Parse(userClaims.Value));

                if (user == null)
                    return RedirectToAction(nameof(Index), "Auth");

                return View(user);
            }
            return RedirectToAction(nameof(Index), "Auth");
        }

        public IActionResult Message()
        {
            return View();
        }
    }
}
