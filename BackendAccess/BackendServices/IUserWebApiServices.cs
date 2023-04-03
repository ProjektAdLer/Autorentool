using ApiAccess.BackendEntities;

namespace ApiAccess.WebApi;

public interface IUserWebApiServices
{
    /// <summary>
    ///     This method is used to get the user token from the API.
    /// </summary>
    /// <param name="username">The Username used to log into the LMS</param>
    /// <param name="password">The Password used to log into the LMS</param>
    /// <returns>A Token to pass to all other LMS Functions</returns>
    public Task<UserTokenBE> GetUserTokenAsync(string username, string password);


    /// <summary>
    ///     This method is used to get the user information from the API.
    /// </summary>
    /// <param name="token">Token to authenticate the user</param>
    /// <returns>User Information</returns>
    public Task<UserInformationBE> GetUserInformationAsync(string token);

    public Task<bool> UploadLearningWorldAsync(string token, string backupPath, string awtPath);
}