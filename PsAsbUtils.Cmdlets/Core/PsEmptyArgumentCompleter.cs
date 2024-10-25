using System.Collections;
using System.Management.Automation;
using System.Management.Automation.Language;

namespace PsAsbUtils.Cmdlets.Core;

internal class PsEmptyArgumentCompleter : IArgumentCompleter
{
    public IEnumerable<CompletionResult> CompleteArgument(string commandName, string parameterName, string wordToComplete, CommandAst commandAst, IDictionary fakeBoundParameters)
    {
        return Array.Empty<CompletionResult>();
    }
}
