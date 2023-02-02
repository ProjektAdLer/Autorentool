using Presentation.PresentationLogic.Element;

namespace Presentation.PresentationLogic.Space;

public interface ISpacePresenterToolboxInterface
{
    void AddElement(IElementViewModel element, int slotIndex);
}