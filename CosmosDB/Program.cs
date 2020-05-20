using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
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

namespace testExjobb
{
    class Program
    {
        public static async Task Main()
        {
            await Run();
        }
        public async static Task Run()
        {
            await CreateContainer("Test");
            await CreateDocument();
            await CreateContainer("Advanced");
            await CreateAdvancedDocument();
            await ExecuteQueries();
            var container = Shared.Client.GetContainer("ToDoList", "Test");
            var advanced = Shared.Client.GetContainer("ToDoList", "Advanced");
            await DeleteContainer(container);
            await DeleteContainer(advanced);
        }

        private static async Task CreateContainer(string containerId, int throughput = 10000, string partitionKey = "/info")
        {
            if (containerId == "Advanced")
            {
                
                partitionKey = "/country";
            }
            Console.WriteLine($">>>Create container {containerId} in DB <<<");
            Console.WriteLine();
            Console.WriteLine($"Throughput: {throughput} RU/sec");
            Console.WriteLine($"Partition key : {partitionKey}");
            Console.WriteLine();
            var containerDef = new ContainerProperties
            {
                Id = containerId,
                PartitionKeyPath = partitionKey,
            };
            var database = Shared.Client.GetDatabase("ToDoList");
            await database.CreateContainerAsync(containerDef, throughput);
            Console.WriteLine($"Container created {containerId}");
        }

        private static async Task CreateDocument()
        {
            var container = Shared.Client.GetContainer("ToDoList", "Test");
            var toString = File.ReadAllText(@"{Insert filepath here for the file insertion.json}");
            var items = JsonConvert.DeserializeObject<List<Items>>(toString);
            var sw = new Stopwatch();
            var task = new List<Task>();
            sw.Start();
            foreach (var testers in items)
            {
                task.Add(container.CreateItemAsync(items));
            }
            Task.WaitAll();
            sw.Stop();
            var result = sw.ElapsedMilliseconds;
            Console.WriteLine($"The insertion of SIMPLE ITEMS was executed in {result} ms");
        }

        private static async Task CreateAdvancedDocument()
        {
            var container = Shared.Client.GetContainer("ToDoList", "Advanced");
            var toString = File.ReadAllText(@"{Insert filepath here for the file addvanced.json}");
            var items = JsonConvert.DeserializeObject<List<AdvancedItems>>(toString);
            var task = new List<Task>();
            var sw = new Stopwatch();
            sw.Start();
            foreach (var testers in items)
            {
                task.Add(container.CreateItemAsync(items));
            }
            Task.WaitAll();
            sw.Stop();
            var result = sw.ElapsedMilliseconds;
            Console.WriteLine($"The insertion of ADVANCED ITEMS was executed in {result} ms");
        }

        private static async Task ExecuteQueries(){
            await SimpleQuery();
            await MediumQuery();
            await AllQuery();
            await ComplexQueryOne();
            await ComplexQueryTwo();
        }


        private static async Task DeleteContainer(Container container)
        {
            await container.DeleteContainerAsync();
        }

        private static async Task SimpleQuery()
        {
            var container = Shared.Client.GetContainer("ToDoList", "Test");
            var sw = new Stopwatch();
            sw.Start();
            var mediumQuery = container.GetItemLinqQueryable<Items>();
            var iterator2 = mediumQuery.Where(x => x.ObjectId == "1").ToFeedIterator();
            sw.Stop();
            var result = sw.ElapsedMilliseconds;
            Console.WriteLine($"The SIMPLE query was executed in {result} ms ");
        }

        private static async Task MediumQuery()
        {
            var container = Shared.Client.GetContainer("ToDoList", "Test");
            var sw = new Stopwatch();
            var task = new List<Task>();
            sw.Start();
            var mediumQuery = container.GetItemLinqQueryable<Items>();
            var iterator2 = mediumQuery.Where(x => x.Info == "Video about sky").ToFeedIterator();
            task.Add(iterator2.ReadNextAsync());
            Task.WaitAll();
            sw.Stop();
            var result = sw.ElapsedMilliseconds;
            Console.WriteLine($"The MEDIUM query was executed in {result} ms ");
        }
        private static async Task AllQuery()
        {
            var container = Shared.Client.GetContainer("ToDoList", "Test");
            var task = new List<Task>();
            var sw = new Stopwatch();
            sw.Start();
            var allItems = container.GetItemLinqQueryable<Items>();
            var allIterator = allItems.ToFeedIterator();
            task.Add(allIterator.ReadNextAsync());
            Task.WaitAll();
            sw.Stop();
            var result = sw.ElapsedMilliseconds;
            Console.WriteLine($"The ALL ITEMS query was executed in {result} ms ");
        }

        private static async Task ComplexQueryOne()
        {
            var container = Shared.Client.GetContainer("ToDoList", "Advanced");
            var sw = new Stopwatch();
            sw.Restart();
            var query = container.GetItemQueryIterator<QueryResult>(
                "SELECT COUNT (1) AS count, c.country FROM c GROUP BY c.country");
            var results = query.ReadNextAsync().Result.OrderBy(r => r.country).ToList();
            sw.Stop();
            Console.WriteLine(string.Format("Advanced query returned {0} results in {1} milliseconds", results.Count, sw.ElapsedMilliseconds));
            foreach (var queryResult in results)
                Console.WriteLine(queryResult.country + ": " + queryResult.Count);
        }        
    }   
}
