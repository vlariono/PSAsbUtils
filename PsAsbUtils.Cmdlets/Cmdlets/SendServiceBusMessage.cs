using System.Management.Automation;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Amqp;
using PsAsbUtils.Cmdlets.Cmdlets.Base;
using PsAsbUtils.Cmdlets.Constants;
using PsAsbUtils.Cmdlets.Exceptions;

namespace PsAsbUtils.Cmdlets.Cmdlets;

[Cmdlet(VerbsCommunications.Send, $"{CmdletConst.Prefix}Message")]
public class SendServiceBusMessage : ServiceBusQueueCmdlet
{
    private ServiceBusSender? _sender;

    [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = nameof(ServiceBusMessage))]
    public ServiceBusMessage Message { get; set; } = null!;

    [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = nameof(ServiceBusReceivedMessage))]
    public ServiceBusReceivedMessage ReceivedMessage { get; set; } = null!;

    [Parameter(Mandatory = false)]
    public DateTimeOffset EnqueueAt { get; set; }

    protected override Task BeginProcessingAsync(CancellationToken cancellationToken)
    {
        _sender = Connection.GetSender(QueueName);
        return Task.CompletedTask;
    }
    protected override async Task ProcessRecordAsync(CancellationToken cancellationToken)
    {
        var message = ParameterSetName switch
        {
            nameof(ServiceBusMessage) => Message,
            nameof(ServiceBusReceivedMessage) => new ServiceBusMessage(ReceivedMessage),
            _ => throw new NotImplementedException()
        };

        await SendMessageAsync(message, cancellationToken);
    }

    private Task SendMessageAsync(ServiceBusMessage message, CancellationToken cancellationToken)
    {
        if (_sender is null)
        {
            throw new PSSbInvalidSender();
        }

        if (EnqueueAt != default)
        {
            return _sender.ScheduleMessageAsync(message, EnqueueAt, cancellationToken);
        }

        return _sender.SendMessageAsync(message, cancellationToken);
    }
}
