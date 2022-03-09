using AuthoringTool.API.Configuration;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.API;

internal class DataAccess : IDataAccess
{
    public DataAccess(IAuthoringToolConfiguration configuration)
    {
        Configuration = configuration;
        EmptyWorld = new ExportEmptyWorld();
    }
    
    //We dont want to Test this Constructor
    internal DataAccess(IAuthoringToolConfiguration configuration, IExportEmptyWorld emptyWorld)
    {
        Configuration = configuration;
        EmptyWorld = emptyWorld;
    }
    
    public void ExportWorld()
    {
        EmptyWorld.ModifyExistingXMLStructure();
    }
    
    public IAuthoringToolConfiguration Configuration { get; }
    public IExportEmptyWorld EmptyWorld { get; set; }
}