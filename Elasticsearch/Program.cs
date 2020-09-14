using Elasticsearch.Net;
using System;
using System.Configuration;
using Nest;
using Microsoft.Diagnostics.Tracing.Parsers;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using testExjobb;

namespace elasticsearch
{
    class Program
    {
        private const string Test100 = "test100";
        private const string Test1000 = "test1000";
        private const string Test10000 = "test10000";
        private const string AdvancedName = "advanced";

        public static async Task Main()
        {
            await Run();
        }

        private static async Task Run()
        {
            var client = Shared.Client;
            await InsertData(client, Test100);
            await InsertData(client, Test1000);
            await InsertData(client, Test10000);
            await InsertAdvanced(client);
            await ExecuteQueries(client, Test100);
            await ExecuteQueries(client, Test1000);
            await ExecuteQueries(client, Test10000);
            await ComplexQueryOne(client);
            await DeleteIndex(client);
        }

        private static async Task InsertData(ElasticClient client, String indexName)
        {
            string toString = "";
            string indexType = "";
            int amount = 0;
            if (indexName == "test100")
            {
                toString = File.ReadAllText(@"INSERT FILEPATH");
                indexType = "test100";
                amount = 100;
            }
            else if (indexName == "test1000")
            {
                toString = File.ReadAllText(@"INSERT FILEPATH");
                indexType = "test1000";
                amount = 1000;
            }
            else if (indexName == "test10000")
            {
                toString = File.ReadAllText(@"INSERT FILEPATH");
                indexType = "test10000";
                amount = 10000;
            }
            var items = JsonConvert.DeserializeObject<List<Information>>(toString);
            var sw = new Stopwatch();
            sw.Start();
            await client.IndexManyAsync(items, indexType);
            sw.Stop();
            var result = sw.ElapsedMilliseconds;
            Console.WriteLine($"The insertion of {amount} ITEMS was executed in {result} ms \n");
        }

        private static async Task InsertAdvanced(ElasticClient client)
        {
            var toString = File.ReadAllText(@"INSERT FILEPATH");
            var items = JsonConvert.DeserializeObject<List<AdvancedItems>>(toString);
            var createIndexResponse = client.Indices.Create(AdvancedName, c => c
                    .Map<AdvancedItems>(m => m.AutoMap()));
            var sw = new Stopwatch();
            sw.Start();
            await client.IndexManyAsync(items, AdvancedName);
            sw.Stop();
            var result = sw.ElapsedMilliseconds;
            Console.WriteLine($"The insertion of the ADVANCED ITEMS was executed in {result} ms");
        }

        private static async Task ExecuteQueries(ElasticClient client, String IndexName)
        {
            await SimpleQuery(client, IndexName);
            await MediumQuery(client, IndexName);
            await AllQuery(client, IndexName);
        }

        private static async Task DeleteIndex(ElasticClient client)
        {
            client.Indices.Delete(Test100);
            client.Indices.Delete(Test1000);
            client.Indices.Delete(Test10000);
            client.Indices.Delete(AdvancedName);
        }

        private static async Task SimpleQuery(ElasticClient client, String IndexName)
        {
            int amount = 0;
            if (IndexName == "test100")
            {
                amount = 100;
            }
            else if (IndexName == "test1000")
            {
                amount = 1000;
            }
            else if (IndexName == "test10000")
            {
                amount = 10000;
            }
            var sw = new Stopwatch();
            sw.Start();
            var searchResponse = client.Search<Information>(s => s.Index(IndexName).Query(q => q.Match(m => m.Field(f => f.ObjectId == "1"))));
            sw.Stop();
            var result = sw.ElapsedMilliseconds;
            Console.WriteLine($"The SIMPLE QUERY for {amount} ITEMS was executed in {result} ms \n");
        }

        private static async Task MediumQuery(ElasticClient client, String IndexName)
        {
            int amount = 0;
            if (IndexName == "test100")
            {
                amount = 100;
            }
            else if (IndexName == "test1000")
            {
                amount = 1000;
            }
            else if (IndexName == "test10000")
            {
                amount = 10000;
            }
            var sw = new Stopwatch();
            sw.Start();
            var searchResponse = client.Search<Information>(s => s.Index(IndexName).Query(q => q.Match(m => m.Field(f => f.Info == "Video about sky"))));
            sw.Stop();
            var result = sw.ElapsedMilliseconds;
            Console.WriteLine($"The MEDIUM QUERY for {amount} ITEMS was executed in {result} ms \n");
        }

        private static async Task AllQuery(ElasticClient client, String IndexName)
        {
            int amount = 0;
            if (IndexName == "test100")
            {
                amount = 100;
            }
            else if (IndexName == "test1000")
            {
                amount = 1000;
            }
            else if (IndexName == "test10000")
            {
                amount = 10000;
            }
            var sw = new Stopwatch();
            sw.Start();
            var searchResponse = client.Search<Information>(s => s.Index(IndexName).Query(q => q.MatchAll()));
            sw.Stop();
            var result = sw.ElapsedMilliseconds;
            Console.WriteLine($"The ALL QUERY for {amount} ITEMS was executed in {result} ms");
        }

        private static async Task ComplexQueryOne(ElasticClient client)
        {
            var sw = new Stopwatch();
            sw.Start();
            var searchResponse = client.Search<AdvancedItems>(s => s.Index(AdvancedName)
              .Aggregations(a => a
                  .Terms("documents_per_country", f => f.Field(c => c.Country.Suffix("keyword")))));
            sw.Stop();
            var result = sw.ElapsedMilliseconds;
            Console.WriteLine($"The COMPLEX QUERY was executed in {result} ms");
            Console.WriteLine(searchResponse);

        }
    }
}
