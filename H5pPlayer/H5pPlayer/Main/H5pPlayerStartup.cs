using H5pPlayer.Presentation.PresentationLogic.DisplayH5p;
using Microsoft.Extensions.DependencyInjection;

namespace H5pPlayer.Main;

public static class H5pPlayerStartup
{
    public static void ConfigureH5pPlayer(IServiceCollection services)
    {
        services.AddTransient<IStartH5pPlayerFactory, StartH5pPlayerFactory>();
        services.AddTransient<IDisplayH5pFactory, DisplayH5pFactory>();

    }


}