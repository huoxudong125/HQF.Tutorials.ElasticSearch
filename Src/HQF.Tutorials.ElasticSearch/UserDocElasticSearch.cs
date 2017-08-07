using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Elasticsearch.Net;
using Nest;

namespace HQF.Tutorials.ElasticSearch
{
    public class UserDocElasticSearch
    {
        private readonly string _elasticSearchUrl = "http://172.16.1.27:9200/";
        private Random rand=new Random();

        public void InitUsers()
        {
            var connection = new MyCustomHttpConnection();
            var connectionPool = new SingleNodeConnectionPool(new Uri(_elasticSearchUrl));
            var settings = new ConnectionSettings(connectionPool, connection).DefaultIndex("user");
            var client = new ElasticClient(settings);

            for (int i = 51; i < 100; i++)
            {
                var user=new UserDoc()
                {
                    Id=i+1,
                    IsArtist = rand.Next()>0.5,
                    NickName = "名"+i,
                    ProductCount = rand.Next(0,100),
                    ShopId = i+200
                };
                var indexResponse = client.Index(user);

                Debug.WriteLine(indexResponse.ToString());
            }

        }



        public List<UserDoc> SearchUser(string nickName)
        {
            var connection = new MyCustomHttpConnection();
            var connectionPool = new SingleNodeConnectionPool(new Uri(_elasticSearchUrl));
            var settings = new ConnectionSettings(connectionPool, connection).DefaultIndex("user");
            var client = new ElasticClient(settings);


            var searchResponse = client.Search<UserDoc>(s => s
                .From(0)
                .Size(10)
                .Query(q => q//.Match(m=>m.Query(nickName))

                    .Bool(b => b.Must(mu => mu.Match(
                            m => m
                                .Field(f => f.NickName)
                                .Query(nickName)
                        ))
                    .Filter(fi => fi.Range(r => r.Field(f => f.ProductCount)
                        .GreaterThan(0)
                    ))
                    )

                )
                
            );

            var userDocs = searchResponse.Documents;

            Debug.WriteLine("user Count: [{0}] ", userDocs.Count);


            return userDocs.ToList();
        }
    }

    public class UserDoc
    {
        public int Id { get; set; }
        public string NickName { get; set; }
        public bool IsArtist { get; set; }
        public int ProductCount { get; set; }
        public int ShopId { get; set; }
    }
}