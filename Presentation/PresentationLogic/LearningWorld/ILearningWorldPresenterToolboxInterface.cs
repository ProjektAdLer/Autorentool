using Presentation.PresentationLogic.LearningSpace;

namespace Presentation.PresentationLogic.LearningWorld;

public interface ILearningWorldPresenterToolboxInterface
{
    /// <inheritdoc cref="ILearningWorldPresenter.AddLearningSpace"/>
    void AddLearningSpace(ILearningSpaceViewModel learningSpace);
    
    /// <summary>
    /// Returns whether or not a learning space is currently being displayed (aka. "is open").
    /// </summary>
    bool ShowingLearningSpaceView { get; }
}