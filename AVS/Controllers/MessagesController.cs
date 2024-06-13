using AVS.Models.UserModels;
using AVS.Repository;
using AVS.Services;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace AVS.Controllers
{
    public class MessagesController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly MessagesRepository _messagesRepository;
        private readonly AdvertisementService _advertisementService;

        public MessagesController(UserRepository userRepository,
            MessagesRepository messagesRepository, AdvertisementService advertisement)
        {
            _userRepository = userRepository;
            _messagesRepository = messagesRepository;
            _advertisementService = advertisement;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Guid userId)
        {
            if (!HttpContext.Request.Cookies.TryGetValue("something", out var jwtToken))
                return RedirectToAction(nameof(AuthController.Index), "Auth");

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);

            var userClaims = token.Claims.FirstOrDefault(user => user.Type == "user_id");
            if (userClaims == null)
                return RedirectToAction(nameof(AuthController.Index), "Auth");

            var sender = await _userRepository.GetByIdInclude(Guid.Parse(userClaims.Value));
            if (sender == null)
                return RedirectToAction(nameof(AuthController.Index), "Auth");

            if (sender.Id == userId)
                return RedirectToAction(nameof(PersonalAccountController.MyAdvertisements), "PersonalAccount");

            var recipient = await _userRepository.GetById(userId);
            if (recipient == null)
                return NotFound();

            List<Message> messages = await _messagesRepository.GetMessagesBetweenUsers(sender.Id, userId);

            ViewBag.Sender = sender;
            ViewBag.Recipient = recipient;
            ViewBag.Messages = messages.OrderBy(m => m.CreatedAt).ToList();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostMessage(string content, Guid userId)
        {
            if (!HttpContext.Request.Cookies.TryGetValue("something", out var jwtToken))
                return RedirectToAction(nameof(AuthController.Index), "Auth");

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);

            var userClaims = token.Claims.FirstOrDefault(user => user.Type == "user_id");
            if (userClaims == null)
                return RedirectToAction(nameof(AuthController.Index), "Auth");

            var sender = await _userRepository.GetByIdInclude(Guid.Parse(userClaims.Value));
            if (sender == null)
                return RedirectToAction(nameof(AuthController.Index), "Auth");

            var recipient = await _userRepository.GetById(userId);
            if (recipient == null)
                return NotFound();

            var newMessage = new Message
            {
                Content = content,
                CreatedAt = DateTime.Now,
                SenderUserId = sender.Id,
                ReceiverUserId = userId
            };

            await _messagesRepository.Add(newMessage);

            return RedirectToAction(nameof(Index), new { userId });
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages(Guid userId)
        {
            if (!HttpContext.Request.Cookies.TryGetValue("something", out var jwtToken))
                return RedirectToAction(nameof(AuthController.Index), "Auth");

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);

            var userClaims = token.Claims.FirstOrDefault(user => user.Type == "user_id");
            if (userClaims == null)
                return Unauthorized();

            var sender = await _userRepository.GetByIdInclude(Guid.Parse(userClaims.Value));
            if (sender == null)
                return Unauthorized();

            var recipient = await _userRepository.GetById(userId);
            if (recipient == null)
                return NotFound("User not found");

            List<Message> messages = await _messagesRepository.GetMessagesBetweenUsers(sender.Id, userId);

            var result = messages.Select(m => new
            {
                m.Content,
                m.CreatedAt,
                SenderUserId = m.SenderUserId,
                SenderUser = new
                {
                    m.SenderUser.Name
                }
            }).OrderBy(m => m.CreatedAt).ToList();

            return Json(result);
        }
    }
}
