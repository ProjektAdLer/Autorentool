using ApiAccess.ApiResponses;

namespace ApiAccess.WebApi;

public interface IUserWebApiServices
{

    public  Task<UserTokenWebApiResponse> GetUserTokenAsync(string username, string password);
}