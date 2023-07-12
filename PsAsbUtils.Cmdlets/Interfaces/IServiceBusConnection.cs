using Azure.Messaging.ServiceBus;

namespace PsAsbUtils.Cmdlets.Interfaces;

public interface IServiceBusConnection : IAsyncDisposable
{
    public string Namespace { get; }
    public string Identifier { get; }

    public bool IsClosed { get; }

    public ServiceBusReceiver GetReceiver(string queueName);

    public bool TryGetReceiver(object message, out ServiceBusReceiver? receiver);

    public ServiceBusSender GetSender(string queueName);
}
