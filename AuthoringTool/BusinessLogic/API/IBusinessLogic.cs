using AuthoringTool.DataAccess.API;

namespace AuthoringTool.BusinessLogic.API;

internal interface IBusinessLogic
{
    IDataAccess DataAccess { get;  }
}