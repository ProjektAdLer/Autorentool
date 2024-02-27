using BusinessLogic.Commands.Element;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using BusinessLogic.Entities.LearningContent.FileContent;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Shared;
using TestHelpers;

namespace BusinessLogicTest.Commands.Element;

[TestFixture]
public class CreateLearningElementInSlotUt
{
    [Test]
    public void Execute_CreatesLearningElement()
    {
        var testParameter = new TestParameter();
        var actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateLearningElementInSlot(testParameter.SpaceParent, 0, testParameter.Name,
            testParameter.Content, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.ElementModel, testParameter.Workload, testParameter.Points, testParameter.PositionX,
            testParameter.PositionY, mappingAction, new NullLogger<CreateLearningElementInSlot>());

        Assert.Multiple(() =>
        {
            Assert.That(testParameter.SpaceParent.ContainedLearningElements, Is.Empty);
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(testParameter.WorldParent.UnsavedChanges, Is.False);
            Assert.That(testParameter.SpaceParent.UnsavedChanges, Is.False);
        });

        command.Execute();

        var element = testParameter.SpaceParent.ContainedLearningElements.First();
        Assert.Multiple(() =>
        {
            Assert.That(testParameter.SpaceParent.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(element.Name, Is.EqualTo(testParameter.Name));
            Assert.That(element.Parent, Is.EqualTo(testParameter.SpaceParent));
            Assert.That(element.LearningContent, Is.EqualTo(testParameter.Content));
            Assert.That(element.Description, Is.EqualTo(testParameter.Description));
            Assert.That(element.Goals, Is.EqualTo(testParameter.Goals));
            Assert.That(element.Workload, Is.EqualTo(testParameter.Workload));
            Assert.That(element.Points, Is.EqualTo(testParameter.Points));
            Assert.That(element.Difficulty, Is.EqualTo(testParameter.Difficulty));
            Assert.That(testParameter.WorldParent.UnsavedChanges, Is.True);
            Assert.That(testParameter.SpaceParent.UnsavedChanges, Is.True);
        });
    }

    [Test]
    public void CreateLearningElement_SetElementAsSelectedInParent()
    {
        var testParameter = new TestParameter();
        var actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;
        var element = new LearningElement(testParameter.Name, testParameter.Content, testParameter.Description,
            testParameter.Goals, testParameter.Difficulty, testParameter.ElementModel, testParameter.SpaceParent,
            workload: testParameter.Workload, points: testParameter.Points, positionX: 1, positionY: 2);

        var command = new CreateLearningElementInSlot(testParameter.SpaceParent, 0, element, mappingAction,
            new NullLogger<CreateLearningElementInSlot>());

        Assert.Multiple(() =>
        {
            Assert.That(testParameter.SpaceParent.ContainedLearningElements, Is.Empty);
            Assert.That(actionWasInvoked, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(testParameter.SpaceParent.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(testParameter.SpaceParent.ContainedLearningElements.First(), Is.EqualTo(element));
        });
    }

    [Test]
    public void UndoRedo_UndoesRedoesCreateLearningElement()
    {
        var testParameter = new TestParameter();
        var spaceParent = testParameter.SpaceParent;
        var actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateLearningElementInSlot(spaceParent, 1, testParameter.Name, testParameter.Content,
            testParameter.Description, testParameter.Goals, testParameter.Difficulty, testParameter.ElementModel,
            testParameter.Workload, testParameter.Points, testParameter.PositionX, testParameter.PositionY,
            mappingAction, new NullLogger<CreateLearningElementInSlot>());
        var element2 = EntityProvider.GetLearningElement(unsavedChanges: false, append: "2");
        spaceParent.LearningSpaceLayout.LearningElements = new Dictionary<int, ILearningElement>
        {
            {
                0, element2
            }
        };

        Assert.Multiple(() =>
        {
            Assert.That(spaceParent.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(spaceParent.ContainedLearningElements.First(), Is.EqualTo(element2));
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(testParameter.WorldParent.UnsavedChanges, Is.False);
            Assert.That(testParameter.SpaceParent.UnsavedChanges, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(spaceParent.ContainedLearningElements.Count(), Is.EqualTo(2));

            Assert.That(actionWasInvoked, Is.True);
            Assert.That(testParameter.WorldParent.UnsavedChanges, Is.True);
            Assert.That(testParameter.SpaceParent.UnsavedChanges, Is.True);
        });

        actionWasInvoked = false;

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(spaceParent.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(spaceParent.ContainedLearningElements.First(), Is.EqualTo(element2));

            Assert.That(actionWasInvoked, Is.True);
            Assert.That(testParameter.WorldParent.UnsavedChanges, Is.False);
            Assert.That(testParameter.SpaceParent.UnsavedChanges, Is.False);
        });

        actionWasInvoked = false;

        command.Redo();

        Assert.Multiple(() =>
        {
            Assert.That(spaceParent.ContainedLearningElements.Count(), Is.EqualTo(2));

            Assert.That(actionWasInvoked, Is.True);
            Assert.That(testParameter.WorldParent.UnsavedChanges, Is.True);
            Assert.That(testParameter.SpaceParent.UnsavedChanges, Is.True);
        });
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var testParameter = new TestParameter();
        var actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateLearningElementInSlot(testParameter.SpaceParent, 0, testParameter.Name,
            testParameter.Content, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.ElementModel, testParameter.Workload, testParameter.Points, testParameter.PositionX,
            testParameter.PositionY, mappingAction, null!);

        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
            Assert.That(actionWasInvoked, Is.False);
        });
    }
}

public class TestParameter
{
    public readonly ILearningContent Content;
    public readonly string Description;
    public readonly LearningElementDifficultyEnum Difficulty;
    public readonly ElementModel ElementModel;
    public readonly string Goals;
    public readonly string Name;
    public readonly int Points;
    public readonly double PositionX;
    public readonly double PositionY;
    public readonly LearningSpace SpaceParent;
    public readonly int Workload;
    public readonly LearningWorld WorldParent;

    internal TestParameter()
    {
        SpaceParent = new LearningSpace("l", "o", "p", 0, Theme.CampusAschaffenburg,
            new LearningSpaceLayout(new Dictionary<int, ILearningElement>(), FloorPlanEnum.R_20X30_8L))
        {
            UnsavedChanges = false
        };
        WorldParent = new LearningWorld("q", "r", "s", "t", "u", "o")
        {
            UnsavedChanges = false,
            LearningSpaces = new List<ILearningSpace>
            {
                SpaceParent
            }
        };
        Name = "a";
        Content = new FileContent("bar", "foo", "");
        Description = "e";
        Goals = "f";
        Difficulty = LearningElementDifficultyEnum.Easy;
        ElementModel = ElementModel.l_h5p_slotmachine_1;
        Workload = 3;
        Points = 4;
        PositionX = 5;
        PositionY = 6;
    }
}