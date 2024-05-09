using AVS.Models.AddressModels;
using AVS.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AVS.Controllers
{
    public class AdvertisementController : Controller
    {
        private readonly CountryRepository _countryRepository;
        private readonly RegionsRepository _regionRepository;
        private readonly LocalitiesRepository _localityRepository;
        private readonly StreetRepository _streetRepository;

        public AdvertisementController(CountryRepository countryRepository,
            RegionsRepository regionRepository, LocalitiesRepository localityRepository,
            StreetRepository streetRepository)
        {
            _countryRepository = countryRepository;
            _regionRepository = regionRepository;
            _localityRepository = localityRepository;
            _streetRepository = streetRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CreateAdvertisement()
        {
            List<Country> countries = (List<Country>) await _countryRepository.GetAllCountry();
            ViewBag.Country = countries;
            return View();
        }

        [HttpGet]
        public Task<IEnumerable<Region>> GetRegions(Guid id) => _regionRepository.GetAllRegionsByCountryId(id);

        [HttpGet]
        public Task<IEnumerable<Locality>> GetLocalities(Guid id) => _localityRepository.GetLocalitieByRegionId(id);

        public Task<IEnumerable<Street>> GetStreets(Guid id) => _streetRepository.GetAllStreetByLocalityId(id);
    }
}
