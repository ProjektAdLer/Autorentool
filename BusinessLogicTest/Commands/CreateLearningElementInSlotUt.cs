using BusinessLogic.Commands.Element;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands;

[TestFixture]

public class CreateLearningElementInSlotUt
{
    [Test]
    public void Execute_CreatesLearningElement()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateLearningElementInSlot(testParameter.SpaceParent, 0, testParameter.Name, testParameter.ShortName, testParameter.Content,
            testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, testParameter.Points, testParameter.PositionX,testParameter.PositionY, mappingAction);

        Assert.IsEmpty(testParameter.SpaceParent.ContainedLearningElements);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(testParameter.SpaceParent.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
        var element = testParameter.SpaceParent.ContainedLearningElements.First();
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo(testParameter.Name));
            Assert.That(element.Shortname, Is.EqualTo(testParameter.ShortName));
            Assert.That(element.Parent, Is.EqualTo(testParameter.SpaceParent));
            Assert.That(element.LearningContent, Is.EqualTo(testParameter.Content));
            Assert.That(element.Authors, Is.EqualTo(testParameter.Authors));
            Assert.That(element.Description, Is.EqualTo(testParameter.Description));
            Assert.That(element.Goals, Is.EqualTo(testParameter.Goals));
            Assert.That(element.Workload, Is.EqualTo(testParameter.Workload));
            Assert.That(element.Points, Is.EqualTo(testParameter.Points));
            Assert.That(element.Difficulty, Is.EqualTo(testParameter.Difficulty));
        });
    }

    [Test]
    public void CreateLearningElement_SetElementAsSelectedInParent()
    {
        var testParameter = new TestParameter();
        var actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;
        var element = new LearningElement(testParameter.Name, testParameter.ShortName, testParameter.Content,
            testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty, testParameter.SpaceParent,
            testParameter.Workload, testParameter.Points,1,2);
        
        var command = new CreateLearningElementInSlot(testParameter.SpaceParent, 0, element, mappingAction);

        Assert.IsEmpty(testParameter.SpaceParent.ContainedLearningElements);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();
        
        Assert.That(actionWasInvoked, Is.True);
        Assert.That(testParameter.SpaceParent.ContainedLearningElements.Count(), Is.EqualTo(1));
        
        Assert.Multiple(() =>
        {
            Assert.That(testParameter.SpaceParent.ContainedLearningElements.First(), Is.EqualTo(element));
            Assert.That(testParameter.SpaceParent.SelectedLearningElement, Is.EqualTo(element));
        });
    }

    [Test]
    public void UndoRedo_UndoesRedoesCreateLearningElement()
    {
        var testParameter = new TestParameter();
        var spaceParent = testParameter.SpaceParent;
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateLearningElementInSlot(spaceParent, 1, testParameter.Name, testParameter.ShortName, testParameter.Content,
            testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, testParameter.Points, testParameter.PositionX, testParameter.PositionY,
            mappingAction);
        var element2 = new LearningElement("x", "x", null!, "x", "x", "x", LearningElementDifficultyEnum.Easy);
        spaceParent.LearningSpaceLayout.LearningElements = new ILearningElement?[]{element2, null, null, null, null, null};
        spaceParent.SelectedLearningElement = element2;
        

        Assert.That(spaceParent.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.That(spaceParent.ContainedLearningElements.First(), Is.EqualTo(element2));
        Assert.That(spaceParent.SelectedLearningElement, Is.EqualTo(element2));
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(spaceParent.ContainedLearningElements.Count(), Is.EqualTo(2));
        Assert.That(spaceParent.SelectedLearningElement, Is.EqualTo(spaceParent.ContainedLearningElements.Last()));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Undo();
        
        Assert.That(spaceParent.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.That(spaceParent.ContainedLearningElements.First(), Is.EqualTo(element2));
        Assert.That(spaceParent.SelectedLearningElement, Is.EqualTo(element2));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Redo();
        
        Assert.That(spaceParent.ContainedLearningElements.Count(), Is.EqualTo(2));
        Assert.That(spaceParent.SelectedLearningElement, Is.EqualTo(spaceParent.ContainedLearningElements.Last()));
        Assert.IsTrue(actionWasInvoked);
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateLearningElementInSlot(testParameter.SpaceParent, 0, testParameter.Name, testParameter.ShortName, testParameter.Content,
            testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, testParameter.Points, testParameter.PositionX, testParameter.PositionY, mappingAction);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        Assert.IsFalse(actionWasInvoked);
    }
}

public class TestParameter
{
    public readonly LearningSpace SpaceParent;
    public readonly LearningWorld WorldParent;
    public readonly string Name;
    public readonly string ShortName;
    public readonly ILearningContent Content;
    public readonly string Authors;
    public readonly string Description;
    public readonly string Goals;
    public readonly LearningElementDifficultyEnum Difficulty;
    public readonly int Workload;
    public readonly int Points;
    public readonly double PositionX;
    public readonly double PositionY;

    internal TestParameter()
    {
        SpaceParent = new LearningSpace("l", "m", "n", "o", "p", 0, new LearningSpaceLayout(new ILearningElement?[6], FloorPlanEnum.Rectangle2X3));
        WorldParent = new LearningWorld("q", "r", "s", "t", "u","o");
        Name = "a";
        ShortName = "b";
        Content = new FileContent("bar", "foo", "");
        Authors = "d";
        Description = "e";
        Goals = "f";
        Difficulty = LearningElementDifficultyEnum.Easy;
        Workload = 3;
        Points = 4;
        PositionX = 5;
        PositionY = 6;
    }
}