using PsAsbUtils.Cmdlets.Core;

namespace PsAsbUtils.Tests;

public class PsAsyncSynchronizerTests
{
    [Fact]
    public void RestoresOriginalContext()
    {
        var origin = SynchronizationContext.Current;
        PSAsyncSynchronizer.Run(() => Task.CompletedTask, CancellationToken.None);
        Assert.True(ReferenceEquals(origin, SynchronizationContext.Current));
    }

    [Fact]
    public void PumpWorks()
    {
        var origin = SynchronizationContext.Current;
        PSAsyncSynchronizer synchronizer = new PSAsyncSynchronizer();
        var result = false;
        SendOrPostCallback callBack = state => result = state is bool s && s;

        synchronizer.Post(callBack, true);
        synchronizer.Complete();
        synchronizer.Pump(CancellationToken.None);
        Assert.True(result);
    }
}
