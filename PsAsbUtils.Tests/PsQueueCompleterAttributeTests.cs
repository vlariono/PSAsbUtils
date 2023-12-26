using System.Collections;
using System.Management.Automation;
using Moq;
using PsAsbUtils.Cmdlets;
using PsAsbUtils.Cmdlets.Cmdlets.Base;
using PsAsbUtils.Cmdlets.Constants;
using PsAsbUtils.Cmdlets.Interfaces;

namespace PsAsbUtils.Tests;

[PsContext(ContextProvider = nameof(Context))]
public class ContextProviderMock
{
    internal static IPsContext Context { get; set; }
}

public class PsQueueCompleterAttributeTests : IClassFixture<ContextProviderMock>
{
    private readonly ContextProviderMock _contextProviderMock;

    public PsQueueCompleterAttributeTests(ContextProviderMock contextProviderMock)
    {
        _contextProviderMock = contextProviderMock;
    }

    [Fact]
    public void ContextIsNull()
    {
        var attribute = new PsQueueCompleterAttribute
        {
            ContextProvider = null
        };

        var completer = attribute.Create();
        Assert.IsType<PsEmptyArgumentCompleter>(completer);
        Assert.Empty(completer.CompleteArgument(string.Empty, string.Empty, string.Empty, null, new Hashtable()));
    }

    [Fact]
    public void ContextProviderWithoutContextMethod()
    {
        var attribute = new PsQueueCompleterAttribute
        {
            ContextProvider = typeof(object)
        };

        var completer = attribute.Create();
        Assert.IsType<PsEmptyArgumentCompleter>(completer);
        Assert.Empty(completer.CompleteArgument(string.Empty, string.Empty, string.Empty, null, new Hashtable()));
    }

    [Fact]
    public void ContextProviderReturnsContext()
    {
        var contextMock = new Mock<IPsContext>();
        var connectionMock = new Mock<IServiceBusConnection>();

        connectionMock.Setup(p => p.GetQueueCompletion(It.IsAny<string>(), It.IsAny<int>())).Returns(new List<CompletionResult>(){
            new("test","test",CompletionResultType.Text,"test")
        });

        contextMock.Setup(p => p.DefaultParameters).Returns(new DefaultParameterDictionary()
        {
            {
                PsModule.DefaultConnectionPrefix,connectionMock.Object
            }
        });

        var context = contextMock.Object;
        ContextProviderMock.Context = context;

        var attribute = new PsQueueCompleterAttribute
        {
            ContextProvider = typeof(ContextProviderMock)
        };

        var completer = attribute.Create();
        var result = completer.CompleteArgument("test", "test", "test", null, new Hashtable());
        Assert.Single(result);
        Assert.Equal("test", result.First().CompletionText);
    }

    [Fact]
    public void ContextProviderReturnsNull()
    {
        ContextProviderMock.Context = null;

        var attribute = new PsQueueCompleterAttribute
        {
            ContextProvider = typeof(ContextProviderMock)
        };

        var completer = attribute.Create();
        Assert.IsType<PsEmptyArgumentCompleter>(completer);
    }

    [Fact]
    public void BoundParametersContainConnection()
    {
        var contextMock = new Mock<IPsContext>();
        var connectionMock = new Mock<IServiceBusConnection>();

        connectionMock.Setup(p => p.GetQueueCompletion(It.IsAny<string>(), It.IsAny<int>())).Returns(new List<CompletionResult>(){
            new("test","test",CompletionResultType.Text,"test")
        });

        var context = contextMock.Object;
        ContextProviderMock.Context = context;
        var attribute = new PsQueueCompleterAttribute()
        {
            ContextProvider = typeof(ContextProviderMock)
        };

        var completer = attribute.Create();
        var dictionaryMock = new Hashtable
        {
            {
                nameof(ServiceBusCmdlet.Connection),
                new PSObject(connectionMock.Object)
            }
        };
        var result = completer.CompleteArgument("test", "test", "test", null, dictionaryMock);
        Assert.Single(result);
        Assert.Equal("test", result.First().CompletionText);
    }

    [Fact]
    public void ConnectionCannotBeFoundInAnyWays()
    {
        var contextMock = new Mock<IPsContext>();
        var context = contextMock.Object;
        ContextProviderMock.Context = context;
        var attribute = new PsQueueCompleterAttribute()
        {
            ContextProvider = typeof(ContextProviderMock)
        };

        var completer = attribute.Create();
        var result = completer.CompleteArgument("test", "test", "test", null, new Hashtable());
        Assert.Empty(result);
    }
}
