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
    }
}
