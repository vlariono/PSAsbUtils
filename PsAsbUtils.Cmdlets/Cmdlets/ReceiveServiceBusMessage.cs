using System.Management.Automation;
using Azure.Messaging.ServiceBus;
using PsAsbUtils.Cmdlets.Cmdlets.Base;
using PsAsbUtils.Cmdlets.Constants;

namespace PsAsbUtils.Cmdlets.Cmdlets;

[Cmdlet(VerbsCommunications.Receive, $"{PsModule.Prefix}Message")]
[OutputType(typeof(ServiceBusReceivedMessage))]
public class ReceiveServiceBusMessage : ServiceBusQueueCmdlet
{
    [Parameter(Mandatory = false)]
    public TimeSpan MaxWaitTime { get; set; } = TimeSpan.FromSeconds(10);

    protected override async Task ProcessRecordAsync(CancellationToken cancellationToken)
    {
        var receiver = Connection.GetReceiver(QueueName);
        ServiceBusReceivedMessage? message;
        do
        {
            message = await receiver.ReceiveMessageAsync(MaxWaitTime, cancellationToken);
            if (message is not null)
            {
                WriteObject(message);
            }
        }
        while (message is not null);
    }
}
