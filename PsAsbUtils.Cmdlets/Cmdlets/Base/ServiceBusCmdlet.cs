using System.Management.Automation;
using PsAsbUtils.Cmdlets.Interfaces;

namespace PsAsbUtils.Cmdlets.Cmdlets.Base;

public abstract class ServiceBusCmdlet : PsAsyncCmdlet
{
    [Parameter(Mandatory = true)]
    public virtual IServiceBusConnection Connection { get; set; } = null!;
}
