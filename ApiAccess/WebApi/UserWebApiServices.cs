using ApiAccess.ApiResponses;
using Shared.Configuration;

namespace ApiAccess.WebApi;

public class UserWebApiServices : IUserWebApiServices
{
    public UserWebApiServices(IAuthoringToolConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IAuthoringToolConfiguration Configuration { get; }

    public async Task<UserTokenWebApiResponse> GetUserTokenAsync(string username, string password)
    {
        return await Task.FromResult(new UserTokenWebApiResponse("token"));
    }
}