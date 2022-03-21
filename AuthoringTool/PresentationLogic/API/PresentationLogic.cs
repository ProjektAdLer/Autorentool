using AuthoringTool.API.Configuration;
using AuthoringTool.BusinessLogic.API;

namespace AuthoringTool.PresentationLogic.API
{
    internal class PresentationLogic : IPresentationLogic
    {
        public PresentationLogic(
            IAuthoringToolConfiguration configuration,
            IBusinessLogic businessLogic)
        {
            Configuration = configuration;
            BusinessLogic = businessLogic;
        }

        public void ConstructBackup()
        {
            BusinessLogic.ConstructBackup();
        }
        
        public IAuthoringToolConfiguration Configuration { get; }
        public IBusinessLogic BusinessLogic { get; }
    }
}