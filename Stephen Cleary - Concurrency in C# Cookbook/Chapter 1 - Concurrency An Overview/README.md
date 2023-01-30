# Chapter 1 - Concurrency: An Overview

**Concurrency** - Doing more than one thing at a time.
**Multithreading** - A form of concurrency that uses multiple threads of execution.
**Parallel Processing** - Doing lots of work by dividing it up among multiple threads that run concurrently. Parallel processing is one type of multithreading, and multithreading is one type of concurrency.
**Asynchronous Programming** - A form of concurrency that uses futures or callbacks to avoid unnecessary threads.
**Reactive Programming** - A declarative style of programming where the application reacts to events.

## Introduction to TPL Dataflows

__Dataflows__ - mix of asynchonous and parallel technoligies. The basic unit of dataflow mesh is a dataflow block. A block can be either a target block (receiving data), a source block (producing data), or both. You can create all the blocks, link them together and then start putting data in one end.

# Introduction to Multithreaded Programming

A __thread__ is an independent executor. Each thread has its own independent stack but shares the same memory with all the other threads in a process.Every .NET application has a thread pool.

**There is almost no need to ever create a new thread yourself.**

## Collections for Concurrent Applications

Concurrent collections allow multiple threads to update them simultaneously in a safe way. Most concurrent collections use snapshots to allow one thread to enumerate the values while another thread may be adding or removing values. Concurrent collections are usually more efficient than just protecting a regular collection with a lock.

<p align="center">
    <img src=".platform_support.png" width="400"/>
</p>