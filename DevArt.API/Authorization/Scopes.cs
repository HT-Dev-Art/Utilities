namespace DevArt.API.Authorization;

public static class Scopes
{
    public const string ClaimType = "scope";

    public const string ReadCurrentUser = "read:current_user";

    public const string ReadAllUsers = "read:all_users";

    public const string WriteCurrentUser = "write:current_user";

    public const string WriteAllUsers = "write:all_users";

    public static readonly IReadOnlyDictionary<string, ScopesFlags> ScopesDictionary =
        new Dictionary<string, ScopesFlags>
        {
            { ReadCurrentUser, ScopesFlags.ReadCurrentUser },
            { ReadAllUsers, ScopesFlags.ReadAllUsers },
            { WriteCurrentUser, ScopesFlags.WriteCurrentUser },
            { WriteAllUsers, ScopesFlags.WriteAllUsers }
        };
}

[Flags]
public enum ScopesFlags
{
    None = 0b_0,
    ReadCurrentUser = 0b_1,
    ReadAllUsers = 0b_10,
    WriteCurrentUser = 0b_100,
    WriteAllUsers = 0b_1000,

    User = ReadCurrentUser | WriteCurrentUser,
    Admin = ReadAllUsers | WriteAllUsers | User
}
