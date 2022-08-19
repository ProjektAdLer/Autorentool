using AuthoringToolLib.PresentationLogic.LearningElement;
using AuthoringToolLib.PresentationLogic.LearningSpace;

namespace AuthoringToolLib.PresentationLogic.LearningWorld;

public interface ILearningWorldPresenterToolboxInterface
{
    /// <inheritdoc cref="ILearningWorldPresenter.AddLearningSpace"/>
    void AddLearningSpace(ILearningSpaceViewModel learningSpace);
    
    /// <inheritdoc cref="ILearningWorldPresenter.AddLearningElement"/>
    void AddLearningElement(ILearningElementViewModel learningElement);
    
    /// <summary>
    /// Returns whether or not a learning space is currently being displayed (aka. "is open").
    /// </summary>
    bool ShowingLearningSpaceView { get; }
}