﻿namespace ApiAccess.BackendEntities;

public class UserInformationBE
{
    public string LmsUserName { get; init; }
    public bool IsAdmin { get; init; }
    public int UserId { get; init; }
    public string UserEmail { get; init; }
}