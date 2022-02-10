using log4net;

namespace AuthoringTool.API.Configuration;

public interface IAuthoringToolConfiguration
{
    ILog Logger { get; set; }
}