using System.Runtime.Serialization;

namespace PsAsbUtils.Cmdlets.Exceptions;

public class PSSbInvalidSender : PSSbException
{
    public PSSbInvalidSender()
    {
    }

    public PSSbInvalidSender(string? message) : base(message)
    {
    }

    public PSSbInvalidSender(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected PSSbInvalidSender(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
