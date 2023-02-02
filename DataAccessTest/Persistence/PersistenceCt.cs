using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using DataAccess.Persistence;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using PersistEntities;

namespace DataAccessTest.Persistence;

/// <summary>
/// Component tests to test whether Persistence roundtrip produces equal objects
/// </summary>
[TestFixture]
public class PersistenceCt
{
    private const string FilePath = "awesomefile.txt";

    [Test]
    public void Persistence_SaveAndLoadWorld_Stream_ObjectsAreEquivalent()
    {
        var world = new WorldPe("Name", "Shortname", "Authors", "Language",
            "Description", "Goals");
        var initialWorldId = world.Id;
        var space1 = new SpacePe("Name", "Shortname", "Authors", "Description", "Goals", 5, new SpaceLayoutPe(new IElementPe[6], FloorPlanEnumPe.Rectangle2X3));
        var initialSpace1Id = space1.Id;
        var space2 = new SpacePe("Name", "Shortname", "Authors", "Description", "Goals", 5, new SpaceLayoutPe(new IElementPe[6], FloorPlanEnumPe.Rectangle2X3));
        var initialSpace2Id = space2.Id;
        var condition1 = new PathWayConditionPe(ConditionEnumPe.Or, 2, 1);
        var initialCondition1Id = condition1.Id;
        var condition2 = new PathWayConditionPe(ConditionEnumPe.And, 5, 6);
        var initialCondition2Id = condition2.Id;
        var content = new ContentPe("a", "b", "");
        var element = new ElementPe("le", "la", content, "url","lll", "llll","lllll", ElementDifficultyEnumPe.Easy);
        var initialElementId = element.Id;
        space1.SpaceLayout.Elements[0] = element;
        world.Spaces.Add(space1);
        world.Spaces.Add(space2);
        world.PathWayConditions.Add(condition1);
        world.PathWayConditions.Add(condition2);
        space1.OutBoundObjects.Add(condition1);
        condition1.InBoundObjects.Add(space1);
        condition1.OutBoundObjects.Add(space2);
        space2.InBoundObjects.Add(condition1);
        space2.OutBoundObjects.Add(condition2);
        condition2.InBoundObjects.Add(space2);
        world.Pathways.Add(new PathwayPe(space1, condition1));
        world.Pathways.Add(new PathwayPe(condition1, space2));
        world.Pathways.Add(new PathwayPe(space2, condition2));

        using var stream = new MemoryStream();
        var systemUnderTest = CreateTestableFileSaveHandler<WorldPe>();
        
        systemUnderTest.SaveToStream(world, stream);
        stream.Position = 0;
        var restoredWorld = systemUnderTest.LoadFromStream(stream);
        
        restoredWorld.Should().BeEquivalentTo(world, options => options.IgnoringCyclicReferences()
            .Excluding(obj => obj.Id)
            .For(obj => obj.Spaces).Exclude(obj => obj.Id)
            .For(obj => obj.Spaces).Exclude(obj => obj.InBoundObjects)
            .For(obj => obj.Spaces).Exclude(obj => obj.OutBoundObjects)
            .For(obj => obj.PathWayConditions).Exclude(obj => obj.Id)
            .For(obj => obj.PathWayConditions).Exclude(obj => obj.InBoundObjects)
            .For(obj => obj.PathWayConditions).Exclude(obj => obj.OutBoundObjects)
            .For(obj => obj.ObjectsInPathWaysPe).Exclude(obj => obj.Id)
            .For(obj => obj.ObjectsInPathWaysPe).Exclude(obj => obj.InBoundObjects)
            .For(obj => obj.ObjectsInPathWaysPe).Exclude(obj => obj.OutBoundObjects)
            .For(obj => obj.Spaces).For(obj => obj.SpaceLayout.ContainedElements).Exclude(obj => obj.Id)
            .Excluding(obj => obj.Pathways));

        Assert.That(restoredWorld.Spaces[0].OutBoundObjects, Does.Contain(restoredWorld.PathWayConditions[0]));
        Assert.That(restoredWorld.PathWayConditions[0].InBoundObjects, Does.Contain(restoredWorld.Spaces[0]));
        Assert.That(restoredWorld.PathWayConditions[0].OutBoundObjects, Does.Contain(restoredWorld.Spaces[1]));
        Assert.That(restoredWorld.Spaces[1].InBoundObjects, Does.Contain(restoredWorld.PathWayConditions[0]));
        Assert.That(restoredWorld.Spaces[1].OutBoundObjects, Does.Contain(restoredWorld.PathWayConditions[1]));
        Assert.That(restoredWorld.PathWayConditions[1].InBoundObjects, Does.Contain(restoredWorld.Spaces[1]));
        
        Assert.That(restoredWorld.Pathways[0].SourceObject, Is.EqualTo(restoredWorld.Spaces[0]));
        Assert.That(restoredWorld.Pathways[0].TargetObject, Is.EqualTo(restoredWorld.PathWayConditions[0]));
        Assert.That(restoredWorld.Pathways[1].SourceObject, Is.EqualTo(restoredWorld.PathWayConditions[0]));
        Assert.That(restoredWorld.Pathways[1].TargetObject, Is.EqualTo(restoredWorld.Spaces[1]));
        Assert.That(restoredWorld.Pathways[2].SourceObject, Is.EqualTo(restoredWorld.Spaces[1]));
        Assert.That(restoredWorld.Pathways[2].TargetObject, Is.EqualTo(restoredWorld.PathWayConditions[1]));
        
        Assert.That(initialWorldId, Is.Not.EqualTo(restoredWorld.Id));
        Assert.That(initialSpace1Id, Is.Not.EqualTo(restoredWorld.Spaces.First().Id));
        Assert.That(initialSpace2Id, Is.Not.EqualTo(restoredWorld.Spaces.Last().Id));
        Assert.That(initialElementId, Is.Not.EqualTo(restoredWorld.Spaces.First().SpaceLayout.ContainedElements.First().Id));
        Assert.That(initialCondition1Id, Is.Not.EqualTo(restoredWorld.PathWayConditions.First().Id));
        Assert.That(initialCondition2Id, Is.Not.EqualTo(restoredWorld.PathWayConditions.Last().Id));
    }
    
