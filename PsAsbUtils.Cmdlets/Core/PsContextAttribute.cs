namespace PsAsbUtils.Cmdlets;

/// <summary>
/// Defines context provider property.
/// The property must be static and must return an instance of <see cref="IPsContext"/>.
/// This information is used by <see cref="PsQueueCompleterAttribute"/> to find the context.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class PsContextAttribute:Attribute
{
    /// <summary>
    /// Property name
    /// </summary>
    public required string ContextProvider { get; init; }
}
