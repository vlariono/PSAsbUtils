using System.Management.Automation;

namespace PsAsbUtils.Cmdlets.Cmdlets.Base;

public abstract class ServiceBusQueueCmdlet : ServiceBusCmdlet
{
    [Parameter(Mandatory = true, Position = PsPosition.First)]
    public virtual string QueueName { get; set; } = null!;
}
