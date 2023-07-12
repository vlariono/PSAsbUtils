using namespace PsBusUtils.Cmdlets

function Set-AsbDefaultConnection
{
    [CmdletBinding()]
    param (
        # Connection to set
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [IServiceBusConnection]
        $Connection
    )

    process
    {
        $prefix = [CmdletConst]::Prefix
        $PSDefaultParameterValues["*-$prefix*:Connection"] = $Connection
    }
}