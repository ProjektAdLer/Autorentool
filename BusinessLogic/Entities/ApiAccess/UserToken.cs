using JetBrains.Annotations;

namespace BusinessLogic.Entities.ApiElements;

public class UserToken : IUserToken
{
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    [UsedImplicitly]
    private UserToken()
    {
    }
    
    public UserToken(string userToken)
    {
        Token = userToken;
    }
    
    public string Token { get; set; }
}