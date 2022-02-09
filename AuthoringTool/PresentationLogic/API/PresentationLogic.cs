using AuthoringTool.BusinessLogic.API;

namespace AuthoringTool.PresentationLogic.API
{
    internal class PresentationLogic : IPresentationLogic
    {
        internal PresentationLogic(IBusinessLogic businessLogic)
        {
            BusinessLogic = businessLogic;
        }
        
        public IBusinessLogic BusinessLogic { get; }
    }
}