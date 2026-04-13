using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using CarRentalApplication.Models;

namespace CarRentalApplication.Controllers
{
    public class CustomersController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonOptions;

        public CustomersController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        private HttpClient CreateClient()
        {
            return _httpClientFactory.CreateClient("ApiGateway");
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            var client = CreateClient();
            var response = await client.GetAsync("/gateway/customers");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to load customers. Status: {response.StatusCode}");
            }

            var json = await response.Content.ReadAsStringAsync();
            var customers = JsonSerializer.Deserialize<List<Customer>>(json, _jsonOptions) ?? new List<Customer>();

            return View(customers);
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = CreateClient();
            var response = await client.GetAsync($"/gateway/customers/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound();
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to load customer details. Status: {response.StatusCode}");
            }

            var json = await response.Content.ReadAsStringAsync();
            var customer = JsonSerializer.Deserialize<Customer>(json, _jsonOptions);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,FirstName,LastName,Phone,Email")] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return View(customer);
            }

            var client = CreateClient();

            var json = JsonSerializer.Serialize(customer);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/gateway/customers", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to create customer. Status: {response.StatusCode}");
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = CreateClient();
            var response = await client.GetAsync($"/gateway/customers/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound();
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to load customer for edit. Status: {response.StatusCode}");
            }

            var json = await response.Content.ReadAsStringAsync();
            var customer = JsonSerializer.Deserialize<Customer>(json, _jsonOptions);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,FirstName,LastName,Phone,Email")] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(customer);
            }

            var client = CreateClient();

            var json = JsonSerializer.Serialize(customer);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/gateway/customers/{id}", content);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound();
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to update customer. Status: {response.StatusCode}");
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = CreateClient();
            var response = await client.GetAsync($"/gateway/customers/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound();
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to load customer for delete. Status: {response.StatusCode}");
            }

            var json = await response.Content.ReadAsStringAsync();
            var customer = JsonSerializer.Deserialize<Customer>(json, _jsonOptions);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = CreateClient();

            var response = await client.DeleteAsync($"/gateway/customers/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound();
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to delete customer. Status: {response.StatusCode}");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}