    [Test]
    public void Persistence_SaveAndLoadSpace_Stream_ObjectsAreEquivalent()
    {
        var space = new SpacePe("Name", "Shortname", "Authors", "Description", "Goals", 5, new SpaceLayoutPe(new IElementPe[6], FloorPlanEnumPe.Rectangle2X3));
        var initialSpaceId = space.Id;
        var content = new ContentPe("a", "b", "");
        var element = new ElementPe("le", "la", content,"url","ll", "l" ,"lll", ElementDifficultyEnumPe.Easy);
        var initialElementId = element.Id;
        space.SpaceLayout.Elements[0] = element;
        
        using var stream = new MemoryStream();
        var systemUnderTest = CreateTestableFileSaveHandler<SpacePe>();
        
        systemUnderTest.SaveToStream(space, stream);
        stream.Position = 0;
        var restoredSpace = systemUnderTest.LoadFromStream(stream);
        
        restoredSpace.Should().BeEquivalentTo(space, options => options.Excluding(obj => obj.Id)
            .For(obj => obj.SpaceLayout.ContainedElements).Exclude(obj => obj.Id));
        Assert.That(initialSpaceId, Is.Not.EqualTo(restoredSpace.Id));
        Assert.That(initialElementId, Is.Not.EqualTo(restoredSpace.SpaceLayout.ContainedElements.First().Id));
    }
    
    [Test]
    public void Persistence_SaveAndLoadElement_Stream_ObjectsAreEquivalent()
    {
        var content = new ContentPe("a", "b", "");
        var element = new ElementPe("le", "la", content, "url","ll", "ll", "lll", ElementDifficultyEnumPe.Easy);
        var initialElementId = element.Id;
        
        using var stream = new MemoryStream();
        var systemUnderTest = CreateTestableFileSaveHandler<ElementPe>();
        
        systemUnderTest.SaveToStream(element, stream);
        stream.Position = 0;
        var restoredElement = systemUnderTest.LoadFromStream(stream);

        restoredElement.Should().BeEquivalentTo(element, options => options.Excluding(obj => obj.Id));
        Assert.That(initialElementId, Is.Not.EqualTo(restoredElement.Id));
    }

