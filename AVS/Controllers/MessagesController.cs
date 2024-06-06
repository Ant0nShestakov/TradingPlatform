using AVS.Models.AdvertisementModels;
using AVS.Repository;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;

namespace AVS.Controllers
{
    public class MessagesController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly MessagesRepository _messagesRepository;

        public MessagesController(UserRepository userRepository, MessagesRepository messagesRepository)
        {
            _userRepository = userRepository;
            _messagesRepository = messagesRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Advertisement advertisement)
        {
            if (!HttpContext.Request.Cookies.TryGetValue("something", out var jwtToken))
                return RedirectToAction(nameof(Index), "Auth");

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);

            var userClaims = token.Claims.FirstOrDefault(user => user.Type == "user_id");
            if (userClaims == null)
                return RedirectToAction(nameof(Index), "Auth");

            var sender = await _userRepository.GetById(Guid.Parse(userClaims.Value));

            if (sender == null)
                return RedirectToAction(nameof(Index), "Auth");

            Message message = new Message();

            message.Users.Add(sender);
            message.Users.Add(advertisement.User);
            message.Advertisements.Add(advertisement);

            List<Message> messages = await _messagesRepository
                .GetAllMessageBySenderAndReciever(sender, advertisement.User) ?? [];

            messages.Add(message);

            ViewBag.Messages = messages;


            return View(message);
        }

        [HttpPost]
        public async Task<IActionResult> PostMessage(Message message)
        {

            return RedirectToAction(nameof(Index), "Messages");
        }
    }
}
