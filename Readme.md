# Readme


## DSL

Fluent API Query

``` csharp
q
.Bool(b => b
    .MustNot(m => m.MatchAll())
    .Should(m => m.MatchAll())
    .Must(m => m.MatchAll())
    .Filter(f => f.MatchAll())
    .MinimumShouldMatch(1)
    .Boost(2))
```

Object initializer Syntax

```csharp
// more detail find https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/bool-query-usage.html
var query=new BoolQuery
{
    MustNot = new QueryContainer[] { new MatchAllQuery() },
    Should = new QueryContainer[] { new MatchAllQuery() },
    Must = new QueryContainer[] { new MatchAllQuery() },
    Filter = new QueryContainer[] { new MatchAllQuery() },
    MinimumShouldMatch = 1,
    Boost = 2
}
var searchRequest = new SearchRequest(EsIndex.USER_INDEX, EsType.USER_TYPE)
{
    From = startIndex,
    Size = pageSize,
    Query =query
                    
};

var searchResponse = SearchClientFactory.GetClient().Search<UserDoc>(searchRequest);
```

## References


[Elasticsearch: 权威指南](https://www.elastic.co/guide/cn/elasticsearch/guide/current/index.html)   
[NEST - High level client  ](https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/nest-getting-started.html)     
[Community Contributed Clients](https://www.elastic.co/guide/en/elasticsearch/client/community/current/index.html#dotnet)  