    [Test]
    public void Persistence_SaveAndLoadWorld_File_ObjectsAreEquivalent()
    {
        var world = new WorldPe("Name", "Shortname", "Authors", "Language",
            "Description", "Goals");
        var initialWorldId = world.Id;
        var space1 = new SpacePe("Name", "Shortname", "Authors", "Description", "Goals", 5, new SpaceLayoutPe(new IElementPe[6], FloorPlanEnumPe.Rectangle2X3));
        var initialSpace1Id = space1.Id;
        var space2 = new SpacePe("Name", "Shortname", "Authors", "Description", "Goals", 5, new SpaceLayoutPe(new IElementPe[6], FloorPlanEnumPe.Rectangle2X3));
        var initialSpace2Id = space2.Id;
        var condition1 = new PathWayConditionPe(ConditionEnumPe.Or, 2, 1);
        var initialCondition1Id = condition1.Id;
        var condition2 = new PathWayConditionPe(ConditionEnumPe.And, 5, 6);
        var initialCondition2Id = condition2.Id;
        var content = new ContentPe("a", "b", "");
        var element = new ElementPe("le", "la", content, "url","lll", "llll","lllll", ElementDifficultyEnumPe.Easy);
        var initialElementId = element.Id;
        space1.SpaceLayout.Elements[0] = element;
        world.Spaces.Add(space1);
        world.Spaces.Add(space2);
        world.PathWayConditions.Add(condition1);
        world.PathWayConditions.Add(condition2);
        space1.OutBoundObjects.Add(condition1);
        condition1.InBoundObjects.Add(space1);
        condition1.OutBoundObjects.Add(space2);
        space2.InBoundObjects.Add(condition1);
        space2.OutBoundObjects.Add(condition2);
        condition2.InBoundObjects.Add(space2);
        world.Pathways.Add(new PathwayPe(space1, condition1));
        world.Pathways.Add(new PathwayPe(condition1, space2));
        world.Pathways.Add(new PathwayPe(space2, condition2));
        var mockFileSystem = new MockFileSystem();

        var systemUnderTest = CreateTestableFileSaveHandler<WorldPe>(fileSystem:mockFileSystem);
        
        systemUnderTest.SaveToDisk(world, FilePath);
        var restoredWorld = systemUnderTest.LoadFromDisk(FilePath);

        restoredWorld.Should().BeEquivalentTo(world, options => options.IgnoringCyclicReferences()
            .Excluding(obj => obj.Id)
            .For(obj => obj.Spaces).Exclude(obj => obj.Id)
            .For(obj => obj.Spaces).Exclude(obj => obj.InBoundObjects)
            .For(obj => obj.Spaces).Exclude(obj => obj.OutBoundObjects)
            .For(obj => obj.PathWayConditions).Exclude(obj => obj.Id)
            .For(obj => obj.PathWayConditions).Exclude(obj => obj.InBoundObjects)
            .For(obj => obj.PathWayConditions).Exclude(obj => obj.OutBoundObjects)
            .For(obj => obj.ObjectsInPathWaysPe).Exclude(obj => obj.Id)
            .For(obj => obj.ObjectsInPathWaysPe).Exclude(obj => obj.InBoundObjects)
            .For(obj => obj.ObjectsInPathWaysPe).Exclude(obj => obj.OutBoundObjects)
            .For(obj => obj.Spaces).For(obj => obj.SpaceLayout.ContainedElements)
            .Exclude(obj => obj.Id)
            .Excluding(obj => obj.Pathways));
        Assert.That(restoredWorld.Spaces[0].OutBoundObjects, Does.Contain(restoredWorld.PathWayConditions[0]));
        Assert.That(restoredWorld.PathWayConditions[0].InBoundObjects, Does.Contain(restoredWorld.Spaces[0]));
        Assert.That(restoredWorld.PathWayConditions[0].OutBoundObjects, Does.Contain(restoredWorld.Spaces[1]));
        Assert.That(restoredWorld.Spaces[1].InBoundObjects, Does.Contain(restoredWorld.PathWayConditions[0]));
        Assert.That(restoredWorld.Spaces[1].OutBoundObjects, Does.Contain(restoredWorld.PathWayConditions[1]));
        Assert.That(restoredWorld.PathWayConditions[1].InBoundObjects, Does.Contain(restoredWorld.Spaces[1]));
        
        Assert.That(restoredWorld.Pathways[0].SourceObject, Is.EqualTo(restoredWorld.Spaces[0]));
        Assert.That(restoredWorld.Pathways[0].TargetObject, Is.EqualTo(restoredWorld.PathWayConditions[0]));
        Assert.That(restoredWorld.Pathways[1].SourceObject, Is.EqualTo(restoredWorld.PathWayConditions[0]));
        Assert.That(restoredWorld.Pathways[1].TargetObject, Is.EqualTo(restoredWorld.Spaces[1]));
        Assert.That(restoredWorld.Pathways[2].SourceObject, Is.EqualTo(restoredWorld.Spaces[1]));
        Assert.That(restoredWorld.Pathways[2].TargetObject, Is.EqualTo(restoredWorld.PathWayConditions[1]));
        
        Assert.That(initialWorldId, Is.Not.EqualTo(restoredWorld.Id));
        Assert.That(initialSpace1Id, Is.Not.EqualTo(restoredWorld.Spaces.First().Id));
        Assert.That(initialSpace2Id, Is.Not.EqualTo(restoredWorld.Spaces.Last().Id));
        Assert.That(initialElementId, Is.Not.EqualTo(restoredWorld.Spaces.First().SpaceLayout.ContainedElements.First().Id));
        Assert.That(initialCondition1Id, Is.Not.EqualTo(restoredWorld.PathWayConditions.First().Id));
        Assert.That(initialCondition2Id, Is.Not.EqualTo(restoredWorld.PathWayConditions.Last().Id));
    }
    
