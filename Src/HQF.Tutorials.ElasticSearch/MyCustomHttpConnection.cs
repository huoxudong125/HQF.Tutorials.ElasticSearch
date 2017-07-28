using System.Net;
using Elasticsearch.Net;

namespace HQF.Tutorials.ElasticSearch
{
    public class MyCustomHttpConnection : HttpConnection
    {
        protected override void AlterServicePoint(ServicePoint requestServicePoint, RequestData requestData)
        {
            base.AlterServicePoint(requestServicePoint, requestData);
            requestServicePoint.ConnectionLimit = 10000;
            requestServicePoint.UseNagleAlgorithm = true;
        }
    }
}