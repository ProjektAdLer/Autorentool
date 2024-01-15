using BusinessLogic.Commands.Element;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using TestHelpers;

namespace BusinessLogicTest.Commands.Element;

[TestFixture]
public class DeleteLearningElementInSpaceUt
{
    [Test]
    public void Execute_DeletesLearningElement()
    {
        var space = EntityProvider.GetLearningSpace(unsavedChanges: false);
        var element = EntityProvider.GetLearningElement(parent: space, unsavedChanges: false);

        space.LearningSpaceLayout.LearningElements = new Dictionary<int, ILearningElement> { { 0, element } };
        var actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningElementInSpace(element, space, mappingAction,
            new NullLogger<DeleteLearningElementInSpace>());

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
        var space = EntityProvider.GetLearningSpace(unsavedChanges: false);
        var element1 = EntityProvider.GetLearningElement(parent: space, unsavedChanges: false);
        var element2 = EntityProvider.GetLearningElement(parent: space, unsavedChanges: false, append: "2");
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

        var command = new DeleteLearningElementInSpace(element1, space, mappingAction,
            new NullLogger<DeleteLearningElementInSpace>());

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
        var space = EntityProvider.GetLearningSpace();
        var element = EntityProvider.GetLearningElement(parent: space);
        var actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningElementInSpace(element, space, mappingAction, null!);

        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
            Assert.That(actionWasInvoked, Is.False);
        });
    }

    [Test]
    public void UndoRedo_UndoesRedoesDeleteLearningElement()
    {
        var space = EntityProvider.GetLearningSpace(unsavedChanges: false);
        var element1 = EntityProvider.GetLearningElement(parent: space, unsavedChanges: false);
        var element2 = EntityProvider.GetLearningElement(parent: space, unsavedChanges: false, append: "2");
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

        var command = new DeleteLearningElementInSpace(element1, space, mappingAction,
            new NullLogger<DeleteLearningElementInSpace>());

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