using AuthoringTool.BusinessLogic.API;
using AuthoringTool.DataAccess.API;
using AuthoringTool.PresentationLogic.API;

namespace AuthoringTool.API;

public class AuthoringToolFactory : IAuthoringToolFactory
{
    public IAuthoringTool CreateAuthoringTool()
    {
        IDataAccess dataAccess = new DataAccess.API.DataAccess();
        IBusinessLogic businessLogic = new BusinessLogic.API.BusinessLogic(dataAccess);
        IPresentationLogic presentationLogic = new PresentationLogic.API.PresentationLogic(businessLogic);
        return new AuthoringTool(presentationLogic);
    }
}