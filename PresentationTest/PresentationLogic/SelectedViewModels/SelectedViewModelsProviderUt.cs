using System;
using BusinessLogic.Commands;
using BusinessLogic.Commands.Space;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.SelectedViewModels;
using Shared;
using Shared.Theme;
using TestHelpers;

namespace PresentationTest.PresentationLogic.SelectedViewModels;

[TestFixture]
public class SelectedViewModelsProviderUt
{
    [Test]
    public void SetLearningWorld_SetsLearningWorld()
    {
        var onUndoRedo = Substitute.For<IOnUndoRedo>();
        var logger = Substitute.For<ILogger<SelectedViewModelsProvider>>();
        var systemUnderTest = new SelectedViewModelsProvider(onUndoRedo, logger);
        var learningWorld = new LearningWorldViewModel("a", "b", "c", "d", "e", "f", "h", "i", "j", "k", "g");
        var mockCommand = Substitute.For<ICreateLearningSpace>();

        systemUnderTest.SetActiveElementSlotInSpace(1, null);

        systemUnderTest.SetLearningWorld(learningWorld, mockCommand);

        Assert.That(systemUnderTest.LearningWorld, Is.EqualTo(learningWorld));

        Assert.That(systemUnderTest.ActiveElementSlotInSpace, Is.EqualTo(-1));

        onUndoRedo.OnUndo += Raise.Event<Action<ICommand>>(mockCommand);
        Assert.That(systemUnderTest.LearningWorld, Is.Null);
        Assert.That(systemUnderTest.ActiveElementSlotInSpace, Is.EqualTo(1));

        onUndoRedo.OnRedo += Raise.Event<Action<ICommand>>(mockCommand);
        Assert.That(systemUnderTest.LearningWorld, Is.EqualTo(learningWorld));
        Assert.That(systemUnderTest.ActiveElementSlotInSpace, Is.EqualTo(-1));
    }

    [Test]
    public void SetLearningObjectInPathWay_SetsLearningSpace()
    {
        var onUndoRedo = Substitute.For<IOnUndoRedo>();
        var logger = Substitute.For<ILogger<SelectedViewModelsProvider>>();
        var systemUnderTest = new SelectedViewModelsProvider(onUndoRedo, logger);
        var learningObjectInPathWay = new LearningSpaceViewModel("a", "b", Theme.CampusAschaffenburg);
        var mockCommand = Substitute.For<ICreateLearningSpace>();

        systemUnderTest.SetActiveElementSlotInSpace(1, null);
        systemUnderTest.SetLearningObjectInPathWay(learningObjectInPathWay, mockCommand);

        Assert.That(systemUnderTest.LearningObjectInPathWay, Is.EqualTo(learningObjectInPathWay));
        Assert.That(systemUnderTest.ActiveElementSlotInSpace, Is.EqualTo(-1));

        onUndoRedo.OnUndo += Raise.Event<Action<ICommand>>(mockCommand);
        Assert.That(systemUnderTest.LearningObjectInPathWay, Is.Null);
        Assert.That(systemUnderTest.ActiveElementSlotInSpace, Is.EqualTo(1));

        onUndoRedo.OnRedo += Raise.Event<Action<ICommand>>(mockCommand);
        Assert.That(systemUnderTest.LearningObjectInPathWay, Is.EqualTo(learningObjectInPathWay));
        Assert.That(systemUnderTest.ActiveElementSlotInSpace, Is.EqualTo(-1));
    }

    [Test]
    public void SetLearningObjectInPathWay_SetsPathWay()
    {
        var onUndoRedo = Substitute.For<IOnUndoRedo>();
        var logger = Substitute.For<ILogger<SelectedViewModelsProvider>>();
        var systemUnderTest = new SelectedViewModelsProvider(onUndoRedo, logger);
        var space1 = new LearningSpaceViewModel("a", "b", Theme.CampusAschaffenburg);
        var space2 = new LearningSpaceViewModel("c", "d", Theme.CampusAschaffenburg);
        var learningObjectInPathWay = new LearningPathwayViewModel(space1, space2);
        var mockCommand = Substitute.For<ICreateLearningSpace>();

        systemUnderTest.SetActiveElementSlotInSpace(1, null);
        systemUnderTest.SetLearningObjectInPathWay(learningObjectInPathWay, mockCommand);

        Assert.That(systemUnderTest.LearningObjectInPathWay, Is.EqualTo(learningObjectInPathWay));
        Assert.That(systemUnderTest.ActiveElementSlotInSpace, Is.EqualTo(-1));

        onUndoRedo.OnUndo += Raise.Event<Action<ICommand>>(mockCommand);
        Assert.That(systemUnderTest.LearningObjectInPathWay, Is.Null);
        Assert.That(systemUnderTest.ActiveElementSlotInSpace, Is.EqualTo(1));

        onUndoRedo.OnRedo += Raise.Event<Action<ICommand>>(mockCommand);
        Assert.That(systemUnderTest.LearningObjectInPathWay, Is.EqualTo(learningObjectInPathWay));
        Assert.That(systemUnderTest.ActiveElementSlotInSpace, Is.EqualTo(-1));
    }

