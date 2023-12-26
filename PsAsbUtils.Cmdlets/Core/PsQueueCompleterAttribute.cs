using System.Collections;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Reflection;

namespace PsAsbUtils.Cmdlets;

internal sealed class PsQueueCompleterAttribute : ArgumentCompleterAttribute, IArgumentCompleterFactory
{
    private static readonly PsEmptyArgumentCompleter s_emptyCompleter = new();

    public Type? ContextProvider { get; init; }

    public IArgumentCompleter Create()
    {
        if (ContextProvider is null)
        {
            return s_emptyCompleter;
        }

        var contextProviderProperty = ContextProvider.GetCustomAttribute<PsContextAttribute>()?.ContextProvider;
        if (contextProviderProperty is null)
        {
            return s_emptyCompleter;
        }

        var property = ContextProvider.GetProperty(contextProviderProperty, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);

        if (property?.GetValue(null) is not IPsContext context)
        {
            return s_emptyCompleter;
        }

        return new PsArgumentCompleter(context);
    }
}
