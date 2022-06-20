using AuthoringTool;
using ElectronWrapper;

Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(builder =>
{
    builder.UseElectronWrapper(args);
    builder.UseStartup<Startup>();

}).Build().Run();