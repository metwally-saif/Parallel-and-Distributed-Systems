using System.Diagnostics;

namespace section1_task1
{
    internal class Program
    {
        public static void ParallelBubbleSortArray(int[] array, int numOfTasks)
        {
            int n = array.Length;

            numOfTasks = Math.Min(numOfTasks, n / 2);
            Console.WriteLine($"Using {numOfTasks} tasks");

            bool swapped;

            // Optimize even/odd phases
            for (int i = 0; i < n - 1; i++)
            {
                swapped = false;

                // Parallelize the even phase
                if (i % 2 == 0)
                {
                    Parallel.For(0, n / 2, new ParallelOptions { MaxDegreeOfParallelism = numOfTasks }, j =>
                    {
                        int evenIndex = j * 2;
                        if (evenIndex + 1 < n && array[evenIndex] > array[evenIndex + 1])
                        {
                            Swap(array, evenIndex, evenIndex + 1);
                            swapped = true;
                        }
                    });
                }
                // Parallelize the odd phase
                else
                {
                    Parallel.For(0, (n - 1) / 2, new ParallelOptions { MaxDegreeOfParallelism = numOfTasks }, j =>
                    {
                        int oddIndex = j * 2 + 1;
                        if (oddIndex + 1 < n && array[oddIndex] > array[oddIndex + 1])
                        {
                            Swap(array, oddIndex, oddIndex + 1);
                            swapped = true;
                        }
                    });
                }

                if (!swapped) break;
            }
        }

        private static void Swap(int[] array, int i, int j)
        {
            (array[j], array[i]) = (array[i], array[j]);
        }

        private static int[] GenerateRandomArray(int size)
        {
            Random rand = new();
            return Enumerable.Range(0, size).Select(_ => rand.Next(1, 100000)).ToArray();
        }


        static void Main()
        {
            int arraySize = 100000;
            int[] originalArray = GenerateRandomArray(arraySize);

            foreach (int numThreads in new[] { 2, 3, 4, 6 })
            {
                // Copy the original array for each run
                int[] arrayToSort = (int[])originalArray.Clone();

                Stopwatch stopwatch = Stopwatch.StartNew();
                ParallelBubbleSortArray(arrayToSort, numThreads);
                stopwatch.Stop();

                Console.WriteLine($"Time taken with {numThreads} thread(s): {stopwatch.ElapsedMilliseconds} ms");
            }
        }
    }
}
