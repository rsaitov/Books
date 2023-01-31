# Chapter 2 - Async Basics

Look ConcurrencySandbox project, AsyncBasics class for demonstration.

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

## 2.5 Waiting for Any Taskto Complete

Use the Task.WhenAny() method. The result of the returned task is the task that completed.

The task returned by Task.WhenAny never completes in a faulted or canceled state. It
always results in the first Task to complete; if that task completed with an exception,
then the exception is not propogated to the task returned by Task.WhenAny. For this
reason, you should usually await the task after it has completed.