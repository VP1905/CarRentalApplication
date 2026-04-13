using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VehicleInventory.Domain_VP.AggregatesModel.VehicleAggregate;
using VehicleInventory.Infrastructure_VP.Data;
using VehicleInventory.Infrastructure_VP.Repositories;

namespace VehicleInventory.Infrastructure_VP
{
    public static class DependencyInjection
    {
        //Registers database context and repository implementations
        //The IServiceCollection used to resigster application services.
        public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
        {
            services.AddDbContext<InventoryDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IVehicleRepository, VehicleRepository>();

            return services;
        }
    }
}
