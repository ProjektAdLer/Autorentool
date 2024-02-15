using Presentation.Components.Forms.Models;
using Shared;

namespace Presentation.Components.Forms.Element;

public interface IElementModelHandler
{
    string GetIconForElementModel(ElementModel elementModel);

    IEnumerable<ElementModel> GetElementModels(ElementModelContentType contentType, string fileType = "",
        Theme? theme = null, bool npcMode = false);

    ElementModel GetElementModelRandom();
}