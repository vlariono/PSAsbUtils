using namespace PsAsbUtils.Cmdlets.Interfaces
using namespace PsAsbUtils.Cmdlets.Constants

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
        $global:PSDefaultParameterValues["*-$prefix*:Connection"] = $Connection
    }
}