using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;

namespace ElasticKibanaNetCore.Web.Extensions
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
                .Enrich.FromLogContext()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri))
                {
                    AutoRegisterTemplate = true,
                    ModifyConnectionSettings = x => x.BasicAuthentication(elasticUsername, elasticPassword),
                    IndexFormat = $"{elasticIndex}-{{0:yyyy.MM.dd}}"
                })
                .CreateLogger();
        }
    }
}
