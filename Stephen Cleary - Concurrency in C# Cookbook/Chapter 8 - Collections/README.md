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

