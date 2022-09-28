using System;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.Toolbox;
using Shared;

namespace PresentationTest.PresentationLogic.Toolbox;

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
    public void ToolboxController_LoadObjectIntoWorkspace_Element_CallsSpacePresenterIfSpaceViewShowing()
    {
        var worldPresenter = Substitute.For<ILearningWorldPresenterToolboxInterface>();
        worldPresenter.ShowingLearningSpaceView.Returns(true);
        var spacePresenter = Substitute.For<ILearningSpacePresenterToolboxInterface>();
        var learningElement = new LearningElementViewModel("foo", "bar", null!,
            "foo", "bar", "bar",LearningElementDifficultyEnum.Easy);

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