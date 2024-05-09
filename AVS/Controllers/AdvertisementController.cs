using Microsoft.AspNetCore.Mvc;

namespace AVS.Controllers
{
    public class AdvertisementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
