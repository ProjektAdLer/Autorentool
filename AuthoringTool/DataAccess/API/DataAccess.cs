using AuthoringTool.API.Configuration;

namespace AuthoringTool.DataAccess.API;

internal class DataAccess : IDataAccess
{
    internal DataAccess(IAuthoringToolConfiguration configuration)
    {
        Configuration = configuration;
    }
    
    public IAuthoringToolConfiguration Configuration { get; }
}