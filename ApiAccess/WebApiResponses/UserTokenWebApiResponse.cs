namespace ApiAccess.ApiResponses;

public class UserTokenWebApiResponse
{
    public UserTokenWebApiResponse(string lmsToken)
    {
        LmsToken = lmsToken;
    }

    public string LmsToken { get; }
}