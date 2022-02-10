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
    internal AuthoringToolConfiguration(ILog logger)
    {
        Logger = logger;
    }
    
    public ILog Logger { get; set; }
}