using AVS.Models.UserModels;
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
            if(!ModelState.IsValid)
                return RedirectToAction(nameof(Index));

            await _userService.Registration(newUser);
            return RedirectToAction(nameof(AuthController.Index), "Auth");
        }
    }
}
