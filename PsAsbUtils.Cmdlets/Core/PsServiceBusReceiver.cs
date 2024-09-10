using System.Runtime.CompilerServices;
using Azure.Messaging.ServiceBus;
using PsAsbUtils.Cmdlets.Interfaces;

namespace PsAsbUtils.Cmdlets.Core;

internal class PsServiceBusReceiver : ServiceBusReceiver, ITrackable
{
    private readonly ServiceBusReceiver _receiver;

    private IMessageTracker? _tracker;

    internal PsServiceBusReceiver(ServiceBusReceiver receiver)
    {
        _receiver = receiver;
    }

    public override string FullyQualifiedNamespace => _receiver.FullyQualifiedNamespace;
    public override string Identifier => _receiver.Identifier;
    public override bool IsClosed => _receiver.IsClosed;
    public override string EntityPath => _receiver.EntityPath;
    public override int PrefetchCount => _receiver.PrefetchCount;
    public override ServiceBusReceiveMode ReceiveMode => _receiver.ReceiveMode;

    public override Task AbandonMessageAsync(ServiceBusReceivedMessage message, IDictionary<string, object>? propertiesToModify = null, CancellationToken cancellationToken = default)
    {
        return _receiver.AbandonMessageAsync(message, propertiesToModify, cancellationToken);
    }

    public override Task CloseAsync(CancellationToken cancellationToken = default)
    {
        return _receiver.CloseAsync(cancellationToken);
    }

    public override Task CompleteMessageAsync(ServiceBusReceivedMessage message, CancellationToken cancellationToken = default)
    {
        return _receiver.CompleteMessageAsync(message, cancellationToken);
    }

    public override Task DeadLetterMessageAsync(ServiceBusReceivedMessage message, IDictionary<string, object>? propertiesToModify = null, CancellationToken cancellationToken = default)
    {
        return _receiver.DeadLetterMessageAsync(message, propertiesToModify, cancellationToken);
    }

    public override Task DeadLetterMessageAsync(ServiceBusReceivedMessage message, IDictionary<string, object> propertiesToModify, string deadLetterReason, string? deadLetterErrorDescription = null, CancellationToken cancellationToken = default)
    {
        return _receiver.DeadLetterMessageAsync(message, propertiesToModify, deadLetterReason, deadLetterErrorDescription, cancellationToken);
    }

    public override Task DeadLetterMessageAsync(ServiceBusReceivedMessage message, string deadLetterReason, string? deadLetterErrorDescription = null, CancellationToken cancellationToken = default)
    {
        return _receiver.DeadLetterMessageAsync(message, deadLetterReason, deadLetterErrorDescription, cancellationToken);
    }

    public override Task DeferMessageAsync(ServiceBusReceivedMessage message, IDictionary<string, object>? propertiesToModify = null, CancellationToken cancellationToken = default)
    {
        return _receiver.DeferMessageAsync(message, propertiesToModify, cancellationToken);
    }

    public override ValueTask DisposeAsync()
    {
        _tracker = null;
        return _receiver.DisposeAsync();
    }

    public override bool Equals(object obj)
    {
        return _receiver.Equals(obj);
    }

    public override int GetHashCode()
    {
        return _receiver.GetHashCode();
    }

    public override Task<ServiceBusReceivedMessage> PeekMessageAsync(long? fromSequenceNumber = null, CancellationToken cancellationToken = default)
    {
        return _receiver.PeekMessageAsync(fromSequenceNumber, cancellationToken);
    }

    public override Task<IReadOnlyList<ServiceBusReceivedMessage>> PeekMessagesAsync(int maxMessages, long? fromSequenceNumber = null, CancellationToken cancellationToken = default)
    {
        return _receiver.PeekMessagesAsync(maxMessages, fromSequenceNumber, cancellationToken);
    }

    public override Task<ServiceBusReceivedMessage> ReceiveDeferredMessageAsync(long sequenceNumber, CancellationToken cancellationToken = default)
    {
        return _receiver.ReceiveDeferredMessageAsync(sequenceNumber, cancellationToken);
    }

    public override Task<IReadOnlyList<ServiceBusReceivedMessage>> ReceiveDeferredMessagesAsync(IEnumerable<long> sequenceNumbers, CancellationToken cancellationToken = default)
    {
        return _receiver.ReceiveDeferredMessagesAsync(sequenceNumbers, cancellationToken);
    }

    public override async Task<ServiceBusReceivedMessage> ReceiveMessageAsync(TimeSpan? maxWaitTime = null, CancellationToken cancellationToken = default)
    {
        var message = await _receiver.ReceiveMessageAsync(maxWaitTime, cancellationToken);
        _tracker?.OnReceived(message, this);
        return message;
    }

    public override IAsyncEnumerable<ServiceBusReceivedMessage> ReceiveMessagesAsync(CancellationToken cancellationToken = default)
    {
        return _receiver.ReceiveMessagesAsync(cancellationToken);
    }

    public override Task<IReadOnlyList<ServiceBusReceivedMessage>> ReceiveMessagesAsync(int maxMessages, TimeSpan? maxWaitTime = null, CancellationToken cancellationToken = default)
    {
        return _receiver.ReceiveMessagesAsync(maxMessages, maxWaitTime, cancellationToken);
    }

    public override Task RenewMessageLockAsync(ServiceBusReceivedMessage message, CancellationToken cancellationToken = default)
    {
        return _receiver.RenewMessageLockAsync(message, cancellationToken);
    }

    public override string ToString()
    {
        return _receiver.ToString();
    }

    public void Track(IMessageTracker tracker)
    {
        _tracker = tracker;
    }
}
