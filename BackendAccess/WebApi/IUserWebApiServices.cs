using ApiAccess.BackendEntities;

namespace ApiAccess.WebApi;

public interface IUserWebApiServices
{
    /**
     * This method is used to get a token from the API.
     * It will throw an exception if the response is not successful.
     * 
     * @throws HttpRequestException if the response is not successful.
     * @param username The username of the user.
     * @param password The password of the user.
     * 
     * @return The token of the user in a DTO.
     */
    public Task<UserTokenWebApiBE> GetUserTokenAsync(string username, string password);
}