using System;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.Element;
using Presentation.PresentationLogic.Space;
using Presentation.PresentationLogic.Toolbox;
using Presentation.PresentationLogic.World;
using Shared;

namespace PresentationTest.PresentationLogic.Toolbox;

[TestFixture]
public class ToolboxControllerUt
{
    [Test]
    public void ToolboxController_LoadObjectIntoWorkspace_World_CallsWorkspacePresenter()
    {
        var workspacePresenter = Substitute.For<IAuthoringToolWorkspacePresenterToolboxInterface>();
        var world = new WorldViewModel("foo", "bar", "baz", "foo", "bar", "baz");
        
        var systemUnderTest = GetTestableToolboxController(workspacePresenter);
        
        systemUnderTest.LoadObjectIntoWorkspace(world);
        
        workspacePresenter.Received().AddWorld(world);
    }

    [Test]
    public void ToolboxController_LoadObjectIntoWorkspace_Space_CallsWorldPresenter()
    {
        var worldPresenter = Substitute.For<IWorldPresenterToolboxInterface>();
        var space = new SpaceViewModel("foo", "bar", "baz", "foo", "bar");
            
        var systemUnderTest = GetTestableToolboxController(worldPresenter:worldPresenter);
        
        systemUnderTest.LoadObjectIntoWorkspace(space);
        
        worldPresenter.Received().AddSpace(space);
    }

    [Test]
    public void ToolboxController_LoadObjectIntoWorkspace_Space_CatchesApplicationException()
    {
        var worldPresenter = Substitute.For<IWorldPresenterToolboxInterface>();
        var space = new SpaceViewModel("foo", "bar", "baz", "foo", "bar");
        worldPresenter
            .When(x => x.AddSpace(space))
            .Do(_ => throw new ApplicationException());
        
        var systemUnderTest = GetTestableToolboxController(worldPresenter:worldPresenter);
        
        Assert.DoesNotThrow(() => systemUnderTest.LoadObjectIntoWorkspace(space));
    }

    [Test]
    public void ToolboxController_LoadObjectIntoWorkspace_Element_CallsSpacePresenterIfSpaceViewShowing()
    {
        var worldPresenter = Substitute.For<IWorldPresenterToolboxInterface>();
        worldPresenter.ShowingSpaceView.Returns(true);
        var spacePresenter = Substitute.For<ISpacePresenterToolboxInterface>();
        var element = new ElementViewModel("foo", "bar", null!,
            "url", "foo", "bar", "bar",ElementDifficultyEnum.Easy);

        var systemUnderTest =
            GetTestableToolboxController(worldPresenter: worldPresenter, spacePresenter: spacePresenter);
        
        systemUnderTest.LoadObjectIntoWorkspace(element);
        
        spacePresenter.Received().AddElement(element, 0);
    }
    
    
    private IToolboxController GetTestableToolboxController(
        IAuthoringToolWorkspacePresenterToolboxInterface? workspacePresenter = null,
        IWorldPresenterToolboxInterface? worldPresenter = null,
        ISpacePresenterToolboxInterface? spacePresenter = null,
        ILogger<ToolboxController>? logger = null)
    {
        workspacePresenter ??= Substitute.For<IAuthoringToolWorkspacePresenterToolboxInterface>();
        worldPresenter ??= Substitute.For<IWorldPresenterToolboxInterface>();
        spacePresenter ??= Substitute.For<ISpacePresenterToolboxInterface>();
        logger ??= Substitute.For<ILogger<ToolboxController>>();
        return new ToolboxController(workspacePresenter, worldPresenter, spacePresenter, logger);
    }
}