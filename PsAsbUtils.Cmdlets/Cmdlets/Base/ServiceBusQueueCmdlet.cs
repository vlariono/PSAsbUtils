using System.Management.Automation;

namespace PsAsbUtils.Cmdlets.Cmdlets.Base;

public abstract class ServiceBusQueueCmdlet : ServiceBusCmdlet
{
    [Parameter(Mandatory = true, Position = PsPosition.First)]
    [PsQueueCompleter(ContextProvider = typeof(PsAsyncCmdlet))]
    public virtual string QueueName { get; set; } = null!;
}
