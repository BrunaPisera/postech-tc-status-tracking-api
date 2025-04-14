using Microsoft.Extensions.DependencyInjection;
using ProcessService.Infrastructure.Broker;
using StatusTracking.Infrastructure.Data;
using StatusTracking.Infrastructure.Gateway;
using StatusTracking.UseCases;
using StatusTracking.UseCases.Gateway;
using StatusTracking.UseCases.Interfaces;

namespace StatusTracking.Infrastructure
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IBrokerConnection, BrokerConnection>();
            services.AddScoped<BrokerConsumer>();

            services.AddScoped<IStatusTrackingUseCases, StatusTrackingUseCases>();
            services.AddScoped<IStatusTrackingPersistenceGateway, StatusTrackingPersistentGateway>();

            services.AddDbContext<ApplicationContext>();
            return services;
        }
    }
}
