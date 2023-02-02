using Presentation.PresentationLogic.Space;

namespace Presentation.PresentationLogic.World;

public interface IWorldPresenterToolboxInterface
{
    /// <inheritdoc cref="IWorldPresenter.AddNewSpace"/>
    void AddSpace(ISpaceViewModel space);
    
    /// <summary>
    /// Returns whether or not a space is currently being displayed (aka. "is open").
    /// </summary>
    bool ShowingSpaceView { get; }
}