    [Test]
    public void Persistence_SaveAndLoadSpace_File_ObjectsAreEquivalent()
    {
        var space = new SpacePe("Name", "Shortname", "Authors", "Description", "Goals", 5, new SpaceLayoutPe(new IElementPe?[6], FloorPlanEnumPe.Rectangle2X3));
        var initialSpaceId = space.Id;
        var content = new ContentPe("a", "b", "");
        var element = new ElementPe("le", "la", content, "url","ll", "llll","lllll", ElementDifficultyEnumPe.Easy);
        var initialElementId = element.Id;
        space.SpaceLayout.Elements[0] = element;
        var mockFileSystem = new MockFileSystem();
        
        var systemUnderTest = CreateTestableFileSaveHandler<SpacePe>(fileSystem:mockFileSystem);
        
        systemUnderTest.SaveToDisk(space, FilePath);
        var restoredSpace = systemUnderTest.LoadFromDisk(FilePath);
        
        restoredSpace.Should().BeEquivalentTo(space, options => options.Excluding(obj => obj.Id)
            .For(obj => obj.SpaceLayout.ContainedElements).Exclude(obj => obj.Id));
        Assert.That(initialSpaceId, Is.Not.EqualTo(restoredSpace.Id));
        Assert.That(initialElementId, Is.Not.EqualTo(restoredSpace.SpaceLayout.ContainedElements.First().Id));
    }

    [Test]
    public void Persistence_SaveAndLoadSpace_File_WithAllElementTypes_ObjectsAreEquivalent()
    {
        var space = new SpacePe("Name", "Shortname", "Authors", "Description", "Goals", 5);
        space.SpaceLayout.Elements = GetAllElementTypes();
        
        var systemUnderTest = CreateTestableFileSaveHandler<SpacePe>();
        
        systemUnderTest.SaveToDisk(space, FilePath);
        var restoredSpace = systemUnderTest.LoadFromDisk(FilePath);

        restoredSpace.Should().BeEquivalentTo(space, options => options.Excluding(obj => obj.Id)
            .For(obj => obj.SpaceLayout.ContainedElements).Exclude(obj => obj.Id));
    }
    
    [Test]
    public void Persistence_SaveAndLoadElement_File_ObjectsAreEquivalent()
    {
        var content = new ContentPe("a", "b", "");
        var element = new ElementPe("le", "la", content, "url","ll", "llll","lllll", ElementDifficultyEnumPe.Easy);
        var initialElementId = element.Id;
        var mockFileSystem = new MockFileSystem();

        var systemUnderTest = CreateTestableFileSaveHandler<ElementPe>(fileSystem:mockFileSystem);
        
        systemUnderTest.SaveToDisk(element, FilePath);
        var restoredElement = systemUnderTest.LoadFromDisk(FilePath);

        restoredElement.Should().BeEquivalentTo(element, options => options.Excluding(obj => obj.Id));
        Assert.That(initialElementId, Is.Not.EqualTo(restoredElement.Id));
    }

    [Test]
    [TestCaseSource(nameof(GetAllElementTypes))]
    public void Persistence_SaveAndLoadElement_File_SerializationWorksForEveryType(ElementPe lep)
    {
        var systemUnderTest = CreateTestableFileSaveHandler<ElementPe>();
        
        systemUnderTest.SaveToDisk(lep, "element.aef");
        var restoredLep = systemUnderTest.LoadFromDisk("element.aef");

        restoredLep.Should().BeEquivalentTo(lep, options => options.Excluding(obj => obj.Id));
    }

