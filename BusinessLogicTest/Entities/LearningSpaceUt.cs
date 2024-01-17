using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningOutcome;
using NUnit.Framework;
using Shared;
using TestHelpers;

namespace BusinessLogicTest.Entities;

[TestFixture]
public class LearningSpaceUt
{
    [Test]
    public void Constructor_InitializesAllProperties()
    {
        var name = "asdf";
        var description = "very cool element";
        var learningOutcomes = EntityProvider.GetLearningOutcomes();
        var requiredPoints = 10;
        var positionX = 5f;
        var positionY = 21f;
        var ele1 = EntityProvider.GetLearningElement();
        var ele2 = EntityProvider.GetLearningElement();
        var learningElements = new Dictionary<int, ILearningElement>
        {
            {
                0, ele1
            },
            {
                1, ele2
            }
        };
        var learningSpaceLayout =
            EntityProvider.GetLearningSpaceLayout(learningElements: learningElements,
                floorPlan: FloorPlanEnum.R_20X20_6L);
        var assignedTopic = EntityProvider.GetTopic();

        var systemUnderTest = new LearningSpace(name, description, requiredPoints, Theme.Campus, learningOutcomes,
            learningSpaceLayout, positionX: positionX, positionY: positionY, assignedTopic: assignedTopic);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(name));
            Assert.That(systemUnderTest.Description, Is.EqualTo(description));
            Assert.That(systemUnderTest.LearningOutcomes, Is.EqualTo(learningOutcomes));
            Assert.That(systemUnderTest.ContainedLearningElements, Is.EqualTo(learningElements.Values));
            Assert.That(systemUnderTest.AssignedTopic, Is.EqualTo(assignedTopic));
            Assert.That(systemUnderTest.PositionX, Is.EqualTo(positionX));
            Assert.That(systemUnderTest.PositionY, Is.EqualTo(positionY));
            Assert.That(systemUnderTest.UnsavedChanges);
        });
    }

    [Test]
    public void GetRestoreMemento_RestoresCorrectMemento()
    {
        var name = "asdf";
        var description = "very cool element";
        var learningOutcomes = EntityProvider.GetLearningOutcomes();
        var requiredPoints = 10;
        var positionX = 5f;
        var positionY = 21f;
        var content1 = EntityProvider.GetFileContent(append: "1");
        var content2 = EntityProvider.GetFileContent(append: "2");
        var ele1 = EntityProvider.GetLearningElement(append: "1", content: content1);
        var ele2 = EntityProvider.GetLearningElement(append: "2", content: content2);
        var learningElements = new Dictionary<int, ILearningElement>
        {
            {
                0, ele1
            },
            {
                1, ele2
            }
        };
        var learningSpaceLayout = EntityProvider.GetLearningSpaceLayout(learningElements: learningElements);
        var assignedTopic = EntityProvider.GetTopic();

        var systemUnderTest = new LearningSpace(name, description, requiredPoints, Theme.Campus, learningOutcomes,
            learningSpaceLayout, positionX: positionX, positionY: positionY, assignedTopic: assignedTopic);

        var learningSpaceMemento = systemUnderTest.GetMemento();
        var learningSpaceLayoutMemento = systemUnderTest.LearningSpaceLayout.GetMemento();

        var nameChanged = "qwertz";
        var descriptionChanged = "changed description";
        var LOutcomesChanged = new List<ILearningOutcome>() { new ManualLearningOutcome("New Outcome") };
        var positionXChanged = 10f;
        var positionYChanged = 14f;
        var content1Changed = EntityProvider.GetFileContent(append: "c1");
        var content2Changed = EntityProvider.GetFileContent(append: "c2");
        var ele1Changed = EntityProvider.GetLearningElement(content: content1Changed, append: "c1");
        var ele2Changed = EntityProvider.GetLearningElement(content: content2Changed, append: "c2");
        learningElements = new Dictionary<int, ILearningElement>
        {
            {
                0, ele1Changed
            },
            {
                1, ele2Changed
            }
        };
        var topicChanged = EntityProvider.GetTopic(append: "2");

        systemUnderTest.Name = nameChanged;
        systemUnderTest.Description = descriptionChanged;
        systemUnderTest.LearningOutcomes = LOutcomesChanged;
        systemUnderTest.PositionX = positionXChanged;
        systemUnderTest.PositionY = positionYChanged;
        systemUnderTest.AssignedTopic = topicChanged;
        systemUnderTest.LearningSpaceLayout.LearningElements = learningElements;

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(nameChanged));
            Assert.That(systemUnderTest.Description, Is.EqualTo(descriptionChanged));
            Assert.That(systemUnderTest.LearningOutcomes, Is.EqualTo(LOutcomesChanged));
            Assert.That(systemUnderTest.ContainedLearningElements, Contains.Item(ele1Changed));
            Assert.That(systemUnderTest.ContainedLearningElements, Contains.Item(ele2Changed));
            Assert.That(systemUnderTest.PositionX, Is.EqualTo(positionXChanged));
            Assert.That(systemUnderTest.PositionY, Is.EqualTo(positionYChanged));
            Assert.That(systemUnderTest.AssignedTopic, Is.EqualTo(topicChanged));
        });

        systemUnderTest.RestoreMemento(learningSpaceMemento);
        systemUnderTest.LearningSpaceLayout.RestoreMemento(learningSpaceLayoutMemento);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(name));
            Assert.That(systemUnderTest.Description, Is.EqualTo(description));
            Assert.That(systemUnderTest.LearningOutcomes, Is.EqualTo(learningOutcomes));
            Assert.That(systemUnderTest.ContainedLearningElements, Does.Not.Contain(ele1Changed));
            Assert.That(systemUnderTest.ContainedLearningElements, Does.Not.Contain(ele2Changed));
            Assert.That(systemUnderTest.PositionX, Is.EqualTo(positionX));
            Assert.That(systemUnderTest.PositionY, Is.EqualTo(positionY));
            Assert.That(systemUnderTest.AssignedTopic, Is.EqualTo(assignedTopic));
        });
    }

    [Test]
    public void RestoreMemento_MementoIsNotLearningSpaceMemento_ThrowsException()
    {
        var name = "asdf";
        var description = "very cool element";
        var learningOutcomes = EntityProvider.GetLearningOutcomes();
        var requiredPoints = 10;
        var positionX = 5f;
        var positionY = 21f;
        var content1 = EntityProvider.GetFileContent(append: "1");
        var content2 = EntityProvider.GetFileContent(append: "2");
        var ele1 = EntityProvider.GetLearningElement(append: "1", content: content1);
        var ele2 = EntityProvider.GetLearningElement(append: "2", content: content2);
        var learningElements = new Dictionary<int, ILearningElement>
        {
            {
                0, ele1
            },
            {
                1, ele2
            }
        };
        var learningSpaceLayout =
            EntityProvider.GetLearningSpaceLayout(learningElements: learningElements,
                floorPlan: FloorPlanEnum.R_20X20_6L);

        var systemUnderTest = new LearningSpace(name, description, requiredPoints, Theme.Campus, learningOutcomes,
            learningSpaceLayout, positionX: positionX, positionY: positionY);

        var mementoMock = new MementoMock();

        var ex = Assert.Throws<ArgumentException>(() => systemUnderTest.RestoreMemento(mementoMock));
        Assert.That(ex!.Message, Is.EqualTo("Incorrect IMemento implementation (Parameter 'memento')"));
    }

    private class MementoMock : IMemento
    {
    }
}