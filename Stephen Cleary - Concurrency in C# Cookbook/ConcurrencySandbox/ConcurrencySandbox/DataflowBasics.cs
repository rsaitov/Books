using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks.Dataflow;
using static System.Reflection.Metadata.BlobBuilder;

namespace ConcurrencySandbox;

public static class DataflowBasics
{
    public async static Task Demonstrate()
    {
        await LinkingBlocks();
        await PropagationErrors();
        UnlinkingBlocks();
    }

    /// <summary>
    /// 4.1. Linking Blocks
    /// </summary>
    private static async Task LinkingBlocks()
    {
        var multiplyBlock = new TransformBlock<int, int>(item =>
        {
            return item * 2;
        });

        var subtractBlock = new TransformBlock<int, int>(item =>
        {
            return item - 2;
        });

        var actionBlock = new ActionBlock<int>(item =>
        {
            Console.WriteLine($"Transformed result: {item}");
        });

        var options = new DataflowLinkOptions { PropagateCompletion = true };
        multiplyBlock.LinkTo(subtractBlock, options);
        subtractBlock.LinkTo(actionBlock, options);

        multiplyBlock.Post(5);
        multiplyBlock.Complete();

        await actionBlock.Completion;
    }

    /// <summary>
    /// 4.2. Propagating Errors
    /// </summary>
    private static async Task PropagationErrors()
    {
        try
        {
            var multiplyBlock = new TransformBlock<int, int>(item =>
            {
                if (item == 1)
                    throw new InvalidOperationException("Blech.");
                return item * 2;
            });
            var subtractBlock = new TransformBlock<int, int>(item => item - 2);
            multiplyBlock.LinkTo(subtractBlock, new DataflowLinkOptions { PropagateCompletion = true });
            multiplyBlock.Post(1);
            await subtractBlock.Completion;
        }

        // Catches of number of blocks == 1
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"InvalidOperationException catched {ex.Message}");
        }

        // Each block wraps incoming errors in an AggregateException
        catch (AggregateException ex)
        {
            Console.WriteLine($"AggregateException catched {ex.Message}");
        }

        await Task.Delay(1);
    }

    /// <summary>
    /// 4.3. UnlinkingBlocks
    /// </summary>
    private static void UnlinkingBlocks()
    {
        var multiplyBlock = new TransformBlock<int, int>(item =>
        {
            Console.WriteLine($"MultiplyBlock {item}");
            return item * 2;
        });
        var subtractBlock = new TransformBlock<int, int>(item => {

            Console.WriteLine($"SubtractBlock {item}");
            return item - 2;
        });

        IDisposable link = multiplyBlock.LinkTo(subtractBlock);
        multiplyBlock.Post(1);
        multiplyBlock.Post(2);
        // Unlink the blocks.
        // The data posted above may or may not have already gone through the link.
        // In real-world code, consider a using block rather than calling Dispose.
        link.Dispose();

        multiplyBlock.Post(3);
    }
}
