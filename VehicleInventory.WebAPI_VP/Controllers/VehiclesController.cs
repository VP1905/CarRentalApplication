using Microsoft.AspNetCore.Mvc;

namespace VehicleInventory.WebAPI_VP.Controllers
{
    public class VehiclesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
