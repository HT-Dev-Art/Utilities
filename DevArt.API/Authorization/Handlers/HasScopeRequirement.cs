using Microsoft.AspNetCore.Authorization;

namespace DevArt.API.Authorization.Handlers;

public sealed class HasScopeRequirement(string issuer, ScopesFlags scopes) : IAuthorizationRequirement
{
    public string Issuer { get; } = issuer ?? throw new ArgumentNullException(nameof(issuer));

    public ScopesFlags Scopes { get; } = scopes;
}
