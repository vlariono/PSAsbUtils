using namespace PsAsbUtils.Cmdlets.Interfaces
using namespace System.Management.Automation
using namespace PsAsbUtils.Cmdlets.Constants
using namespace PsAsbUtils.Cmdlets.Core

$completionScript = {
    param(
        $CommandName,
        $ParameterName,
        $WordToComplete,
        $CommandAst,
        $BoundParameters
    )

    $connection = [IServiceBusConnection]$BoundParameters['Connection']
    if ($null -eq $connection)
    {
        $connection = [IServiceBusConnection] $PSDefaultParameterValues[[PsModule]::DefaultConnectionPrefix]
    }

    if ($null -eq $connection)
    {
        return
    }

    foreach ($completion in $connection.GetQueueCompletion("^$WordToComplete.*", 10))
    {
        [CompletionResult]$completion
    }
}

$commands = Get-Command -Module PsAsbUtils | Select-Object -ExpandProperty Name

if ($null -ne $commands)
{
    Register-ArgumentCompleter -CommandName $commands -ParameterName QueueName -ScriptBlock $completionScript
}