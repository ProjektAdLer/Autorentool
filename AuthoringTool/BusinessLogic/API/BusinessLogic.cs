using System.Runtime.CompilerServices;
using AuthoringTool.DataAccess.API;

namespace AuthoringTool.BusinessLogic.API;

internal class BusinessLogic : IBusinessLogic
{

    internal BusinessLogic()
    {
        DataAccess = new DataAccess.API.DataAccess();
    }
    
    /// <summary>
    /// Testable Constructor
    /// </summary>
    internal BusinessLogic(IDataAccess dataAccess)
    {
        DataAccess = dataAccess;
    }
    
    
    internal IDataAccess DataAccess { get;  }

}