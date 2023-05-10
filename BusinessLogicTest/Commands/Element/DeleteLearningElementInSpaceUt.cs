using BusinessLogic.Commands.Element;
using BusinessLogic.Entities;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands.Element;

[TestFixture]

public class DeleteLearningElementInSpaceUt
{
    [Test]
    public void Execute_DeletesLearningElement()
    {
        var space = new LearningSpace("a","d", "e", 5)
        {
            UnsavedChanges = false
        };
        var element = new LearningElement("g", null!, "j", "k", LearningElementDifficultyEnum.Easy, space)
        {
            UnsavedChanges = false
        };
        space.LearningSpaceLayout.LearningElements = new Dictionary<int, ILearningElement>() { { 0, element } };
        var actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningElementInSpace(element, space, mappingAction);
        
        Assert.Multiple(() =>
        {
            Assert.That(space.ContainedLearningElements, Does.Contain(element));
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(space.UnsavedChanges, Is.False);
        });
        
        command.Execute();
        
        Assert.Multiple(() =>
        {
            Assert.That(space.ContainedLearningElements, Is.Empty);
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(space.UnsavedChanges, Is.True);
        });
    }

    [Test]
    public void Execute_DeletesLearningElementAndSetsAnotherElementSelectedLearningElement()
    {
        var space = new LearningSpace("a","d", "e", 5)
        {
            UnsavedChanges = false
        };
        var element1 = new LearningElement("g", null!, "j", "k", LearningElementDifficultyEnum.Easy, space)
        {
            UnsavedChanges = false
        };
        var element2 = new LearningElement("l", null!, "o", "p", LearningElementDifficultyEnum.Easy, space)
        {
            UnsavedChanges = false
        };
        space.LearningSpaceLayout.LearningElements = new Dictionary<int, ILearningElement>
        {
            {
                0, element1
            },
            {
                1, element2
            }
        };
        var actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningElementInSpace(element1, space, mappingAction);
        
        Assert.That(space.ContainedLearningElements, Does.Contain(element1));
        Assert.Multiple(() =>
        {
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(2));
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(space.UnsavedChanges, Is.False);
        });
        
        command.Execute();
        
        Assert.Multiple(() =>
        {
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(space.ContainedLearningElements, Does.Not.Contain(element1));
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(space.UnsavedChanges, Is.True);
        });
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var space = new LearningSpace("a","d", "e", 5);
        var element = new LearningElement("g", null!, "j", "k", LearningElementDifficultyEnum.Easy, space);
        var actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningElementInSpace(element, space, mappingAction);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        
        Assert.IsFalse(actionWasInvoked);
    }

    [Test]
    public void UndoRedo_UndoesRedoesDeleteLearningElement()
    {
        var space = new LearningSpace("a","d", "e", 5)
        {
            UnsavedChanges = false
        };
        var element1 = new LearningElement("g", null!, "j", "k", LearningElementDifficultyEnum.Easy, space)
        {
            UnsavedChanges = false
        };
        var element2 = new LearningElement("l", null!, "o", "p", LearningElementDifficultyEnum.Easy, space)
        {
            UnsavedChanges = false
        };
        space.LearningSpaceLayout.LearningElements = new Dictionary<int, ILearningElement>
        {
            {
                0, element1
            },
            {
                1, element2
            }
        };
        var actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningElementInSpace(element1, space, mappingAction);
        
        Assert.Multiple(() =>
        {
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(2));
            Assert.That(space.ContainedLearningElements, Does.Contain(element1));
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(space.UnsavedChanges, Is.False);
        });
        
        command.Execute();
        
        Assert.Multiple(() =>
        {
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
            
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(space.UnsavedChanges, Is.True);
        });
        
        actionWasInvoked = false;
        
        command.Undo();
        
        Assert.Multiple(() =>
        {
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(2));
            Assert.That(space.ContainedLearningElements, Does.Contain(element1));
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(space.UnsavedChanges, Is.False);
        });
        
        actionWasInvoked = false;
        
        command.Redo();
        
        Assert.Multiple(() =>
        {
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
            
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(space.UnsavedChanges, Is.True);
        });
    }
}