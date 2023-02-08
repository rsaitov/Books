# Chapter 4 - Dataflow Basics
TPL Dataflow is a powerful library that allows you to create a mesh or pipeline and then (asynchronously) send your data through it. 

Each mesh is comprised of various blocks that are linked to each other. The individual blocks are simple and are responsible for a single step in the data processing. 

To use TPL Dataflow, install the NuGet package Microsoft.Tpl.Dataflow into your application. 

## 4.1. Linking Blocks
You need to link dataflow blocks into each other to create a mesh.

The blocks provided by the TPL Dataflow library define only the most basic members.
Many of the useful TPL Dataflow methods are actually extension methods.

## 4.2. Propagating Errors
You need a way to respond to errors that can happen in your dataflow mesh.

Each block wraps incoming errors in an AggregateException, even if the incoming error is already an AggregateException. 

When you build your mesh (or pipeline), consider how errors should be handled. In simpler situations, it can be best to just propagate the errors and catch them once at the end. In more complex meshes, you may need to observe each block when the dataflow has completed.

## 4.3. Unlinking Blocks
During processing, you need to dynamically change the structure of your dataflow.

You can link or unlink dataflow blocks at any time; data can be freely passing through the mesh and it is still safe to link or unlink at any time. Both linking and unlinking are fully threadsafe.

## 4.4. Throttling Blocks
You have a fork scenario in your dataflow mesh and want the data to flow in a loadbalancing way.

## 4.5. Parallel Processing with Dataflow Blocks
You want some parallel processing done within your dataflow mesh.

If you have one particular block that does heavy CPU computations—then you can instruct that block to operate in parallel on its input data by setting the MaxDegreeOfParallelism option. By default, MaxDegreeOfParallelism is set to 1, so each dataflow block will only process one piece of data at a time.

## 4.6. Creating Custom Blocks
You have some reusable logic that you wish to place into a custom dataflow block. This
enables you to create larger blocks that contain complex logic.

You can cut out any part of a dataflow mesh that has a single input and output block by using the Encapsulate method. Encapsulate will create a single block out of the two endpoints.