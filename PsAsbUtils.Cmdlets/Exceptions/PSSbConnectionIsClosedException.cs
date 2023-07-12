using System.Runtime.Serialization;

namespace PsAsbUtils.Cmdlets.Exceptions;

public class PSSbConnectionIsClosedException : PSSbException
{
    public PSSbConnectionIsClosedException()
    {
    }

    public PSSbConnectionIsClosedException(string? message) : base(message)
    {
    }

    public PSSbConnectionIsClosedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected PSSbConnectionIsClosedException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
