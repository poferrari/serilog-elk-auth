using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Filters;
using Serilog.Sinks.Elasticsearch;
using System;

namespace ElasticKibanaNetCore.Api.Extensions
{
    public static class SerilogExtensions
    {
        public static void AddSerilog(IConfiguration configuration)
        {
            var elasticUri = configuration["ElasticConfiguration:Uri"];
            var elasticUsername = configuration["ElasticConfiguration:Username"];
            var elasticPassword = configuration["ElasticConfiguration:Password"];
            var elasticIndex = configuration["ElasticConfiguration:Index"];

            Log.Logger = new LoggerConfiguration()
                .Enrich.WithProperty("ApplicationName", $"API Exemplo - {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}")
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithDemystifiedStackTraces()
                .WriteTo.Debug()
                .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.StaticFiles"))
                .Filter.ByExcluding(z => z.MessageTemplate.Text.Contains("erro de negócio"))
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri))
                {
                    AutoRegisterTemplate = true,
                    ModifyConnectionSettings = x => x.BasicAuthentication(elasticUsername, elasticPassword),
                    IndexFormat = $"{elasticIndex}-{{0:yyyy.MM.dd}}"
                })
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
                .CreateLogger();
        }
    }
}
