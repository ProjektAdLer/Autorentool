using BusinessLogic.Commands.Element;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging.Abstractions;
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
        var content = EntityProvider.GetFileContent();
        var element = EntityProvider.GetLearningElement(parent: parent, content: content,
            elementModel: ElementModel.l_h5p_slotmachine_1, unsavedChanges: false);
        parent.LearningSpaceLayout.LearningElements = new Dictionary<int, ILearningElement>
        {
            {
                0, element
            }
        };

        var name = "new element";
        var description = "video of learning stuff";
        var goals = "learn";
        var workload = 7;
        var points = 8;
        var difficulty = LearningElementDifficultyEnum.Hard;
        var elementModel = ElementModel.l_h5p_blackboard_1;
        var newContent = EntityProvider.GetFileContent(append: "new");
        var actionWasInvoked = false;
        Action<LearningElement> mappingAction = _ => actionWasInvoked = true;

        var command = new EditLearningElement(element, parent, name, description, goals, difficulty, elementModel,
            workload, points, newContent, mappingAction, new NullLogger<EditLearningElement>());

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(element.Name, Is.Not.EqualTo(name));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.LearningContent, Is.EqualTo(content));
            Assert.That(element.Description, Is.Not.EqualTo(description));
            Assert.That(element.Goals, Is.Not.EqualTo(goals));
            Assert.That(element.Workload, Is.Not.EqualTo(workload));
            Assert.That(element.Points, Is.Not.EqualTo(points));
            Assert.That(element.Difficulty, Is.Not.EqualTo(difficulty));
            Assert.That(element.ElementModel, Is.Not.EqualTo(elementModel));
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
            Assert.That(element.UnsavedChanges, Is.True);
            Assert.That(parent.ContainedLearningElements.Count(), Is.EqualTo(1));
        });
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var parent = EntityProvider.GetLearningSpace();
        var element = EntityProvider.GetLearningElement(elementModel: ElementModel.l_h5p_slotmachine_1);
        var name = "new element";
        var description = "video of learning stuff";
        var goals = "learn";
        var workload = 7;
        var points = 8;
        var difficulty = LearningElementDifficultyEnum.Easy;
        var elementModel = ElementModel.l_h5p_blackboard_1;
        var content = EntityProvider.GetFileContent();
        var actionWasInvoked = false;
        Action<LearningElement> mappingAction = _ => actionWasInvoked = true;

        var command = new EditLearningElement(element, parent, name, description, goals, difficulty, elementModel,
            workload, points, content, mappingAction, null!);

        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));

        Assert.IsFalse(actionWasInvoked);
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesEditLearningElement()
    {
        var parent = EntityProvider.GetLearningSpace();
        var content = EntityProvider.GetFileContent();
        var element = EntityProvider.GetLearningElement(parent: parent, content: content,
            elementModel: ElementModel.l_h5p_slotmachine_1, unsavedChanges: false);
        parent.LearningSpaceLayout.LearningElements = new Dictionary<int, ILearningElement>
        {
            {
                0, element
            }
        };

        var name = "new element";
        var description = "video of learning stuff";
        var goals = "learn";
        var workload = 7;
        var points = 8;
        var difficulty = LearningElementDifficultyEnum.Hard;
        var elementModel = ElementModel.l_h5p_blackboard_1;
        var newContent = EntityProvider.GetFileContent(append: "new");
        var actionWasInvoked = false;
        Action<LearningElement> mappingAction = _ => actionWasInvoked = true;

        var command = new EditLearningElement(element, parent, name, description, goals, difficulty, elementModel,
            workload, points, newContent, mappingAction, new NullLogger<EditLearningElement>());

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(element.Name, Is.Not.EqualTo(name));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.LearningContent, Is.EqualTo(content));
            Assert.That(element.Description, Is.Not.EqualTo(description));
            Assert.That(element.Goals, Is.Not.EqualTo(goals));
            Assert.That(element.Workload, Is.Not.EqualTo(workload));
            Assert.That(element.Points, Is.Not.EqualTo(points));
            Assert.That(element.Difficulty, Is.Not.EqualTo(difficulty));
            Assert.That(element.ElementModel, Is.Not.EqualTo(elementModel));
            Assert.That(element.UnsavedChanges, Is.False);
            Assert.That(parent.ContainedLearningElements.Count(), Is.EqualTo(1));
        });

        var oldName = element.Name;
        var oldDescription = element.Description;
        var oldGoals = element.Goals;
        var oldWorkload = element.Workload;
        var oldPoints = element.Points;
        var oldDifficulty = element.Difficulty;
        var oldElementModel = element.ElementModel;

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
            Assert.That(element.UnsavedChanges, Is.True);
            Assert.That(parent.ContainedLearningElements.Count(), Is.EqualTo(1));
        });
        actionWasInvoked = false;

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(element.Name, Is.EqualTo(oldName));
            Assert.That(element.Parent, Is.EqualTo(parent));
            Assert.That(element.LearningContent, Is.EqualTo(content));
            Assert.That(element.Description, Is.EqualTo(oldDescription));
            Assert.That(element.Goals, Is.EqualTo(oldGoals));
            Assert.That(element.Workload, Is.EqualTo(oldWorkload));
            Assert.That(element.Points, Is.EqualTo(oldPoints));
            Assert.That(element.Difficulty, Is.EqualTo(oldDifficulty));
            Assert.That(element.ElementModel, Is.EqualTo(oldElementModel));
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
            Assert.That(element.UnsavedChanges, Is.True);
            Assert.That(parent.ContainedLearningElements.Count(), Is.EqualTo(1));
        });
    }
}