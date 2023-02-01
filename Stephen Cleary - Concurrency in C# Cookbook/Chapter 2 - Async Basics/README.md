# Chapter 2 - Async Basics

[Look ConcurrencySandbox project, AsyncBasics class for demonstration.](https://github.com/rsaitov/Books/blob/master/Stephen%20Cleary%20-%20Concurrency%20in%20C%23%20Cookbook/ConcurrencySandbox/ConcurrencySandbox/AsyncBasics.cs)

## 2.1 Pausing for a Period of Time
Use Task.Delay(x) - returns a task that completes after the specified time.

Task.Delay is a fine option for unit testing asynchronous code or for implementing retry logic. However, if you need to implement a timeout, a CancellationToken is usually a better choice.

## 2.2 Returning Completed Tasks
You need to implement a synchronous method with an asynchronous signature.

Use Task.FromResult() method.

Task.FromResult() provides synchronous tasks only for successful results. If you need a task with different kind of result (e.g., a task that is completed with a NotImplementedException), then you can create your own hlper method using TaskCompletionSource.

Consider cahing the actual task, if you regularly use Task.FromResult() with the same value.

## 2.3 Reporting Progress
Use the provided IProgress<T> and Progress<T> types. Your async method should take an IProgress<T> argument; the T is whatever type of progress you need to report.

## 2.4 Waiting for a Set of Tasks to Complete
The framework provides a Task.WhenAll method for this purpose.

## 2.5 Waiting for Any Task to Complete
Use the Task.WhenAny() method. The result of the returned task is the task that completed.

The task returned by Task.WhenAny never completes in a faulted or canceled state. It always results in the first Task to complete; if that task completed with an exception, then the exception is not propogated to the task returned by Task.WhenAny. For this reason, you should usually await the task after it has completed.

## 2.6. Processing Tasks as They Complete
You have a collection of tasks to await, and you want to do some processing on each task after it completes. However, you want to do the processing for each one as soon as it  completes, not waiting for any of the other tasks.

The easiest solution is to restructure the code by introducing a higher-level async method that handles awaiting the task and processing its result.

There's an alternative way, using an extension method like OrderByCompletion.

## 2.7 Avoiding Context for Continuations

When an async method resumes after an await, by default it will resume executing within the same context. This can cause performance problems if that context was a UI context and a large number of async methods are resuming on the UI context.

To avoid resuming on a context, await the result of ConfigureAwait and pass false for its continueOnCapturedContext parameter.

For every async method you write, if it doesn’t need to resume to its original context, then use ConfigureAwait. There’s no disadvanage to doing so.

## 2.8. Handling Exceptions from async Task Methods
Exception handling is a critical part of any design. It’s easy to design for the success case but a design is not correct until it also handles the failure cases. Fortunately, handling exceptions from async Task methods is straightforward.

## 2.9. Handling Exceptions from async Void Methods
You have an async void method and need to handle exceptions propagated out of that method.

There is no good solution. If at all possible, change the method to return Task instead of void.

It’s best to avoid propagating exceptions out of async void methods. If you must use an async void method, consider wrapping all of its code in a try block and handling the exception directly.