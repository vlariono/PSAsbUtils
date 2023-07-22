using System.Collections;
using System.Management.Automation;
using Azure.Messaging.ServiceBus;
using PsAsbUtils.Cmdlets.Cmdlets.Base;
using PsAsbUtils.Cmdlets.Constants;

namespace PsAsbUtils.Cmdlets;

[Cmdlet(VerbsCommon.New, $"{PsModule.Prefix}Message")]
[OutputType(typeof(ServiceBusMessage))]
public sealed class NewServiceBusMessage : PsAsyncCmdlet
{
    [Parameter(Mandatory = false, ValueFromPipeline = true, Position = PsPosition.First)]
    [ValidateNotNull]
    public ServiceBusReceivedMessage? ReceivedMessage { get; set; }

    [Parameter(Mandatory = false)]
    [ValidateNotNull]
    public string? Body { get; set; }

    [Parameter(Mandatory = false)]
    [ValidateNotNull]
    public string? ContentType { get; set; }

    [Parameter(Mandatory = false)]
    [ValidateNotNull]
    public string? CorrelationId { get; set; }

    [Parameter(Mandatory = false)]
    [ValidateNotNull]
    public string? MessageId { get; set; }

    [Parameter(Mandatory = false)]
    [ValidateNotNull]
    public string? PartitionKey { get; set; }

    [Parameter(Mandatory = false)]
    [ValidateNotNull]
    public string? ReplyTo { get; set; }

    [Parameter(Mandatory = false)]
    [ValidateNotNull]
    public string? ReplyToSessionId { get; set; }

    [Parameter(Mandatory = false)]
    public DateTimeOffset ScheduledEnqueueTime { get; set; }

    [Parameter(Mandatory = false)]
    [ValidateNotNull]
    public string? SessionId { get; set; }

    [Parameter(Mandatory = false)]
    [ValidateNotNull]
    public string? Subject { get; set; }

    [Parameter(Mandatory = false)]
    public TimeSpan TimeToLive { get; set; }

    [Parameter(Mandatory = false)]
    [ValidateNotNull]
    public string? To { get; set; }

    [Parameter(Mandatory = false)]
    [ValidateNotNull]
    public string? TransactionPartitionKey { get; set; }

    [Parameter(Mandatory = false)]
    [ValidateNotNull]
    public Hashtable? CustomProperties { get; set; }

    protected override Task ProcessRecordAsync(CancellationToken cancellationToken)
    {
        var message = ReceivedMessage is null ? new ServiceBusMessage() : new ServiceBusMessage(ReceivedMessage);

        AssignIfSet(nameof(Body), message, static (m, p) => m.Body = BinaryData.FromString((string)p));
        AssignIfSet(nameof(ContentType), message, static (m, p) => m.ContentType = (string)p);
        AssignIfSet(nameof(CorrelationId), message, static (m, p) => m.CorrelationId = (string)p);
        AssignIfSet(nameof(MessageId), message, static (m, p) => m.MessageId = (string)p);
        AssignIfSet(nameof(PartitionKey), message, static (m, p) => m.PartitionKey = (string)p);
        AssignIfSet(nameof(ReplyTo), message, static (m, p) => m.ReplyTo = (string)p);
        AssignIfSet(nameof(ReplyToSessionId), message, static (m, p) => m.ReplyToSessionId = (string)p);
        AssignIfSet(nameof(ScheduledEnqueueTime), message, static (m, p) => m.ScheduledEnqueueTime = (DateTimeOffset)p);
        AssignIfSet(nameof(SessionId), message, static (m, p) => m.SessionId = (string)p);
        AssignIfSet(nameof(Subject), message, static (m, p) => m.Subject = (string)p);
        AssignIfSet(nameof(TimeToLive), message, static (m, p) => m.TimeToLive = (TimeSpan)p);
        AssignIfSet(nameof(To), message, static (m, p) => m.To = (string)p);
        AssignIfSet(nameof(TransactionPartitionKey), message, static (m, p) => m.TransactionPartitionKey = (string)p);
        AssignIfSet(nameof(CustomProperties), message, static (m, p) => m.ApplicationProperties.AddHashtable((Hashtable)p));

        WriteObject(message);
        return Task.CompletedTask;
    }

    private void AssignIfSet(string parameterName, ServiceBusMessage message, Action<ServiceBusMessage, object> action)
    {
        if (MyInvocation.BoundParameters.TryGetValue(parameterName, out var value))
        {
            action.Invoke(message, value);
        }
    }
}
