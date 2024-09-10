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
}
