using BusinessLogic.Commands.Element;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using NUnit.Framework;
using Shared;
using TestHelpers;

namespace BusinessLogicTest.Commands.Element;

[TestFixture]
public class EditLearningElementUt
{
    [Test]
    public void Execute_EditsLearningElement()
    {
        var parent = EntityProvider.GetLearningSpace();
        var content = new FileContent("bar", "foo", "");
        var element = new LearningElement("a", content, "f", "g", LearningElementDifficultyEnum.Medium,
            ElementModel.L_H5P_SPIELAUTOMAT_1, parent, workload: 8, points: 9, positionX: 17f, positionY: 29f)
        {
            UnsavedChanges = false
        };
        parent.LearningSpaceLayout.LearningElements = new Dictionary<int, ILearningElement>
        {
            {
                0, element
            }
        };

        var name = "new element";
        var url = "google.com";
        var description = "video of learning stuff";
        var goals = "learn";
        var workload = 7;
        var points = 8;
        var difficulty = LearningElementDifficultyEnum.Easy;
        var elementModel = ElementModel.L_H5P_TAFEL_1;
        var newContent = new FileContent("foo", "bar", "foobar");
        var actionWasInvoked = false;
        Action<LearningElement> mappingAction = _ => actionWasInvoked = true;

        var command = new EditLearningElement(element, parent, name, description, goals, difficulty, elementModel,
            workload, points, newContent, mappingAction);

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(element.Name, Is.EqualTo("a"));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.LearningContent, Is.EqualTo(content));
            Assert.That(element.Description, Is.EqualTo("f"));
            Assert.That(element.Goals, Is.EqualTo("g"));
            Assert.That(element.Workload, Is.EqualTo(8));
            Assert.That(element.Points, Is.EqualTo(9));
            Assert.That(element.Difficulty, Is.EqualTo(LearningElementDifficultyEnum.Medium));
            Assert.That(element.ElementModel, Is.EqualTo(ElementModel.L_H5P_SPIELAUTOMAT_1));
            Assert.That(element.PositionX, Is.EqualTo(17f));
            Assert.That(element.PositionY, Is.EqualTo(29f));
            Assert.That(element.UnsavedChanges, Is.False);
            Assert.That(parent.ContainedLearningElements.Count(), Is.EqualTo(1));
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.IsTrue(actionWasInvoked);
            Assert.That(element.Name, Is.EqualTo(name));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.LearningContent, Is.EqualTo(newContent));
            Assert.That(element.Description, Is.EqualTo(description));
            Assert.That(element.Goals, Is.EqualTo(goals));
            Assert.That(element.Workload, Is.EqualTo(workload));
            Assert.That(element.Points, Is.EqualTo(points));
            Assert.That(element.Difficulty, Is.EqualTo(difficulty));
            Assert.That(element.ElementModel, Is.EqualTo(elementModel));
            Assert.That(element.PositionX, Is.EqualTo(17f));
            Assert.That(element.PositionY, Is.EqualTo(29f));
            Assert.That(element.UnsavedChanges, Is.True);
            Assert.That(parent.ContainedLearningElements.Count(), Is.EqualTo(1));
        });
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var parent = EntityProvider.GetLearningSpace();
        var element = new LearningElement("a", null!, "d", "e", LearningElementDifficultyEnum.Easy,
            ElementModel.L_H5P_SPIELAUTOMAT_1);
        var name = "new element";
        var url = "google.com";
        var description = "video of learning stuff";
        var goals = "learn";
        var workload = 7;
        var points = 8;
        var difficulty = LearningElementDifficultyEnum.Easy;
        var elementModel = ElementModel.L_H5P_TAFEL_1;
        var content = new FileContent("bar", "foo", "");
        var actionWasInvoked = false;
        Action<LearningElement> mappingAction = _ => actionWasInvoked = true;

        var command = new EditLearningElement(element, parent, name, description, goals, difficulty, elementModel,
            workload, points, content, mappingAction);

        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));

        Assert.IsFalse(actionWasInvoked);
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesEditLearningElement()
    {
        var parent = EntityProvider.GetLearningSpace();
        var content = new FileContent("bar", "foo", "");
        var element = new LearningElement("a", content, "f", "g", LearningElementDifficultyEnum.Medium,
            ElementModel.L_H5P_SPIELAUTOMAT_1, parent, workload: 8, points: 9, positionX: 17f, positionY: 29f)
        {
            UnsavedChanges = false
        };
        parent.LearningSpaceLayout.LearningElements = new Dictionary<int, ILearningElement>
        {
            {
                0, element
            }
        };

        var name = "new element";
        var url = "google.com";
        var description = "video of learning stuff";
        var goals = "learn";
        var workload = 7;
        var points = 8;
        var difficulty = LearningElementDifficultyEnum.Easy;
        var elementModel = ElementModel.L_H5P_TAFEL_1;
        var newContent = new FileContent("foo", "bar", "foobar");
        var actionWasInvoked = false;
        Action<LearningElement> mappingAction = _ => actionWasInvoked = true;

        var command = new EditLearningElement(element, parent, name, description, goals, difficulty, elementModel,
            workload, points, newContent, mappingAction);

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(element.Name, Is.EqualTo("a"));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.LearningContent, Is.EqualTo(content));
            Assert.That(element.Description, Is.EqualTo("f"));
            Assert.That(element.Goals, Is.EqualTo("g"));
            Assert.That(element.Workload, Is.EqualTo(8));
            Assert.That(element.Points, Is.EqualTo(9));
            Assert.That(element.Difficulty, Is.EqualTo(LearningElementDifficultyEnum.Medium));
            Assert.That(element.ElementModel, Is.EqualTo(ElementModel.L_H5P_SPIELAUTOMAT_1));
            Assert.That(element.PositionX, Is.EqualTo(17f));
            Assert.That(element.PositionY, Is.EqualTo(29f));
            Assert.That(element.UnsavedChanges, Is.False);
            Assert.That(parent.ContainedLearningElements.Count(), Is.EqualTo(1));
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(element.Name, Is.EqualTo(name));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.LearningContent, Is.EqualTo(newContent));
            Assert.That(element.Description, Is.EqualTo(description));
            Assert.That(element.Goals, Is.EqualTo(goals));
            Assert.That(element.Workload, Is.EqualTo(workload));
            Assert.That(element.Points, Is.EqualTo(points));
            Assert.That(element.Difficulty, Is.EqualTo(difficulty));
            Assert.That(element.ElementModel, Is.EqualTo(elementModel));
            Assert.That(element.PositionX, Is.EqualTo(17f));
            Assert.That(element.PositionY, Is.EqualTo(29f));
            Assert.That(element.UnsavedChanges, Is.True);
            Assert.That(parent.ContainedLearningElements.Count(), Is.EqualTo(1));
        });
        actionWasInvoked = false;

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(element.Name, Is.EqualTo("a"));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.LearningContent, Is.EqualTo(content));
            Assert.That(element.Description, Is.EqualTo("f"));
            Assert.That(element.Goals, Is.EqualTo("g"));
            Assert.That(element.Workload, Is.EqualTo(8));
            Assert.That(element.Points, Is.EqualTo(9));
            Assert.That(element.Difficulty, Is.EqualTo(LearningElementDifficultyEnum.Medium));
            Assert.That(element.ElementModel, Is.EqualTo(ElementModel.L_H5P_SPIELAUTOMAT_1));
            Assert.That(element.PositionX, Is.EqualTo(17f));
            Assert.That(element.PositionY, Is.EqualTo(29f));
            Assert.That(element.UnsavedChanges, Is.False);
            Assert.That(parent.ContainedLearningElements.Count(), Is.EqualTo(1));
        });
        actionWasInvoked = false;

        command.Redo();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(element.Name, Is.EqualTo(name));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.LearningContent, Is.EqualTo(newContent));
            Assert.That(element.Description, Is.EqualTo(description));
            Assert.That(element.Goals, Is.EqualTo(goals));
            Assert.That(element.Workload, Is.EqualTo(workload));
            Assert.That(element.Points, Is.EqualTo(points));
            Assert.That(element.Difficulty, Is.EqualTo(difficulty));
            Assert.That(element.ElementModel, Is.EqualTo(elementModel));
            Assert.That(element.PositionX, Is.EqualTo(17f));
            Assert.That(element.PositionY, Is.EqualTo(29f));
            Assert.That(element.UnsavedChanges, Is.True);
            Assert.That(parent.ContainedLearningElements.Count(), Is.EqualTo(1));
        });
    }
}