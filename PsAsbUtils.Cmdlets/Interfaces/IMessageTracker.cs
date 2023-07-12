using Azure.Messaging.ServiceBus;

namespace PsAsbUtils.Cmdlets.Interfaces;

internal interface IMessageTracker
{
    public void OnReceived(ServiceBusReceivedMessage message, ServiceBusReceiver receiver);
}
