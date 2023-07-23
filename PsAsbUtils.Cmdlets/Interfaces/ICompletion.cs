using System.Management.Automation;

namespace PsAsbUtils.Cmdlets.Interfaces;

public interface ICompletion
{
    public IEnumerable<CompletionResult> QueueCompletion(IServiceBusConnection connection, string pattern, int maxCount);
}