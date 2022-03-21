using AuthoringTool.API.Configuration;
using AuthoringTool.DataAccess.API;

namespace AuthoringTool.BusinessLogic.API;

internal interface IBusinessLogic
{
    IAuthoringToolConfiguration Configuration { get; }
    IDataAccess DataAccess { get;  }
    void ConstructBackup();
}