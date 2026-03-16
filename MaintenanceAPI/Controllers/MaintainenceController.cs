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

        public MaintenanceController(
            IRepairHistoryService service,
            ConcurrentDictionary<string, int> usageCounts)
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
                repair);
        }

        [HttpGet("crash")]
        public IActionResult Crash()
        {
            throw new System.Exception("Test exception for global handler.");
        }

        [HttpGet("usage")]
        public IActionResult Usage()
        {
            var gatewayHeader = Request.Headers["X-Internal-Gateway"].ToString();

            var count = _usageCounts.AddOrUpdate(
                gatewayHeader,
                1,
                (_, oldValue) => oldValue + 1);

            return Ok(new
            {
                source = "API Gateway",
                callCount = count
            });
        }
    }
}