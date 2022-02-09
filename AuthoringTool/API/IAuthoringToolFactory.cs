using AuthoringTool.API.Configuration;

namespace AuthoringTool.API;

public interface IAuthoringToolFactory  
{
    IAuthoringTool CreateAuthoringTool(IAuthoringToolConfiguration config);
}