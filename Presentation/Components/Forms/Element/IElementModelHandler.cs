using Presentation.PresentationLogic.LearningContent;
using Shared;

namespace Presentation.Components.Forms.Element;

public interface IElementModelHandler
{
    IEnumerable<ElementModel> GetElementModels(ILearningContentViewModel? learningContentViewModel = null);

    string GetIconForElementModel(ElementModel elementModel);
}