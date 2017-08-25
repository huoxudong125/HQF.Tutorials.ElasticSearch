using System;
using System.Diagnostics;
using System.Linq;
using Nest;

namespace HQF.Tutorials.ElasticSearch
{
    public static class NuSearchConfiguration
    {
        private static readonly ConnectionSettings _connectionSettings;

        public static string LiveIndexAlias => "momansearch";

        public static string OldIndexAlias => "momansearch-old";

        public static Uri CreateUri(int port)
        {
            var host = "172.16.1.27";
           
            return new Uri("http://" + host + ":" + port);
        }

        static NuSearchConfiguration()
        {
            _connectionSettings = new ConnectionSettings(CreateUri(9200))
                .DefaultIndex("momansearch")
                .InferMappingFor<UserDoc>(i => i
                    .TypeName("userdocs")
                    .IndexName("momansearch")
                );
        }

        public static ElasticClient GetClient()
        {
            return new ElasticClient(_connectionSettings);
        }

        public static string CreateIndexName()
        {
            return $"{LiveIndexAlias}-{DateTime.UtcNow:dd-MM-yyyy-HH-mm-ss}";
        }

    }
}