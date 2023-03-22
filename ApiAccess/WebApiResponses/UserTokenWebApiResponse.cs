namespace ApiAccess.ApiResponses;

public class UserTokenWebApiResponse
{
    public UserTokenWebApiResponse(string token)
    {
        Token = token;
    }

    public string Token { get; }
}