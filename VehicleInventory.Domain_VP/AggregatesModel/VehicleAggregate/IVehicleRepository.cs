using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleInventory.Domain_VP.SeedWork;

namespace VehicleInventory.Domain_VP.AggregatesModel.VehicleAggregate
{
    public interface IVehicleRepository : IRepository<Vehicle>
    {
        Vehicle Add(Vehicle vehicle);
        Vehicle Update(Vehicle vehicle);
        Task<Vehicle?> FindByIdAsync(int id);
        Task<IEnumerable<Vehicle>> GetAllAsync();
        void Delete(Vehicle vehicle);
        Task SaveChangesAsync();
    }
}
