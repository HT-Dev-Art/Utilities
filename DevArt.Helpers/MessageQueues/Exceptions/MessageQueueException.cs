namespace DevArt.Helpers.MessageQueues.Exceptions;

[Serializable]
public sealed class MessageQueueException : Exception
{
    public MessageQueueException(string message) : base(message)
    {
    }

    public MessageQueueException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
