using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Nest;

namespace HQF.Tutorials.ElasticSearch
{
    public class CustomAnalyer
    {
        private static readonly Random rand = new Random();
        private static ElasticClient Client { get; set; }
        private static string CurrentIndexName { get; set; }

        public static void Do()
        {
            Client = NuSearchConfiguration.GetClient();
            CurrentIndexName = NuSearchConfiguration.CreateIndexName();

            CreateIndex();
            IndexDumps();
            SwapAlias();
        }

        public static void DeleteIndexIfExists()
        {
            if (Client.IndexExists(CurrentIndexName).Exists)
                Client.DeleteIndex(CurrentIndexName);
        }

        private static void CreateIndex()
        {
            Client.CreateIndex(CurrentIndexName, i => i
                .Settings(s => s
                    .NumberOfShards(2)
                    .NumberOfReplicas(0)
                    .Analysis(Analysis)
                )
                .Mappings(m => m
                    .Map<UserDoc>(MapPackage)
                )
            );
        }

        private static TypeMappingDescriptor<UserDoc> MapPackage(TypeMappingDescriptor<UserDoc> map)
        {
            return map
                .AutoMap()
                .Properties(ps => ps
                    .Text(t => t
                        .Name(p => p.Id)
                        .Analyzer("nuget-id-analyzer")
                        .Fields(f => f
                            .Text(p => p.Name("keyword")
                                .Analyzer("nuget-id-keyword"))
                            .Keyword(p => p.Name("raw"))
                        )
                    )
                );
        }

        private static AnalysisDescriptor Analysis(AnalysisDescriptor analysis)
        {
            return analysis
                .Tokenizers(tokenizers => tokenizers
                    .Pattern("nuget-id-tokenizer", p => p.Pattern(@"\W+"))
                )
                .TokenFilters(tokenfilters => tokenfilters
                    .WordDelimiter("nuget-id-words", w => w
                        .SplitOnCaseChange()
                        .PreserveOriginal()
                        .SplitOnNumerics()
                        .GenerateNumberParts(false)
                        .GenerateWordParts()
                    )
                )
                .Analyzers(analyzers => analyzers
                    .Custom("nuget-id-analyzer", c => c
                        .Tokenizer("nuget-id-tokenizer")
                        .Filters("nuget-id-words", "lowercase")
                    )
                    .Custom("nuget-id-keyword", c => c
                        .Tokenizer("keyword")
                        .Filters("lowercase")
                    )
                );
        }

        private static void IndexDumps()
        {
            Console.WriteLine("Setting up a lazy xml files reader that yields packages...");
            var packages = GetUserDocs();

            Console.WriteLine("Indexing documents into elasticsearch...");
            var waitHandle = new CountdownEvent(1);

            var bulkAll = Client.BulkAll(packages, b => b
                .Index(CurrentIndexName)
                .BackOffRetries(2)
                .BackOffTime("30s")
                .RefreshOnCompleted(true)
                .MaxDegreeOfParallelism(4)
                .Size(1000)
            );

            bulkAll.Subscribe(new BulkAllObserver(
                b => { Console.Write("."); },
                e => { throw e; },
                () => waitHandle.Signal()
            ));
            waitHandle.Wait();
            Console.WriteLine("Done.");
        }


        public static void Index(UserDoc userDoc)
        {
            Client = NuSearchConfiguration.GetClient();
            Client.Index(userDoc);
        }

        private static List<UserDoc> GetUserDocs()
        {
            var userDocs = new List<UserDoc>();
            for (var i = 1; i <= 100; i++)
            {
                var user = new UserDoc
                {
                    Id = i + 1,
                    IsArtist = rand.Next() > 0.5,
                    NickName = "名" + i,

                    ProductCount = rand.Next(0, 100),
                    ShopId = i + 200
                };

                userDocs.Add(user);
            }

            return userDocs;
        }

        private static void SwapAlias()
        {
            var indexExists = Client.IndexExists(NuSearchConfiguration.LiveIndexAlias).Exists;

            Client.Alias(aliases =>
            {
                if (indexExists)
                    aliases.Add(a => a.Alias(NuSearchConfiguration.OldIndexAlias)
                        .Index(NuSearchConfiguration.LiveIndexAlias));

                return aliases
                    .Remove(a => a.Alias(NuSearchConfiguration.LiveIndexAlias).Index("*"))
                    .Add(a => a.Alias(NuSearchConfiguration.LiveIndexAlias).Index(CurrentIndexName));
            });

            var oldIndices = Client.GetIndicesPointingToAlias(NuSearchConfiguration.OldIndexAlias)
                .OrderByDescending(name => name)
                .Skip(2);

            foreach (var oldIndex in oldIndices)
                Client.DeleteIndex(oldIndex);
        }
    }
}