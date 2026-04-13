using Microsoft.Extensions.DependencyInjection;
using VehicleInventory.Application_VP.Interfaces;
using VehicleInventory.Application_VP.Services;

namespace VehicleInventory.Application_VP
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IVehicleService, VehicleService>();
            return services;
        }
    }
}
