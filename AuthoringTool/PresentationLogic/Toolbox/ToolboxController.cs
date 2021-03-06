
using AuthoringTool.PresentationLogic.AuthoringToolWorkspace;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningSpace;
using AuthoringTool.PresentationLogic.LearningWorld;

namespace AuthoringTool.PresentationLogic.Toolbox;

public class ToolboxController : IToolboxController
{
    public ToolboxController(IAuthoringToolWorkspacePresenterToolboxInterface workspacePresenter,
        ILearningWorldPresenterToolboxInterface worldPresenter,
        ILearningSpacePresenterToolboxInterface spacePresenter,
        ILogger<ToolboxController> logger)
    {
        _workspacePresenter = workspacePresenter;
        _worldPresenter = worldPresenter;
        _spacePresenter = spacePresenter;
        _logger = logger;
    }
    
    private readonly IAuthoringToolWorkspacePresenterToolboxInterface _workspacePresenter;
    private readonly ILearningWorldPresenterToolboxInterface _worldPresenter;
    private readonly ILearningSpacePresenterToolboxInterface _spacePresenter;
    private readonly ILogger<ToolboxController> _logger;

    /// <inheritdoc cref="IToolboxController.LoadObjectIntoWorkspace"/>
    public void LoadObjectIntoWorkspace(IDisplayableLearningObject obj)
    {
        switch (obj)
        {
            case LearningWorldViewModel learningWorldViewModel:
                _workspacePresenter.AddLearningWorld(learningWorldViewModel);
                break;
            case LearningSpaceViewModel learningSpaceViewModel:
                try
                {
                    _worldPresenter.AddLearningSpace(learningSpaceViewModel);
                }
                catch (ApplicationException ex)
                {
                    _logger.LogError(ex, "Failed to add space to world");
                }
                break;
            case LearningElementViewModel learningElementViewModel:
                if (_worldPresenter.ShowingLearningSpaceView)
                {
                    try
                    {
                        _spacePresenter.AddLearningElement(learningElementViewModel);
                    }
                    catch (ApplicationException ex)
                    {
                        _logger.LogError(ex, "Failed to add element to space");
                    }
                }
                else
                {
                    try
                    {
                        _worldPresenter.AddLearningElement(learningElementViewModel);
                    }
                    catch (ApplicationException ex)
                    {
                        _logger.LogError(ex, "Failed to add element to world");
                    }
                }
                break;
            default: 
                _logger.LogError("Unknown type {} encountered in ToolboxController", obj.GetType().Name);
                break;
        }
    }
}