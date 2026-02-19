using MaintenanceAPI.Models;
using MaintenanceAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace MaintenanceAPI.Controllers
{
    [ApiController]
    [Route("api/maintenance")]
    public class MaintenanceController : ControllerBase
    {
        private readonly IRepairHistoryService _service;
        private readonly ConcurrentDictionary<string, int> _usageCounts;
        public MaintenanceController(IRepairHistoryService service, ConcurrentDictionary<string, int> usageCounts)
        {
            _service = service;
            _usageCounts = usageCounts;
        }
        [HttpGet("vehicles/{vehicleId}/repairs")]
        public IActionResult GetRepairHistory(int vehicleId)
        {
            var history = _service.GetByVehicleId(vehicleId);
            return Ok(history);
        }
        [HttpPost("repairs")]
        public IActionResult AddRepair([FromBody] RepairHistoryDto repair)
        {
            if (repair.VehicleId <= 0)
            {
                return BadRequest(new
                {
                    error = "InvalidParameter",
                    message = "VehicleId must be greater than zero."
                });
            }

            if (string.IsNullOrWhiteSpace(repair.Description))
            {
                return BadRequest(new
                {
                    error = "InvalidParameter",
                    message = "Description must not be empty."
                });
            }

            if (repair.Cost < 0)
            {
                return BadRequest(new
                {
                    error = "InvalidParameter",
                    message = "Cost cannot be negative."
                });
            }

            return CreatedAtAction(
                nameof(GetRepairHistory),
                new { vehicleId = repair.VehicleId },
                repair
            );
        }

        [HttpGet("crash")]
        public IActionResult Crash()
        {
            int x = 0;
            int y = 5 / x;
            return Ok();
        }

        [HttpGet("usage")]
        public IActionResult Usage()
        {
            var key = Request.Headers["X-Api-Key"].ToString();

            var count = _usageCounts.AddOrUpdate(
                key,
                1,
                (k, oldValue) => oldValue + 1
            );

            return Ok(new
            {
                clientId = key,
                callCount = count
            });
        }
    }
}