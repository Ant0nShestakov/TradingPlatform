using Microsoft.AspNetCore.Mvc;

namespace AVS.Controllers
{
    public class SearchController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
