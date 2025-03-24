using DevArt.API.Authorization.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace DevArt.API.Authorization;
public sealed class AuthenticatedUserProvider : IAuthenticatedUserProvider
{
    public AuthenticatedUserProvider(IHttpContextAccessor httpContextAccessor, IOptions<JwtBearerOptions> jwtOptions)
    {
        var context = httpContextAccessor.HttpContext
                      ?? throw new HttpContextNotFoundException("HttpContext not found.");

        User = new AuthenticatedUser(context, jwtOptions.Value.Authority ?? string.Empty);
    }

    public AuthenticatedUser User { get; }
}