    static IElementPe?[] GetAllElementTypes()
    {
        var content = new ContentPe("a", "b", "");
        return new IElementPe[]
        {
            new H5PActivationElementPe("h5pAct", "asdf", content, "", "me :)", "description", "a goal",
                ElementDifficultyEnumPe.Easy, 123, 42, 0, 0),
            new H5PInteractionElementPe("h5pInt", "blabla", content, "", "me :)", "description", "a goal",
                ElementDifficultyEnumPe.Medium, 123, 42, 0, 0),
            new H5PTestElementPe("h5pTest", "bababubu", content, "", "me :)", "description", "a goal",
                ElementDifficultyEnumPe.Hard, 123, 42, 0, 0),
            new ImageTransferElementPe("imgTrans", "bababubu", content, "", "me :)", "description", "a goal",
                ElementDifficultyEnumPe.Hard, 123, 42, 0, 0),
            new PdfTransferElementPe("pdfTrans", "bababubu", content, "", "me :)", "description", "a goal",
                ElementDifficultyEnumPe.Hard, 123, 42, 0, 0),
            new TextTransferElementPe("txtTrans", "bababubu", content, "", "me :)", "description", "a goal",
                ElementDifficultyEnumPe.Hard, 123, 42, 0, 0),
            new VideoActivationElementPe("vidAct", "bababubu", content, "", "me :)", "description", "a goal",
                ElementDifficultyEnumPe.Hard, 123, 42, 0, 0),
            new VideoTransferElementPe("vidTrans", "bababubu", content, "", "me :)", "description", "a goal",
                ElementDifficultyEnumPe.Hard, 123, 42, 0, 0),
        };
    }

    [Test]
    public void SaveAndLoadWorld_WithExactSameElementInTwoSpaces_ElementIsEqualObject()
    {
        var content = new ContentPe("a", "b", "");
        var element = new ElementPe("le", "la", content,"",  "ll", "llll", "lllll",
            ElementDifficultyEnumPe.Easy);
        var space1 = new SpacePe("Name", "Shortname", "Authors", "Description", "Goals", 5,
            new SpaceLayoutPe(new IElementPe?[] {element}, FloorPlanEnumPe.Rectangle2X3));
        var space2 = new SpacePe("Name", "Shortname", "Authors", "Description", "Goals", 5,
            new SpaceLayoutPe(new IElementPe?[] {element}, FloorPlanEnumPe.Rectangle2X3));
        var world = new WorldPe("Name", "Shortname", "Authors", "Language",
            "Description", "Goals", spaces: new List<SpacePe> { space1, space2 });
        
        var mockFileSystem = new MockFileSystem();

        var systemUnderTest = CreateTestableFileSaveHandler<WorldPe>(fileSystem:mockFileSystem);
        
        systemUnderTest.SaveToDisk(world, "foobar.txt");

        var actual = systemUnderTest.LoadFromDisk("foobar.txt");

        Assert.That(actual.Spaces[0].SpaceLayout.ContainedElements.First(),
            Is.EqualTo(actual.Spaces[1].SpaceLayout.ContainedElements.First()));
    }
    
    [Test]
    public void SaveAndLoadWorld_WithTwoEquivalentElementsInTwoSpaces_ElementIsNotEqualObject()
    {
        var content = new ContentPe("a", "b", "");
        var element1 = new ElementPe("le", "la", content, "", "ll", "llll", "lllll",
            ElementDifficultyEnumPe.Easy);
        var element2 = new ElementPe("le", "la", content, "","ll", "llll", "lllll",
            ElementDifficultyEnumPe.Easy);
        var space1 = new SpacePe("Name", "Shortname", "Authors", "Description", "Goals", 5,
            new SpaceLayoutPe(new IElementPe?[] {element1}, FloorPlanEnumPe.Rectangle2X3));
        var space2 = new SpacePe("Name", "Shortname", "Authors", "Description", "Goals", 5,
            new SpaceLayoutPe(new IElementPe[] {element2}, FloorPlanEnumPe.Rectangle2X3));
        var world = new WorldPe("Name", "Shortname", "Authors", "Language",
            "Description", "Goals", spaces: new List<SpacePe> { space1, space2 });
        
        var mockFileSystem = new MockFileSystem();

        var systemUnderTest = CreateTestableFileSaveHandler<WorldPe>(fileSystem:mockFileSystem);
        
        systemUnderTest.SaveToDisk(world, "foobar.txt");

        var actual = systemUnderTest.LoadFromDisk("foobar.txt");

        Assert.That(actual.Spaces[0].SpaceLayout.ContainedElements.First(),
            Is.Not.EqualTo(actual.Spaces[1].SpaceLayout.ContainedElements.First()));
    }
    
    private XmlFileHandler<T> CreateTestableFileSaveHandler<T>(ILogger<XmlFileHandler<T>>? logger = null, IFileSystem? fileSystem = null) where T : class
    {
        logger ??= Substitute.For<ILogger<XmlFileHandler<T>>>();
        return fileSystem == null ? new XmlFileHandler<T>(logger) : new XmlFileHandler<T>(logger, fileSystem);
    }

}