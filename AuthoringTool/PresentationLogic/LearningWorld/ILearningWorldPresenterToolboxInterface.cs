using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningSpace;

namespace AuthoringTool.PresentationLogic.LearningWorld;

public interface ILearningWorldPresenterToolboxInterface
{
    /// <inheritdoc cref="ILearningWorldPresenter.AddLearningSpace"/>
    void AddLearningSpace(ILearningSpaceViewModel learningSpace);
    
    /// <inheritdoc cref="ILearningWorldPresenter.AddLearningElement"/>
    void AddLearningElement(LearningElementViewModel learningElement);
    
    /// <summary>
    /// Returns whether or not a learning space is currently being displayed (aka. "is open").
    /// </summary>
    bool ShowingLearningSpaceView { get; }
}