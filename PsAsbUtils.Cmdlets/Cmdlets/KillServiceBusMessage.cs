using System;
using System.Management.Automation;
using Azure.Messaging.ServiceBus;
using PsAsbUtils.Cmdlets.Cmdlets.Base;
using PsAsbUtils.Cmdlets.Constants;
using PsAsbUtils.Cmdlets.Exceptions;

namespace PsAsbUtils.Cmdlets.Cmdlets;

[Cmdlet("Kill", $"{PsModule.Prefix}Message")]
public class KillServiceBusMessage: ServiceBusCmdlet
{
    [Parameter(Mandatory = true, ValueFromPipeline = true, Position = PsPosition.First)]
    public ServiceBusReceivedMessage Message { get; set; } = null!;

    [Parameter(Mandatory = true, ValueFromPipeline = false, Position = PsPosition.Second)]
    public string DeadLetterReason { get; set; } = null!;

    [Parameter(Mandatory = false, ValueFromPipeline = false, Position = PsPosition.Third)]
    [ValidateNotNullOrWhiteSpace]
    public string? DeadLetterErrorDescription { get; set; }

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

        await receiver.DeadLetterMessageAsync(Message, DeadLetterReason, DeadLetterErrorDescription, cancellationToken);
    }
}
