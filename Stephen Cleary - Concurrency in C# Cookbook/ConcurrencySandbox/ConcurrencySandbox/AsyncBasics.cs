using System.Diagnostics;

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

        await WaitingForAnyTaskToComplete();

        await ProcessingTasksAsTheyComplete();

        await HandlingExceptionsFromAsyncTaskMethods();

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
            await Task.Delay(5);
            if (progress != null)
            {
                progress.Report(i);
            }

            n = i;
        }

        return n;
    }

    /// <summary>
    /// 2.4 Waiting for a Set of Tasks to Complete
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


    /// <summary>
    /// 2.5 Waiting for Any Task to Complete
    /// </summary>
    public static async Task WaitingForAnyTaskToComplete()
    {
        Task task1 = Task.Delay(TimeSpan.FromMilliseconds(100));
        Task task2 = Task.Delay(TimeSpan.FromMilliseconds(200));
        Task task3 = Task.Delay(TimeSpan.FromMilliseconds(300));
        var result = await Task.WhenAny(task1, task2, task3);

        await result;

        Console.WriteLine("One of the task finished on Task.WhenAny() awaiting");
    }

    /// <summary>
    /// 2.6. Processing Tasks as They Complete
    /// You have a collection of tasks to await, and you want to do some processing on each task after it completes
    /// </summary>
    public static async Task ProcessingTasksAsTheyComplete()
    {
        // Create a sequence of tasks.
        Task<int> taskA = DelayAndReturnAsync(2);
        Task<int> taskB = DelayAndReturnAsync(3);
        Task<int> taskC = DelayAndReturnAsync(1);

        var tasks = new[] { taskA, taskB, taskC };
        var processingTasks = tasks.Select(async t =>
        {
            var result = await t;
            Console.WriteLine($"ProcessingTasksAsTheyComplete: {result}");
        }).ToArray();

        // Await all processing to complete
        await Task.WhenAll(processingTasks);
    }

    static async Task<int> DelayAndReturnAsync(int val)
    {
        await Task.Delay(TimeSpan.FromSeconds(val));
        return val;
    }

    /// <summary>
    /// 2.7 Avoiding Context for Continuations
    /// Catchs and resume on the same context
    /// </summary>
    public static async Task ResumeOnContextAsync()
    {
        await Task.Delay(TimeSpan.FromSeconds(1));

        // This method resumes within the same context...
    }

    /// <summary>
    /// 2.7 Avoiding Context for Continuations
    /// Discards context 
    /// </summary>
    public static async Task ResumeWithoutContextAsync()
    {
        await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);

        // This method discards its context when it resumes.
    }

    /// <summary>
    /// 2.8. Handling Exceptions from async Task Methods
    /// </summary>
    static async Task ThrowExceptionAsync()
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
        throw new InvalidOperationException("Test");
    }
    static async Task HandlingExceptionsFromAsyncTaskMethods()
    {
        Task task = ThrowExceptionAsync();

        try
        {
            // The exception is reraised here, where the task is awaited.
            await task;
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine("InvalidOperationException catched");
        }
    }
}
