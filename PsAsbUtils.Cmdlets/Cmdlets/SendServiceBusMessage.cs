using System.Management.Automation;
using Azure.Messaging.ServiceBus;
using PsAsbUtils.Cmdlets.Cmdlets.Base;
using PsAsbUtils.Cmdlets.Constants;
using PsAsbUtils.Cmdlets.Exceptions;

namespace PsAsbUtils.Cmdlets.Cmdlets;

[Cmdlet(VerbsCommunications.Send, $"{PsModule.Prefix}Message")]
public class SendServiceBusMessage : ServiceBusQueueCmdlet
{
    private ServiceBusSender? _sender;

    [Parameter(Mandatory = true, ValueFromPipeline = true, Position = PsPosition.Second)]
    public ServiceBusMessage Message { get; set; } = null!;

    [Parameter(Mandatory = false, Position = PsPosition.Third)]
    public DateTimeOffset EnqueueAt { get; set; }

    protected override Task BeginProcessingAsync(CancellationToken cancellationToken)
    {
        _sender = Connection.GetSender(QueueName);
        return Task.CompletedTask;
    }
    protected override async Task ProcessRecordAsync(CancellationToken cancellationToken)
    {
        await SendMessageAsync(Message, cancellationToken);
    }

    private Task SendMessageAsync(ServiceBusMessage message, CancellationToken cancellationToken)
    {
        if (_sender is null)
        {
            throw new PsSbInvalidSender();
        }

        if (EnqueueAt != default)
        {
            return _sender.ScheduleMessageAsync(message, EnqueueAt, cancellationToken);
        }

        return _sender.SendMessageAsync(message, cancellationToken);
    }
}
