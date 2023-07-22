using namespace PsAsbUtils.Cmdlets.Interfaces
using namespace System.Management.Automation
using namespace PsAsbUtils.Cmdlets.Constants

$completionScript = {
    param(
        $CommandName,
        $ParameterName,
        $WordToComplete,
        $CommandAst,
        $BoundParameters
    )

    $connection = [IServiceBusConnection]$BoundParameters['Connection']
    if($connection -eq $null)
    {
        $connection = $PSDefaultParameterValues[[PsModule]::DefaultConnectionPrefix]
    }

    foreach($queue in $connection.GetQueues()|Where-Object {$_.Name -like "$WordToComplete*"})
    {
        [CompletionResult]::new($queue.Name, $queue.Name,[CompletionResultType]::ParameterValue, $queue.Name)
    }
}

$commands = Get-Command -Module PsAsbUtils|Select-Object -ExpandProperty Name
Register-ArgumentCompleter -CommandName $commands -ParameterName QueueName -ScriptBlock $completionScript