using BusinessLogic.Entities.BackendAccess;
using BusinessLogic.ErrorManagement.BackendAccess;

namespace BusinessLogic.API;

public interface IBackendAccess
{
    /// <summary>
    ///     This method is used to get the user token from the API.
    /// </summary>
    /// <param name="username">The Username used to log into the LMS</param>
    /// <param name="password">The Password used to log into the LMS</param>
    /// <returns>A Token to pass to all other LMS Functions</returns>
    /// <exception cref="ErrorManagement.BackendAccess.BackendInvalidLoginException">
    /// Login credentials are invalid.
    /// </exception>
    /// <exception cref="BackendApiUnreachableException">
    /// The API healthcheck failed, the API is not reachable.
    /// </exception>
    public Task<UserToken> GetUserTokenAsync(string username, string password);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    /// <exception cref="BackendApiUnreachableException">
    /// The API healthcheck failed, the API is not reachable.
    /// </exception>
    public Task<UserInformation> GetUserInformationAsync(UserToken token);

    public Task UploadLearningWorldAsync(UserToken token, string backupPath, string awtPath,
        IProgress<int>? mockProgress = null);
}