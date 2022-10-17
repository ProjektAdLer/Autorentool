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
        var world = new LearningWorldPe("Name", "Shortname", "Authors", "Language",
            "Description", "Goals");
        var space = new LearningSpacePe("Name", "Shortname", "Authors", "Description", "Goals", 5);
        var content = new LearningContentPe("a", "b", "");
        var element = new LearningElementPe("le", "la", content, "url","lll", "llll","lllll", LearningElementDifficultyEnumPe.Easy);
        space.LearningElements.Add(element);
        world.LearningSpaces.Add(space);

        using var stream = new MemoryStream();
        var systemUnderTest = CreateTestableFileSaveHandler<LearningWorldPe>();
        
        systemUnderTest.SaveToStream(world, stream);
        stream.Position = 0;
        var restoredWorld = systemUnderTest.LoadFromStream(stream);
        
        restoredWorld.Should().BeEquivalentTo(world, options => options.IgnoringCyclicReferences());
    }
    
    [Test]
    public void Persistence_SaveAndLoadSpace_Stream_ObjectsAreEquivalent()
    {
        var space = new LearningSpacePe("Name", "Shortname", "Authors", "Description", "Goals", 5);
        var content = new LearningContentPe("a", "b", "");
        var element = new LearningElementPe("le", "la", content,"url","ll", "l" ,"lll", LearningElementDifficultyEnumPe.Easy);
        space.LearningElements.Add(element);
        
        using var stream = new MemoryStream();
        var systemUnderTest = CreateTestableFileSaveHandler<LearningSpacePe>();
        
        systemUnderTest.SaveToStream(space, stream);
        stream.Position = 0;
        var restoredSpace = systemUnderTest.LoadFromStream(stream);
        
        restoredSpace.Should().BeEquivalentTo(space);
    }
    
    [Test]
    public void Persistence_SaveAndLoadElement_Stream_ObjectsAreEquivalent()
    {
        var content = new LearningContentPe("a", "b", "");
        var element = new LearningElementPe("le", "la", content, "url","ll", "ll", "lll", LearningElementDifficultyEnumPe.Easy);
        
        using var stream = new MemoryStream();
        var systemUnderTest = CreateTestableFileSaveHandler<LearningElementPe>();
        
        systemUnderTest.SaveToStream(element, stream);
        stream.Position = 0;
        var restoredElement = systemUnderTest.LoadFromStream(stream);

        restoredElement.Should().BeEquivalentTo(element);
    }

    [Test]
    public void Persistence_SaveAndLoadWorld_File_ObjectsAreEquivalent()
    {
        var world = new LearningWorldPe("Name", "Shortname", "Authors", "Language",
            "Description", "Goals");
        var space1 = new LearningSpacePe("Name", "Shortname", "Authors", "Description", "Goals", 5);
        var space2 = new LearningSpacePe("Name", "Shortname", "Authors", "Description", "Goals", 5);
        var content = new LearningContentPe("a", "b", "");
        var element = new LearningElementPe("le", "la", content, "url","lll", "llll","lllll", LearningElementDifficultyEnumPe.Easy);
        space1.LearningElements.Add(element);
        world.LearningSpaces.Add(space1);
        world.LearningSpaces.Add(space2);
        space1.OutBoundSpaces.Add(space2);
        space2.InBoundSpaces.Add(space1);
        world.LearningPathways.Add(new LearningPathwayPe(space1, space2));
        var mockFileSystem = new MockFileSystem();

        var systemUnderTest = CreateTestableFileSaveHandler<LearningWorldPe>(fileSystem:mockFileSystem);
        
        systemUnderTest.SaveToDisk(world, FilePath);
        var restoredWorld = systemUnderTest.LoadFromDisk(FilePath);
        
        restoredWorld.Should().BeEquivalentTo(world, options => options.IgnoringCyclicReferences());
        Assert.That(restoredWorld.LearningSpaces[0].OutBoundSpaces, Does.Contain(restoredWorld.LearningSpaces[1]));
        Assert.That(restoredWorld.LearningSpaces[1].InBoundSpaces, Does.Contain(restoredWorld.LearningSpaces[0]));
    }
    
    [Test]
    public void Persistence_SaveAndLoadSpace_File_ObjectsAreEquivalent()
    {
        var space = new LearningSpacePe("Name", "Shortname", "Authors", "Description", "Goals", 5);
        var content = new LearningContentPe("a", "b", "");
        var element = new LearningElementPe("le", "la", content, "url","ll", "llll","lllll", LearningElementDifficultyEnumPe.Easy);
        space.LearningElements.Add(element);
        var mockFileSystem = new MockFileSystem();
        
        var systemUnderTest = CreateTestableFileSaveHandler<LearningSpacePe>(fileSystem:mockFileSystem);
        
        systemUnderTest.SaveToDisk(space, FilePath);
        var restoredSpace = systemUnderTest.LoadFromDisk(FilePath);
        
        restoredSpace.Should().BeEquivalentTo(space);
    }

    [Test]
    public void Persistence_SaveAndLoadSpace_File_WithAllElementTypes_ObjectsAreEquivalent()
    {
        var space = new LearningSpacePe("Name", "Shortname", "Authors", "Description", "Goals", 5);
        space.LearningElements.AddRange(GetAllLearningElementTypes());
        
        var systemUnderTest = CreateTestableFileSaveHandler<LearningSpacePe>();
        
        systemUnderTest.SaveToDisk(space, FilePath);
        var restoredSpace = systemUnderTest.LoadFromDisk(FilePath);

        restoredSpace.Should().BeEquivalentTo(space);
    }
    
    [Test]
    public void Persistence_SaveAndLoadElement_File_ObjectsAreEquivalent()
    {
        var content = new LearningContentPe("a", "b", "");
        var element = new LearningElementPe("le", "la", content, "url","ll", "llll","lllll", LearningElementDifficultyEnumPe.Easy);
        var mockFileSystem = new MockFileSystem();

        var systemUnderTest = CreateTestableFileSaveHandler<LearningElementPe>(fileSystem:mockFileSystem);
        
        systemUnderTest.SaveToDisk(element, FilePath);
        var restoredElement = systemUnderTest.LoadFromDisk(FilePath);

        restoredElement.Should().BeEquivalentTo(element);
    }

    [Test]
    [TestCaseSource(nameof(GetAllLearningElementTypes))]
    public void Persistence_SaveAndLoadElement_File_SerializationWorksForEveryType(LearningElementPe lep)
    {
        var systemUnderTest = CreateTestableFileSaveHandler<LearningElementPe>();
        
        systemUnderTest.SaveToDisk(lep, "element.aef");
        var restoredLep = systemUnderTest.LoadFromDisk("element.aef");

        restoredLep.Should().BeEquivalentTo(lep);
    }

    static IEnumerable<LearningElementPe> GetAllLearningElementTypes()
    {
        var content = new LearningContentPe("a", "b", "");
        yield return new H5PActivationElementPe("h5pAct", "asdf", content, "", "me :)", "description", "a goal",
            LearningElementDifficultyEnumPe.Easy, 123, 42, 0, 0);
        yield return new H5PInteractionElementPe("h5pInt", "blabla", content, "", "me :)", "description", "a goal",
            LearningElementDifficultyEnumPe.Medium, 123, 42, 0, 0);
        yield return new H5PTestElementPe("h5pTest", "bababubu", content, "", "me :)", "description", "a goal",
            LearningElementDifficultyEnumPe.Hard, 123, 42, 0, 0);
        yield return new ImageTransferElementPe("imgTrans", "bababubu", content, "", "me :)", "description", "a goal",
            LearningElementDifficultyEnumPe.Hard, 123, 42, 0, 0);
        yield return new PdfTransferElementPe("pdfTrans", "bababubu", content, "", "me :)", "description", "a goal",
            LearningElementDifficultyEnumPe.Hard, 123, 42, 0, 0);
        yield return new TextTransferElementPe("txtTrans", "bababubu", content, "", "me :)", "description", "a goal",
            LearningElementDifficultyEnumPe.Hard, 123, 42, 0, 0);
        yield return new VideoActivationElementPe("vidAct", "bababubu", content, "", "me :)", "description", "a goal",
            LearningElementDifficultyEnumPe.Hard, 123, 42, 0, 0);
        yield return new VideoTransferElementPe("vidTrans", "bababubu", content, "", "me :)", "description", "a goal",
            LearningElementDifficultyEnumPe.Hard, 123, 42, 0, 0);
    }

    [Test]
    public void SaveAndLoadWorld_WithExactSameElementInTwoSpaces_ElementIsEqualObject()
    {
        var content = new LearningContentPe("a", "b", "");
        var element = new LearningElementPe("le", "la", content,"",  "ll", "llll", "lllll",
            LearningElementDifficultyEnumPe.Easy);
        var space1 = new LearningSpacePe("Name", "Shortname", "Authors", "Description", "Goals", 5,
            new List<LearningElementPe> { element });
        var space2 = new LearningSpacePe("Name", "Shortname", "Authors", "Description", "Goals", 5,
            new List<LearningElementPe> { element });
        var world = new LearningWorldPe("Name", "Shortname", "Authors", "Language",
            "Description", "Goals", learningSpaces: new List<LearningSpacePe> { space1, space2 });
        
        var mockFileSystem = new MockFileSystem();

        var systemUnderTest = CreateTestableFileSaveHandler<LearningWorldPe>(fileSystem:mockFileSystem);
        
        systemUnderTest.SaveToDisk(world, "foobar.txt");

        var actual = systemUnderTest.LoadFromDisk("foobar.txt");
        
        Assert.That(actual.LearningSpaces[0].LearningElements.First(), Is.EqualTo(actual.LearningSpaces[1].LearningElements.First()));
    }
    
    [Test]
    public void SaveAndLoadWorld_WithTwoEquivalentElementsInTwoSpaces_ElementIsNotEqualObject()
    {
        var content = new LearningContentPe("a", "b", "");
        var element1 = new LearningElementPe("le", "la", content, "", "ll", "llll", "lllll",
            LearningElementDifficultyEnumPe.Easy);
        var element2 = new LearningElementPe("le", "la", content, "","ll", "llll", "lllll",
            LearningElementDifficultyEnumPe.Easy);
        var space1 = new LearningSpacePe("Name", "Shortname", "Authors", "Description", "Goals", 5,
            new List<LearningElementPe> { element1 });
        var space2 = new LearningSpacePe("Name", "Shortname", "Authors", "Description", "Goals", 5,
            new List<LearningElementPe> { element2 });
        var world = new LearningWorldPe("Name", "Shortname", "Authors", "Language",
            "Description", "Goals", learningSpaces: new List<LearningSpacePe> { space1, space2 });
        
        var mockFileSystem = new MockFileSystem();

        var systemUnderTest = CreateTestableFileSaveHandler<LearningWorldPe>(fileSystem:mockFileSystem);
        
        systemUnderTest.SaveToDisk(world, "foobar.txt");

        var actual = systemUnderTest.LoadFromDisk("foobar.txt");
        
        Assert.That(actual.LearningSpaces[0].LearningElements.First(), Is.Not.EqualTo(actual.LearningSpaces[1].LearningElements.First()));
    }
    
    private XmlFileHandler<T> CreateTestableFileSaveHandler<T>(ILogger<XmlFileHandler<T>>? logger = null, IFileSystem? fileSystem = null) where T : class
    {
        logger ??= Substitute.For<ILogger<XmlFileHandler<T>>>();
        return fileSystem == null ? new XmlFileHandler<T>(logger) : new XmlFileHandler<T>(logger, fileSystem);
    }

}