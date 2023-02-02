using BusinessLogic.Entities;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Entities;

[TestFixture]
public class SpaceLayoutUt
{
    [Test]
    public void Constructor_InitializesAllProperties()
    {
        var content1 = new Content("a", "b", "");
        var content2 = new Content("w", "e", "");
        var ele1 = new Element("a", "b", content1, "url", "pupup", "g", "h", ElementDifficultyEnum.Easy,
            null, 17, 6, 23);
        var ele2 = new Element("z", "zz", content2, "url", "baba", "z", "zz",
            ElementDifficultyEnum.Medium, null, 444, 9, double.MaxValue);
        var elements = new IElement?[] {ele1, ele2, null, null, null, null};
        var floorPlanName = FloorPlanEnum.Rectangle2X3;

        var systemUnderTest = new SpaceLayout(elements, floorPlanName);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.FloorPlanName, Is.EqualTo(floorPlanName));
            Assert.That(systemUnderTest.Elements, Is.EqualTo(elements));
            Assert.That(systemUnderTest.Elements[0], Is.SameAs(ele1));
            Assert.That(systemUnderTest.Elements[1], Is.SameAs(ele2));
            Assert.That(systemUnderTest.ContainedElements.Count(), Is.EqualTo(2));
            Assert.That(systemUnderTest.ContainedElements, Has.Member(ele1));
            Assert.That(systemUnderTest.ContainedElements, Has.Member(ele2));
        });
    }

    [Test]
    public void GetRestoreMemento_RestoresCorrectMemento()
    {
        var content1 = new Content("a", "b", "");
        var content2 = new Content("w", "e", "");
        var ele1 = new Element("a", "b", content1, "url", "pupup", "g", "h", ElementDifficultyEnum.Easy,
            null, 17, 6, 23);
        var ele2 = new Element("z", "zz", content2, "url", "baba", "z", "zz",
            ElementDifficultyEnum.Medium, null, 444, 9, double.MaxValue);
        var elements = new IElement?[] {ele1, ele2, null, null, null, null};
        var floorPlanName = FloorPlanEnum.Rectangle2X3;

        var systemUnderTest = new SpaceLayout(elements, floorPlanName);

        var spaceLayoutMemento = systemUnderTest.GetMemento();

        var content1Changed = new Content("c", "d", "");
        var content2Changed = new Content("e", "f", "");
        var ele1Changed = new Element("ab", "bc", content1Changed, "url", "pupuper", "ffg", "hgg",
            ElementDifficultyEnum.Medium, null, 20, 50, 33);
        var ele2Changed = new Element("uu", "iii", content2Changed, "url", "lll", "kkk", "fff",
            ElementDifficultyEnum.Hard, null, 77, 40, 66);
        var elementsChanged = new IElement?[] {null, ele1Changed, ele2Changed, null, null};
        var floorPlanNameChanged = FloorPlanEnum.LShape3L2;

        systemUnderTest.FloorPlanName = floorPlanNameChanged;
        systemUnderTest.Elements = elementsChanged;

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.FloorPlanName, Is.EqualTo(floorPlanNameChanged));
            Assert.That(systemUnderTest.Elements, Is.EqualTo(elementsChanged));
            Assert.That(systemUnderTest.Elements[1], Is.SameAs(ele1Changed));
            Assert.That(systemUnderTest.Elements[2], Is.SameAs(ele2Changed));
            Assert.That(systemUnderTest.ContainedElements.Count(), Is.EqualTo(2));
            Assert.That(systemUnderTest.ContainedElements, Has.Member(ele1Changed));
            Assert.That(systemUnderTest.ContainedElements, Has.Member(ele2Changed));
        });

        systemUnderTest.RestoreMemento(spaceLayoutMemento);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.FloorPlanName, Is.EqualTo(floorPlanName));
            Assert.That(systemUnderTest.Elements, Is.EqualTo(elements));
            Assert.That(systemUnderTest.Elements[0], Is.SameAs(ele1));
            Assert.That(systemUnderTest.Elements[1], Is.SameAs(ele2));
            Assert.That(systemUnderTest.ContainedElements.Count(), Is.EqualTo(2));
            Assert.That(systemUnderTest.ContainedElements, Has.Member(ele1));
            Assert.That(systemUnderTest.ContainedElements, Has.Member(ele2));
        });
    }

    [Test]
    public void RestoreMemento_MementoIsNotSpaceLayoutMemento_ThrowsException()
    {
        var content1 = new Content("a", "b", "");
        var content2 = new Content("w", "e", "");
        var ele1 = new Element("a", "b", content1, "url", "pupup", "g", "h", ElementDifficultyEnum.Easy,
            null, 17, 6, 23);
        var ele2 = new Element("z", "zz", content2, "url", "baba", "z", "zz",
            ElementDifficultyEnum.Medium, null, 444, 9, double.MaxValue);
        var elements = new IElement?[] {ele1, ele2, null, null, null, null};
        var floorPlanName = FloorPlanEnum.Rectangle2X3;

        var systemUnderTest = new SpaceLayout(elements, floorPlanName);

        var mementoMock = new MementoMock();

        var ex = Assert.Throws<ArgumentException>(() => systemUnderTest.RestoreMemento(mementoMock));
        Assert.That(ex!.Message, Is.EqualTo("Incorrect IMemento implementation (Parameter 'memento')"));
    }

    private class MementoMock : IMemento
    {
    }
}