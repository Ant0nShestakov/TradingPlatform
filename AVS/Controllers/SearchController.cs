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


        public SearchController(AdvertisementRepository advertisementRepository, AdvertisementService advertisementService)
        {
            _advertisementRepository = advertisementRepository;
            _advertisementService = advertisementService;
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
    }
}
