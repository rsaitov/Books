# Chapter 8 - Collections
Introduce newer collections that are specifically intended for concurrent or asynchronous use.

- Immutable collections.
- Threadsafe collections.
- Producer/consumer collections.

## 8.1. Immutable Stacks and Queues
You need a stack or queueu that does not change very often and can be accessed by multiple threads safely.

```C#

var stack = ImmutableStack<int>.Empty;
stack = stack.Push(13);
var biggerStack = stack.Push(7);

// Displays "7" followed by "13".
foreach (var item in biggerStack)
Trace.WriteLine(item);

// Only displays "13".
foreach (var item in stack)
Trace.WriteLine(item);

```

Immutable collections are especially useful when the code is more functional or when you need to store a large number of snapshots and want them to share memory as much as possible.

Don’t use an immutable queue to communicate between threads; producer/consumer queues work much better for that.

## 8.2. Imuuutable Lists
You need a data structure you can index into that does not change very often and can be accessed by multiple threads safely.

However, the immutable list is internally organized as a binary tree. This is done so that immutable list instances may maximize the amount of memory they share with other instances.

```C#

var list = ImmutableList<int>.Empty;
list = list.Insert(0, 13);
list = list.Insert(0, 7);

// Displays "7" followed by "13".
foreach (var item in list)
    Trace.WriteLine(item);

list = list.RemoveAt(1);

```

The indexing operation for ImmutableList<T> is O(log N), not O(1) as you may expect. This means that you should use foreach instead of for whenever possible. A foreach loop over an ImmutableList<T> executes in O(N) time,  while a for loop over the same collection executes in O(N * log N) time.

## 8.3. Immutable Sets
You need a data structure that does not need to store duplicates, does not change very often, and can be accessed by multiple threads safely.

- `ImmutableHashSet<T>` is just a collection of unique items (has no indexer).
- `ImmutableSortedSet<T>` is a sorted collection of unique items (index operation has `O(log N)` complexity). 

Most immutable collections have special builders that can be used to construct them quickly in a mutable way and then convert them into an immutable collection.

## 8.4. Immutable Dictionaries
You need a key/value collection that does not change very often and can be accessed by multiple threads safely.

- ImmutableDictionary<TKey, TValue>.
- ImmutableSortedDictionary<TKey, TValue>.

Dictionaries are a common and useful tool when dealing with application state. They can be used in any kind of key/value or lookup scenario.

## 8.5. Threadsafe Dictinaries
You have a key/value collection that you need to keep in sync, even though multiple threads are both reading from and writing to it. For example, consider a simple in-memory cache.

The `ConcurrentDictionary<TKey, TValue>` type in the .NET framework is a true gem of data structures. It is threadsafe, using a mixture of fine-grained locks and lock-free techniques to ensure fast access in the vast majority of scenarios.

```C#

var dictionary = new ConcurrentDictionary<int, string>();
var newValue = dictionary.AddOrUpdate(0,
    key => "Zero",
    (key, oldValue) => "Zero");

// Adds (or updates) key 0 to have the value "Zero".
dictionary[0] = "Zero";

// Read a value
string currentValue;
bool keyExists = dictionary.TryGetValue(0, out currentValue);

// Remove value
string removedValue;
bool keyExisted = dictionary.TryRemove(0, out removedValue);

```

## 8.6. Blocking Queues
You need a conduit to pass messages or data from one thread to another. For example, one thread could be loading data, which it pushes down the conduit as it loads; meanwhile, there are other threads on the receiving end of the conduit that receive the data and process it.

The .NET type `BlockingCollection<T>` was designed to be this kind of conduit. 

Producer threads can add items by calling Add, and when the producer thread is finished (i.e., all items have been added), it can finish the collection by calling CompleteAdding. This notifies the collection that no more items will be added to it, and the collection can then inform its consumers that there are no more items.

