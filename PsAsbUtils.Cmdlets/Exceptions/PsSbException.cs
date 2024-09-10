using System.Runtime.Serialization;

namespace PsAsbUtils.Cmdlets.Exceptions;

public class PsSbException : Exception
{
    public PsSbException()
    {
    }

    public PsSbException(string? message) : base(message)
    {
    }

    public PsSbException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
