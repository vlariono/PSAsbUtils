using System.Runtime.Serialization;

namespace PsAsbUtils.Cmdlets.Exceptions;

public class PsSbConnectionIsClosedException : PsSbException
{
    public PsSbConnectionIsClosedException()
    {
    }

    public PsSbConnectionIsClosedException(string? message) : base(message)
    {
    }

    public PsSbConnectionIsClosedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected PsSbConnectionIsClosedException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
