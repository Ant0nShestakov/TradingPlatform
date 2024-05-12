using AVS.DB_Context;
using AVS.Models.UserModels;
using AVS.Repository;
using AVS.Services;
using Microsoft.AspNetCore.Mvc;

namespace AVS.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly UserService _userService = null!;

        public RegistrationController(UserService service) 
        {
            this._userService = service;
        }

        public IActionResult Index()
        {
            return View(new User());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(User newUser)
        {
            await _userService.Registration(newUser);
            return RedirectToAction(nameof(Index), "Auth");
        }
    }
}
