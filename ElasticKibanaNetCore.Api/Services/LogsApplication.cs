using Microsoft.Extensions.Configuration;
using Nest;
using System;
using System.Collections.Generic;

namespace ElasticKibanaNetCore.Api.Tests
{
    public class LogsApplication : ILogsApplication
    {
        private readonly IElasticClient _elasticClient;
        private readonly string _elasticIndex;

        public LogsApplication(IElasticClient elasticClient,
            IConfiguration configuration)
        {
            _elasticClient = elasticClient;

            _elasticIndex = configuration["ElasticConfiguration:Index"];
        }

        public void PostLogsSample()
        {
            try
            {
                var descriptor = new BulkDescriptor();

                descriptor.UpdateMany(IndexLog.GetSampleData(), (b, u) => b
                    .Index($"{_elasticIndex}-{DateTime.Now:yyyy.MM.dd}")
                    .Doc(u)
                    .DocAsUpsert());

                var insert = _elasticClient.Bulk(descriptor);

                if (!insert.IsValid)
                {
                    throw new Exception(insert.OriginalException.ToString());
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class IndexLog
    {
        public string Id { get; private set; } = Guid.NewGuid().ToString();
        public string Message { get; private set; } = $"Message {Guid.NewGuid()} at {DateTime.UtcNow}";
        [Date(Name = "@timestamp")]
        public DateTime Timestamp { get; private set; } = DateTime.UtcNow;

        public static IEnumerable<IndexLog> GetSampleData()
        {
            var list = new List<IndexLog>()
            {
                new IndexLog(),
                new IndexLog()
            };
            return list;
        }
    }
}
