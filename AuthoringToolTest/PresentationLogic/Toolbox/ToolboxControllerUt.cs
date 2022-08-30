using System;
using AuthoringTool.PresentationLogic.AuthoringToolWorkspace;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningSpace;
using AuthoringTool.PresentationLogic.LearningWorld;
using AuthoringTool.PresentationLogic.Toolbox;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace AuthoringToolTest.PresentationLogic.Toolbox;

[TestFixture]
public class ToolboxControllerUt
{
    [Test]
    public void ToolboxController_LoadObjectIntoWorkspace_World_CallsWorkspacePresenter()
    {
        var workspacePresenter = Substitute.For<IAuthoringToolWorkspacePresenterToolboxInterface>();
        var learningWorld = new LearningWorldViewModel("foo", "bar", "baz", "foo", "bar", "baz");
        
        var systemUnderTest = GetTestableToolboxController(workspacePresenter);
        
        systemUnderTest.LoadObjectIntoWorkspace(learningWorld);
        
        workspacePresenter.Received().AddLearningWorld(learningWorld);
    }

    [Test]
    public void ToolboxController_LoadObjectIntoWorkspace_Space_CallsWorldPresenter()
    {
        var worldPresenter = Substitute.For<ILearningWorldPresenterToolboxInterface>();
        var learningSpace = new LearningSpaceViewModel("foo", "bar", "baz", "foo", "bar");
            
        var systemUnderTest = GetTestableToolboxController(worldPresenter:worldPresenter);
        
        systemUnderTest.LoadObjectIntoWorkspace(learningSpace);
        
        worldPresenter.Received().AddLearningSpace(learningSpace);
    }

    [Test]
    public void ToolboxController_LoadObjectIntoWorkspace_Space_CatchesApplicationException()
    {
        var worldPresenter = Substitute.For<ILearningWorldPresenterToolboxInterface>();
        var learningSpace = new LearningSpaceViewModel("foo", "bar", "baz", "foo", "bar");
        worldPresenter
            .When(x => x.AddLearningSpace(learningSpace))
            .Do(_ => throw new ApplicationException());
        
        var systemUnderTest = GetTestableToolboxController(worldPresenter:worldPresenter);
        
        Assert.DoesNotThrow(() => systemUnderTest.LoadObjectIntoWorkspace(learningSpace));
    }

    [Test]
    public void ToolboxController_LoadObjectIntoWorkspace_Element_CallsWorldPresenterIfNoSpaceViewShowing()
    {
        var worldPresenter = Substitute.For<ILearningWorldPresenterToolboxInterface>();
        worldPresenter.ShowingLearningSpaceView.Returns(false);
        var spacePresenter = Substitute.For<ILearningSpacePresenterToolboxInterface>();
        var learningElement = new LearningElementViewModel("foo", "bar", null,"foo", "bar",
            "bar",LearningElementDifficultyEnum.Easy, null);

        var systemUnderTest =
            GetTestableToolboxController(worldPresenter: worldPresenter, spacePresenter: spacePresenter);
        
        systemUnderTest.LoadObjectIntoWorkspace(learningElement);
        
        worldPresenter.Received().AddLearningElement(learningElement);
    }

    [Test]
    public void ToolboxController_LoadObjectIntoWorkspace_Element_CatchesApplicationException()
    {
        var worldPresenter = Substitute.For<ILearningWorldPresenterToolboxInterface>();
        worldPresenter.ShowingLearningSpaceView.Returns(false);
        var spacePresenter = Substitute.For<ILearningSpacePresenterToolboxInterface>();
        var logger = Substitute.For<ILogger<ToolboxController>>();
        var learningElement = new LearningElementViewModel("foo", "bar", null, "foo", "bar", 
            "bar",LearningElementDifficultyEnum.Easy, null);
        worldPresenter
            .When(x => x.AddLearningElement(learningElement))
            .Do(_ => throw new ApplicationException());

        var systemUnderTest =
            GetTestableToolboxController(worldPresenter: worldPresenter, spacePresenter: spacePresenter, logger:logger);
        
        Assert.DoesNotThrow(() => systemUnderTest.LoadObjectIntoWorkspace(learningElement));

        worldPresenter.ShowingLearningSpaceView.Returns(true);
        spacePresenter
            .When(x => x.AddLearningElement(learningElement))
            .Do(_ => throw new ApplicationException());
        
        Assert.DoesNotThrow(() => systemUnderTest.LoadObjectIntoWorkspace(learningElement));
    }

    [Test]
    public void ToolboxController_LoadObjectIntoWorkspace_Element_CallsSpacePresenterIfSpaceViewShowing()
    {
        var worldPresenter = Substitute.For<ILearningWorldPresenterToolboxInterface>();
        worldPresenter.ShowingLearningSpaceView.Returns(true);
        var spacePresenter = Substitute.For<ILearningSpacePresenterToolboxInterface>();
        var learningElement = new LearningElementViewModel("foo", "bar", null,
            "foo", "bar", "bar",LearningElementDifficultyEnum.Easy, null);

        var systemUnderTest =
            GetTestableToolboxController(worldPresenter: worldPresenter, spacePresenter: spacePresenter);
        
        systemUnderTest.LoadObjectIntoWorkspace(learningElement);
        
        spacePresenter.Received().AddLearningElement(learningElement);
    }
    
    
    private IToolboxController GetTestableToolboxController(
        IAuthoringToolWorkspacePresenterToolboxInterface? workspacePresenter = null,
        ILearningWorldPresenterToolboxInterface? worldPresenter = null,
        ILearningSpacePresenterToolboxInterface? spacePresenter = null,
        ILogger<ToolboxController>? logger = null)
    {
        workspacePresenter ??= Substitute.For<IAuthoringToolWorkspacePresenterToolboxInterface>();
        worldPresenter ??= Substitute.For<ILearningWorldPresenterToolboxInterface>();
        spacePresenter ??= Substitute.For<ILearningSpacePresenterToolboxInterface>();
        logger ??= Substitute.For<ILogger<ToolboxController>>();
        return new ToolboxController(workspacePresenter, worldPresenter, spacePresenter, logger);
    }
}