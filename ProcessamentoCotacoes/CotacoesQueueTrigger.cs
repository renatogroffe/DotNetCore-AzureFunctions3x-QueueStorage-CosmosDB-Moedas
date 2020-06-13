using System.Text.Json;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using ProcessamentoCotacoes.Models;
using ProcessamentoCotacoes.Validators;
using ProcessamentoCotacoes.Data;

namespace ProcessamentoCotacoes
{
    public static class CotacoesQueueTrigger
    {
        [FunctionName("CotacoesQueueTrigger")]
        public static void Run([QueueTrigger("queue-cotacoes", Connection = "AzureWebJobsStorage")]string myQueueItem, ILogger log)
        {
           log.LogInformation($"CotacoesQueueTrigger - Dados: {myQueueItem}");

            Cotacao cotacao = null;
            try
            {
                cotacao = JsonSerializer.Deserialize<Cotacao>(myQueueItem,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
            catch
            {
                log.LogInformation($"CotacoesQueueTrigger - Erro durante a deserialização");
            }
            
            if (cotacao == null)
                return;

            var validationResult = new CotacaoValidator().Validate(cotacao);
            if (validationResult.IsValid)
            {
                log.LogInformation($"CotacoesQueueTrigger - Dados pós formatação: {JsonSerializer.Serialize(cotacao)}");
                CotacoesRepository.Save(cotacao);
                log.LogInformation("CotacoesQueueTrigger - Transação registrada com sucesso!");
            }
            else
            {
                log.LogInformation("CotacoesQueueTrigger - Dados inválidos para a Transação");
            }
        }
    }
}