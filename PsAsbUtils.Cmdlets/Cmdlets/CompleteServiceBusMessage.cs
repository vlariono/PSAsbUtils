using System.Management.Automation;
using Azure.Messaging.ServiceBus;
using PsAsbUtils.Cmdlets.Cmdlets.Base;
using PsAsbUtils.Cmdlets.Constants;
using PsAsbUtils.Cmdlets.Exceptions;

namespace PsAsbUtils.Cmdlets.Cmdlets;

[Cmdlet(VerbsLifecycle.Complete, $"{CmdletConst.Prefix}Message")]
[OutputType(typeof(ServiceBusReceivedMessage))]
public class CompleteServiceBusMessage : ServiceBusCmdlet
{
    [Parameter(Mandatory = true, ValueFromPipeline = true)]
    public ServiceBusReceivedMessage Message { get; set; } = null!;

    protected override async Task ProcessRecordAsync(CancellationToken cancellationToken)
    {
        if (!Connection.TryGetReceiver(Message, out var receiver) || receiver is null)
        {
            var error = new ErrorRecord(
                new PSSbInvalidReceiver(),
                nameof(Message.MessageId),
                ErrorCategory.InvalidOperation,
                Message);

            WriteError(error);
            return;
        }

        await receiver.CompleteMessageAsync(Message, cancellationToken);
    }
}