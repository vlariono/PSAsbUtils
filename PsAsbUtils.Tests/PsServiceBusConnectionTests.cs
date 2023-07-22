using Azure.Messaging.ServiceBus;
using Moq;
using PsAsbUtils.Cmdlets.Core;
using PsAsbUtils.Cmdlets.Exceptions;

namespace PsAsbUtils.Tests;

public class PsServiceBusConnectionTests
{
    public class ServiceBusClientMock : ServiceBusClient
    {
        public ServiceBusClientMock()
        {

        }
    }


    [Fact]
    public void ThrowIfClosed()
    {
        var serviceBusConnectionMock = new Mock<ServiceBusClientMock>();
        serviceBusConnectionMock.Setup(c => c.IsClosed).Returns(true);
        var connection = PSServiceBusConnection.Create(serviceBusConnectionMock.Object);

        Assert.Throws<PsSbConnectionIsClosedException>(() => connection.GetReceiver("Test"));
        Assert.Throws<PsSbConnectionIsClosedException>(() => connection.GetSender("Test"));
    }

    [Fact]
    public void TryGetReceiver_Returns_False()
    {
        var serviceBusConnectionMock = new Mock<ServiceBusClientMock>();
        serviceBusConnectionMock.Setup(c => c.IsClosed).Returns(true);
        var connection = PSServiceBusConnection.Create(serviceBusConnectionMock.Object);

        var result = connection.TryGetReceiver("Test", out var receiver);
        Assert.False(result);
        Assert.Null(receiver);
    }
}