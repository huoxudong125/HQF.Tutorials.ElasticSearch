using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Elasticsearch.Net;
using Nest;

namespace HQF.Tutorials.ElasticSearch
{
    public class ElasticSearchHelper
    {
        public void InsertData<T>(T t) where T : class
        {
            if (t == null)
                throw new ArgumentNullException("t is null!");
            var client = GetElasticClient();

            client.Index(t);
        }

        public List<T> GetData<T>(string firstName) where T : class
        {
            var t = new List<T>();

            if (!string.IsNullOrEmpty(firstName))
            {
                var client = GetElasticClient();

                var searchRequest = new SearchRequest<Person>(Indices.All, Types.All)
                {
                    From = 0,
                    Size = 10,
                    Query = new MatchQuery
                    {
                        Field = Infer.Field<Person>(f => f.FirstName),
                        Query = firstName
                    }
                };

                var searchResponse = client.Search<T>(searchRequest);
                t = searchResponse.Documents.ToList();
            }

            return t;
        }


        #region private functions

        private ElasticClient GetElasticClient(string indexName = "people")
        {
            var list = new List<string>();
            var uris = new[]
            {
                new Uri("http://172.16.1.27:9200")
            };

            var connectionPool = new SniffingConnectionPool(uris);
            var settings = new ConnectionSettings(connectionPool)
                .DefaultIndex(indexName)
                .DisableDirectStreaming()
                .OnRequestCompleted(apiCallDetails =>
                {
                    // log out the request and the request body, if one exists for the type of request
                    if (apiCallDetails.RequestBodyInBytes != null)
                        list.Add(
                            $"{apiCallDetails.HttpMethod} {apiCallDetails.Uri} " +
                            $"{Encoding.UTF8.GetString(apiCallDetails.RequestBodyInBytes)}");
                    else
                        list.Add($"{apiCallDetails.HttpMethod} {apiCallDetails.Uri}");

                    // log out the response and the response body, if one exists for the type of response
                    if (apiCallDetails.ResponseBodyInBytes != null)
                        list.Add($"Status: {apiCallDetails.HttpStatusCode}" +
                                 $"{Encoding.UTF8.GetString(apiCallDetails.ResponseBodyInBytes)}");
                    else
                        list.Add($"Status: {apiCallDetails.HttpStatusCode}");
                });

            var client = new ElasticClient(settings);

            return client;
        }

        #endregion
    }
}