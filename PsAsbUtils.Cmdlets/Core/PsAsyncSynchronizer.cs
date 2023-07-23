using System.Collections.Concurrent;

namespace PsAsbUtils.Cmdlets.Core;

internal class PsAsyncSynchronizer : SynchronizationContext
{
    private readonly BlockingCollection<(SendOrPostCallback CallBack, object? State)> _callbacks;

    private readonly ref struct ContextSwitch<T> where T : SynchronizationContext
    {
        private readonly SynchronizationContext? _origin;

        public ContextSwitch(T context)
        {
            _origin = Current;
            SetSynchronizationContext(context);
        }

        public void Dispose()
        {
            SetSynchronizationContext(_origin);
        }
    }

    internal PsAsyncSynchronizer()
    {
        _callbacks = new();
    }

    public override void Post(SendOrPostCallback d, object? state)
    {
        _callbacks.Add((d, state));
    }

    public void Complete()
    {
        _callbacks.CompleteAdding();
    }

    public void Pump(CancellationToken token)
    {
        while (!token.IsCancellationRequested && !_callbacks.IsCompleted)
        {
            if (_callbacks.TryTake(out var item, 20, token))
            {
                item.CallBack(item.State);
            }
        }
    }

    public static void Run(Func<Task> action, CancellationToken token)
    {
        var context = new PsAsyncSynchronizer();
        using var contextSwitch = new ContextSwitch<PsAsyncSynchronizer>(context);

        var actionTask = action.Invoke().ContinueWith(t => context.Complete(), token);
        context.Pump(token);
        actionTask.GetAwaiter().GetResult();
    }
}
