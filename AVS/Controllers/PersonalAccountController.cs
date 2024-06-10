using AVS.Models.AdvertisementModels;
using AVS.Models.UserModels;
using AVS.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;

namespace AVS.Controllers
{
    public class PersonalAccountController : Controller
    {
        private UserRepository _userRepository;
        private AdvertisementRepository _advertisementRepository;

        public PersonalAccountController(UserRepository userRepository, 
            AdvertisementRepository advertisementRepository)
        {
            this._userRepository = userRepository;
            _advertisementRepository = 
            _advertisementRepository = advertisementRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!HttpContext.Request.Cookies.TryGetValue("something", out var jwtToken))
                return RedirectToAction(nameof(Index), "Auth");

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);

            var userClaims = token.Claims.FirstOrDefault(user => user.Type == "user_id");
            if (userClaims == null)
                return RedirectToAction(nameof(Index), "Auth");

            var userId = Guid.Parse(userClaims.Value);

            var user = await _userRepository.GetByIdInclude(userId);

            if (user == null)
                return RedirectToAction(nameof(Index), "Auth");

            var allMessages = user.MessagesReceive
                .Concat(user.MessagesSent)
                .ToList();

            var groupedMessages = allMessages
                .GroupBy(m => m.SenderUserId == userId ? m.ReceiverUserId : m.SenderUserId)
                .Select(g => g.OrderByDescending(m => m.CreatedAt).FirstOrDefault())
                .OrderByDescending(m => m.CreatedAt)
                .ToList();

            var conversationUsers = groupedMessages
                .Select(m => m.SenderUserId == userId ? m.ReceiverUserId : m.SenderUserId)
                .Distinct()
                .ToList();

            var users = new List<User>();

            foreach (var id in conversationUsers)
            {
                var newUser = await _userRepository.GetByIdInclude(id);
                users.Add(newUser);
            }

            ViewBag.Messages = users;
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> MyAdvertisements()
        {
            if (!HttpContext.Request.Cookies.TryGetValue("something", out var jwtToken))
                return RedirectToAction(nameof(Index), "Auth");

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);

            var userClaims = token.Claims.FirstOrDefault(user => user.Type == "user_id");
            
            if (userClaims == null)
                return RedirectToAction(nameof(Index), "Auth");

            var user = await _userRepository.GetById(Guid.Parse(userClaims.Value));

            if (user == null)
                return RedirectToAction(nameof(Index), "Auth");

            user.Advertisements = await _advertisementRepository.GetAllAdvertisementByUserId(user.Id);
            return View(user);
        }

        [HttpGet] 
        public IActionResult Logout()
        {
            if(!HttpContext.Request.Cookies.TryGetValue("something", out var jwtToken))
                return RedirectToAction(nameof(Index), "Auth");

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);

            var userClaims = token.Claims.FirstOrDefault(user => user.Type == "user_id");
            if (userClaims == null)
                return RedirectToAction(nameof(AuthController.Index), "Auth");
            Response.Cookies.Delete("something");

            return RedirectToAction(nameof(AuthController.Index), "Auth");
        }

        public IActionResult Message()
        {
            return View();
        }
    }
}
