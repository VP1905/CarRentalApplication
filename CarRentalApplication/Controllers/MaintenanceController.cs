using CarRentalApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalApplication.Controllers
{
    public class MaintenanceController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public MaintenanceController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [HttpGet]
        public IActionResult History()
        {
            return View(new List<RepairHistoryDto>());
        }
        [HttpPost]
        public async Task<IActionResult> History(int vehicleId)
        {
            var client = _httpClientFactory.CreateClient("MaintenanceApi");
            var repairs = await client.GetFromJsonAsync<List<RepairHistoryDto>>(
                $"api/maintenance/vehicles/{vehicleId}/repairs");
            return View(repairs ?? new List<RepairHistoryDto>());
        }
        public async Task<IActionResult> Usage()
        {
            var client = _httpClientFactory.CreateClient("MaintenanceApi");

            var result = await client.GetFromJsonAsync<object>(
                "api/RepairHistory/usage"
            );

            return View(result);
        }
    }
}