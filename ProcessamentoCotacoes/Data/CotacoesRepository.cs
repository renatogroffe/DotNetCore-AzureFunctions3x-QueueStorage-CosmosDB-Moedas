using System;
using System.Linq;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using ProcessamentoCotacoes.Models;

namespace ProcessamentoCotacoes.Data
{
    public static class CotacoesRepository
    {
        private const string DB_COTACOES = "DBMoedas";
        private const string COLLECTION_COTACOES = "Cotacoes";

        static CotacoesRepository()
        {
            using var client = GetDocumentClient();

            client.CreateDatabaseIfNotExistsAsync(
                new Database { Id = DB_COTACOES }).Wait();

            DocumentCollection collectionInfo = new DocumentCollection();
            collectionInfo.Id = COLLECTION_COTACOES;

            collectionInfo.IndexingPolicy =
                new IndexingPolicy(new RangeIndex(DataType.String) { Precision = -1 });

            client.CreateDocumentCollectionIfNotExistsAsync(
                UriFactory.CreateDatabaseUri(DB_COTACOES),
                collectionInfo,
                new RequestOptions { OfferThroughput = 400 }).Wait();
        }        

        private static DocumentClient GetDocumentClient()
        {
            return new DocumentClient(
                new Uri(Environment.GetEnvironmentVariable("DBCotacoesEndpointUri")),
                Environment.GetEnvironmentVariable("DBCotacoesEndpointPrimaryKey"));
        }

        public static void Save(Cotacao cotacao)
        {
            var horario = DateTime.Now;
            var document = new CotacaoDocument();
            
            document.id = $"{cotacao.Codigo}-" +
                horario.ToString("yyyyMMdd-HHmmss");
            document.Codigo = cotacao.Codigo;
            document.Valor = cotacao.Valor.Value;
            document.Data = horario.ToString("yyyy-MM-dd HH:mm:ss");

            using var client = GetDocumentClient();
            client.CreateDocumentAsync(
               UriFactory.CreateDocumentCollectionUri(
                   DB_COTACOES, COLLECTION_COTACOES), document).Wait();
        }

        public static object GetAll()
        {
            using var client = GetDocumentClient();
            FeedOptions queryOptions =
                new FeedOptions { MaxItemCount = -1 };
            return client.CreateDocumentQuery(
                UriFactory.CreateDocumentCollectionUri(
                    DB_COTACOES, COLLECTION_COTACOES),
                    "SELECT C.id, C.Codigo, C.Valor, C.Data " +
                    "FROM C " +
                    "ORDER BY C.id DESC", queryOptions)
                .ToList();
        }
    }
}