using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;

namespace ElasticKibanaNetCore.Api.Extensions
{
    public static class ElasticsearchExtensions
    {
        public static void AddElasticsearch(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = new ConnectionSettings(new Uri(configuration["ElasticConfiguration:Uri"]));

            var defaultIndex = configuration["ElasticConfiguration:Index"];

            if (!string.IsNullOrEmpty(defaultIndex))
            {
                settings = settings.DefaultIndex(defaultIndex);
            }

            var basicAuthUser = configuration["ElasticConfiguration:Username"];
            var basicAuthPassword = configuration["ElasticConfiguration:Password"];

            if (!string.IsNullOrEmpty(basicAuthUser) && !string.IsNullOrEmpty(basicAuthPassword))
            {
                settings = settings.BasicAuthentication(basicAuthUser, basicAuthPassword);
            }

            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);
        }
    }
}
