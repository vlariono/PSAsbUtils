using System.Runtime.CompilerServices;
using Azure.Messaging.ServiceBus;
using PsAsbUtils.Cmdlets.Exceptions;
using PsAsbUtils.Cmdlets.Interfaces;

namespace PsAsbUtils.Cmdlets.Core;

public sealed class PSServiceBusConnection : IServiceBusConnection, IMessageTracker
{
    private static readonly ConditionalWeakTable<IServiceBusConnection, ServiceBusClient> s_connections = new();

    private readonly ServiceBusClient _client;
    private readonly ConditionalWeakTable<object, ServiceBusReceiver> _receivers;

    private PSServiceBusConnection(ServiceBusClient client)
    {
        _client = client;
        _receivers = new();
    }

    public string Namespace => _client.FullyQualifiedNamespace;

    public string Identifier => _client.Identifier;
    public bool IsClosed => _client.IsClosed;

    public ServiceBusReceiver GetReceiver(string queueName)
    {
        ThrowIfClosed();
        var receiver = new PSServiceBusReceiver(_client.CreateReceiver(queueName));
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

    public async ValueTask DisposeAsync()
    {
        await _client.DisposeAsync();
        s_connections.Remove(this);
    }

    internal static IServiceBusConnection Create(ServiceBusClient client)
    {
        var connection = new PSServiceBusConnection(client);
        s_connections.TryAdd(connection, client);
        return connection;
    }

    internal static IEnumerable<IServiceBusConnection> Get()
    {
        foreach (var connection in s_connections)
        {
            yield return connection.Key;
        }
    }

    private void ThrowIfClosed()
    {
        if (_client.IsClosed)
        {
            throw new PsSbConnectionIsClosedException();
        }
    }
}
