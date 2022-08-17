using Microsoft.AspNetCore.Hosting;

namespace ElectronWrapper;

class WebHostBuilderExtensionsWrapper : IWebHostBuilderExtensionsWrapper
{
    /// <summary>
    /// Use a Electron support for this .NET Core Project.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="args">The arguments.</param>
    /// <returns></returns>
    public IWebHostBuilder UseElectron(IWebHostBuilder builder, string[] args)
    {
        return ElectronNET.API.WebHostBuilderExtensions.UseElectron(builder, args);
    }
}