using ApiAccess.WebApi;
using AutoMapper;
using BusinessLogic.API;
using BusinessLogic.Entities.ApiElements;

namespace ApiAccess.API;

public class BackendAccess : IBackendAccess

{
    public BackendAccess(IMapper mapper, IUserWebApiServices userWebApiServices)
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

    public async Task<UserInformation> GetUserInformationAsync(string token)
    {
        var receivedUserInformation = await UserWebApiServices.GetUserInformationAsync(token);

        return Mapper.Map<UserInformation>(receivedUserInformation);
    }

    public async Task<bool> UploadLearningWorldAsync(string token, string backupPath, string awtPath)
    {
        var isSuccessful = await UserWebApiServices.UploadLearningWorldAsync(token, backupPath, awtPath);

        return isSuccessful;
    }
}