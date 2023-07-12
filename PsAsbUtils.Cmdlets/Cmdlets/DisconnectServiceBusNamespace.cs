using System.Management.Automation;
using PsAsbUtils.Cmdlets.Cmdlets.Base;
using PsAsbUtils.Cmdlets.Constants;
using PsAsbUtils.Cmdlets.Interfaces;

namespace PsAsbUtils.Cmdlets.Cmdlets;

[Cmdlet(VerbsCommunications.Disconnect, $"{CmdletConst.Prefix}Namespace")]
public class DisconnectServiceBusNamespace : ServiceBusClientCmdlet
{
    [Parameter(Mandatory = true, ValueFromPipeline = true)]
    public IServiceBusConnection Connection { get; set; } = null!;

    protected override async Task ProcessRecordAsync(CancellationToken cancellationToken)
    {
        await Connection.DisposeAsync();
    }
}
