using System.Management.Automation;
using PsAsbUtils.Cmdlets.Core;

namespace PsAsbUtils.Cmdlets.Cmdlets.Base;

[PsContext(ContextProvider = nameof(CmdletContext))]
public abstract class PsAsyncCmdlet : PSCmdlet
{
    private readonly CancellationTokenSource _cancellationTokenSource;

    private static PsCmdletContext CmdletContext { get; set; }

    protected PsAsyncCmdlet()
    {
        _cancellationTokenSource = new CancellationTokenSource();
    }

    sealed protected override void BeginProcessing()
    {
        var context = new PsCmdletContext(SessionState);
        if (CmdletContext != context)
        {
            CmdletContext = context;
        }

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

    private readonly record struct PsCmdletContext : IPsContext
    {
        private readonly PSVariable _defaultParameters;

        internal PsCmdletContext(SessionState sessionState)
        {
            _defaultParameters = sessionState.PSVariable.Get("PSDefaultParameterValues");
        }

        public DefaultParameterDictionary? DefaultParameters => _defaultParameters.Value as DefaultParameterDictionary;

        public bool Equals(PsCmdletContext other)
        {
            return ReferenceEquals(_defaultParameters, other._defaultParameters);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_defaultParameters.GetHashCode);
        }
    }
}
