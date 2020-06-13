using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ProcessamentoCotacoes.Data;

namespace ProcessamentoCotacoes
{
    public static class Cotacoes
    {
        [FunctionName("Cotacoes")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Function Cotacoes - HTTP GET");
            return new OkObjectResult(CotacoesRepository.GetAll());
        }
    }
}