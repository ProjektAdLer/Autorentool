using AuthoringTool.BusinessLogic.API;

namespace AuthoringTool.PresentationLogic.API;

internal interface IPresentationLogic
{
    IBusinessLogic BusinessLogic { get;  }
}