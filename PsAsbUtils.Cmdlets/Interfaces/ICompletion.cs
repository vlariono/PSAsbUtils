using System.Management.Automation;

namespace PsAsbUtils.Cmdlets.Interfaces;

internal interface ICompletion
{
    public IEnumerable<CompletionResult> QueueCompletion(IServiceBusConnection connection, string pattern, int maxCount);
}