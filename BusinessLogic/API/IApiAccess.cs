using BusinessLogic.Entities.ApiElements;

namespace BusinessLogic.API;

public interface IApiAccess
{
    public Task<UserToken> GetUserTokenAsync(string username, string password);
}