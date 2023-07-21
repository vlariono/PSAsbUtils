using System.Collections;
using System.Management.Automation;
using Azure.Messaging.ServiceBus;
using PsAsbUtils.Cmdlets.Cmdlets.Base;
using PsAsbUtils.Cmdlets.Constants;

namespace PsAsbUtils.Cmdlets;

[Cmdlet(VerbsCommon.New, $"{CmdletConst.Prefix}Message")]
public sealed class NewServiceBusMessage : PSAsyncCmdlet
{
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
    public string? MessageId { get; set; } = Guid.NewGuid().ToString("N");

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
        var message = new ServiceBusMessage();
        AssignIfSet(nameof(Body), p => message.Body = BinaryData.FromString((string)p));
        AssignIfSet(nameof(ContentType), p => message.ContentType = (string)p);
        AssignIfSet(nameof(CorrelationId), p => message.CorrelationId = (string)p);
        AssignIfSet(nameof(MessageId), p => message.MessageId = (string)p);
        AssignIfSet(nameof(PartitionKey), p => message.PartitionKey = (string)p);
        AssignIfSet(nameof(ReplyTo), p => message.ReplyTo = (string)p);
        AssignIfSet(nameof(ReplyToSessionId), p => message.ReplyToSessionId = (string)p);
        AssignIfSet(nameof(ScheduledEnqueueTime), p => message.ScheduledEnqueueTime = (DateTimeOffset)p);
        AssignIfSet(nameof(SessionId), p => message.SessionId = (string)p);
        AssignIfSet(nameof(Subject), p => message.Subject = (string)p);
        AssignIfSet(nameof(TimeToLive), p => message.TimeToLive = (TimeSpan)p);
        AssignIfSet(nameof(To), p => message.To = (string)p);
        AssignIfSet(nameof(TransactionPartitionKey), p => message.TransactionPartitionKey = (string)p);
        AssignIfSet(nameof(CustomProperties), p => message.ApplicationProperties.AddHashtable((Hashtable)p));

        WriteObject(message);
        return Task.CompletedTask;
    }

    private void AssignIfSet(string parameterName, Action<object> action)
    {
        if (MyInvocation.BoundParameters.TryGetValue(parameterName, out var value))
        {
            action.Invoke(value);
        }
    }
}
