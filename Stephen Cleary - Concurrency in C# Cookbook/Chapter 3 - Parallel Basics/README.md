# Chapter 3 - Parallel Basics

Parallel programming is used to split up CPU-bound pieces of work and divide them among multiple threads.

## 3.1 Parallel Processing of Data
You have a collection of data and you need to perform the same operation on each element of the data. This operation is CPU-bound and may take some time.

The Parallel type contains a ForEach method specifically designed for this. 

The Parallel.ForEach method allows parallel processing over a sequence of values. A similar solution is Parallel LINQ (PLINQ). Parallel LINQ provides much of the same capabilities with a LINQ-like syntax. One difference between Parallel and PLINQ is that PLINQ assumes it can use all the cores on the computer, while Parallel will dynamically react to changing CPU conditions.

## 3.2. Parallel Aggregation
At the conclusion of a parallel operation, you have to aggregate the results. Examples of aggregation are sums, averages, etc.

- Parallel.ForEach with localValue
- PLINQ AsParallel().Sum()
- PLINQ AsParallel().Aggregate().

## 3.3. Parallel Invocation
You have a number of methods to call in parallel, and these methods are (mostly) independent of each other.

The Parallel class contains a simple Invoke member that is designed for this scenario. 

Parallel.Invoke is a great solution for simple parallel invocation. However, it’s not a great fit if you want to invoke an action for each item of input data (use Parallel.ForEach instead), or if each action produces some output (use Parallel LINQ instead).

## 3.4. Dynamic Parallelism
You have a more complex parallel situation where the structure and number of parallel tasks depends on information known only at runtime.

The Task Parallel Library (TPL) is centered around the Task type. The Parallel class and Parallel LINQ are just convenience wrappers around the powerful Task. When you need dynamic parallelism, it’s easiest to use the Task type directly.

## 3.5. Parallel LINQ
You have parallel processing to perform on a sequence of data, producing another sequence of data or a summary of that data.

PLINQ works well in streaming scenarios, when you have a sequence of inputs and are producing a sequence of outputs.

Parallel class is more friendly to other processes on the system than PLINQ; this is especially a consideration if the parallel processing is done on a server machine.