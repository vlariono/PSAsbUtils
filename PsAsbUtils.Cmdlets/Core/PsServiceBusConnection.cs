using System.Management.Automation;
using System.Runtime.CompilerServices;
using Azure.Messaging.ServiceBus;
using PsAsbUtils.Cmdlets.Exceptions;
using PsAsbUtils.Cmdlets.Interfaces;

namespace PsAsbUtils.Cmdlets.Core;

internal sealed class PsServiceBusConnection : IServiceBusConnection, IMessageTracker
{
    private readonly ServiceBusClient _client;
    private readonly ICompletion _completion;
    private readonly ConditionalWeakTable<object, ServiceBusReceiver> _receivers;

    internal PsServiceBusConnection(ServiceBusClient client, ICompletion completion)
    {
        _client = client;
        _completion = completion;
        _receivers = new();
    }

    public string Namespace => _client.FullyQualifiedNamespace;

    public string Identifier => _client.Identifier;
    public bool IsClosed => _client.IsClosed;

    public ServiceBusReceiver GetReceiver(string queueName)
    {
        ThrowIfClosed();
        var receiver = new PsServiceBusReceiver(_client.CreateReceiver(queueName));
        receiver.Track(this);
        return receiver;
    }

    public ServiceBusSender GetSender(string queueName)
    {
        ThrowIfClosed();
        return _client.CreateSender(queueName);
    }

    public void OnReceived(ServiceBusReceivedMessage message, ServiceBusReceiver receiver)
    {
        _receivers.TryAdd(message, receiver);
    }

    public bool TryGetReceiver(object message, out ServiceBusReceiver? receiver)
    {
        receiver = null;
        if (!_client.IsClosed && _receivers.TryGetValue(message, out var messageReceiver) && !messageReceiver.IsClosed)
        {
            receiver = messageReceiver;
            return true;
        }

        return false;
    }

    public IEnumerable<CompletionResult> GetQueueCompletion(string pattern, int maxCount)
    {
        ThrowIfClosed();
        return _completion.QueueCompletion(this, pattern, maxCount);
    }

    public void Dispose()
    {
        _client.DisposeAsync().AsTask().Wait();
    }

    private void ThrowIfClosed()
    {
        if (_client.IsClosed)
        {
            throw new PsSbConnectionIsClosedException();
        }
    }
}
