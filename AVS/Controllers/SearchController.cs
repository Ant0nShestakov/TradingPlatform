using AVS.Models.AdvertisementModels;
using AVS.Repository;
using AVS.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
            ViewBag.SearchInput = "";
            return View();
        }

        public async Task<IActionResult> GetAllForCategory(Guid id)
        {
            //ViewBag.Advertisements = await _advertisementRepository.GetAllAdvertisements();
            ViewBag.Categories = await _advertisementService.GetAllCategories();
            ViewBag.Locality = await _advertisementService.GetAllLocalities();

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

            List<Advertisement> adv = (List<Advertisement>) _luceneIndex.Search(SearchInput);

            List<Advertisement> advertisements = [];

            foreach (var item in adv)
            {
                Console.WriteLine(item.Title);

                var advertisement = await _advertisementRepository.GetById(item.ID);
                if(advertisement != null) 
                    advertisements.Add(advertisement);
            }


            ViewBag.SearchInput = ($"по запросу: {SearchInput}");
            return View("Index", advertisements);
        }

        [HttpGet]
        public async Task<IActionResult> Autocomplete(string prefix)
        {
            // Получаем рекомендации для заданного префикса
            var suggestions = _luceneIndex.AutoComplete(prefix);

            // Преобразуем рекомендации в список строк
            var suggestionList = suggestions.ToList();

            // Возвращаем рекомендации в формате JSON
            return Json(suggestionList);
        }
    }
}
