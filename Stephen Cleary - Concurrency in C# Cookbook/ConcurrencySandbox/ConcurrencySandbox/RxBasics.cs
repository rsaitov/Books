using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;

public static class RxBasics
{
    public static void Demonstrate()
    {
        ConvertingNetEvents();
        SendingNotificationsToContext();
        GroupingEventData();
        ThrottlingAndSampling();
        Timeouts();
    }

    /// <summary>
    /// 5.1. Converting .NET Events
    /// </summary>
    private static void ConvertingNetEvents()
    {
        // create sequences from events

        var progress = new Progress<int>();
        var progressReports = Observable.FromEventPattern<int>(
            handler => progress.ProgressChanged += handler,
            handler => progress.ProgressChanged -= handler
        );

        progressReports.Subscribe(data => Console.WriteLine("OnNext: " + data.EventArgs));
    }

    /// <summary>
    /// 5.2. Sending Notifications to a Context
    /// </summary>
    private static void SendingNotificationsToContext()
    {
        var uiContext = SynchronizationContext.Current;

        Trace.WriteLine("UI thread is " + Environment.CurrentManagedThreadId);

        if (uiContext != null)
        {
            // will be called on the UI thread
            Observable.Interval(TimeSpan.FromSeconds(1))
                .ObserveOn(uiContext)
                .Subscribe(x => Console.WriteLine("Interval " + x + " on thread " +
                Environment.CurrentManagedThreadId)
            );
        }
    }

    /// <summary>
    /// 5.3. Grouping Event Data with Windows and Buffers
    /// </summary>
    private static void GroupingEventData()
    {
        // Buffer
        // hold on to the incoming events until the group is complete, at which time
        // it forwards them all at once as a collection of events
        Observable.Interval(TimeSpan.FromSeconds(1))
            .Buffer(2)
            .Subscribe(x => Console.WriteLine(DateTime.Now.Second + ": Got " + x[0] + " and " + x[1])
        );

        // it produces
        // Got 0 and 1
        // Got 2 and 3
        // Got 4 and 5
        // Got 6 and 7
        // Got 8 and 9


        // Window
        // Logically groups the incoming events but will pass them along as they arrive
        Observable.Interval(TimeSpan.FromSeconds(1))
            .Window(2)
            .Subscribe(group =>
            {
                Console.WriteLine(DateTime.Now.Second + ": Starting new group");
                group.Subscribe(
                x => Console.WriteLine(DateTime.Now.Second + ": Saw " + x),
                () => Console.WriteLine(DateTime.Now.Second + ": Ending group"));
            });

        // it produces
        // Starting new group
        // Saw 0
        // Saw 1
        // Ending group
        // Starting new group
        // Saw 2
        // Saw 3
        // Ending group
        // Starting new group
        // Saw 4
        // Saw 5
        // Ending group
        // Starting new group
    }

    /// <summary>
    /// 5.4. Taming Event Streams with Throttling and Sampling
    /// </summary>
    private static void ThrottlingAndSampling()
    {
        // Throttling
        // Cares time-window, reset it window on incoming event
        // When the timeout window expires, publishes the last event value that arrived within the window

        /*
        Observable.FromEventPattern<MouseEventHandler, MouseEventArgs>(
            handler => (s, a) => handler(s, a),
            handler => MouseMove += handler,
            handler => MouseMove -= handler)
            .Select(x => x.EventArgs.GetPosition(this))
            .Throttle(TimeSpan.FromSeconds(1))
            .Subscribe(x => Trace.WriteLine(
            DateTime.Now.Second + ": Saw " + (x.X + x.Y))
        );
        */

        // Saw 139
        // Saw 137
        // Saw 424
        // Saw 226

        // Sampling
        // Establishes a regular timeout period and publishes the most recent value
        // within that window each time the timeout expires
        
        /*
        Observable.FromEventPattern<MouseEventHandler, MouseEventArgs>(
            handler => (s, a) => handler(s, a),
            handler => MouseMove += handler,
            handler => MouseMove -= handler)
            .Select(x => x.EventArgs.GetPosition(this))
            .Sample(TimeSpan.FromSeconds(1))
            .Subscribe(x => Trace.WriteLine(
            DateTime.Now.Second + ": Saw " + (x.X + x.Y))
        );
        */

        // Saw 311
        // Saw 254
        // Saw 269
        // Saw 342
        // Saw 224
        // Saw 277
    }

    /// <summary>
    /// 5.5. Timeouts
    /// </summary>
    private static void Timeouts()
    {
        var client = new HttpClient();
        client.GetStringAsync("http://www.example.com/")
            .ToObservable()
            .Timeout(TimeSpan.FromSeconds(1))
            .Subscribe(
            x => Trace.WriteLine(DateTime.Now.Second + ": Saw " + x.Length),
            ex => Trace.WriteLine(ex)
        );
    }
}