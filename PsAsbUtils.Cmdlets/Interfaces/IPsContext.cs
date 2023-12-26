using System.Management.Automation;

namespace PsAsbUtils.Cmdlets;

internal interface IPsContext
{
    public DefaultParameterDictionary? DefaultParameters { get; }
}
