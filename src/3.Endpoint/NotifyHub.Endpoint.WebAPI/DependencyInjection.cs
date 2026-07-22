using NotifyHub.Core.ApplicationService;
using NotifyHub.Infrastructure.Services;
using NotifyHub.Infrastructure.Workers;
using NotifyHub.Shared.Utility.AppSettings;
using Serilog;

namespace NotifyHub.Endpoint.WebAPI;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AppSettings>(configuration);

        services.AddSerilog(configure =>
        {
            configure.ReadFrom.Configuration(configuration)
                              .WriteTo.MongoDBBson(configure =>
                              {
                                  configure.SetMongoUrl(configuration.GetRequiredSection("MongoUrl").Value!);
                                  configure.SetExpireTTL(TimeSpan.FromDays(10));
                              });
        });

        services.ConfigureInfrastructure(configuration);
        services.ConfigureApplicationService(configuration);
        services.ConfigureWorkers(configuration);
        return services;
    }
}