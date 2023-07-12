using System.Runtime.Serialization;

namespace PsAsbUtils.Cmdlets.Exceptions;

public class PSSbInvalidReceiver : Exception
{
    public PSSbInvalidReceiver()
    {
    }

    public PSSbInvalidReceiver(string? message) : base(message)
    {
    }

    public PSSbInvalidReceiver(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected PSSbInvalidReceiver(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
