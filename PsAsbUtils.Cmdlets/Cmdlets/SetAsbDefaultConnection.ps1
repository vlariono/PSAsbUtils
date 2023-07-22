using namespace PsAsbUtils.Cmdlets.Interfaces
using namespace PsAsbUtils.Cmdlets.Constants

function Set-AsbDefaultConnection
{
    [CmdletBinding()]
    param (
        # Connection to set
        [Parameter(Mandatory = $true, ValueFromPipeline = $true, Position = 0)]
        [IServiceBusConnection]
        $DefaultConnection
    )

    process
    {
        $global:PSDefaultParameterValues[[PsModule]::DefaultConnectionPrefix] = $DefaultConnection
    }
}