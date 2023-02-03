using System;
using System.Collections.Concurrent;
using System.Diagnostics;

public static class ParallelBasics
{
    public async static Task Demonstrate()
    {
        ParallelProcessingOfData();
        ParallelAggregation();
        ParallelInvocation();
    }

    /// <summary>
    /// CPU-bound operations could be divided into different threads
    /// </summary>
    private static void ParallelProcessingOfData()
    {
        var limit = 2_000_000;

        var estimateMilliNotParallel = NotParallelProcessingOfData(limit);
        Console.WriteLine(estimateMilliNotParallel);

        // Parallel: up to 2 times faster
        var estimateMilliParallel = ParallelProcessingOfData(limit);
        Console.WriteLine(estimateMilliParallel);
    }

    private static long NotParallelProcessingOfData(int limit)
    {
        var rand = new Random();
        var numbers = Enumerable.Range(1, limit).Select(x => rand.Next(1, 1_000));
        var list = new List<int>(numbers.Count());

        var stopwatch = new Stopwatch();
        stopwatch.Start();
        foreach (var x in numbers)
        {
            if (IsPrime(x))
                list.Add(x);
        };
        stopwatch.Stop();

        return stopwatch.ElapsedMilliseconds;
    }

    private static long ParallelProcessingOfData(int limit)
    {
        var rand = new Random();
        var numbers = Enumerable.Range(1, limit).Select(x => rand.Next(1, 1_000));
        var bagEven = new ConcurrentBag<int>();

        var stopwatch = new Stopwatch();
        stopwatch.Start();
        Parallel.ForEach(numbers, x =>
        {
            if (IsPrime(x))
                bagEven.Add(x);
        });
        stopwatch.Stop();

        return stopwatch.ElapsedMilliseconds;
    }

    private static bool IsPrime(int number)
    {
        if (number < 2)
        {
            return false;
        }

        for (var divisor = 2; divisor <= Math.Sqrt(number); divisor++)
        {
            if (number % divisor == 0)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 3.2. Parallel Aggregation
    /// </summary>
    private static void ParallelAggregation()
    {
        var limit = 50_000_000;
        var estimate1 = NotParallelAggregation(limit);
        Console.WriteLine(estimate1);

        // PLINQ: up to 2 times faster
        var estimate2 = ParallelAggregation(limit);
        Console.WriteLine(estimate2);
    }

    private static long NotParallelAggregation(int limit)
    {
        var rand = new Random();
        var numbers = Enumerable.Range(1, limit).Select(x => rand.Next(1, 50));

        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var s = numbers.Sum(x => IsPrime(x) ? x : 0);
        stopwatch.Stop();

        return stopwatch.ElapsedMilliseconds;
    }

    private static long ParallelAggregation(int limit)
    {
        var rand = new Random();
        var numbers = Enumerable.Range(1, limit).Select(x => rand.Next(1, 50));

        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var s = numbers.AsParallel().Sum(x => IsPrime(x) ? x : 0);
        stopwatch.Stop();

        return stopwatch.ElapsedMilliseconds;
    }

    /// <summary>
    /// 3.3 Parallel Invocation
    /// </summary>
    private static void ParallelInvocation()
    {
        var rand = new Random();
        var numbers = Enumerable.Range(1, 1000)
            .Select(x => rand.Next(1, 50))
            .ToArray();

        Parallel.Invoke(
                () => ProcessPartialArray(numbers, 0, numbers.Length / 2),
                () => ProcessPartialArray(numbers, numbers.Length / 2, numbers.Length)
        );
    }

    static void ProcessPartialArray(int[] array, int begin, int end)
    {
        for (int i = begin; i < end; i++)
            Console.WriteLine($"Process {i}");
    }
}
