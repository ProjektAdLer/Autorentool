using H5pPlayer.BusinessLogic.Api.FileSystemDataAccess;
using H5pPlayer.DataAccess.FileSystem;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace H5pPlayer.Main;

public static class H5pPlayerStartup
{
    public static void ConfigureH5pPlayer(IServiceCollection services)
    {
        services.AddTransient<IStartH5pPlayerFactory, StartH5pPlayerFactory>();
        services.AddTransient<IDisplayH5pFactory, DisplayH5pFactory>();
        services.AddTransient<IValidateH5pFactory, ValidateH5pFactory>();
        services.AddTransient<IFileSystemDataAccess, FileSystemDataAccess>();
        services.AddSingleton<IStringLocalizer>(sp =>
        {
            var factory = sp.GetRequiredService<IStringLocalizerFactory>();
            return factory.Create(
                "H5pPlayerText", // Basisname deiner ResX in H5pPlayer/Resources
                typeof(H5pPlayerStartup).Assembly.GetName().Name // Assembly-Name von H5pPlayer
            );
        });
    }

}
