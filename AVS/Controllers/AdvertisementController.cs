using AVS.Models.AddressModels;
using AVS.Models.AdvertisementModels;
using AVS.Repository;
using AVS.Services;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace AVS.Controllers
{
    public class AdvertisementController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly AdvertisementService _advertisementService;

        private IWebHostEnvironment _webHostEnvironment;

        public AdvertisementController(UserRepository userRepository,
            AdvertisementService advertisementService)
        {
            _userRepository = userRepository;
            _advertisementService = advertisementService;
        }

        [HttpGet]
        public async Task <IActionResult> ShowAdvertisement(Guid Id)
        {
            var advertisement = await _advertisementService.GetAdvertisementById(Id);
            if (advertisement == null)
                return RedirectToAction(nameof(HomeController.Index), "Home");

            ViewBag.Categories = (List<Category>) await _advertisementService.GetAllCategories();
            ViewBag.Locality = (List<Locality>) await _advertisementService.GetAllLocalities();

            return View(advertisement);
        }

        [HttpGet]
        public async Task<IActionResult> CreateAdvertisement()
        {
            if (!HttpContext.Request.Cookies.TryGetValue("something", out var jwtToken))
                return RedirectToAction(nameof(Index), "Auth");

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);

            var userClaims = token.Claims.FirstOrDefault(user => user.Type == "user_id");
            if (userClaims == null)
                return RedirectToAction(nameof(Index), "Auth");

            var user = await _userRepository.GetById(Guid.Parse(userClaims.Value));

            ViewBag.Country = (List<Country>) await _advertisementService.GetAllCountries();
            ViewBag.User = user;
            ViewBag.Categories = (List<Category>) await _advertisementService.GetAllCategories();

            return View(new Advertisement());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAdvertisement(Advertisement advertisement, List<IFormFile> images)
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

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

            if(!await _advertisementService.Create(user, advertisement, images, baseUrl))
                return NotFound();

            return RedirectToAction(nameof(PersonalAccountController.MyAdvertisements), "PersonalAccount");
        }

        [HttpGet]
        public Task<IEnumerable<Region>> GetRegions(Guid id) => _advertisementService.GetRegions(id);

        [HttpGet]
        public Task<IEnumerable<Locality>> GetLocalities(Guid id) => _advertisementService.GetLocalities(id);

        [HttpGet]
        public Task<IEnumerable<Street>> GetStreets(Guid id) => _advertisementService.GetStreets(id);
    }
}
