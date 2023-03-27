# Chapter 10 - Functional-Friendly OOP

## 10.1. Async Interfaces and Inheritance
You have a method in your interface or base class that you want to make asynchronous.

The key to understanding this problem and its solution is to realize that async is an implementation detail. It is not possible to mark interface methods or abstract methods as async. However, you can define a method with the same signature as an async method, just without the async keyword. 

Remember that __types__ are awaitable, not __methods__. You can await a Task returned by a method, whether or not that method is actually async. So, an interface or abstract method can just return a Task (or Task<T>), and the return value of that method is awaitable.

## 10.2. Async Construction: Factories
You are coding a type that requires some asynchronous work to be done in its constructor.

Constructors cannot be async, nor can they use the await keyword. One possibility is to have a constructor paired with an async initialization method, so the type could be used like this:

```C#

var instance = new MyAsyncClass();
await instance.InitializeAsync();

```

However, there are disadvantages to this approach. It can be easy to forget to call the InitializeAsync method, and the instance is not usable immediately after it was constructed.

A better solution is to make the type its own factory. The following type illustrates the asynchronous factory method pattern:

```C#

class MyAsyncClass
{
    private MyAsyncClass()
    { }
    
    private async Task<MyAsyncClass> InitializeAsync()
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
        return this;
    }

    public static Task<MyAsyncClass> CreateAsync()
    {
        var result = new MyAsyncClass();
        return result.InitializeAsync();
    }
}

```

The primary advantage of this pattern is that there is no way that other code can get an uninitialized instance of `MyAsyncClass`.

## 10.3. Async Construction: The Asynchronous Initialization Pattern
You are coding a type that requires some asynchronous work to be done in its constructor, but you cannot use the asynchronous factory pattern because the instance is created via reflection (e.g., a Dependency Injection/Inversion of Control library, data binding, Activator.CreateInstance, etc.).

```C#

/// <summary>
/// Marks a type as requiring asynchronous initialization
/// and provides the result of that initialization.
/// </summary>
public interface IAsyncInitialization
{
    /// <summary>
    /// The result of the asynchronous initialization of this instance.
    /// </summary>
    Task Initialization { get; }
}

class MyFundamentalType : IMyFundamentalType, IAsyncInitialization
{
    public MyFundamentalType()
    {
        Initialization = InitializeAsync();
    }

    public Task Initialization { get; private set; }

    private async Task InitializeAsync()
    {
        // Asynchronously initialize this instance.
        await Task.Delay(TimeSpan.FromSeconds(1));
    }
}

```

## 10.4. Async Properties
You have a property that you want to make async. The property is not used in data binding.

```C#

// What we think we want (does not compile).
public int Data
{
    async get
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
        return 13;
    }
}

```

If your “asynchronous property” needs to kick off a new (asynchronous) evaluation each time it is read, then it is not a property. It is actually a method in disguise.

```C#

// As an asynchronous method.
public async Task<int> GetDataAsync()
{
    await Task.Delay(TimeSpan.FromSeconds(1));
    return 13;
}

```

The other scenario is that the “asynchronous property” should only kick off a **single (asynchronous) evaluation** and that the resulting value should be cached for future use. In this case, you can use asynchronous lazy initialization.

```C#

// As a cached value.
public AsyncLazy<int> Data
{
    get { return _data; }
}
private readonly AsyncLazy<int> _data =
    new AsyncLazy<int>(async () =>
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
        return 13;
    });

```

## 10.5. Async Events
You have an event that you need to use with handlers that might be async, and you need to detect whether the event handlers have completed.

## 10.6. Async Disposal
You have a type that allows asynchronous operations but also needs to allow disposal of its resources.