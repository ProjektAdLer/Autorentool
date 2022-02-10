using AuthoringTool.API.Configuration;
using AuthoringTool.BusinessLogic.API;
using AuthoringTool.PresentationLogic.API;

namespace AuthoringTool.API;

/// <summary>
///
///
/// <remarks>
/// Structural properties like <see cref="Configuration"/> or <see cref="BusinessLogic"/> or <see cref="PresentationLogic"/>
/// MUST be internal -> Interface <see cref="IAuthoringTool"/> is public. Nobody who uses the <see cref="IAuthoringTool"/>
/// should get access to the structural properties
/// </remarks>
/// </summary>
internal class AuthoringTool : IAuthoringTool
{

    internal AuthoringTool(
        IAuthoringToolConfiguration configuration,
        IPresentationLogic presentationLogic)
    {
        Configuration = configuration;
        BusinessLogic = presentationLogic.BusinessLogic;
        PresentationLogic = presentationLogic;
    }
    
    internal IAuthoringToolConfiguration Configuration { get; }
    internal IBusinessLogic BusinessLogic { get; }
    internal IPresentationLogic PresentationLogic { get; }
}