    [Test]
    public void SetLearningObjectInPathWay_SetsPathWayCondition()
    {
        var onUndoRedo = Substitute.For<IOnUndoRedo>();
        var logger = Substitute.For<ILogger<SelectedViewModelsProvider>>();
        var systemUnderTest = new SelectedViewModelsProvider(onUndoRedo, logger);
        var learningObjectInPathWay = new PathWayConditionViewModel(ConditionEnum.And, false);
        var mockCommand = Substitute.For<ICreateLearningSpace>();

        systemUnderTest.SetActiveElementSlotInSpace(1, null);
        systemUnderTest.SetLearningObjectInPathWay(learningObjectInPathWay, mockCommand);

        Assert.That(systemUnderTest.LearningObjectInPathWay, Is.EqualTo(learningObjectInPathWay));
        Assert.That(systemUnderTest.ActiveElementSlotInSpace, Is.EqualTo(-1));

        onUndoRedo.OnUndo += Raise.Event<Action<ICommand>>(mockCommand);
        Assert.That(systemUnderTest.LearningObjectInPathWay, Is.Null);
        Assert.That(systemUnderTest.ActiveElementSlotInSpace, Is.EqualTo(1));

        onUndoRedo.OnRedo += Raise.Event<Action<ICommand>>(mockCommand);
        Assert.That(systemUnderTest.LearningObjectInPathWay, Is.EqualTo(learningObjectInPathWay));
        Assert.That(systemUnderTest.ActiveElementSlotInSpace, Is.EqualTo(-1));
    }

    [Test]
    public void SetLearningObjectInPathWay_SetsLearningObjectInPathWayToNull()
    {
        var onUndoRedo = Substitute.For<IOnUndoRedo>();
        var logger = Substitute.For<ILogger<SelectedViewModelsProvider>>();
        var systemUnderTest = new SelectedViewModelsProvider(onUndoRedo, logger);
        var mockCommand = Substitute.For<ICreateLearningSpace>();

        systemUnderTest.SetActiveElementSlotInSpace(1, null);
        systemUnderTest.SetLearningObjectInPathWay(null, mockCommand);

        Assert.That(systemUnderTest.LearningObjectInPathWay, Is.EqualTo(null));
        Assert.That(systemUnderTest.ActiveElementSlotInSpace, Is.EqualTo(-1));

        onUndoRedo.OnUndo += Raise.Event<Action<ICommand>>(mockCommand);
        Assert.That(systemUnderTest.LearningObjectInPathWay, Is.Null);
        Assert.That(systemUnderTest.ActiveElementSlotInSpace, Is.EqualTo(1));

        onUndoRedo.OnRedo += Raise.Event<Action<ICommand>>(mockCommand);
        Assert.That(systemUnderTest.LearningObjectInPathWay, Is.EqualTo(null));
        Assert.That(systemUnderTest.ActiveElementSlotInSpace, Is.EqualTo(-1));
    }

    [Test]
    public void SetLearningElement_SetsLearningElement()
    {
        var onUndoRedo = Substitute.For<IOnUndoRedo>();
        var logger = Substitute.For<ILogger<SelectedViewModelsProvider>>();
        var systemUnderTest = new SelectedViewModelsProvider(onUndoRedo, logger);
        var learningElement = ViewModelProvider.GetLearningElement();
        var mockCommand = Substitute.For<ICreateLearningSpace>();

        systemUnderTest.SetActiveElementSlotInSpace(1, null);
        systemUnderTest.SetLearningElement(learningElement, mockCommand);

        Assert.That(systemUnderTest.LearningElement, Is.EqualTo(learningElement));
        Assert.That(systemUnderTest.ActiveElementSlotInSpace, Is.EqualTo(-1));

        onUndoRedo.OnUndo += Raise.Event<Action<ICommand>>(mockCommand);
        Assert.That(systemUnderTest.LearningElement, Is.Null);
        Assert.That(systemUnderTest.ActiveElementSlotInSpace, Is.EqualTo(1));

        onUndoRedo.OnRedo += Raise.Event<Action<ICommand>>(mockCommand);
        Assert.That(systemUnderTest.LearningElement, Is.EqualTo(learningElement));
        Assert.That(systemUnderTest.ActiveElementSlotInSpace, Is.EqualTo(-1));
    }

