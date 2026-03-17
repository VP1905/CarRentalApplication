using CarRentalApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

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
            var client = _httpClientFactory.CreateClient("ApiGateway");

            var repairs = await client.GetFromJsonAsync<List<RepairHistoryDto>>(
                $"gateway/maintenance/vehicles/{vehicleId}/repairs");

            return View(repairs ?? new List<RepairHistoryDto>());
        }

        [HttpGet]
        public async Task<IActionResult> Usage()
        {
            var client = _httpClientFactory.CreateClient("ApiGateway");

            var result = await client.GetFromJsonAsync<object>(
                "gateway/maintenance/usage");

            return View(result);
        }
    }
}