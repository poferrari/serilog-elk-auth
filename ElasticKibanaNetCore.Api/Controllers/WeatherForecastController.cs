using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElasticKibanaNetCore.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;

            _logger.LogDebug("Chamou o construtor do WeatherForecastController.");
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.LogTrace("Trace no Get do WeatherForecastController.");

            var rng = new Random();
            if (rng.Next() % 3 == 0)
            {
                var ex = new Exception("Ops, não foi possível prosseguir.");

                _logger.LogError(ex, "Código está com bug.");

                _logger.LogWarning(ex, "Deu muito ruim.");

                _logger.LogCritical(ex, "Já era.");

                throw ex;
            }

            try
            {
                _logger.LogInformation("OnGet no IndexModel.");

                return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Código está com bug.");

                _logger.LogWarning(ex, "Deu muito ruim.");

                _logger.LogCritical(ex, "Já era.");
            }

            return null;
        }
    }
}
