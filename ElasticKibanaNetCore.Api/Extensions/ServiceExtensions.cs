using ElasticKibanaNetCore.Api.HealthCheck;
using ElasticKibanaNetCore.Api.Tests;
using Microsoft.Extensions.DependencyInjection;

namespace ElasticKibanaNetCore.Api.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IMyCustomService, MyCustomService>();
            services.AddScoped<ILogsApplication, LogsApplication>();
        }
    }
}
