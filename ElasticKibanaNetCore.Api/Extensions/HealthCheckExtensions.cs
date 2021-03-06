﻿using ElasticKibanaNetCore.Api.HealthCheck;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;

namespace ElasticKibanaNetCore.Api.Extensions
{
    public static class HealthCheckExtensions
    {
        public static void AddHealthCheckApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddCheck("Situação", () => HealthCheckResult.Healthy())
                .AddCheck<MyHealthCheck>("Dependências")
                .AddElasticsearch(
                    configuration.GetConnectionString("Elasticsearch"), "ElasticSearch", HealthStatus.Degraded, new[] { "elastic", "search" });

            services.AddHealthChecksUI(config =>
            {
                config.SetEvaluationTimeInSeconds(5);
                config.AddHealthCheckEndpoint("Host Externo", ObterHostNameApiHealthCheck());
                config.AddHealthCheckEndpoint("Aplicação", $"https://localhost:5001/hc");

            }).AddInMemoryStorage();
        }

        public static void UseHealthCheckApi(this IApplicationBuilder app)
        {
            app.UseHealthChecksUI(config =>
            {
                config.UIPath = "/hc-ui";
            });
        }

        public static string ObterHostNameApiHealthCheck()
        {
            var tt = Environment.GetEnvironmentVariable("HostNameHealthCheck") == null ?
                "/hc" : $"{Environment.GetEnvironmentVariable("HostNameHealthCheck")}/hc";
            return tt;
        }
    }
}
