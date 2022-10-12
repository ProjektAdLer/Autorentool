using System.Collections;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Reflection;
using System.Runtime.Serialization;
using DataAccess.Persistence;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using PersistEntities;

namespace DataAccessTest.Persistence;

/// <summary>
/// Component tests to test whether Persistence roundtrip produces equal objects
/// </summary>
[TestFixture]
public class PersistenceUt
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
        var saveHandler = CreateTestableFileSaveHandler<LearningWorldPe>();
        
        saveHandler.SaveToStream(world, stream);
        stream.Position = 0;
        var restoredWorld = saveHandler.LoadFromStream(stream);
        
        PropertyValuesAreEqual(restoredWorld, world);
    }
    
    [Test]
    public void Persistence_SaveAndLoadSpace_Stream_ObjectsAreEquivalent()
    {
        var space = new LearningSpacePe("Name", "Shortname", "Authors", "Description", "Goals", 5);
        var content = new LearningContentPe("a", "b", "");
        var element = new LearningElementPe("le", "la", content,"url","ll", "l" ,"lll", LearningElementDifficultyEnumPe.Easy);
        space.LearningElements.Add(element);
        
        using var stream = new MemoryStream();
        var saveHandler = CreateTestableFileSaveHandler<LearningSpacePe>();
        
        saveHandler.SaveToStream(space, stream);
        stream.Position = 0;
        var restoredSpace = saveHandler.LoadFromStream(stream);
        
        PropertyValuesAreEqual(restoredSpace, space);
    }
    
    [Test]
    public void Persistence_SaveAndLoadElement_Stream_ObjectsAreEquivalent()
    {
        var content = new LearningContentPe("a", "b", "");
        var element = new LearningElementPe("le", "la", content, "url","ll", "ll", "lll", LearningElementDifficultyEnumPe.Easy);
        
        using var stream = new MemoryStream();
        var saveHandler = CreateTestableFileSaveHandler<LearningElementPe>();
        
        saveHandler.SaveToStream(element, stream);
        stream.Position = 0;
        var restoredElement = saveHandler.LoadFromStream(stream);

        PropertyValuesAreEqual(restoredElement, element);
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
        world.LearningPathWays.Add(new LearningPathwayPe(space1, space2));
        var mockFileSystem = new MockFileSystem();

        var saveHandler = CreateTestableFileSaveHandler<LearningWorldPe>(fileSystem:mockFileSystem);
        
        saveHandler.SaveToDisk(world, FilePath);
        var restoredWorld = saveHandler.LoadFromDisk(FilePath);
        
        PropertyValuesAreEqual(restoredWorld, world);
    }
    
    [Test]
    public void Persistence_SaveAndLoadSpace_File_ObjectsAreEquivalent()
    {
        var space = new LearningSpacePe("Name", "Shortname", "Authors", "Description", "Goals", 5);
        var content = new LearningContentPe("a", "b", "");
        var element = new LearningElementPe("le", "la", content, "url","ll", "llll","lllll", LearningElementDifficultyEnumPe.Easy);
        space.LearningElements.Add(element);
        var mockFileSystem = new MockFileSystem();
        
        var saveHandler = CreateTestableFileSaveHandler<LearningSpacePe>(fileSystem:mockFileSystem);
        
        saveHandler.SaveToDisk(space, FilePath);
        var restoredSpace = saveHandler.LoadFromDisk(FilePath);
        
        PropertyValuesAreEqual(restoredSpace, space);
    }
    
    [Test]
    public void Persistence_SaveAndLoadElement_File_ObjectsAreEquivalent()
    {
        var content = new LearningContentPe("a", "b", "");
        var element = new LearningElementPe("le", "la", content, "url","ll", "llll","lllll", LearningElementDifficultyEnumPe.Easy);
        var mockFileSystem = new MockFileSystem();

        var saveHandler = CreateTestableFileSaveHandler<LearningElementPe>(fileSystem:mockFileSystem);
        
        saveHandler.SaveToDisk(element, FilePath);
        var restoredElement = saveHandler.LoadFromDisk(FilePath);

        PropertyValuesAreEqual(restoredElement, element);
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

        var saveHandler = CreateTestableFileSaveHandler<LearningWorldPe>(fileSystem:mockFileSystem);
        
        saveHandler.SaveToDisk(world, "foobar.txt");

        var actual = saveHandler.LoadFromDisk("foobar.txt");
        
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

        var saveHandler = CreateTestableFileSaveHandler<LearningWorldPe>(fileSystem:mockFileSystem);
        
        saveHandler.SaveToDisk(world, "foobar.txt");

        var actual = saveHandler.LoadFromDisk("foobar.txt");
        
        Assert.That(actual.LearningSpaces[0].LearningElements.First(), Is.Not.EqualTo(actual.LearningSpaces[1].LearningElements.First()));
    }
    
    private XmlFileHandler<T> CreateTestableFileSaveHandler<T>(ILogger<XmlFileHandler<T>>? logger = null, IFileSystem? fileSystem = null) where T : class
    {
        logger ??= Substitute.For<ILogger<XmlFileHandler<T>>>();
        return fileSystem == null ? new XmlFileHandler<T>(logger) : new XmlFileHandler<T>(logger, fileSystem);
    }

    private void PropertyValuesAreEqual<T>(T actual, T expected) where T : class?
    {
        if (expected == actual && actual == null)
        {
            return;
        }
        if (expected == null || actual == null && expected != actual)
            Assert.Fail($"expected {expected} != actual {actual}");
        var properties = expected!.GetType().GetProperties();
        foreach (var property in properties)
        {
            var expectedValue = property.GetValue(expected, null);
            var actualValue = property.GetValue(actual, null);
            //ignore extensiondata on deserialization if it is empty in expected but not in actual
            if (property.Name == "ExtensionData" && expectedValue == null && actualValue is ExtensionDataObject edo)
            {
                continue;
            }
            switch (actualValue)
            {
                case IList actualList when expectedValue is IList expectedList:
                    AssertListsAreEqual(property, actualList, expectedList);
                    break;
                case LearningContentPe actualContent when expectedValue is LearningContentPe expectedContent:
                    Assert.Multiple(() =>
                    {
                        Assert.That(actualContent.Name, Is.EqualTo(expectedContent.Name));
                        Assert.That(actualContent.Type, Is.EqualTo(expectedContent.Type));
                        Assert.That(actualContent.Filepath, Is.EqualTo(expectedContent.Filepath));
                    });
                    break;
                default:
                {
                    if (!Equals(expectedValue, actualValue)) 
                        Assert.Fail($"Property {property.DeclaringType?.Name}.{property.Name} does not match. Expected: {expectedValue} but was: {actualValue}");
                    break;
                }
            }
        }
    }
    private void AssertListsAreEqual(PropertyInfo property, IList actualList, IList expectedList)
    { 
        if (actualList.Count != expectedList.Count)
            Assert.Fail($"Property {property.PropertyType.Name}.{property.Name} does not match. Expected IList containing {expectedList.Count} elements but was IList containing {actualList.Count} elements");
 
        for (var i = 0; i < actualList.Count; i++)
            if (!actualList[i]!.GetType().IsValueType)
            {
                PropertyValuesAreEqual(actualList[i], expectedList[i]);
            }
            else if (!Equals(actualList[i], expectedList[i]))
                Assert.Fail("Property {0}.{1} does not match. Expected IList with element {1} equals to {2} but was IList with element {1} equals to {3}", property.PropertyType.Name, property.Name, expectedList[i], actualList[i]);
    }
}