public static class AsyncBasics
{
    public static async Task Demonstrate()
    {
        Console.WriteLine("--> AsyncBasics demonstration");
        await PauseForPeriod(1);

        var result2 = await ReturnCompletedTask(5);
        Console.WriteLine($"GetResultAsync: {result2}");

        try
        {
            await NotImplementedAsync<int>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception thrown: {ex.Message}");
        }

        var progress = new Progress<int>();
        progress.ProgressChanged += (sender, args) =>
        {
            if (args % 10 == 0)
                Console.WriteLine($"Progress changed: {args}");
        };
        var result3 = await EmulateProgressAsync(progress);

        await WaitingForAsetOfTasksToComplete();

        Console.WriteLine("--> AsyncBasics demonstration finished");
    }

    /// <summary>
    /// You need to (asynchronously) wait for a period of time. This can be useful when unit
    /// testing or implementing retry delays.
    /// </summary>
    static async Task PauseForPeriod(int seconds)
    {
        await Task.Delay(seconds * 1000);
        Console.WriteLine("MakeDelayAsync finished");
    }

    /// <summary>
    /// You need to implement a synchronous method with an asynchronous signature. 
    /// </summary>
    static Task<int> ReturnCompletedTask(int result)
    {
        return Task.FromResult(result);
    }

    /// <summary>
    /// If task result not success
    /// </summary>
    static Task<T> NotImplementedAsync<T>()
    {
        var tcs = new TaskCompletionSource<T>();
        tcs.SetException(new NotImplementedException());
        return tcs.Task;
    }

    /// <summary>
    /// You need to respond to progress while an asynchronous operation is executing.
    /// </summary>
    public static async Task<int> EmulateProgressAsync(IProgress<int> progress)
    {
        int n = 0;
        for (int i = 0; i < 100; i++)
        {
            await Task.Delay(10);
            if (progress != null)
            {
                progress.Report(i);
            }

            n = i;
        }

        return n;
    }
    /// <summary>
    /// You have several tasks and need to wait for them all to complete.
    /// </summary>
    public static async Task WaitingForAsetOfTasksToComplete()
    {
        Task task1 = Task.Delay(TimeSpan.FromMilliseconds(100));
        Task task2 = Task.Delay(TimeSpan.FromMilliseconds(200));
        Task task3 = Task.Delay(TimeSpan.FromMilliseconds(100));
        await Task.WhenAll(task1, task2, task3);

        Console.WriteLine("All tasks finished on Task.WhenAll() awaiting");
    }
}
