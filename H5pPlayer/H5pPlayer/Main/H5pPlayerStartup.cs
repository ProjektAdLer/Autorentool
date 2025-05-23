﻿using H5pPlayer.BusinessLogic.Api.FileSystemDataAccess;
using H5pPlayer.DataAccess.FileSystem;
using Microsoft.Extensions.DependencyInjection;

namespace H5pPlayer.Main;

public static class H5pPlayerStartup
{
    public static void ConfigureH5pPlayer(IServiceCollection services)
    {
        services.AddTransient<IStartH5pPlayerFactory, StartH5pPlayerFactory>();
        services.AddTransient<IDisplayH5pFactory, DisplayH5pFactory>();
        services.AddTransient<IValidateH5pFactory, ValidateH5pFactory>();
        services.AddTransient<IFileSystemDataAccess, FileSystemDataAccess>();

    }


}