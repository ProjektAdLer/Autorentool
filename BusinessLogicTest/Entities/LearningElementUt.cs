using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Entities;

[TestFixture]
public class LearningElementUt
{
    [Test]
    public void AutomapperConstructor_InitializesAllProperties()
    {
        var name = "asdf";
        var content = new FileContent("a", "b", "");
        var description = "very cool element";
        var goals = "learn very many things";
        var difficulty = LearningElementDifficultyEnum.Medium;
        var elementModel = ElementModel.L_H5P_SPIELAUTOMAT_1;
        var workload = 5;
        var points = 6;
        var positionX = 5f;
        var positionY = 21f;

        var systemUnderTest = new LearningElement(name, content, description, goals, difficulty, elementModel, null, workload: workload, points: points, positionX: positionX, positionY: positionY);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(name));
            Assert.That(systemUnderTest.Parent, Is.Null);
            Assert.That(systemUnderTest.LearningContent, Is.EqualTo(content));
            Assert.That(systemUnderTest.Description, Is.EqualTo(description));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(goals));
            Assert.That(systemUnderTest.Difficulty, Is.EqualTo(difficulty));
            Assert.That(systemUnderTest.ElementModel, Is.EqualTo(elementModel));
            Assert.That(systemUnderTest.Workload, Is.EqualTo(workload));
            Assert.That(systemUnderTest.Points, Is.EqualTo(points));
            Assert.That(systemUnderTest.PositionX, Is.EqualTo(positionX));
            Assert.That(systemUnderTest.PositionY, Is.EqualTo(positionY));
            Assert.That(systemUnderTest.UnsavedChanges);
        });
    }
    
    [Test]
    public void NormalConstructor_InitializesAllProperties()
    {
        var name = "asdf";
        var parent = new LearningSpace("foo", "", "", 3, Theme.Campus);
        var content = new FileContent("a", "b", "");
        var description = "very cool element";
        var goals = "learn very many things";
        var difficulty = LearningElementDifficultyEnum.Medium;
        var elementModel = ElementModel.L_H5P_SPIELAUTOMAT_1;
        var workload = 5;
        var points = 6;
        var positionX = 5f;
        var positionY = 21f;

        var systemUnderTest = new LearningElement(name, content, description, goals,
             difficulty, elementModel, parent, workload: workload, points: points, positionX: positionX, positionY: positionY);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(name));
            Assert.That(systemUnderTest.Parent, Is.EqualTo(parent));
            Assert.That(systemUnderTest.LearningContent, Is.EqualTo(content));
            Assert.That(systemUnderTest.Description, Is.EqualTo(description));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(goals));
            Assert.That(systemUnderTest.Difficulty, Is.EqualTo(difficulty));
            Assert.That(systemUnderTest.ElementModel, Is.EqualTo(elementModel));
            Assert.That(systemUnderTest.Workload, Is.EqualTo(workload));
            Assert.That(systemUnderTest.Points, Is.EqualTo(points));
            Assert.That(systemUnderTest.PositionX, Is.EqualTo(positionX));
            Assert.That(systemUnderTest.PositionY, Is.EqualTo(positionY));
        });
    }
    
    [Test]
    public void GetRestoreMemento_RestoresCorrectMemento()
    {
        var name = "asdf";
        var parent = new LearningSpace("foo", "", "", 4, Theme.Campus);
        var content = new FileContent("a", "b", "");
        var description = "very cool element";
        var goals = "learn very many things";
        var difficulty = LearningElementDifficultyEnum.Medium;
        var elementModel = ElementModel.L_H5P_SPIELAUTOMAT_1;
        var workload = 5;
        var points = 6;
        var positionX = 5f;
        var positionY = 21f;

        var systemUnderTest = new LearningElement(name, content, description, goals,
            difficulty, elementModel, parent, workload: workload, points: points, positionX: positionX, positionY: positionY);

        var learningElementMemento = systemUnderTest.GetMemento();
        
        var nameChanged = "qwertz";
        var contentChanged = new FileContent("b", "c", "");
        var descriptionChanged = "changed description";
        var goalsChanged = "new goals";
        var difficultyChanged = LearningElementDifficultyEnum.Easy;
        var elementModelChanged = ElementModel.L_H5P_TAFEL_1;
        var workloadChanged = 10;
        var pointsChanged = 20;
        var positionXChanged = 10f;
        var positionYChanged = 14f;

        systemUnderTest.Name = nameChanged;
        systemUnderTest.LearningContent = contentChanged;
        systemUnderTest.Description = descriptionChanged;
        systemUnderTest.Goals = goalsChanged;
        systemUnderTest.Difficulty = difficultyChanged;
        systemUnderTest.ElementModel = elementModelChanged;
        systemUnderTest.Workload = workloadChanged;
        systemUnderTest.Points = pointsChanged;
        systemUnderTest.PositionX = positionXChanged;
        systemUnderTest.PositionY = positionYChanged;

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(nameChanged));
            Assert.That(systemUnderTest.LearningContent, Is.EqualTo(contentChanged));
            Assert.That(systemUnderTest.Description, Is.EqualTo(descriptionChanged));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(goalsChanged));
            Assert.That(systemUnderTest.Difficulty, Is.EqualTo(difficultyChanged));
            Assert.That(systemUnderTest.ElementModel, Is.EqualTo(elementModelChanged));
            Assert.That(systemUnderTest.Workload, Is.EqualTo(workloadChanged));
            Assert.That(systemUnderTest.Points, Is.EqualTo(pointsChanged));
            Assert.That(systemUnderTest.PositionX, Is.EqualTo(positionXChanged));
            Assert.That(systemUnderTest.PositionY, Is.EqualTo(positionYChanged));
        });
        
        systemUnderTest.RestoreMemento(learningElementMemento);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(name));
            Assert.That(systemUnderTest.LearningContent, Is.EqualTo(content));
            Assert.That(systemUnderTest.Description, Is.EqualTo(description));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(goals));
            Assert.That(systemUnderTest.Difficulty, Is.EqualTo(difficulty));
            Assert.That(systemUnderTest.ElementModel, Is.EqualTo(elementModel));
            Assert.That(systemUnderTest.Workload, Is.EqualTo(workload));
            Assert.That(systemUnderTest.Points, Is.EqualTo(points));
            Assert.That(systemUnderTest.PositionX, Is.EqualTo(positionX));
            Assert.That(systemUnderTest.PositionY, Is.EqualTo(positionY));
        });
    }

    [Test]
    public void RestoreMemento_MementoIsNotLearningElementMemento_ThrowsException()
    {
        var name = "asdf";
        var parent = new LearningSpace("foo", "", "", 4, Theme.Campus);
        var content = new FileContent("a", "b", "");
        var description = "very cool element";
        var goals = "learn very many things";
        var difficulty = LearningElementDifficultyEnum.Medium;
        var elementModel = ElementModel.L_H5P_SPIELAUTOMAT_1;
        var workload = 5;
        var points = 6;
        var positionX = 5f;
        var positionY = 21f;

        var systemUnderTest = new LearningElement(name, content, description, goals,
            difficulty, elementModel, parent, workload: workload, points: points, positionX: positionX, positionY: positionY);

        var mementoMock = new MementoMock();
        
        var ex = Assert.Throws<ArgumentException>(() => systemUnderTest.RestoreMemento(mementoMock));
        Assert.That(ex!.Message, Is.EqualTo("Incorrect IMemento implementation (Parameter 'memento')"));
    }

    private class MementoMock : IMemento
    {
        
    }
}