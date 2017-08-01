using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Nest;

namespace HQF.Tutorials.ElasticSearch
{
    public class ElasticSearchDemo
    {

        public int connectElasticSearch()
        {
            var connection = new MyCustomHttpConnection();
            var connectionPool = new SingleNodeConnectionPool(new Uri("http://172.16.1.27:9200"));
            var settings = new ConnectionSettings(connectionPool, connection).DefaultIndex("people");
            var client = new ElasticClient(settings);


            var person = new Person
            {
                Id = 1,
                FirstName = "Martijn",
                LastName = "Laarman"
            };


            var indexResponse = client.Index(person);

            var searchResponse = client.Search<Person>(s => s
                .From(0)
                .Size(10)
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.FirstName)
                        .Query("Martijn")
                    )
                )
            );

            var people = searchResponse.Documents;

            Debug.WriteLine("People Count: [{0}] ",people.Count);


            return people.Count;

        }


        public void InsertData<T>(T t) where T : class
        {
            if (t == null)
            {
                throw new ArgumentNullException("t is null!");
            }
            var client = GetElasticClient();

            client.Index(t);

        }

        public List<T> GetData<T>(string firstName) where T : class
        {
            var t= new List<T>();

            if (!string.IsNullOrEmpty(firstName))
            {
                var client = GetElasticClient();

                var searchRequest = new SearchRequest<Person>(Nest.Indices.All, Types.All)
                {
                    From = 0,
                    Size = 10,
                    Query = new MatchQuery
                    {
                        Field = Infer.Field<Person>(f => f.FirstName),
                        Query = firstName
                    }
                };

                var searchResponse =  client.Search<T>(searchRequest);
                t = searchResponse.Documents.ToList();
                
            }

            return t;
        }

        


        #region private functions

        private ElasticClient GetElasticClient(string indexName= "people")
        {
            var uris = new[]
            {
                new Uri("http://172.16.1.27:9200"),
            
            };

            var connectionPool = new SniffingConnectionPool(uris);
            var settings = new ConnectionSettings(connectionPool)
                .DefaultIndex(indexName);

            var client = new ElasticClient(settings);

            return client;
        }

        #endregion
    }
}
