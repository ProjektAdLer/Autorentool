using ElectronNET.API;

Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(builder =>
{
    builder.UseElectron(args);
    builder.UseStartup<Startup>();
    
}).Build().Run();