using System.Management.Automation;

namespace PsAsbUtils.Cmdlets.Interfaces;

internal interface IPsContext
{
    public DefaultParameterDictionary? DefaultParameters { get; }
}
