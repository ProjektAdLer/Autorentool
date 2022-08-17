using Microsoft.AspNetCore.Hosting;

namespace ElectronWrapper;

interface IWebHostBuilderExtensionsWrapper
{
    IWebHostBuilder UseElectron(IWebHostBuilder builder, string[] args);
}