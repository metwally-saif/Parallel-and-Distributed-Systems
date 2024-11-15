using System.Diagnostics;

namespace section1_task2
{
    class Program
    {
        public class Tool
        {
            public required string Barcode { get; set; }
            public required string Description { get; set; }
            public int Type { get; set; }
        }

        private static readonly Random random = new();

        static List<Tool> GenerateInventory(int size)
        {
            var inventory = new List<Tool>();
            for (int i = 0; i < size; i++)
            {
                inventory.Add(new Tool
                {
                    Barcode = $"BC{i:D6}",
                    Description = $"Tool Description {i}",
                    Type = random.Next(1, 101)
                });
            }
            return inventory;
        }

        static readonly Dictionary<int, int> requiredTools = new()
        {
        {1, 30},
        {7, 15},
        {10, 8}
    };

        static async Task<Dictionary<int, List<string>>> SearchInventoryParallel(List<Tool> inventory, int threadCount)
        {
            var result = new Dictionary<int, List<string>>();
            foreach (var type in requiredTools.Keys)
            {
                result[type] = [];
            }

            var remaining = new Dictionary<int, int>(requiredTools);
            var lockObject = new object();
            var cts = new CancellationTokenSource();

            // Calculate chunk size for each thread
            int chunkSize = inventory.Count / threadCount;

            var tasks = new List<Task>();

            for (int i = 0; i < threadCount; i++)
            {
                int startIndex = i * chunkSize;
                int endIndex = (i == threadCount - 1) ? inventory.Count : (i + 1) * chunkSize;

                tasks.Add(Task.Run(() =>
                {
                    for (int j = startIndex; j < endIndex && !cts.Token.IsCancellationRequested; j++)
                    {
                        var tool = inventory[j];

                        if (remaining.TryGetValue(tool.Type, out int value))
                        {
                            lock (lockObject)
                            {
                                if (value > 0)
                                {
                                    result[tool.Type].Add(tool.Barcode);
                                    remaining[tool.Type] = --value;

                                    // Check if we found all required tools
                                    if (remaining.All(r => r.Value == 0))
                                    {
                                        cts.Cancel();
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }, cts.Token));
            }

            try
            {
                await Task.WhenAll(tasks);
            }
            catch (OperationCanceledException)
            {
                // Expected when we find all tools
            }

            return result;
        }

        static async Task Main()
        {
            Console.WriteLine("Generating inventory...");
            var inventory = GenerateInventory(100000);

            int[] threadCounts = { 2, 3, 4, 6 };

            foreach (int threadCount in threadCounts)
            {
                Console.WriteLine($"\nTesting with {threadCount} threads:");

                Stopwatch stopwatch = Stopwatch.StartNew();
                var result = await SearchInventoryParallel(inventory, threadCount);
                stopwatch.Stop();

                Console.WriteLine($"Time taken: {stopwatch.ElapsedMilliseconds}ms");

                foreach (var kvp in result)
                {
                    Console.WriteLine($"Type {kvp.Key}: Found {kvp.Value.Count} tools");
                    Console.WriteLine($"First few barcodes: {string.Join(", ", kvp.Value.Take(3))}...");
                }
            }
        }
    }
}
