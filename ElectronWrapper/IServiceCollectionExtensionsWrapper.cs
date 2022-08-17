using Microsoft.Extensions.DependencyInjection;

namespace ElectronWrapper;

interface IServiceCollectionExtensionsWrapper
{
    IServiceCollection AddElectron(IServiceCollection services);
}