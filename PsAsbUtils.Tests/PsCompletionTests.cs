using Azure;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Moq;
using PsAsbUtils.Cmdlets.Core;
using PsAsbUtils.Cmdlets.Interfaces;

namespace PsAsbUtils.Tests;

public class PsCompletionTests
{
    [Fact]
    public void CompletionReturned()
    {
        var serviceBusAdminMock = new Mock<ServiceBusAdministrationClient>();
        serviceBusAdminMock
                .Setup(x => x.GetQueuesAsync(It.IsAny<CancellationToken>()))
                .Returns(() =>
                {
                    var responseList = new List<QueueProperties>();
                    for (int i = 0; i < 10; i++)
                    {
                        responseList.Add(ServiceBusModelFactory.QueueProperties(
                            name: $"UnitTest{i}",
                            lockDuration: TimeSpan.FromMinutes(5),
                            defaultMessageTimeToLive: TimeSpan.FromMinutes(10),
                            autoDeleteOnIdle: TimeSpan.FromMinutes(10),
                            duplicateDetectionHistoryTimeWindow: TimeSpan.FromMinutes(30),
                            maxDeliveryCount: 2,
                            userMetadata: "Test"));
                    }

                    var page = Page<QueueProperties>.FromValues(responseList, null, new Mock<Response>().Object);
                    return AsyncPageable<QueueProperties>.FromPages(new[] { page });
                });

        var completionEngine = new PsCompletion(serviceBusAdminMock.Object);
        var connectionMock = new Mock<IServiceBusConnection>();
        var completions = completionEngine.QueueCompletion(connectionMock.Object, "Unit.*", 2);
        Assert.Equal(2, completions.Count());
        Assert.Matches(@"UnitTest\d", completions.First().CompletionText);
    }

        [Fact]
    public void CompletionException()
    {
        var serviceBusAdminMock = new Mock<ServiceBusAdministrationClient>();
        serviceBusAdminMock
                .Setup(x => x.GetQueuesAsync(It.IsAny<CancellationToken>()))
                .Throws<Exception>();

        var completionEngine = new PsCompletion(serviceBusAdminMock.Object);
        var connectionMock = new Mock<IServiceBusConnection>();
        var completions = completionEngine.QueueCompletion(connectionMock.Object, "Unit.*", 2);
        Assert.Empty(completions);
    }

}
