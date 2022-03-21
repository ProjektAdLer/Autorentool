using System.Data;
using System.Runtime.CompilerServices;
using AuthoringTool.API.Configuration;
using AuthoringTool.DataAccess.API;
using ElectronNET.API.Entities;

namespace AuthoringTool.BusinessLogic.API;

internal class BusinessLogic : IBusinessLogic
{

    public BusinessLogic(
        IAuthoringToolConfiguration configuration,
        IDataAccess dataAccess)
    {
        Configuration = configuration;
        DataAccess = dataAccess;
    }
    
    
    
    public IDataAccess DataAccess { get;  }
    public void ConstructBackup()
    {
        DataAccess.ConstructBackup();
    }

    public IAuthoringToolConfiguration Configuration { get; }
  
}