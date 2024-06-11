using AVS.Models;
using AVS.Models.AddressModels;
using AVS.Models.AdvertisementModels;
using AVS.Repository;
using AVS.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AVS.Controllers
{
    public class HomeController : Controller
    {
        private LuceneIndex _luceneIndex;
        private readonly AdvertisementRepository _advertisementRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly LocalitiesRepository _localityRepository;

        public HomeController(AdvertisementRepository advertisements, CategoryRepository categoryRepository, 
            LocalitiesRepository localitiesRepository, LuceneIndex luceneIndex)
        {
            _advertisementRepository = advertisements;
            _categoryRepository = categoryRepository;
            _localityRepository = localitiesRepository;
            _luceneIndex = luceneIndex;
        }

        public async Task <IActionResult> Index()
        {
            List<Advertisement> advertisements = (List<Advertisement>)await _advertisementRepository.GetAllAdvertisements();

            ViewBag.Advertisements = advertisements;
            ViewBag.Categories = (List<Category>) await _categoryRepository.GetAllCategories();
            ViewBag.Locality = (List<Locality>) await _localityRepository.GetAllLocalities();
            
            foreach (var item in advertisements)
            {
                _luceneIndex.AddUpdateLuceneIndex(item);
            }

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
