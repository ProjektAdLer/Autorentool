using ElectronNET.API;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronWrapper;

class ServiceCollectionExtensionsWrapper : IServiceCollectionExtensionsWrapper
{
    /// <summary>
    /// Adds the <see cref="Electron"/> Members to the Service Collection
    /// </summary>
    public IServiceCollection AddElectron(IServiceCollection services)
    {
        return ServiceCollectionExtensions.AddElectron(services);
    }
}