﻿using AVS.Models.AddressModels;
using AVS.Models.AdvertisementModels;
using AVS.Models.UserModels;
using AVS.Repository;

namespace AVS.Services
{
    public class AdvertisementService
    {
        private readonly CountryRepository _countryRepository;
        private readonly RegionsRepository _regionRepository;
        private readonly LocalitiesRepository _localityRepository;
        private readonly StreetRepository _streetRepository;
        private readonly StateRepository _stateRepository;
        private readonly AddressRepository _addressRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly UserRepository _userRepository;
        private readonly AdvertisementRepository _advertisementRepository;
        private IWebHostEnvironment _webHostEnvironment;

        public AdvertisementService(CountryRepository countryRepository, RegionsRepository regionRepository, 
            LocalitiesRepository localityRepository, StreetRepository streetRepository, 
            StateRepository stateRepository, AddressRepository addressRepository, 
            CategoryRepository categoryRepository, UserRepository userRepository, IWebHostEnvironment webHost,
            AdvertisementRepository advertisementRepository)
        {
            _countryRepository = countryRepository;
            _regionRepository = regionRepository;
            _localityRepository = localityRepository;
            _streetRepository = streetRepository;
            _stateRepository = stateRepository;
            _addressRepository = addressRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
            _advertisementRepository = advertisementRepository;
            _webHostEnvironment = webHost;
        }

        public async Task<bool> Create(User user, Advertisement advertisement, List<IFormFile> images, string baseUrl)
        {
            string wwwrootpath = _webHostEnvironment.WebRootPath;

            foreach (var photo in images)
            {
                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(photo.FileName);

                string directoryPath = Path.Combine(wwwrootpath, "advertisements", user.Id.ToString(), advertisement.Title);

                Directory.CreateDirectory(directoryPath);

                string imageFullPath = Path.Combine(directoryPath, newFileName);

                using (var stream = System.IO.File.Create(imageFullPath))
                    photo.CopyTo(stream);
                advertisement.Photos
                    .Add(new AdvertisementPhoto(newFileName,
                    $"{baseUrl}/advertisements/{user.Id}/{advertisement.Title}/{newFileName}"));
            }

            advertisement.Address.Street = await _streetRepository.GetById(advertisement.Address.StreetID);
            advertisement.UserId = user.Id;
            advertisement.CreatedDate = DateTime.UtcNow;
            advertisement.AdvertisementState = await _stateRepository.GetByName("Активное");

            user.Advertisements.Add(advertisement);
            await _userRepository.Update(user);

            return true;
        }

        public async Task<Advertisement?> GetAdvertisementById(Guid id) => await _advertisementRepository.GetById(id);

        public Task<IEnumerable<Region>> GetRegions(Guid id) => _regionRepository.GetAllRegionsByCountryId(id);

        public Task<IEnumerable<Locality>> GetLocalities(Guid id) => _localityRepository.GetLocalitieByRegionId(id);

        public Task<IEnumerable<Street>> GetStreets(Guid id) => _streetRepository.GetAllStreetByLocalityId(id);

        public async Task<IEnumerable<Country>> GetAllCountries() => await _countryRepository.GetAllCountry();

        public async Task<IEnumerable<AdvertisementState>> GetAllStates() => await _stateRepository.GetAllState();

        public async Task<IEnumerable<Category>> GetAllCategories() => await _categoryRepository.GetAllCategories();

        public async Task<IEnumerable<Locality>> GetAllLocalities() => await _localityRepository.GetAllLocalities();

        public async Task<Category?> GetCategoryByID(Guid id) => await _categoryRepository.GetById(id);

        public async Task<Locality?> GetLocalityByID(Guid id) => await _localityRepository.GetById(id);
    }
}
