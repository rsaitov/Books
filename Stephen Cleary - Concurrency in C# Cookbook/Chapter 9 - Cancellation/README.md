# Chapter 9 - Cancellation

The .NET 4.0 framework introduced exhaustive and well-designed cancellation support. This support is cooperative, which means that cancellation can be requested but not enforced on code. Since cancellation is cooperative, it is not possible to cancel code unless it is written to support cancellation. 

Cancellation is treated as a special kind of error. The convention is that canceled code will throw an exception of type OperationCanceledException (or a derived type, such as TaskCanceledException). This way the calling code knows that the cancellation was observed.

To indicate to calling code that your method supports cancellation, you should take a CancellationToken as a parameter. This parameter is usually the last parameter.

```C#

public void CancelableMethodWithOverload(CancellationToken cancellationToken)
{
    // code goes here
}

public void CancelableMethodWithOverload()
{
    CancelableMethodWithOverload(CancellationToken.None);
}

public void CancelableMethodWithDefault(
    CancellationToken cancellationToken = default(CancellationToken))
{
    // code goes here
}

```

`CancellationToken.None` is a special value that is equivalent to default(CancellationToken) and represents a cancellation token that will never be canceled. Consumers pass this value when they don’t ever want the operation to be canceled.

## 9.1. Issuing Cancellation Requests
You have cancelable code (that takes a CancellationToken) and you need to cancel it.

The CancellationTokenSource type is the source for a CancellationToken. The CancellationToken only enables code to respond to cancellation requests; the CancellationTokenSource members allow code to request cancellation.

```C#

void IssueCancelRequest()
{
    var cts = new CancellationTokenSource();
    var task = CancelableMethodAsync(cts.Token);

    // At this point, the operation has been started.
    // Issue the cancellation request.
    cts.Cancel();
}

```

When you cancel code, there are three possibilities: it may respond to the cancellation request (throwing OperationCanceledException), it may finish successfully, or it may finish with an error unrelated to the cancellation (throwing a different exception).

```C#

async Task IssueCancelRequestAsync()
{
    var cts = new CancellationTokenSource();
    var task = CancelableMethodAsync(cts.Token);

    // At this point, the operation is happily running.
    // Issue the cancellation request.
    cts.Cancel();

    // (Asynchronously) wait for the operation to finish.
    try
    {
        await task;
        // If we get here, the operation completed successfully
        // before the cancellation took effect.
    }
    catch (OperationCanceledException)
    {
        // If we get here, the operation was canceled before it completed.
    }
    catch (Exception)
    {
        // If we get here, the operation completed with an error
        // before the cancellation took effect.
        throw;
    }
}

```

## 9.2. Responding to Cancellation Requests by Polling
You have a loop in your code that needs to support cancellation.

You should periodically check whether the token has been canceled. 

```C#

public int CancelableMethod(CancellationToken cancellationToken)
{
    for (int i = 0; i != 100; ++i)
    {
        Thread.Sleep(1000); // Some calculation goes here.

        if (i % 1000 == 0) // can add some limit to increase performance
            cancellationToken.ThrowIfCancellationRequested();
    }
    return 42;
}

```

This polling recipe should only be used if you have a processing loop that needs to support cancellation.

## 9.3. Canceling Due to Timeouts
You have some code that needs to stop running after a certain amount of time.

NET 4.5 introduces some convenience methods for cancellation token sources that automatically issue a cancel request based on a timer. 

```C#

// You can pass the timeout into the constructor
async Task IssueTimeoutAsync()
{
    var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
    var token = cts.Token;
    await Task.Delay(TimeSpan.FromSeconds(10), token);
}

// if you already have a CancellationTokenSource instance, 
// you can start a timeout for that instance
async Task IssueTimeoutAsync()
{
    var cts = new CancellationTokenSource();
    var token = cts.Token;
    cts.CancelAfter(TimeSpan.FromSeconds(5));
    await Task.Delay(TimeSpan.FromSeconds(10), token);
}

```

Whenever you need to execute code with a timeout, you should use `CancellationTokenSource` and `CancelAfter` (or the constructor). There are other ways to do the same thing, but using the existing cancellation system is the easiest and most efficient option. 

Remember that the code to be canceled needs to observe the cancellation token; it is not possible to easily cancel uncancelable code.