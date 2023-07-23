using Azure.Messaging.ServiceBus;
using Moq;
using PsAsbUtils.Cmdlets.Interfaces;
using PsAsbUtils.Cmdlets.Core;


namespace PsAsbUtils.Tests;

public class PsServiceBusReceiverTests
{
    public class ServiceBusReceiverMock : ServiceBusReceiver
    {
        public override Task<ServiceBusReceivedMessage> ReceiveMessageAsync(TimeSpan? maxWaitTime = null, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(ServiceBusModelFactory.ServiceBusReceivedMessage());
        }
    }

    [Fact]
    public async Task MessageIsTracked()
    {
        var tracker = new Mock<IMessageTracker>();
        tracker.Setup(tr => tr.OnReceived(It.IsAny<ServiceBusReceivedMessage>(), It.IsAny<ServiceBusReceiver>()));
        var receiver = new PsServiceBusReceiver(new ServiceBusReceiverMock());
        receiver.Track(tracker.Object);

        var message = await receiver.ReceiveMessageAsync();

        tracker.Verify(tr => tr.OnReceived(It.IsAny<ServiceBusReceivedMessage>(), It.IsAny<ServiceBusReceiver>()), Times.Once());
    }
}
