using AVS.Models.AdvertisementModels;
using AVS.Repository;
using AVS.Services;
using Microsoft.AspNetCore.Mvc;

namespace AVS.Controllers
{
    public class SearchController : Controller
    {
        private readonly AdvertisementRepository _advertisementRepository;
        private readonly AdvertisementService _advertisementService;
        private readonly LuceneIndex _luceneIndex;

        public SearchController(AdvertisementRepository advertisementRepository, 
            AdvertisementService advertisementService, LuceneIndex luceneIndex)
        {
            _advertisementRepository = advertisementRepository;
            _advertisementService = advertisementService;
            _luceneIndex = luceneIndex;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Advertisements = (List<Advertisement>)await _advertisementRepository.GetAllAdvertisements();
            ViewBag.Categories = await _advertisementService.GetAllCategories();
            ViewBag.Locality = await _advertisementService.GetAllLocalities();
            ViewBag.Status = await _advertisementService.GetAllStates();
            ViewBag.SearchInput = "";
            return View();
        }

        public async Task<IActionResult> GetAllForCategory(Guid id)
        {
            //ViewBag.Advertisements = await _advertisementRepository.GetAllAdvertisements();
            ViewBag.Categories = await _advertisementService.GetAllCategories();
            ViewBag.Locality = await _advertisementService.GetAllLocalities();
            ViewBag.Status = await _advertisementService.GetAllStates();

            List<Advertisement> advertisement = 
                await _advertisementRepository.GetAllAdvertisementByCategoryId(id);

            string name = (await _advertisementService.GetCategoryByID(id)).Name;

            ViewBag.SearchInput = $"по категории: {name}";
            return View("Index", advertisement);
        }

        public async Task<IActionResult> GetAllForLocality(Guid id)
        {
            //ViewBag.Advertisements = await _advertisementRepository.GetAllAdvertisements();
            ViewBag.Categories = await _advertisementService.GetAllCategories();
            ViewBag.Locality = await _advertisementService.GetAllLocalities();
            ViewBag.Status = await _advertisementService.GetAllStates();

            List<Advertisement> advertisement = (List<Advertisement>)
                await  _advertisementRepository.GetAllAdvertisementByLocalityId(id);

            string name = (await _advertisementService.GetLocalityByID(id)).Name;

            ViewBag.SearchInput = ($"по городу: {name}");
            return View("Index", advertisement);
        }

        [HttpGet]
        public async Task<IActionResult> SearchWithInput(string SearchInput)
        {
            ViewBag.Categories = await _advertisementService.GetAllCategories();
            ViewBag.Locality = await _advertisementService.GetAllLocalities();
            ViewBag.Status = await _advertisementService.GetAllStates();
            List<Advertisement> adv = (List<Advertisement>) _luceneIndex.Search(SearchInput);

            List<Advertisement> advertisements = [];

            foreach (var item in adv)
            {
                Console.WriteLine(item.Title);

                var advertisement = await _advertisementRepository.GetById(item.ID);
                if(advertisement != null) 
                    advertisements.Add(advertisement);
            }

            ViewBag.SearchInput = SearchInput;
            ViewBag.Status = await _advertisementService.GetAllStates();
            return View("Index", advertisements);
        }

        public async Task<IActionResult> SearchWithInputAndPriceRange(string SearchInput, float? minRange, 
            float? maxRange, Guid AdvertisementStateId)
        {

            ViewBag.Categories = await _advertisementService.GetAllCategories();
            ViewBag.Locality = await _advertisementService.GetAllLocalities();
            ViewBag.Status = await _advertisementService.GetAllStates();
            List<Advertisement> adv = (List<Advertisement>)_luceneIndex.Search(SearchInput);

            List<Advertisement> advertisements = [];

            foreach (var item in adv)
            {
                Console.WriteLine(item.Title);

                var advertisement = await _advertisementRepository.GetById(item.ID);
                if (advertisement != null)
                    advertisements.Add(advertisement);
            }

            var searchedList = new List<Advertisement>();

            if (advertisements.Count > 0)
                searchedList = advertisements
                    .Select(adv => adv)
                    .Where(adv => adv.Price >= minRange && adv.Price <= maxRange && adv.AdvertisementStateId == AdvertisementStateId)
                    .ToList();

            ViewBag.SearchInput = SearchInput;
            ViewBag.MinRange = minRange;
            ViewBag.MaxRange = maxRange;

            return View("Index", searchedList);
        }

        [HttpGet]
        public async Task<IActionResult> Autocomplete(string prefix)
        {
            var suggestions = _luceneIndex.AutoComplete(prefix);
            var suggestionList = suggestions.Distinct().ToList();

            return Json(suggestionList);
        }
    }
}
