using Shared;
using Shared.Theme;

namespace Presentation.Components.Forms.Element;

public interface IElementModelHandler
{
    string GetIconForElementModel(ElementModel elementModel);

    IEnumerable<ElementModel> GetElementModels(ElementModelContentType contentType, string fileType = "",
        SpaceTheme? theme = null);

    ElementModel GetElementModelRandom();
}