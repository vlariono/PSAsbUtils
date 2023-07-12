using Azure.Messaging.ServiceBus;
using Moq;
using PsAsbUtils.Cmdlets.Core;
using PsAsbUtils.Cmdlets.Exceptions;

namespace PsAsbUtils.Tests;

public class ServiceBusClientMock : ServiceBusClient
{
    public ServiceBusClientMock()
    {

    }
}

public class PSServiceBusConnectionTests
{
    [Fact]
    public void ThrowIfClosed()
    {
        var serviceBusConnectionMock = new Mock<ServiceBusClientMock>();
        serviceBusConnectionMock.Setup(c => c.IsClosed).Returns(true);
        var connection = PSServiceBusConnection.Create(serviceBusConnectionMock.Object);

        Assert.Throws<PSSbConnectionIsClosedException>(() => connection.GetReceiver("Test"));
        Assert.Throws<PSSbConnectionIsClosedException>(() => connection.GetSender("Test"));
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