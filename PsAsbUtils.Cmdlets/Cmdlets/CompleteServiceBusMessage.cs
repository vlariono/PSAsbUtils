using System.Management.Automation;
using Azure.Messaging.ServiceBus;
using PsAsbUtils.Cmdlets.Cmdlets.Base;
using PsAsbUtils.Cmdlets.Constants;
using PsAsbUtils.Cmdlets.Exceptions;

namespace PsAsbUtils.Cmdlets.Cmdlets;

[Cmdlet(VerbsLifecycle.Complete, $"{PsModule.Prefix}Message")]
[OutputType(typeof(ServiceBusReceivedMessage))]
public class CompleteServiceBusMessage : ServiceBusCmdlet
{
    [Parameter(Mandatory = true, ValueFromPipeline = true, Position = PsPosition.First)]
    public ServiceBusReceivedMessage Message { get; set; } = null!;

    protected override async Task ProcessRecordAsync(CancellationToken cancellationToken)
    {
        if (!Connection.TryGetReceiver(Message, out var receiver) || receiver is null)
        {
            var error = new ErrorRecord(
                new PsSbInvalidReceiver(),
                nameof(Message.MessageId),
                ErrorCategory.InvalidOperation,
                Message);

            WriteError(error);
            return;
        }

        await receiver.CompleteMessageAsync(Message, cancellationToken);
    }
}