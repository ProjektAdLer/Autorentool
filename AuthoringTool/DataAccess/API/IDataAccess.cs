using AuthoringTool.API.Configuration;

namespace AuthoringTool.DataAccess.API;

internal interface IDataAccess
{
    IAuthoringToolConfiguration Configuration { get; }
}