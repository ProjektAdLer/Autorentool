using System.Runtime.CompilerServices;
using AuthoringTool.API.Configuration;
using AuthoringTool.DataAccess.API;

namespace AuthoringTool.BusinessLogic.API;

internal class BusinessLogic : IBusinessLogic
{

    internal BusinessLogic(
        IAuthoringToolConfiguration configuration,
        IDataAccess dataAccess)
    {
        Configuration = configuration;
        DataAccess = dataAccess;
    }
    
    public IDataAccess DataAccess { get;  }
    
    public IAuthoringToolConfiguration Configuration { get; }
  
}