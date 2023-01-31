# Chapter 2 - Async Basics

## 2.1 Pausing for a Period of Time

Use Task.Delay(x) - returns a task that completes after the specified time.

```C#
static async Task<T> DelayResult<T>(T result, TimeSpan delay)
{
    await Task.Delay(delay);
    return result;
}
```

Task.Delay is a fine option for unit testing asynchronous code or for implementing
retry logic. However, if you need to implement a timeout, a CancellationToken is usu‐
ally a better choice.

## 2.2 Returning Completed Tasks

You need to implement a synchronous method with an asynchronous signature.

Use Task.FromResult() method.

```C#
interface IMyAsyncInterface
{
    Task<int> GetValueAsync();
}
class MySynchronousImplementation : IMyAsyncInterface
{
    public Task<int> GetValueAsync()
    {
        return Task.FromResult(13);
    }
}
```

Task.FromResult() provides synchronous tasks only for successful results. If you need a task with different kind of result (e.g., a task that is completed with a NotImplementedException), then you can create your own hlper method using TaskCompletionSource:

```C#
static Task<T> NotImplementedAsync<T>()
{
    var tcs = new TaskCompletionSource<T>();
    tcs.SetException(new NotImplementedException());
    return tcs.Task;
}
```

Consider cahing the actual task, if you regularly use Task.FromResult() with the same value.

## 2.3 Reporting Progress

Use the provided IProgress<T> and Progress<T> types. Your async method should
take an IProgress<T> argument; the T is whatever type of progress you need to report:

```C#
static async Task MyMethodAsync(IProgress<double> progress = null)
{
    double percentComplete = 0;
    while (!done)
    {
        ...
        if (progress != null)
        progress.Report(percentComplete);
    }
}
```

Calling code can use it as such:

```C#
static async Task CallMyMethodAsync()
{
    var progress = new Progress<double>();
    progress.ProgressChanged += (sender, args) =>
    {
        ...
    };
    await MyMethodAsync(progress);
}
```

## 2.4 Waiting for a Set of Tasks to Complete

The framework provides a Task.WhenAll method for this purpose.

```C#
Task task1 = Task.Delay(TimeSpan.FromSeconds(1));
Task task2 = Task.Delay(TimeSpan.FromSeconds(2));
Task task3 = Task.Delay(TimeSpan.FromSeconds(1));
await Task.WhenAll(task1, task2, task3);
```

## 2.5 Waitnig for Any Taskto Complete

Use the Task.WhenAny() method. The result of the returned task is the task that completed.

```C#
// Returns the length of data at the first URL to respond.
private static async Task<int> FirstRespondingUrlAsync(string urlA, string urlB)
{
    var httpClient = new HttpClient();

    // Start both downloads concurrently.
    Task<byte[]> downloadTaskA = httpClient.GetByteArrayAsync(urlA);
    Task<byte[]> downloadTaskB = httpClient.GetByteArrayAsync(urlB);

    // Wait for either of the tasks to complete.
    Task<byte[]> completedTask =
    await Task.WhenAny(downloadTaskA, downloadTaskB);

    // Return the length of the data retrieved from that URL.
    byte[] data = await completedTask;
    return data.Length;
}
```

The task returned by Task.WhenAny never completes in a faulted or canceled state. It
always results in the first Task to complete; if that task completed with an exception,
then the exception is not propogated to the task returned by Task.WhenAny. For this
reason, you should usually await the task after it has completed.