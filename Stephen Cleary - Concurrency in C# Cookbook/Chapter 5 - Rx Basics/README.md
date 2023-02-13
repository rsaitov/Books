# Chapter 5 - Rx Basics

Reactive Extensions (Rx) treats events as sequences of data that arrive over time. As such, you can think of Rx as LINQ to events (based on `IObservable<T>`). The main difference between observables and other LINQ providers is that Rx is a “push” model. This means that the query defines how the program reacts as events arrive. Rx builds on top of LINQ, adding some powerful new operators as extension methods.

## 5.1. Converting .NET Events
You have an event that you need to treat as an Rx input stream, producing some data via OnNext each time the event is raised.

The Observable class defines several event converters. Most .NET framework events are compatible with `FromEventPattern`, but if you have events that don’t follow the common pattern, you can use `FromEvent` instead.

## 5.2. Sending Notifications to a Context
Rx does its best to be thread agnostic. So, it will raise its notifications (e.g., `OnNext`) in whatever thread happens to be present at the time.

However, you often want these notifications raised in a particular context. For example, UI elements should only be manipulated from the UI thread that owns them, so if you are updating a UI in response to a notification, then you’ll need to “move” over to the UI thread.

You can use `.ObserveOn(Scheduler.Default)` to move calculations to a thread-pool thread.

## 5.3. Grouping Event Data with Windows and Buffers
You have a sequence of events and you want to group the incoming events as they arrive. For one example, you need to react to pairs of inputs. For another example, you need to react to all inputs within a two-second window.

Rx provides a pair of operators that group incoming sequences: `Buffer` and `Window`. Buffer will hold on to the incoming events until the group is complete, at which time it forwards them all at once as a collection of events. Window will logically group the incoming events but will pass them along as they arrive.

## 5.4. Taming Event Streams with Throttling and Sampling
A common problem with writing reactive code is when the events come in too quickly. A fast-moving stream of events can overwhelm your program’s processing.

Rx provides operators specifically for dealing with a flood of event data. The `Throttle` and `Sample` operators give us two different ways to tame fast input events.

The `Throttle` operator establishes a sliding timeout window. When an incoming event arrives, it resets the timeout window. When the timeout window expires, it publishes the last event value that arrived within the window.

Throttle is often used in situations such as autocomplete, when the user is typing text into a textbox, but you don’t want to do the actual lookup until the user stops typing.

`Sample` takes a different approach to taming fast-moving sequences. Sample establishes a regular timeout period and publishes the most recent value within that window each time the timeout expires. If there were no values received within the sample period, then no results are published for that period.

## 5.5. Timeouts
You expect an event to arrive within a certain time and need to ensure that your program will respond in a timely fashion, even if the event does not arrive. 

The `Timeout` operator establishes a sliding timeout window on its input stream. Whenever a new event arrives, the timeout window is reset. If the timeout expires without seeing an event in that window, the Timeout operator will end the stream with an OnError notification containing a `TimeoutException`.