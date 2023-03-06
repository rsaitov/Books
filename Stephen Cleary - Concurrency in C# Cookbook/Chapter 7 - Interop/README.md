# Chapter 7 - Interop

Asynchronous, parallel, reactive - how to combine these different approaches.

## 7.1. Async Wrappers for “Async” Methods with “Completed” Events
There is an older asynchronous pattern that uses methods named `OperationAsync` along with events named `OperationCompleted`. You wish to perform an operation like this and await the result.

You can create wrappers for asynchronous operations by using the TaskCompletion Source<TResult> type. This type controls a Task<TResult> and allows you to complete the task at the appropriate time.

## 7.2. Async Wrappers for “Begin/End” methods
There is an older asynchronous pattern that uses pairs of methods named BeginOperation and EndOperation, with the IAsyncResult representing the asynchronous operation. You have an operation that follows this pattern and wish to consume it with await.

The best approach for wrapping APM is to use one of the FromAsync methods on the TaskFactory type. FromAsync uses TaskCompletionSource<TResult> under the hood, but when you’re wrapping APM, FromAsync is much easier to use.

## 7.3. Async Wrappers for Anything
You have an unusual or nonstandard asynchronous operation or event and wish to consume it via await.

The `TaskCompletionSource<T>` type can be used to construct `Task<T>` objects in any scenario. 

```C#

public static Task<string> DownloadStringAsync(
this IMyAsyncHttpService httpService, Uri address)
{
    var tcs = new TaskCompletionSource<string>();
    httpService.DownloadString(address, (result, exception) =>
    {
        if (exception != null)
            tcs.TrySetException(exception);
        else
            tcs.TrySetResult(result);
    });
    return tcs.Task;
}

```

## 7.4. Async Wrappers for Parallel Code
You have (CPU-bound) parallel processing that you wish to consume using await. Usually, this is desirable so that your UI thread does not block waiting for the parallel processing to complete.

To keep the UI responsive, wrap the parallel processing in a Task.Run and await the result:

```C#

await Task.Run(() => Parallel.ForEach(...));

```

This recipe only applies to UI code. On the server side (e.g., ASP.NET), parallel processing is rarely done. Even if you do perform parallel processing, you should invoke it directly, not push it off to the thread pool.

## 7.5. Async Wrappers for Rx Observables
You have an observable stream that you wish to consume using await.

First, you need to decide which of the observable events in the event stream you’re interested in. The common situations are:
• The last event before the stream ends
• The next event
• All the events

```C#
IObservable<int> observable = ...;

// Last event
int lastElement = await observable.LastAsync();
// or: int lastElement = await observable;

// First event
int nextElement = await observable.FirstAsync();

// All events
IList<int> allElements = await observable.ToList();
```

## 7.6. Rx Observable Wrappers for async Code
You have an asynchronous operation that you want to combine with an observable.

The Rx library includes a simple conversion from Task<T> to IObservable<T> that implements this transformation. The following code starts an asynchronous download of a web page, treating it as an observable sequence:

```C#
var client = new HttpClient();
IObservable<HttpResponseMessage> response =
client.GetAsync("http://www.example.com/")
      .ToObservable();
```

Another approach is to call StartAsync:

```C#

var client = new HttpClient();
IObservable<HttpResponseMessage> response = Observable
    .StartAsync(token => client.GetAsync("http://www.example.com/", token));

```

## 7.7. Rx Observables and Dataflow Meshes
Part of your solution uses Rx observables, and part of your solution uses dataflow meshes, and you need them to communicate.

The
following code creates a buffer block (which does no processing) and creates an observable interface from that block by calling AsObservable:

```C#

var buffer = new BufferBlock<int>();
IObservable<int> integers = buffer.AsObservable();
integers.Subscribe(data => Trace.WriteLine(data),
    ex => Trace.WriteLine(ex),
    () => Trace.WriteLine("Done"));
buffer.Post(13);

```