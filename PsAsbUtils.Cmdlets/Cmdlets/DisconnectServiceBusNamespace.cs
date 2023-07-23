using System.Management.Automation;
using PsAsbUtils.Cmdlets.Cmdlets.Base;
using PsAsbUtils.Cmdlets.Constants;
using PsAsbUtils.Cmdlets.Interfaces;

namespace PsAsbUtils.Cmdlets.Cmdlets;

[Cmdlet(VerbsCommunications.Disconnect, $"{PsModule.Prefix}Namespace")]
public class DisconnectServiceBusNamespace : ServiceBusConnectionCmdlet
{
    [Parameter(Mandatory = true,ValueFromPipeline = true, Position = PsPosition.First)]
    public virtual IServiceBusConnection ClosingConnection { get; set; } = null!;

    protected override Task ProcessRecordAsync(CancellationToken cancellationToken)
    {
        CloseConnection(ClosingConnection);
        return Task.CompletedTask;
    }
}
