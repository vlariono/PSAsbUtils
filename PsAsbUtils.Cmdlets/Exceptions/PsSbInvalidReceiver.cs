using System.Runtime.Serialization;

namespace PsAsbUtils.Cmdlets.Exceptions;

public class PsSbInvalidReceiver : Exception
{
    public PsSbInvalidReceiver()
    {
    }

    public PsSbInvalidReceiver(string? message) : base(message)
    {
    }

    public PsSbInvalidReceiver(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected PsSbInvalidReceiver(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
