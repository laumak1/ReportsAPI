using adform.Repositories;
using adform.Repositories.Interfaces;
using adform.Services;
using adform.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace adform.Configurations
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAllDependencies(this IServiceCollection service)
        {
            return service
                .AddInfrastructureDependencies()
                .AddApplicationDependencies();
        }



        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection service)
        {
            return service
                .AddScoped<IAuthorizationRepository, AuthorizationRepository>()
                .AddScoped<IReportsRepository, ReportsRepository>();

        }

        public static IServiceCollection AddApplicationDependencies(this IServiceCollection service)
        {
            return service
                .AddScoped<IAuthorizationService, AuthorizationService>()
                .AddScoped<IReportsService, ReportsService>();
        }

    }
}
