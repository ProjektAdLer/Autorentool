using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.Element;
using Presentation.PresentationLogic.Space;
using Presentation.PresentationLogic.World;

namespace Presentation.PresentationLogic.Toolbox;

public class ToolboxController : IToolboxController
{
    public ToolboxController(IAuthoringToolWorkspacePresenterToolboxInterface workspacePresenter,
        IWorldPresenterToolboxInterface worldPresenter,
        ISpacePresenterToolboxInterface spacePresenter,
        ILogger<ToolboxController> logger)
    {
        _workspacePresenter = workspacePresenter;
        _worldPresenter = worldPresenter;
        _spacePresenter = spacePresenter;
        _logger = logger;
    }
    
    private readonly IAuthoringToolWorkspacePresenterToolboxInterface _workspacePresenter;
    private readonly IWorldPresenterToolboxInterface _worldPresenter;
    private readonly ISpacePresenterToolboxInterface _spacePresenter;
    private readonly ILogger<ToolboxController> _logger;

    /// <inheritdoc cref="IToolboxController.LoadObjectIntoWorkspace"/>
    public void LoadObjectIntoWorkspace(IDisplayableObject obj)
    {
        switch (obj)
        {
            case WorldViewModel worldViewModel:
                _workspacePresenter.AddWorld(worldViewModel);
                break;
            case SpaceViewModel spaceViewModel:
                try
                {
                    _worldPresenter.AddSpace(spaceViewModel);
                }
                catch (ApplicationException ex)
                {
                    _logger.LogError(ex, "Failed to add space to world");
                }
                break;
            case ElementViewModel elementViewModel:
                if (_worldPresenter.ShowingSpaceView)
                {
                    try
                    {
                        //TODO: We need to specify the slot to add the element to - AW
                        _spacePresenter.AddElement(elementViewModel, 0);
                    }
                    catch (ApplicationException ex)
                    {
                        _logger.LogError(ex, "Failed to add element to space");
                    }
                }
                break;
            default: 
                _logger.LogError("Unknown type {} encountered in ToolboxController", obj.GetType().Name);
                break;
        }
    }
}