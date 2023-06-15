using AutoMapper;
using BackendAccess.BackendServices;
using BusinessLogic.API;
using BusinessLogic.Entities.BackendAccess;

namespace BackendAccess.API;

public class BackendAccess : IBackendAccess

{
    public BackendAccess(IMapper mapper, IUserWebApiServices userWebApiServices)
    {
        Mapper = mapper;
        UserWebApiServices = userWebApiServices;
    }

    public IUserWebApiServices UserWebApiServices { get; }
    public IMapper Mapper { get; }

    /// <inheritdoc cref="IBackendAccess.GetUserTokenAsync"/>
    public async Task<UserToken> GetUserTokenAsync(string username, string password)
    {
        var receivedToken = await UserWebApiServices.GetUserTokenAsync(username, password);

        var retVal = Mapper.Map<UserToken>(receivedToken);

        return retVal;
    }

    public async Task<UserInformation> GetUserInformationAsync(UserToken token)
    {
        var receivedUserInformation = await UserWebApiServices.GetUserInformationAsync(token.Token);

        return Mapper.Map<UserInformation>(receivedUserInformation);
    }

    public async Task UploadLearningWorldAsync(UserToken token, string backupPath, string awtPath,
        IProgress<int>? mockProgress = null)
    {
        await UserWebApiServices.UploadLearningWorldAsync(token.Token, backupPath, awtPath, mockProgress);
    }
}