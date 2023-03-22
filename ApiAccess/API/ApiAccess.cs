using ApiAccess.ApiResponses;
using ApiAccess.WebApi;
using AutoMapper;
using BusinessLogic.API;
using BusinessLogic.Entities.ApiElements;
using Shared.Configuration;

namespace ApiAccess.API;

public class ApiAccess : IApiAccess

{
    public ApiAccess( IMapper mapper, IUserWebApiServices userWebApiServices)
    {
        Mapper = mapper;
        UserWebApiServices = userWebApiServices;
    }

    // Frage an Niklas: Warum ist das hier public?
    // Wird das nur für die Tests benötigt? Ist doch eigendlich bad practice, oder?
    public IUserWebApiServices UserWebApiServices { get; }
    public IMapper Mapper { get; }
    public async Task<UserToken> GetUserTokenAsync(string username, string password)
    {
        var receivedToken = await UserWebApiServices.GetUserTokenAsync(username, password);
        
        return Mapper.Map<UserToken>(receivedToken);
    }
}