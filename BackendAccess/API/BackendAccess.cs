using AutoMapper;
using BackendAccess.BackendServices;
using BusinessLogic.API;
using BusinessLogic.Entities.BackendAccess;
using BusinessLogic.ErrorManagement.BackendAccess;

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
        if (!await UserWebApiServices.GetApiHealthcheck())
            throw new BackendApiUnreachableException("API healthcheck failed, not reachable");

        var receivedToken = await UserWebApiServices.GetUserTokenAsync(username, password);

        var retVal = Mapper.Map<UserToken>(receivedToken);

        return retVal;
    }

    public async Task<UserInformation> GetUserInformationAsync(UserToken token)
    {
        if (!await UserWebApiServices.GetApiHealthcheck())
            throw new BackendApiUnreachableException("API healthcheck failed, not reachable");

        var receivedUserInformation = await UserWebApiServices.GetUserInformationAsync(token.Token);

        return Mapper.Map<UserInformation>(receivedUserInformation);
    }

    public async Task<UploadResponse> UploadLearningWorldAsync(UserToken token, string backupPath, string awtPath,
        IProgress<int>? progress = null, CancellationToken? cancellationToken = null)
    {
        var responseBe = await UserWebApiServices.UploadLearningWorldAsync(token.Token, backupPath, awtPath, progress,
            cancellationToken);
        return Mapper.Map<UploadResponse>(responseBe);
    }

    public async Task<List<LmsWorld>> GetLmsWorldList(UserToken token, int authorId)
    {
        var worldsBe = await UserWebApiServices.GetLmsWorldList(token.Token, authorId);
        return Mapper.Map<List<LmsWorld>>(worldsBe);
    }
}