using System.Management.Automation;
using PsAsbUtils.Cmdlets.Core;

namespace PsAsbUtils.Cmdlets.Cmdlets.Base;

public abstract class PsAsyncCmdlet : PSCmdlet
{
    private readonly CancellationTokenSource _cancellationTokenSource;

    protected PsAsyncCmdlet()
    {
        _cancellationTokenSource = new CancellationTokenSource();
    }

    sealed protected override void BeginProcessing()
    {
        PsAsyncSynchronizer.Run(() => BeginProcessingAsync(_cancellationTokenSource.Token), _cancellationTokenSource.Token);
    }

    sealed protected override void ProcessRecord()
    {
        PsAsyncSynchronizer.Run(() => ProcessRecordAsync(_cancellationTokenSource.Token), _cancellationTokenSource.Token);
    }

    sealed protected override void EndProcessing()
    {
        PsAsyncSynchronizer.Run(() => EndProcessingAsync(_cancellationTokenSource.Token), _cancellationTokenSource.Token);
    }

    protected override void StopProcessing()
    {
        _cancellationTokenSource.Cancel();
    }

    protected virtual Task BeginProcessingAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    protected virtual Task ProcessRecordAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    protected virtual Task EndProcessingAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
