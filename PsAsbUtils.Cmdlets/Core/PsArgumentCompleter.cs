using System.Collections;
using System.Management.Automation;
using System.Management.Automation.Language;
using PsAsbUtils.Cmdlets.Cmdlets.Base;
using PsAsbUtils.Cmdlets.Constants;
using PsAsbUtils.Cmdlets.Interfaces;

namespace PsAsbUtils.Cmdlets.Core;

internal class PsArgumentCompleter : IArgumentCompleter
{
    private readonly IPsContext _context;

    internal PsArgumentCompleter(IPsContext context)
    {
        _context = context;
    }

    public IEnumerable<CompletionResult> CompleteArgument(string commandName, string parameterName, string wordToComplete, CommandAst commandAst, IDictionary fakeBoundParameters)
    {
        var connectionObject = fakeBoundParameters[nameof(ServiceBusCmdlet.Connection)];
        if(connectionObject is PSObject pSObject)
        {
            connectionObject = pSObject.BaseObject;
        }

        var connection = connectionObject as IServiceBusConnection;
        connection ??= _context.DefaultParameters?[PsModule.DefaultConnectionPrefix] as IServiceBusConnection;

        return connection is null
            ? Array.Empty<CompletionResult>()
            : connection.GetQueueCompletion($"^{wordToComplete}.*", 10);
    }
}
