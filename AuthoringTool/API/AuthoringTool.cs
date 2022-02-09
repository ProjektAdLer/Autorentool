using AuthoringTool.BusinessLogic.API;
using AuthoringTool.PresentationLogic.API;

namespace AuthoringTool.API;

internal class AuthoringTool : IAuthoringTool
{

    internal AuthoringTool(IPresentationLogic presentationLogic)
    {
        PresentationLogic = presentationLogic;
        BusinessLogic = presentationLogic.BusinessLogic;
    }
    
    internal IPresentationLogic PresentationLogic { get; }
    internal IBusinessLogic BusinessLogic { get; }
    

}