using System.Runtime.Serialization;

namespace PsAsbUtils.Cmdlets.Exceptions;

public class PSSbException : Exception
{
    public PSSbException()
    {
    }

    public PSSbException(string? message) : base(message)
    {
    }

    public PSSbException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected PSSbException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
