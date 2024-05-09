using AVS.Models.AddressModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AVS.Controllers
{
    //[Authorize(Policy = "UserPolicy")]
    public class PersonalAccountController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Message()
        {
            return View();
        }

        public IActionResult CreateAdvertisement()
        {
            return View();
        }

        public IEnumerable<Region> GetRegions(int id)
        {
            return null;
        }

        public IEnumerable<Region> GetLocalities(int id)
        {
            return null;
        }

        public IEnumerable<Region> GetStreets(int id)
        {
            return null;
        }

    }
}
