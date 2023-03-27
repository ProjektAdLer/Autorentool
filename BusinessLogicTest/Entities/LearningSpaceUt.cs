using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Entities;

[TestFixture]
public class LearningSpaceUt
{
    [Test]
    public void Constructor_InitializesAllProperties()
    {
        var name = "asdf";
        var shortname = "jkl;";
        var authors = "ben and jerry";
        var url = "url";
        var description = "very cool element";
        var goals = "learn very many things";
        var requiredPoints = 10;
        var positionX = 5f;
        var positionY = 21f;
        var content1 = new FileContent("a", "b", "");
        var content2 = new FileContent("w", "e", "");
        var ele1 = new LearningElement("a", content1, "g","h",LearningElementDifficultyEnum.Easy, null, workload: 17, points: 6, positionX: 23);
        var ele2 = new LearningElement("z", content2, "z","zz", LearningElementDifficultyEnum.Medium, null, workload: 444, points: 9,positionX: double.MaxValue);
        var learningElements = new Dictionary<int, ILearningElement>
        {
            {
                0, ele1
            },
            {
                1, ele2
            }
        };
        var learningSpaceLayout = new LearningSpaceLayout(learningElements, FloorPlanEnum.Rectangle2X2);
        
        var systemUnderTest = new LearningSpace(name, shortname, authors, description, goals, requiredPoints, 
            learningSpaceLayout, positionX, positionY);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(name));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo(shortname));
            Assert.That(systemUnderTest.Authors, Is.EqualTo(authors));
            Assert.That(systemUnderTest.Description, Is.EqualTo(description));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(goals));
            Assert.That(systemUnderTest.ContainedLearningElements, Is.EqualTo(learningElements.Values));
            Assert.That(systemUnderTest.PositionX, Is.EqualTo(positionX));
            Assert.That(systemUnderTest.PositionY, Is.EqualTo(positionY));
        });
    }

    [Test]
    public void GetRestoreMemento_RestoresCorrectMemento()
    {
        var name = "asdf";
        var shortname = "jkl;";
        var authors = "ben and jerry";
        var description = "very cool element";
        var goals = "learn very many things";
        var requiredPoints = 10;
        var positionX = 5f;
        var positionY = 21f;
        var content1 = new FileContent("a", "b", "");
        var content2 = new FileContent("w", "e", "");
        var ele1 = new LearningElement("a", content1, "g","h",LearningElementDifficultyEnum.Easy, null, workload: 17,points: 90, positionX: 23);
        var ele2 = new LearningElement("z", content2, "z","zz", LearningElementDifficultyEnum.Medium, null, workload: 444,points: 9, positionX: double.MaxValue);
        var learningElements = new Dictionary<int, ILearningElement>
        {
            {
                0, ele1
            },
            {
                1, ele2
            }
        };
        var learningSpaceLayout = new LearningSpaceLayout(learningElements, FloorPlanEnum.Rectangle2X2);
        
        var systemUnderTest = new LearningSpace(name, shortname, authors, description, goals, requiredPoints, 
            learningSpaceLayout, positionX, positionY);

        var learningSpaceMemento = systemUnderTest.GetMemento();
        var learningSpaceLayoutMemento = systemUnderTest.LearningSpaceLayout.GetMemento();
        
        var nameChanged = "qwertz";
        var shortnameChanged = "uiop";
        var authorsChanged = "sdfg";
        var descriptionChanged = "changed description";
        var goalsChanged = "new goals";
        var positionXChanged = 10f;
        var positionYChanged = 14f;
        var content1Changed = new FileContent("a", "b", "");
        var content2Changed = new FileContent("w", "e", "");
        var ele1Changed = new LearningElement("ab", content1Changed, "ffg","hgg",LearningElementDifficultyEnum.Medium, null, workload: 20,points: 50, positionX: 33);
        var ele2Changed = new LearningElement("uu", content2Changed, "kkk","fff", LearningElementDifficultyEnum.Hard, null, workload: 77,points: 40, positionX: 66);
        learningElements = new Dictionary<int, ILearningElement>
        {
            {
                0, ele1Changed
            },
            {
                1, ele2Changed
            }
        };

        systemUnderTest.Name = nameChanged;
        systemUnderTest.Shortname = shortnameChanged;
        systemUnderTest.Authors = authorsChanged;
        systemUnderTest.Description = descriptionChanged;
        systemUnderTest.Goals = goalsChanged;
        systemUnderTest.PositionX = positionXChanged;
        systemUnderTest.PositionY = positionYChanged;
        systemUnderTest.LearningSpaceLayout.LearningElements = learningElements;

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(nameChanged));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo(shortnameChanged));
            Assert.That(systemUnderTest.Authors, Is.EqualTo(authorsChanged));
            Assert.That(systemUnderTest.Description, Is.EqualTo(descriptionChanged));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(goalsChanged));
            Assert.That(systemUnderTest.ContainedLearningElements, Contains.Item(ele1Changed));
            Assert.That(systemUnderTest.ContainedLearningElements, Contains.Item(ele2Changed));
            Assert.That(systemUnderTest.PositionX, Is.EqualTo(positionXChanged));
            Assert.That(systemUnderTest.PositionY, Is.EqualTo(positionYChanged));
        });
        
        systemUnderTest.RestoreMemento(learningSpaceMemento);
        systemUnderTest.LearningSpaceLayout.RestoreMemento(learningSpaceLayoutMemento);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(name));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo(shortname));
            Assert.That(systemUnderTest.Authors, Is.EqualTo(authors));
            Assert.That(systemUnderTest.Description, Is.EqualTo(description));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(goals));
            Assert.That(systemUnderTest.ContainedLearningElements, Does.Not.Contain(ele1Changed));
            Assert.That(systemUnderTest.ContainedLearningElements, Does.Not.Contain(ele2Changed));
            Assert.That(systemUnderTest.PositionX, Is.EqualTo(positionX));
            Assert.That(systemUnderTest.PositionY, Is.EqualTo(positionY));
        });
    }

    [Test]
    public void RestoreMemento_MementoIsNotLearningSpaceMemento_ThrowsException()
    {
        var name = "asdf";
        var shortname = "jkl;";
        var authors = "ben and jerry";
        var description = "very cool element";
        var goals = "learn very many things";
        var requiredPoints = 10;
        var positionX = 5f;
        var positionY = 21f;
        var content1 = new FileContent("a", "b", "");
        var content2 = new FileContent("w", "e", "");
        var ele1 = new LearningElement("a", content1, "g","h",LearningElementDifficultyEnum.Easy, null, workload: 17,points: 13, positionX: 23);
        var ele2 = new LearningElement("z", content2, "z","zz", LearningElementDifficultyEnum.Medium, null, workload: 444,points: 34, positionX: double.MaxValue);
        var learningElements = new Dictionary<int, ILearningElement>
        {
            {
                0, ele1
            },
            {
                1, ele2
            }
        };
        var learningSpaceLayout = new LearningSpaceLayout(learningElements, FloorPlanEnum.Rectangle2X2);
        
        var systemUnderTest = new LearningSpace(name, shortname, authors, description, goals, requiredPoints, 
            learningSpaceLayout, positionX, positionY);

        var mementoMock = new MementoMock();
        
        var ex = Assert.Throws<ArgumentException>(() => systemUnderTest.RestoreMemento(mementoMock));
        Assert.That(ex!.Message, Is.EqualTo("Incorrect IMemento implementation (Parameter 'memento')"));
    }

    private class MementoMock : IMemento
    {
        
    }

}