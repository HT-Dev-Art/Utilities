namespace DevArt.Helpers.MessageQueues.Configurations;

public sealed  class QueueOptions
{
    public required string Host { get; init; }

    public required string Username { get; init; }

    public required string Password { get; init; }

    public required string QueueName { get; init; }
}
