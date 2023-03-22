using JetBrains.Annotations;

namespace ApiAccess.ApiResponses;

public class UserTokenWebApiResponse
{
    /// <summary>
    ///     Private Constructor for AutoMapper
    /// </summary>
    [UsedImplicitly]
    private UserTokenWebApiResponse()
    {
        // Frage an Niklas: WTF? Plz explain. Vor allem wegen Testabdeckung
    }

    public UserTokenWebApiResponse(string token)
    {
        Token = token;
    }

    public string Token { get; }
}