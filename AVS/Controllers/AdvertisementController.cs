using AVS.Models.AddressModels;
using AVS.Models.AdvertisementModels;
using AVS.Repository;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

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

        private readonly UserRepository _userRepository;

        public AdvertisementController(CountryRepository countryRepository,
            RegionsRepository regionRepository, LocalitiesRepository localityRepository,
            StreetRepository streetRepository, 
            UserRepository userRepository, StateRepository stateRepository, AddressRepository addressRepository)
        {
            _countryRepository = countryRepository;
            _regionRepository = regionRepository;
            _localityRepository = localityRepository;
            _streetRepository = streetRepository;
            _userRepository = userRepository;
            _stateRepository = stateRepository;
            _addressRepository = addressRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateAdvertisement()
        {
            if (HttpContext.Request.Cookies.TryGetValue("something", out var jwtToken))
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(jwtToken);

                var userClaims = token.Claims.FirstOrDefault(user => user.Type == "user_id");
                if (userClaims == null)
                    return RedirectToAction(nameof(Index), "Auth");

                var user = await _userRepository.GetById(Guid.Parse(userClaims.Value));

                List<Country> countries = (List<Country>)await _countryRepository.GetAllCountry();
                List<AdvertisementState> states = (List<AdvertisementState>) await _stateRepository.GetAllState();
                ViewBag.Country = countries;
                ViewBag.State = states;
                ViewBag.User = user;

                return View(new Advertisement());
            }
            return RedirectToAction(nameof(Index), "Auth");
        }

        public async Task<IActionResult> AddAdvertisement(Advertisement advertisement)
        {
            if (HttpContext.Request.Cookies.TryGetValue("something", out var jwtToken))
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(jwtToken);

                var userClaims = token.Claims.FirstOrDefault(user => user.Type == "user_id");
                if (userClaims == null)
                    return RedirectToAction(nameof(Index), "Auth");

                var user = await _userRepository.GetById(Guid.Parse(userClaims.Value));

                if(user == null)
                    return RedirectToAction(nameof(Index), "Auth");

                Address address = new Address();
                address = advertisement.Address;
                address.Street = await _streetRepository.GetById(advertisement.Address.StreetID);
               
                advertisement.Address = address;
                advertisement.UserId = user.Id;
                advertisement.CreatedDate = DateTime.UtcNow;

                user.Advertisements.Add(advertisement);
                await _userRepository.Update(user);

                return RedirectToAction(nameof(Index), "PersonalAccount");
            }
            return RedirectToAction(nameof(Index), "Auth");
        }

        [HttpGet]
        public Task<IEnumerable<Region>> GetRegions(Guid id) => _regionRepository.GetAllRegionsByCountryId(id);

        [HttpGet]
        public Task<IEnumerable<Locality>> GetLocalities(Guid id) => _localityRepository.GetLocalitieByRegionId(id);

        public Task<IEnumerable<Street>> GetStreets(Guid id) => _streetRepository.GetAllStreetByLocalityId(id);
    }
}
