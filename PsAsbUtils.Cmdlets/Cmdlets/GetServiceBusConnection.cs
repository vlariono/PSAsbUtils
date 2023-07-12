using System.Management.Automation;
using PsAsbUtils.Cmdlets.Cmdlets.Base;
using PsAsbUtils.Cmdlets.Constants;
using PsAsbUtils.Cmdlets.Core;
using PsAsbUtils.Cmdlets.Interfaces;

namespace PsAsbUtils.Cmdlets.Cmdlets;

[Cmdlet(VerbsCommon.Get, $"{CmdletConst.Prefix}NamespaceConnection")]
[OutputType(typeof(IServiceBusConnection))]
public class GetServiceBusNamespaceConnection : ServiceBusClientCmdlet
{
    protected override Task ProcessRecordAsync(CancellationToken cancellationToken)
    {
        foreach (var connection in PSServiceBusConnection.Get())
        {
            WriteObject(connection);
        }

        return Task.CompletedTask;
    }
}
