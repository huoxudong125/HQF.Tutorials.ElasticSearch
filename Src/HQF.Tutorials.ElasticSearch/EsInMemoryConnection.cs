using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Nest;
using Newtonsoft.Json;

namespace HQF.Tutorials.ElasticSearch
{
   public class EsInMemoryConnection
    {
        /// <summary>
        /// 
        /// 
        /// https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/modifying-default-connection.html
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Person> GetResponseFromInMemory()
        {
            var response = new
            {
                took = 1,
                timed_out = false,
                _shards = new
                {
                    total = 2,
                    successful = 2,
                    failed = 0
                },
                hits = new
                {
                    total = 25,
                    max_score = 1.0,
                    hits = Enumerable.Range(1, 25).Select(i => (object)new
                    {
                        _index = "people",
                        _type = "Person",
                        _id = $"{i}",
                        _score = 1.0,
                        _source = new Person(){ FirstName = $"FirstName {i}" }
                    }).ToArray()
                }
            };

            var responseBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
            var connection = new InMemoryConnection(responseBytes, 200);
            var connectionPool = new SingleNodeConnectionPool(new Uri("http://localhost:9200"));
            var settings = new ConnectionSettings(connectionPool, connection).DefaultIndex("project");
            var client = new ElasticClient(settings);

            var searchResponse = client.Search<Person>(s => s.MatchAll());

            return searchResponse.Documents.ToList();
        }
    }
}
