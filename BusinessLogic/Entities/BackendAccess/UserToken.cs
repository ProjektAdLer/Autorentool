namespace BusinessLogic.Entities.ApiElements;

public class UserToken : IUserToken
{
    public UserToken(string userToken = "")
    {
        Token = userToken;
    }

    public string Token { get; set; }
}