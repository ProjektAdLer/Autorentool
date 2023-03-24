using ApiAccess.WebApi;
using AutoMapper;
using BusinessLogic.API;
using BusinessLogic.Entities.ApiElements;

namespace ApiAccess.API;

public class ApiAccess : IApiAccess

{
    public ApiAccess(IMapper mapper, IUserWebApiServices userWebApiServices)
    {
        Mapper = mapper;
        UserWebApiServices = userWebApiServices;
    }

    public IUserWebApiServices UserWebApiServices { get; }
    public IMapper Mapper { get; }

    public async Task<UserToken> GetUserTokenAsync(string username, string password)
    {
        var receivedToken = await UserWebApiServices.GetUserTokenAsync(username, password);

        var retVal = Mapper.Map<UserToken>(receivedToken);

        return retVal;
    }
}