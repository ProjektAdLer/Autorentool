using AuthoringTool.API.Configuration;
using AuthoringTool.BusinessLogic.API;
using AuthoringTool.DataAccess.API;
using AuthoringTool.PresentationLogic.API;
using log4net;

namespace AuthoringTool.API;

public class AuthoringToolFactory : IAuthoringToolFactory
{
    public IAuthoringToolConfiguration CreateAuthoringToolConfiguration(ILog logger)
    {
        return new AuthoringToolConfiguration(logger);
    }
    
    public IAuthoringTool CreateAuthoringTool(IAuthoringToolConfiguration configuration)
    {
        IDataAccess dataAccess = new DataAccess.API.DataAccess(configuration);
        IBusinessLogic businessLogic = new BusinessLogic.API.BusinessLogic(configuration, dataAccess);
        IPresentationLogic presentationLogic = new PresentationLogic.API.PresentationLogic(configuration, businessLogic);
        return new AuthoringTool(configuration, presentationLogic);
    }
}