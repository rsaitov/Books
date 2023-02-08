using System.Data;
using System.Threading.Tasks.Dataflow;

namespace ConcurrencySandbox;

public static class DataflowBasics
{
    public async static Task Demonstrate()
    {
        await LinkingBlocks();
        await PropagationErrors();
        UnlinkingBlocks();
        ThrottlingBlocks();
        ParallelProcessingWithDataflowBlocks();
        CreateCustomBlock();
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
        var subtractBlock = new TransformBlock<int, int>(item =>
        {

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

    /// <summary>
    /// 4.4. Throttling Blocks
    /// </summary>
    private static void ThrottlingBlocks()
    {
        var sourceBlock = new BufferBlock<int>();

        var options = new DataflowBlockOptions { BoundedCapacity = 1 };

        var targetBlockA = new BufferBlock<int>(options);
        var targetBlockB = new BufferBlock<int>(options);

        sourceBlock.LinkTo(targetBlockA);
        sourceBlock.LinkTo(targetBlockB);
    }

    /// <summary>
    /// 4.5. Parallel Processing with Dataflow Blocks
    /// </summary>
    private static void ParallelProcessingWithDataflowBlocks()
    {
        var multiplyBlock = new TransformBlock<int, int>(
            item => item * 2,
            new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = DataflowBlockOptions.Unbounded
            }
        );

        var subtractBlock = new TransformBlock<int, int>(item => item - 2);

        multiplyBlock.LinkTo(subtractBlock);
    }

    /// <summary>
    /// 4.6. Creating Custom Blocks
    /// </summary>
    private static IPropagatorBlock<int, int> CreateCustomBlock()
    {
        var multiplyBlock = new TransformBlock<int, int>(item => item * 2);
        var addBlock = new TransformBlock<int, int>(item => item + 2);
        var divideBlock = new TransformBlock<int, int>(item => item / 2);
        var flowCompletion = new DataflowLinkOptions { PropagateCompletion = true };
        multiplyBlock.LinkTo(addBlock, flowCompletion);
        addBlock.LinkTo(divideBlock, flowCompletion);
        return DataflowBlock.Encapsulate(multiplyBlock, divideBlock);
    }
}
