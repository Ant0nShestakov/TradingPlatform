using AVS.Models;
using AVS.Models.AddressModels;
using AVS.Models.AdvertisementModels;
using AVS.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AVS.Controllers
{
    public class HomeController : Controller
    {

        private readonly AdvertisementRepository _advertisementRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly LocalitiesRepository _localityRepository;

        public HomeController(AdvertisementRepository advertisements, CategoryRepository categoryRepository, 
            LocalitiesRepository localitiesRepository)
        {
            _advertisementRepository = advertisements;
            _categoryRepository = categoryRepository;
            _localityRepository = localitiesRepository;
        }

        public async Task <IActionResult> Index()
        {
            ViewBag.Advertisements = (List<Advertisement>) await _advertisementRepository.GetAllAdvertisements();
            ViewBag.Categories = (List<Category>) await _categoryRepository.GetAllCategories();
            ViewBag.Locality = (List<Locality>) await _localityRepository.GetAllLocalities();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
