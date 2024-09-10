namespace PsAsbUtils.Cmdlets.Exceptions;

public class PsSbInvalidSender : PsSbException
{
    public PsSbInvalidSender()
    {
    }

    public PsSbInvalidSender(string? message) : base(message)
    {
    }

    public PsSbInvalidSender(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
