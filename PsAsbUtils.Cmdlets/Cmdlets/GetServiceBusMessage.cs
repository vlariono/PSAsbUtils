using System.Management.Automation;
using Azure.Messaging.ServiceBus;
using PsAsbUtils.Cmdlets.Cmdlets.Base;
using PsAsbUtils.Cmdlets.Constants;

namespace PsAsbUtils.Cmdlets.Cmdlets;

[Cmdlet(VerbsCommon.Get, $"{CmdletConst.Prefix}Message")]
[OutputType(typeof(ServiceBusReceivedMessage))]
public class GetServiceBusMessage : ServiceBusQueueCmdlet
{
    [Parameter(Mandatory = false)]
    public int MaxMessages { get; set; } = 100;

    [Parameter(Mandatory = false)]
    public int FromSequenceNumber { get; set; } = 0;
    protected override async Task ProcessRecordAsync(CancellationToken cancellationToken)
    {
        await using var receiver = Connection.GetReceiver(QueueName);
        foreach (var message in await receiver.PeekMessagesAsync(MaxMessages, FromSequenceNumber, cancellationToken))
        {
            WriteObject(message);
        }
    }
}
