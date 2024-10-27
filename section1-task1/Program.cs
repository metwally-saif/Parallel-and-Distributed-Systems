namespace section1_task1
{
    internal class Program
    {
        public static void ParallelBubbleSortArray(int[] array)
        {
            int n = array.Length;
            bool swapped;

            for (int i = 0; i < n - 1; i++)
            {
                swapped = false;

                // Create tasks for the two halves of the array
                Task[] tasks =
                [
                    // First half
                    Task.Run(() =>
                    {
                        for (int j = 0; j < n - i - 1; j += 2)
                        {
                            if (j + 1 < n && array[j] > array[j + 1])
                            {
                                Swap(array, j, j + 1);
                                swapped = true;
                            }
                        }
                    }),
                    // Second half
                    Task.Run(() =>
                    {
                        for (int j = 1; j < n - i - 1; j += 2)
                        {
                            if (j + 1 < n && array[j] > array[j + 1])
                            {
                                Swap(array, j, j + 1);
                                swapped = true;
                            }
                        }
                    }),
                ];

                // Wait for both tasks to complete
                Task.WaitAll(tasks);

                // If no elements were swapped, the array is sorted
                if (!swapped)
                    break;
            }
        }

        private static void Swap(int[] array, int i, int j)
        {
            (array[j], array[i]) = (array[i], array[j]);
        }

        static void Main()
        {
            int[] array = { 64, 34, 25, 12, 22, 11, 90 };
            Console.WriteLine("Original array: " + string.Join(", ", array));

            ParallelBubbleSortArray(array);

            Console.WriteLine("Sorted array: " + string.Join(", ", array));
        }
    }
}
