using System.Management.Automation;
using PsAsbUtils.Cmdlets.Interfaces;

namespace PsAsbUtils.Cmdlets.Cmdlets.Base;

public abstract class ServiceBusCmdlet : PSAsyncCmdlet
{
    [Parameter(Mandatory = true)]
    public virtual IServiceBusConnection Connection { get; set; } = null!;
}
