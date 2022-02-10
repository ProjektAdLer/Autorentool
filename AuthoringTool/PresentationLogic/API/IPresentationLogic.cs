using AuthoringTool.API.Configuration;
using AuthoringTool.BusinessLogic.API;

namespace AuthoringTool.PresentationLogic.API;

internal interface IPresentationLogic
{
    IAuthoringToolConfiguration Configuration { get; }
    IBusinessLogic BusinessLogic { get;  }
}