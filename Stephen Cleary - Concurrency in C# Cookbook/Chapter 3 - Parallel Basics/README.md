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