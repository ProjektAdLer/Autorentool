using AuthoringTool.API.Configuration;
using AuthoringTool.BusinessLogic.API;

namespace AuthoringTool.PresentationLogic.API
{
    internal class PresentationLogic : IPresentationLogic
    {
        internal PresentationLogic(
            IAuthoringToolConfiguration configuration,
            IBusinessLogic businessLogic)
        {
            Configuration = configuration;
            BusinessLogic = businessLogic;
        }
        
        public IAuthoringToolConfiguration Configuration { get; }
        public IBusinessLogic BusinessLogic { get; }
    }
}