    [Test]
    public void SetStoryElement_SetsStoryElement()
    {
        var onUndoRedo = Substitute.For<IOnUndoRedo>();
        var logger = Substitute.For<ILogger<SelectedViewModelsProvider>>();
        var systemUnderTest = new SelectedViewModelsProvider(onUndoRedo, logger);
        var storyElement = ViewModelProvider.GetStoryElement();
        var mockCommand = Substitute.For<ICommand>();

        // Set initial state
        systemUnderTest.SetActiveStorySlotInSpace(1, null);
        systemUnderTest.SetLearningElement(storyElement, mockCommand);

        // Validate initial state
        Assert.That(systemUnderTest.LearningElement, Is.EqualTo(storyElement));
        Assert.That(systemUnderTest.ActiveStorySlotInSpace, Is.EqualTo(-1));
        Assert.That(systemUnderTest.ActiveElementSlotInSpace, Is.EqualTo(-1));

        // Simulate undo event
        onUndoRedo.OnUndo += Raise.Event<Action<ICommand>>(mockCommand);
        Assert.That(systemUnderTest.LearningElement, Is.Null);
        Assert.That(systemUnderTest.ActiveStorySlotInSpace, Is.EqualTo(1));

        // Simulate redo event
        onUndoRedo.OnRedo += Raise.Event<Action<ICommand>>(mockCommand);
        Assert.That(systemUnderTest.LearningElement, Is.EqualTo(storyElement));
        Assert.That(systemUnderTest.ActiveStorySlotInSpace, Is.EqualTo(-1));
        Assert.That(systemUnderTest.ActiveElementSlotInSpace, Is.EqualTo(-1));
    }

    [Test]
    public void SetLearningContent_SetsLearningContent()
    {
        var onUndoRedo = Substitute.For<IOnUndoRedo>();
        var logger = Substitute.For<ILogger<SelectedViewModelsProvider>>();
        var systemUnderTest = new SelectedViewModelsProvider(onUndoRedo, logger);
        var content = ViewModelProvider.GetFileContent();
        var mockCommand = Substitute.For<ICreateLearningSpace>();

        systemUnderTest.SetActiveElementSlotInSpace(1, null);
        systemUnderTest.SetLearningContent(content, mockCommand);

        Assert.That(systemUnderTest.LearningContent, Is.EqualTo(content));
        Assert.That(systemUnderTest.ActiveElementSlotInSpace, Is.EqualTo(1));

        onUndoRedo.OnUndo += Raise.Event<Action<ICommand>>(mockCommand);
        Assert.That(systemUnderTest.LearningContent, Is.Null);
        Assert.That(systemUnderTest.ActiveElementSlotInSpace, Is.EqualTo(1));

        onUndoRedo.OnRedo += Raise.Event<Action<ICommand>>(mockCommand);
        Assert.That(systemUnderTest.LearningContent, Is.EqualTo(content));
        Assert.That(systemUnderTest.ActiveElementSlotInSpace, Is.EqualTo(1));
    }

    [Test]
    public void SetActiveSlotInSpace_SetsActiveSlotInSpace()
    {
        var onUndoRedo = Substitute.For<IOnUndoRedo>();
        var logger = Substitute.For<ILogger<SelectedViewModelsProvider>>();
        var systemUnderTest = new SelectedViewModelsProvider(onUndoRedo, logger);
        var mockCommand = Substitute.For<ICreateLearningSpace>();

        systemUnderTest.SetActiveElementSlotInSpace(1, mockCommand);

        Assert.That(systemUnderTest.ActiveElementSlotInSpace, Is.EqualTo(1));

        onUndoRedo.OnUndo += Raise.Event<Action<ICommand>>(mockCommand);
        Assert.That(systemUnderTest.ActiveElementSlotInSpace, Is.EqualTo(-1));

        onUndoRedo.OnRedo += Raise.Event<Action<ICommand>>(mockCommand);
        Assert.That(systemUnderTest.ActiveElementSlotInSpace, Is.EqualTo(1));
    }
}