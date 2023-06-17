using Presentation.PresentationLogic.LearningContent;
using Shared;

namespace Presentation.Components.Forms.Element;

public interface IElementModelHandler
{
    string GetIconForElementModel(ElementModel elementModel);
    IEnumerable<ElementModel> GetElementModels(ILearningContentViewModel? learningContentViewModel = null);
}