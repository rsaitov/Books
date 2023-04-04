# Chapter 11 - Synchronization

## 11.1 Blocking Locks
You have some shared data and need to safely read and write it from multiple threads.

The best solution for this situation is to use the lock statement. When a thread enters a lock, it will prevent any other threads from entering that lock until the lock is released:

```C#

class MyClass
{
    // This lock protects the _value field.
    private readonly object _mutex = new object();

    private int _value;

    public void Increment()
    {
        lock (_mutex)
        {
            _value = _value + 1;
        }
    }
}

```

There are four important guidelines when using locks:
- Restrict lock visibility.
- Document what the lock protects.
- Minimize code under lock.
- Never execute arbitrary code while holding a lock.

## 11.2. Async Locks
You have some shared data and need to safely read and write it from multiple code blocks, which may be using await.

The .NET framework SemaphoreSlim type has been updated in version 4.5 to be compatible with async. It can be used as such:

```C#

class MyClass
{
    // This lock protects the _value field.
    private readonly SemaphoreSlim _mutex = new SemaphoreSlim(1);

    private int _value;

    public async Task DelayAndIncrementAsync()
    {
        await _mutex.WaitAsync();
        
        try
        {
            var oldValue = _value;
            await Task.Delay(TimeSpan.FromSeconds(oldValue));
            _value = oldValue + 1;
        }
        finally
        {
            _mutex.Release();
        }
    }
}

```

## 11.3. Blocking Signals
You have to send a notification from one thread to another.

The most common and general-purpose cross-thread signal is `ManualResetEventSlim`. A manual-reset event can be in one of two states: signaled or unsignaled. Any thread may set the event to a signaled state or reset the event to an unsignaled state. A thread may also wait for the event to be signaled. 

The following two methods are invoked by separate threads; one thread waits for a signal from the other:

```C#

class MyClass
{
    private readonly ManualResetEventSlim _initialized =
        new ManualResetEventSlim();

    private int _value;

    public int WaitForInitialization()
    {
        _initialized.Wait();
        return _value;
    }

    public void InitializeFromAnotherThread()
    {
        _value = 13;
        _initialized.Set();
    }
}

```

There are other thread synchronization signal types in the .NET framework that are less commonly used. If ManualResetEventSlim doesn’t suit your needs, consider `AutoResetEvent`, `CountdownEvent`, or `Barrier`.

## 11.4. Async Signals
You need to send a notification from one part of the code to another, and the receiver of the notification must wait for it asynchronously.

If the notification only needs to be sent once, then you can use `TaskCompletionSource<T>` to send the notification asynchronously. The sending code calls `TrySetResult`, and the receiving code awaits its Task property:

```C#

class MyClass
{
    private readonly TaskCompletionSource<object> _initialized =
        new TaskCompletionSource<object>();

    private int _value1;
    private int _value2;

    public async Task<int> WaitForInitializationAsync()
    {
        await _initialized.Task;
        return _value1 + _value2;
    }

    public void Initialize()
    {
        _value1 = 13;
        _value2 = 17;
        _initialized.TrySetResult(null);
    }
}

```

This works well if the signal is only sent once, but does not work as well if you need to turn the signal
off as well as on.

Signals are a general-purpose notification mechanism. But if that “signal” is a message, used to send data from one piece of code to another, then consider using a producer/consumer queue. Similarly, do not use general-purpose signals just to coordinate access to shared data; in that situation, use a lock.

## 11.5. Throttling
You have highly concurrent code that is actually __too__ concurrent, and you need some way to throttle the concurrency.

The solution varies based on the type of concurrency your code is doing. These solutions all restrict concurrency to a specific value. 