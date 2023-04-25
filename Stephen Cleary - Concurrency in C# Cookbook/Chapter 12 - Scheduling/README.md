# Chapter 12 - Scheduling

There are a few different scheduler types in the .NET framework, and they’re used with slight differences by parallel and dataflow code.

I recommend that you **not** specify a scheduler whenever possible; the defaults are usually correct.

## 12.1. Scheduling Work to the Thread Pool
You have a piece of code that you explicitly want to execute on a thread-pool thread.

The vast majority of the time, you’ll want to use Task.Run, which is quite simple. 

```C#

Task task = Task.Run(() =>
{
    Thread.Sleep(TimeSpan.FromSeconds(2));
});

// with return value and async lambda
Task<int> task = Task.Run(async () =>
{
    await Task.Delay(TimeSpan.FromSeconds(2));
    return 13;
});

```

Task.Run is an effective replacement for BackgroundWorker, Delegate.BeginInvoke, and ThreadPool.QueueUserWorkItem. None of those should be used in new code; code using Task.Run is much easier to write correctly and maintain over time.

## 12.2. Executing Code with a Task Scheduler
You have multiple pieces of code that you need to execute in a certain way. For example, you may need all the pieces of code to execute on the UI thread, or you may need to execute only a certain number at a time.

The simplest TaskScheduler is `TaskScheduler.Default`, which queues work to the thread pool. 

You can capture a specific context and later schedule work back to it by using Task
Scheduler.FromCurrentSynchronizationContext, as follows:

```C#

TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();

```

ConcurrentExclusiveSchedulerPair: ConcurrentScheduler (multiple tasks at the same time) and ExclusiveScheduler (one task at a time).

```C#

var schedulerPair = new ConcurrentExclusiveSchedulerPair();
TaskScheduler concurrent = schedulerPair.ConcurrentScheduler;
TaskScheduler exclusive = schedulerPair.ExclusiveScheduler;

```

## 12.3. Scheduling Parallel Code
You need to control how the individual pieces of code are executed in parallel code.

Once you create an appropriate TaskScheduler instance, you can include it in the options that you pass to a Parallel method.

```C#

var schedulerPair = new ConcurrentExclusiveSchedulerPair(
    TaskScheduler.Default, maxConcurrencyLevel: 8);

TaskScheduler scheduler = schedulerPair.ConcurrentScheduler;

ParallelOptions options = new ParallelOptions { TaskScheduler = scheduler };

Parallel.ForEach(collections, options,
    matrices => Parallel.ForEach(matrices, options,
    matrix => matrix.Rotate(degrees)));

```

## 12.4. Dataflow Synchronization Using Schedulers
You need to control how the individual pieces of code are executed in dataflow code.

Once you create an appropriate TaskScheduler instance, you can include it in the options that you pass to a dataflow block.

```C#

var options = new ExecutionDataflowBlockOptions
{
    TaskScheduler = TaskScheduler.FromCurrentSynchronizationContext(),
};

var multiplyBlock = new TransformBlock<int, int>(item => item * 2);

var displayBlock = new ActionBlock<int>(
    result => ListBox.Items.Add(result), options);

multiplyBlock.LinkTo(displayBlock);

```