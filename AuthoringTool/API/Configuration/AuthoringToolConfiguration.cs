using log4net;

namespace AuthoringTool.API.Configuration;

/// <summary>
/// Use this Class to configurate the AuthoringTool-Component
/// <para>
/// for example:
/// DI Container; 
/// Logger; 
/// System Configuration Data -> Use this or this API
/// </para>
/// </summary>
public class AuthoringToolConfiguration : IAuthoringToolConfiguration
{
    public AuthoringToolConfiguration(ILogger logger)
    {
        Logger = logger;
    }
    
    public ILogger Logger { get; set; }
}