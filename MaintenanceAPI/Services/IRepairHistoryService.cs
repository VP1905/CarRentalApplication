using MaintenanceAPI.Models;

namespace MaintenanceAPI.Services
{
    public interface IRepairHistoryService
    {
        List<RepairHistoryDto> GetByVehicleId(int vehicleId);
    }
}
