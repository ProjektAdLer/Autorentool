namespace BusinessLogic.Entities.BackendAccess;

public class UserToken : IUserToken
{
    public UserToken(string userToken = "")
    {
        Token = userToken;
    }

    public string Token { get; private set; } // private set is needed for AutoMapper
}