```C#

BlockingCollection<int> _blockingQueue = new BlockingCollection<int>(boundedCapacity: 1);
_blockingQueue.Add(7);
_blockingQueue.Add(13);
_blockingQueue.CompleteAdding();

```

Consumer threads usually run in a loop, waiting for the next item and then processing it.

Blocking queues are great when you have a separate thread (such as a thread-pool thread) acting as a producer or consumer. They’re not as great when you want to access the conduit asynchronously — for example, if a UI thread wants to act as a consumer.

## 8.7. Blocking Stacks and Bags
You need a conduit to pass messages or data from one thread to another, but you don’t want (or need) the conduit to have first-in, first-out semantics.

```C#

BlockingCollection<int> _blockingStack = new BlockingCollection<int>(new ConcurrentStack<int>());
BlockingCollection<int> _blockingBag = new BlockingCollection<int>(new ConcurrentBag<int>());

```

## 8.8. Asynchronous Queues
You need a conduit to pass messages or data from one part of code to another in a firstin, first-out manner.

What you need is a queue with an asynchronous API. There is no type like this in the core .NET framework, but there are a couple of options available from NuGet.

1. Use BufferBlock<T> from the TPL Dataflow library.

```C#

BufferBlock<int> _asyncQueue = new BufferBlock<int>();

// Producer code
await _asyncQueue.SendAsync(7);
await _asyncQueue.SendAsync(13);
_asyncQueue.Complete();

// Consumer code
// Displays "7" followed by "13".
while (await _asyncQueue.OutputAvailableAsync())
    Trace.WriteLine(await _asyncQueue.ReceiveAsync());

```

2. AsyncProducerConsumerQueue<T> type from the Nito.AsyncEx NuGet library

```C#

var _asyncQueue = new AsyncProducerConsumerQueue<int>();

// Producer code
await _asyncQueue.EnqueueAsync(7);
await _asyncQueue.EnqueueAsync(13);
await _asyncQueue.CompleteAdding();

// Consumer code
// Displays "7" followed by "13".
while (await _asyncQueue.OutputAvailableAsync())
    Trace.WriteLine(await _asyncQueue.DequeueAsync());

```

## 8.9. Asynchronous Stacks and Bags
You need a conduit to pass messages or data from one part of code to another, but you don’t want (or need) the conduit to have first-in, first-out semantics.

The Nito.AsyncEx library provides a type AsyncCollection<T>, which acts like an asynchronous queue by default, but it can also act like any kind of producer/consumer collection. AsyncCollection<T> is a wrapper around an IProducerConsumerCollection<T>.

AsyncCollection<T> supports last-in, first-out (stack) or unordered (bag) semantics, based on whatever collection you pass to its constructor:

```C#

AsyncCollection<int> _asyncStack = new AsyncCollection<int>(new ConcurrentStack<int>());
AsyncCollection<int> _asyncBag = new AsyncCollection<int>(new ConcurrentBag<int>());

```

If all producers complete before consumers start, then the order of items is like a regular stack:

```C#

// Producer code
await _asyncStack.AddAsync(7);
await _asyncStack.AddAsync(13);
await _asyncStack.CompleteAddingAsync();

// Consumer code
// Displays "13" followed by "7".
while (await _asyncStack.OutputAvailableAsync())
    Trace.WriteLine(await _asyncStack.TakeAsync());

```

However, when both producers and consumers are executing **concurrently** (which is the usual case), the consumer will always get **the most recently** added item next.

`AsyncCollection<T>` is really just the asynchronous equivalent of `BlockingCollection<T>`.

## 8.10. Blocking/Asynchronous Queues
You need a conduit to pass messages or data from one part of code to another in a firstin, first-out manner, and you need the flexibility to treat either the producer end or the consumer end as synchronous or asynchronous.

The first is BufferBlock<T> and ActionBlock<T> from the TPL Dataflow NuGet library. BufferBlock<T> can be easily used as an asynchronous producer/consumer queue.