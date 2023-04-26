# Chapter 13 - Scenarios

## 13.1. Initializing Shared Resources
You have a resource that is shared between multiple parts of your code. This resource needs to be initialized the first time it is accessed.

The .NET framework includes a type specifically for this purpose: Lazy<T>. 

```C#

static int _simpleValue;

static readonly Lazy<int> MySharedInteger = new Lazy<int>(() => _simpleValue++);

void UseSharedInteger()
{
    int sharedValue = MySharedInteger.Value;
}

```

No matter how many threads call UseSharedInteger simultaneously, the factory delegate is only executed once, and all threads wait for the same instance.

## 13.2. Rx Deferred Evaluation
You want to create a new source observable whenever someone subscribes to it. For example, you want each subscription to represent a different request to a web service.

The Rx library has an operator Observable.Defer, which will execute a delegate each time the observable is subscribed to.

## 13.3. Asynchronous Data Binding
You are retrieving data asynchronously and need to data-bind the results (e.g., in the ViewModel of a Model-View-ViewModel design).

The AsyncEx library has a type NotifyTaskCompletion that can be used for this.

## 13.4. Implicit State
You have some state variables that need to be accessible at different points in your call stack. 

The CallContext type in .NET provides LogicalSetData and LogicalGetData methods that allow you to give your state a name and place it on a logical “context.” When you are done with that state, you can call FreeNamedDataSlot to remove it from the context. The following code shows how to use these methods to set an operation identifier that is later read by a logging method:

```C#

void DoLongOperation()
{
    var operationId = Guid.NewGuid();
    CallContext.LogicalSetData("OperationId", operationId);
    DoSomeStepOfOperation();
    CallContext.FreeNamedDataSlot("OperationId");
}

void DoSomeStepOfOperation()
{
    // Do some logging here.
    Trace.WriteLine("In operation: " +
    CallContext.LogicalGetData("OperationId"));
}

```