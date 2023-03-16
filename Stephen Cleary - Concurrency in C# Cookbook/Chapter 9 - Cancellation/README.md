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

## 9.4. Canceling async Code
You are using async code and need to support cancellation.

The easiest way to support cancellation in asynchronous code is to just pass the CancellationToken through to the next layer.

```C#

public async Task<int> CancelableMethodAsync(CancellationToken cancellationToken)
{
    await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
    return 42;
}

```

Cancellation should be provided as an option whenever possible. This is because proper cancellation at a higher level depends on proper cancellation at the lower level. 

## 9.5. Canceling Parallel Code
You are using parallel code and need to support cancellation.

The easiest way to support cancellation is to pass the CancellationToken through to the parallel code. Parallel methods support this by taking a ParallelOptions instance. 

```C#

static void RotateMatrices(IEnumerable<Matrix> matrices, float degrees, CancellationToken token)
{
    Parallel.ForEach(matrices,
    new ParallelOptions { CancellationToken = token },
    matrix => matrix.Rotate(degrees));
}

// the alternative way is to observe CancellationTOken directly
// not recommended
static void RotateMatrices2(IEnumerable<Matrix> matrices, float degrees,
CancellationToken token)
{
    // Warning: not recommended; see below.
    Parallel.ForEach(matrices, matrix =>
    {
        matrix.Rotate(degrees);
        token.ThrowIfCancellationRequested();
    });
}

```

Parallel LINQ (PLINQ) also has built-in support for cancellation, via the WithCancellation operator:

```C#

static IEnumerable<int> MultiplyBy2(IEnumerable<int> values, CancellationToken cancellationToken)
{
    return values.AsParallel()
    .WithCancellation(cancellationToken)
    .Select(item => item * 2);
}

```

## 9.6. Canceling Reactive Code
You have some reactive code, and you need it to be cancelable.

The Reactive Extensions library has a notion of a subscription to an observable stream. Your code can dispose of the subscription to unsubscribe from the stream. 

```C#

private IDisposable _mouseMovesSubscription;
private void StartButton_Click(object sender, RoutedEventArgs e)
{
    var mouseMoves = Observable
        .FromEventPattern<MouseEventHandler, MouseEventArgs>(
            handler => (s, a) => handler(s, a),
            handler => MouseMove += handler,
            handler => MouseMove -= handler)
        .Select(x => x.EventArgs.GetPosition(this));
    _mouseMovesSubscription = mouseMoves.Subscribe(val =>
    {
        MousePositionLabel.Content = "(" + val.X + ", " + val.Y + ")";
    });
}
private void CancelButton_Click(object sender, RoutedEventArgs e)
{
    if (_mouseMovesSubscription != null)
        _mouseMovesSubscription.Dispose();
}

```

However, it can be really convenient to make Rx work with the CancellationToken Source/CancellationToken system that everything else uses for cancellation.

## 9.7. Canceling Dataflow Meshes
You are using dataflow meshes and need to support cancellation.

The best way to support cancellation in your own code is to pass the CancellationToken through to a cancelable API. Each block in a dataflow mesh supports cancellation as a part of its DataflowBlockOptions. If we want to extend our custom dataflow block with cancellation support, we just set the CancellationToken property on the block options:

```C#

IPropagatorBlock<int, int> CreateMyCustomBlock(CancellationToken cancellationToken)
{
	var blockOptions = new ExecutionDataflowBlockOptions
	{
		CancellationToken = cancellationToken
	};
	var multiplyBlock = new TransformBlock<int, int>(item => item * 2, blockOptions);
	var addBlock = new TransformBlock<int, int>(item => item + 2, blockOptions);
	var divideBlock = new TransformBlock<int, int>(item => item / 2, blockOptions);
	var flowCompletion = new DataflowLinkOptions
	{
		PropagateCompletion = true
	};
	
	multiplyBlock.LinkTo(addBlock, flowCompletion);
	addBlock.LinkTo(divideBlock, flowCompletion);
	
	return DataflowBlock.Encapsulate(multiplyBlock, divideBlock);
}

```

## 9.8. Injecting Cancellation Requests
You have a layer of your code that needs to respond to cancellation requests and also issue its own cancellation requests to the next layer.

The .NET 4.0 cancellation system has built-in support for this scenario, known as linked cancellation tokens. A cancellation token source can be created linked to one (or many) existing tokens. When you create a linked cancellation token source, the resulting token is canceled when any of the existing tokens is canceled or when the linked source is explicitly canceled.

```C#

async Task<HttpResponseMessage> GetWithTimeoutAsync(string url, CancellationToken cancellationToken)
{
	var client = new HttpClient();
	using (var cts = CancellationTokenSource
		.CreateLinkedTokenSource(cancellationToken))
	{
		cts.CancelAfter(TimeSpan.FromSeconds(2));
		var combinedToken = cts.Token;
		return await client.GetAsync(url, combinedToken);
	}
}

```

The resulting combinedToken is canceled when either the user cancels the existing cancellationToken or when the linked source is canceled by CancelAfter. CreateLinkedTokenSource method can take any number of cancellation tokens as parameters.

To prevent memory leaks, dispose of the linked cancellation token source when you no longer need the combined token.

## 9.9. Interop with Other Cancellation Systems
You have some external or legacy code with its own notion of cancellation, and you want to control it using a standard CancellationToken.

For example, let’s say we’re wrapping the System.Net.NetworkInformation. Ping type and we want to be able to cancel a ping. The Ping class already has a Task-based API but does not support CancellationToken. Instead, the Ping type has its own SendAsyncCancel method that we can use to cancel a ping. So, we register a callback that
invokes that method, as follows:

```C#

async Task<PingReply> PingAsync(string hostNameOrAddress, CancellationToken cancellationToken)
{
    var ping = new Ping();

    using (cancellationToken.Register(() => ping.SendAsyncCancel()))
    {
        return await ping.SendPingAsync(hostNameOrAddress);
    }
}

```

Now, when a cancellation is requested, it will invoke the SendAsyncCancel method for us, canceling the SendPingAsync method.