using System.Management.Automation;
using PsAsbUtils.Cmdlets.Cmdlets.Base;
using PsAsbUtils.Cmdlets.Constants;
using PsAsbUtils.Cmdlets.Interfaces;

namespace PsAsbUtils.Cmdlets.Cmdlets;

[Cmdlet(VerbsCommunications.Disconnect, $"{PsModule.Prefix}Namespace")]
public class DisconnectServiceBusNamespace : ServiceBusCmdlet
{
    protected override async Task ProcessRecordAsync(CancellationToken cancellationToken)
    {
        await Connection.DisposeAsync();
    }
}
