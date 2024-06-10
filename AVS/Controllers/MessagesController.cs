using AVS.Models.AdvertisementModels;
using AVS.Models.UserModels;
using AVS.Repository;
using AVS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Immutable;
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
        public async Task<IActionResult> Index(Guid advertisementId, Guid? messageId)
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

            Advertisement? advertisement = await _advertisementService.GetAdvertisementById(advertisementId);
            if (advertisement == null)
                return NotFound();

            Guid recipientId;

            if (advertisement.UserId != sender.Id || messageId == null)
            {
                recipientId = advertisement.UserId;
            }
            else
            {
                // Объединяем сообщения и группируем по SenderUserId
                var allMessages = sender.MessagesReceive
                    .Where(m => m.AdvertisementId == advertisementId)
                    .Concat(sender.MessagesSent.Where(m => m.AdvertisementId == advertisementId))
                    .GroupBy(m => m.SenderUserId)
                    .Select(g => g.First())
                    .OrderBy(m => m.CreatedAt)
                    .ToList();

                // Найти сообщение по messageId
                var message = allMessages.FirstOrDefault(m => m.Id == messageId);
                if (message == null)
                    return BadRequest("Invalid messageId.");

                // Определяем recipientId в зависимости от того, является ли сообщение отправленным или полученным
                recipientId = message.SenderUserId == sender.Id ? message.ReceiverUserId : message.SenderUserId;
            }

            List<Message> messages = await _messagesRepository.GetMessagesBetweenUsers(sender.Id, recipientId, advertisementId);

            ViewBag.Sender = sender;
            ViewBag.Advertisement = advertisement;
            ViewBag.Messages = messages.OrderBy(m => m.CreatedAt).ToList();
            ViewBag.MessageId = messageId;

            return View();
        }




        [HttpPost]
        public async Task<IActionResult> PostMessage(Guid advertisementId, string content, Guid? messageId)
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

            Advertisement? advertisement = await _advertisementService.GetAdvertisementById(advertisementId);
            if (advertisement == null)
                return NotFound();

            Guid receiverId;

            if (advertisement.UserId != sender.Id || messageId == null)
            {
                receiverId = advertisement.UserId;
            }
            else
            {
                // Объединяем сообщения и группируем по SenderUserId
                var allMessages = sender.MessagesReceive
                    .Where(m => m.AdvertisementId == advertisementId)
                    .Concat(sender.MessagesSent.Where(m => m.AdvertisementId == advertisementId))
                    .GroupBy(m => m.SenderUserId)
                    .Select(g => g.First())
                    .OrderBy(m => m.CreatedAt)
                    .ToList();

                // Найти сообщение по messageId
                var message = allMessages.FirstOrDefault(m => m.Id == messageId);
                if (message == null)
                    return BadRequest("Invalid messageId.");

                // Определяем receiverId в зависимости от того, является ли сообщение отправленным или полученным
                receiverId = message.SenderUserId == sender.Id ? message.ReceiverUserId : message.SenderUserId;
            }

            var newMessage = new Message
            {
                Content = content,
                CreatedAt = DateTime.Now,
                SenderUserId = sender.Id,
                ReceiverUserId = receiverId,
                AdvertisementId = advertisementId
            };

            // Сохраняем новое сообщение
            await _messagesRepository.Add(newMessage);

            // Перенаправляем пользователя обратно на страницу диалога
            return RedirectToAction(nameof(Index), new { advertisementId, messageId });
        }


    }
}
