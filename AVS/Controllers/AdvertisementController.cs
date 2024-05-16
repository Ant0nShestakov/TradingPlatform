using AVS.Models.AddressModels;
using AVS.Models.AdvertisementModels;
using AVS.Repository;
using AVS.Services;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace AVS.Controllers
{
    public class AdvertisementController : Controller
    {
        private readonly CountryRepository _countryRepository;
        private readonly RegionsRepository _regionRepository;
        private readonly LocalitiesRepository _localityRepository;
        private readonly StreetRepository _streetRepository;
        private readonly StateRepository _stateRepository;
        private readonly AddressRepository _addressRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly UserRepository _userRepository;
        private readonly AdvertisementService _advertisementService;

        private IWebHostEnvironment _webHostEnvironment;

        public AdvertisementController(CountryRepository countryRepository,
            RegionsRepository regionRepository, LocalitiesRepository localityRepository,
            StreetRepository streetRepository,
            UserRepository userRepository, StateRepository stateRepository,
            AddressRepository addressRepository, CategoryRepository categoryRepository,
            AdvertisementService advertisementService,
            IWebHostEnvironment webHostEnvironment)
        {
            _countryRepository = countryRepository;
            _regionRepository = regionRepository;
            _localityRepository = localityRepository;
            _streetRepository = streetRepository;
            _userRepository = userRepository;
            _stateRepository = stateRepository;
            _addressRepository = addressRepository;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
            _advertisementService = advertisementService;
        }

        public IActionResult Index()
        {
            return View();
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
            ViewBag.States = (List<AdvertisementState>) await _advertisementService.GetAllStates();
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

            if (!ModelState.IsValid)
            {
                ViewBag.Country = (List<Country>)await _advertisementService.GetAllCountries();
                ViewBag.States = (List<AdvertisementState>)await _advertisementService.GetAllStates();
                ViewBag.User = user;
                ViewBag.Categories = (List<Category>)await _advertisementService.GetAllCategories();

                return View(nameof(CreateAdvertisement), advertisement);
            }

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
