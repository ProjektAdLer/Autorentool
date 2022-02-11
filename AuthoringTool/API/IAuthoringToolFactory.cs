using AuthoringTool.API.Configuration;
using log4net;

namespace AuthoringTool.API;

public interface IAuthoringToolFactory  
{
    IAuthoringTool CreateAuthoringTool(IAuthoringToolConfiguration config);
    IAuthoringToolConfiguration CreateAuthoringToolConfiguration(ILog logger);
}