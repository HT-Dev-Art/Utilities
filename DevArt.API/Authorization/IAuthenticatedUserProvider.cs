namespace DevArt.API.Authorization;

public interface IAuthenticatedUserProvider
{
    AuthenticatedUser User { get; }
}