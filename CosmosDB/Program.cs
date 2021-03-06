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
using System.Reflection.Metadata;
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
            await InsertionTest("Test100");
            await InsertionTest("Test1000");
            await InsertionTest("Test10000");
            await InsertionTest("Advanced");
            var container = Shared.Client.GetContainer("ToDoList", "Test100");
            var container1000 = Shared.Client.GetContainer("ToDoList", "Test1000");
            var container10000 = Shared.Client.GetContainer("ToDoList", "Test10000");
            var advanced = Shared.Client.GetContainer("ToDoList", "Advanced");
            await ExecuteQueries(container, "Test100");
            await ExecuteQueries(container1000, "Test1000");
            await ExecuteQueries(container10000, "Test10000");
            await ComplexQueryOne(advanced);
            await DeleteContainer(container);
            await DeleteContainer(container1000);
            await DeleteContainer(container10000);
            await DeleteContainer(advanced);
        }


        private static async Task InsertionTest(string type)
        {
            await CreateContainer(type);
            if (type != "Advanced")
                await CreateDocument(type);
            else
                await CreateAdvancedDocument();
        }
        private static async Task CreateContainer(string containerId, int throughput = 1000, string partitionKey = "/info")
        {
            if (containerId == "Test100")
            {
                throughput = 400;
            }          
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

        private static async Task CreateDocument(string containerName)
        {
            var container = Shared.Client.GetContainer("ToDoList", containerName);
            string toString;
            if (containerName == "Test100")
            {
                toString = File.ReadAllText(@"INSERT FILEPATH");
            }
            else if (containerName == "Test1000")
            {
                toString = File.ReadAllText(@"INSERT FILEPATH");
            }
            else
            {
                toString = File.ReadAllText(@"INSERT FILEPATH");
            }
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
            Console.WriteLine($"The insertion of {containerName} was executed in {result} ms");
        }

        private static async Task CreateAdvancedDocument()
        {
            var container = Shared.Client.GetContainer("ToDoList", "Advanced");
            var toString = File.ReadAllText(@"INSERT FILEPATH");
            var items = JsonConvert.DeserializeObject<List<AdvancedItems>>(toString);
            var task = new List<Task>();
            var sw = new Stopwatch();
            sw.Start();
            foreach (var testers in items)
            {
                ItemResponse<List<AdvancedItems>> itemResponse = await container.CreateItemAsync(items);
            }
            Task.WaitAll();
            sw.Stop();
            var result = sw.ElapsedMilliseconds;
            Console.WriteLine($"The insertion of ADVANCED ITEMS was executed in {result} ms");
        }

        private static async Task ExecuteQueries(Container container, string containerName)
        {
            await SimpleQuery(container, containerName);
            await MediumQuery(container, containerName);
            await AllQuery(container, containerName);
        }

        private static async Task DeleteContainer(Container container)
        {
            await container.DeleteContainerAsync();
        }

        private static async Task SimpleQuery(Container container, string containerName)
        {
            var sw = new Stopwatch();
            sw.Start();
            var simpleQuery = container.GetItemLinqQueryable<Items>();
            var iterator1 = simpleQuery.Where(x => x.ObjectId == "1").ToFeedIterator();
            sw.Stop();
            var result = sw.ElapsedMilliseconds;
            foreach (var item in await iterator1.ReadNextAsync())
            {
                Console.WriteLine(item.ObjectName);
            }
            Console.WriteLine($"The SIMPLE query for {containerName} was executed in {result} ms");
        }

        private static async Task MediumQuery(Container container, string containerName)
        {
            var sw = new Stopwatch();
            var task = new List<Task>();
            sw.Start();
            var mediumQuery = container.GetItemLinqQueryable<Items>();
            var iterator2 = mediumQuery.Where(x => x.Info == "Video about sky").ToFeedIterator();
            task.Add(iterator2.ReadNextAsync());
            Task.WaitAll();
            sw.Stop();
            var result = sw.ElapsedMilliseconds;  
            Console.WriteLine($"The MEDIUM query for {containerName} was executed in {result} ms ");
        }
        private static async Task AllQuery(Container container, string containerName)
        {
            var task = new List<Task>();
            var sw = new Stopwatch();
            sw.Start();
            var allItems = container.GetItemLinqQueryable<Items>();
            var allIterator = allItems.ToFeedIterator();
            task.Add(allIterator.ReadNextAsync());
            Task.WaitAll();
            sw.Stop();
            var result = sw.ElapsedMilliseconds;
            Console.WriteLine($"The ALL ITEMS query for {containerName} was executed in {result} ms ");
        }

        private static async Task ComplexQueryOne(Container container)
        {
            var sw = new Stopwatch();
            sw.Restart();
            var query = container.GetItemQueryIterator<QueryResult>(
                "SELECT COUNT (1) AS count, c.country FROM c GROUP BY c.country");
            var results = query.ReadNextAsync().Result.OrderBy(r => r.country).ToList();
            sw.Stop();
            Console.WriteLine(string.Format("Advanced query returned {0} results in {1} milliseconds", results.Count, sw.ElapsedMilliseconds));
        }
    }
}
