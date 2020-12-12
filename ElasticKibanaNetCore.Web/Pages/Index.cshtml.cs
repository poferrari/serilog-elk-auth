using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;

namespace ElasticKibanaNetCore.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;

            _logger.LogDebug("Chamou o construtor do IndexModel.");
        }

        public void OnGet()
        {
            _logger.LogTrace("Trace no IndexModel.");

            try
            {
                _logger.LogInformation("OnGet no IndexModel.");

                throw new Exception("Ops, não foi possível prosseguir.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Seu código está bugado.");

                _logger.LogWarning(ex, "Deu muito ruim.");

                _logger.LogCritical(ex, "Já era.");
            }
        }
    }
}
