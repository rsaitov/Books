# Chapter 5 - Bend, or Break
## Decoupling and the Law of Demeter
Organize your code into cells (modules) and limit the interaction between them. Systems with many unnecessary dependencies are very hard (and expensive) to maintain, and tend to be highly unstable.

**The Law of Demeter** for functions states that any method of an object should call only methods belonging to:
- itself;
- any parameters that were passed in to the method;
- any objects it created;
- any directly held component objects.

Logical and physical design must proceed in tandem.

## Metaprogramming
**Configure, Don't Integrate**
Use __metadata__ to describe configuration options for an application: tuning parameters, user preferences, the  installation directory, and so on. Metadata is any data that describes the application—how it should run, what resources it should use, and so on. 
**Put Abstractions in Code, Details in Metadata**
Benefits to this approach:
- It forces you to decouple you design.
- It forces you to create a more robust, abstract design by deferring details.
- You can customize th application without recompiling it.
- Metadata can be expressed in a manner that's much closer to the problem domain than a general-purpose programming language might be.
- You may even be able to implement several different projects using the same application engine, but with different metadata.

Business logic also may use a configurable approach.

We recommend representing configurtion metadata in **plain text** - it makes life that much easier. 

A more flexible approach is to write programs that **can reload** their configuration **while they're running**.

### An Example: Enterprise Java Beans
You write a __bean__ - a self-contained object that follows certain conventions - and place it in a __bean container__ that manages much of the low-level detail on your behalf.

We can let applications configure each other - software that adapts itself to its environment.

Keep metadata in **human-readable** format.

## Temporal Coupling
There are two aspects of time that are important to us: concurrency (things happening at the same time) and ordering (the relative positions of things in time). We need to allow for cuncurrency and to think about decoupling any time or order dependencies.

You can use activity diagram to maximize parallelism by identifying activities that __could__ be performed in parallel, but aren't.

In a **hungry consumer** model, you replace the central scheduler with a number of independent consumer tasks and a centralized work queue. Each consumer task grabs a piece from the work queue and goes on about the business of processing it. 

**Design Using Services**

Instead of components, we have really created __services__ - independent, concurrent objects behind well-defined, consistens interfaces.

**Always Design for Concurrency**
By planning for **concurrency, and decoupling operations in time**, you have all these options—including the stand-alone option, where you can choose not to be concurrent.

## It's Just a View
Each module has a single, well-defined responsibility. Using **events** minimizes coupling between objects.

### Public/Subscribe
Pushing all the events through a single routine violates encapsulation - one routine now has to have intimate knowledge of the interactions among many objects.

Object should be able to register only the events they need, and should never be sent events they don't need.

We can use this publish/subscribe mechanism to implement a very important design concept: the separation of a model from views of the model.

### Model-View-Controller
MVC-idiom: separating the model from both the GUI that represents it and the controls that manage the view.

__The view and controller are tightly coupled, and in some implementations of MVC the view and controller are a single component.__

- **Model**. The abstract data model representing the target object. The model has no direct knowledge of any views or controllers.
- **View**. A way to interpret the model. It subscribes to changes in the model and logical events from the controller.
- **Controller**. A way to control the view and provide the model with new data. It publishes events to both the model and the view.

## Blackboards
A blackboard system lets us decouple our objects from each other completely, providing a forum where knowledge consumers and producers can exchange data anonymously and asynchronously.