using BusinessLogic.Commands.Element;
using BusinessLogic.Entities;
using NUnit.Framework;

namespace BusinessLogicTest.Commands;

[TestFixture]

public class CreateUnplacedLearningElementUt
{
    [Test]
    public void Execute_CreatesLearningElement()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateUnplacedLearningElement(testParameter.WorldParent, testParameter.Name, testParameter.ShortName, testParameter.Content,
            testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, testParameter.Points, testParameter.PositionX,testParameter.PositionY, mappingAction);

        Assert.IsEmpty(testParameter.WorldParent.UnplacedLearningElements);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(testParameter.WorldParent.UnplacedLearningElements.Count(), Is.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
        var element = testParameter.WorldParent.UnplacedLearningElements.First();
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo(testParameter.Name));
            Assert.That(element.Shortname, Is.EqualTo(testParameter.ShortName));
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
    public void UndoRedo_UndoesRedoesCreateLearningElement()
    {
        var testParameter = new TestParameter();
        var worldParent = testParameter.WorldParent;
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateUnplacedLearningElement(testParameter.WorldParent, testParameter.Name, testParameter.ShortName, testParameter.Content,
            testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, testParameter.Points, testParameter.PositionX,testParameter.PositionY, mappingAction);


        Assert.That(worldParent.UnplacedLearningElements.Count(), Is.EqualTo(0));
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(worldParent.UnplacedLearningElements.Count(), Is.EqualTo(1));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Undo();
        
        Assert.That(worldParent.UnplacedLearningElements.Count(), Is.EqualTo(0));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Redo();
        
        Assert.That(worldParent.UnplacedLearningElements.Count(), Is.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateUnplacedLearningElement(testParameter.WorldParent, testParameter.Name,
            testParameter.ShortName, testParameter.Content,
            testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, testParameter.Points, testParameter.PositionX, testParameter.PositionY,
            mappingAction);
            
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        Assert.IsFalse(actionWasInvoked);
    }
}


