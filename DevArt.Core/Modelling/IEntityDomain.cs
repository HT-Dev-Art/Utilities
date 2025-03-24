namespace DevArt.Core.Modelling;

public interface IEntityDomain<out TId>
    where TId : notnull
{
    TId Id { get; }

    DateTimeOffset CreatedAt { get; }

    DateTimeOffset UpdatedAt { get; }
}
