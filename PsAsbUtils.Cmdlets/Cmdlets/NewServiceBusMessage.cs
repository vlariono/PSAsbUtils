using System.Management.Automation;
using Azure.Messaging.ServiceBus;
using PsAsbUtils.Cmdlets.Cmdlets.Base;
using PsAsbUtils.Cmdlets.Constants;

namespace PsAsbUtils.Cmdlets;

[Cmdlet(VerbsCommon.New, $"{CmdletConst.Prefix}Message")]
public class NewServiceBusMessage : ServiceBusQueueCmdlet,IDynamicParameters
{
    public object? GetDynamicParameters()
    {
        var builder = new PSDynamicParameterBuilder<ServiceBusMessage>();
        builder.AddParameter("MegaParam", typeof(string), false);
        return builder.Activate();
    }

    protected override Task ProcessRecordAsync(CancellationToken cancellationToken)
    {
        var message = new ServiceBusMessage(QueueName);

        return Task.CompletedTask;
    }
}
