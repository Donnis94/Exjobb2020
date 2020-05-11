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
        private const string IndexName = "exjobb";
        private const string AdvancedName = "advanced";

        public static async Task Main()
        {
            await Run();
        }

        private static async Task Run()
        {
            var client = Shared.Client;
            await InsertData(client);
            await InsertAdvanced(client);
            await ExecuteQueries(client);
            await DeleteIndex(client);
        }

        private static async Task InsertData(ElasticClient client)
        {
            var toString = File.ReadAllText(@"{path to the file inserts.json}");
            var items = JsonConvert.DeserializeObject<List<Information>>(toString);
            var sw = new Stopwatch();
            sw.Start();
            await client.IndexManyAsync(items,IndexName);
            sw.Stop();
            var result = sw.ElapsedMilliseconds;
            Console.WriteLine($"The insertion of ITEMS was executed in {result} ms");
        }

        private static async Task InsertAdvanced(ElasticClient client)
        {
            var toString = File.ReadAllText(@"{path to the file advanced.json}");
            var items = JsonConvert.DeserializeObject<List<AdvancedItems>>(toString);
            var sw = new Stopwatch();
            sw.Start();
            await client.IndexManyAsync(items, AdvancedName);
            sw.Stop();
            var result = sw.ElapsedMilliseconds;
            Console.WriteLine($"The insertion of the ADVANCED ITEMS was executed in {result} ms");
        }

        private static async Task ExecuteQueries(ElasticClient client)
        {
            await SimpleQuery(client);
            await MediumQuery(client);
            await AllQuery(client);
            //await ComplexQuery(client);

        }

        private static async Task DeleteIndex(ElasticClient client)
        {
            client.Indices.Delete(IndexName);
            client.Indices.Delete(AdvancedName);
        }

        private static async Task SimpleQuery(ElasticClient client)
        {
            var sw = new Stopwatch();
            sw.Start();
            var searchResponse = client.Search<Information>(s => s.Index(IndexName).Query(q => q.Match(m=>m.Field(f => f.ObjectId=="1"))));
            sw.Stop();
            var result = sw.ElapsedMilliseconds;
            Console.WriteLine($"The SIMPLE QUERY was executed in {result} ms");
        }

        private static async Task MediumQuery(ElasticClient client)
        {
            var sw = new Stopwatch();
            sw.Start();
            var searchResponse = client.Search<Information>(s => s.Index(IndexName).Query(q => q.Match(m => m.Field(f => f.Info == "Video about sky"))));
            sw.Stop();
            var result = sw.ElapsedMilliseconds;
            Console.WriteLine($"The MEDIUM QUERY was executed in {result} ms");
        }

        private static async Task AllQuery(ElasticClient client)
        {
            var sw = new Stopwatch();
            sw.Start();
            var searchResponse = client.Search<Information>(s => s.Index(IndexName).Query(q => q.MatchAll()));
            sw.Stop();
            var result = sw.ElapsedMilliseconds;
            Console.WriteLine($"The ALL QUERY was executed in {result} ms");
        }

        private static async Task ComplexQuery(ElasticClient client)
        {
            var sw = new Stopwatch();
            sw.Start();
            var searchResponse = client.Search<AdvancedItems>(s => s.Index(AdvancedName).Query(q => q.Match(m => m.Field(f => f.));
            sw.Stop();
            var result = sw.ElapsedMilliseconds;
            Console.WriteLine($"The ALL QUERY was executed in {result} ms");
        }


    }
}
