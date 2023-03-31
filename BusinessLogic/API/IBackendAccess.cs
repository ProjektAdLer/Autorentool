﻿using BusinessLogic.Entities.ApiElements;

namespace BusinessLogic.API;

public interface IBackendAccess
{
    public Task<UserToken> GetUserTokenAsync(string username, string password);
    public Task<UserInformation> GetUserInformationAsync(string token);
}