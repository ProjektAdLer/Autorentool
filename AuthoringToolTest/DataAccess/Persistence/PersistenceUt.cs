using System.Collections;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Reflection;
using AuthoringTool.DataAccess.Persistence;
using AuthoringTool.Entities;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.Persistence;

/// <summary>
/// Component tests to test whether Persistence roundtrip produces equal objects
/// </summary>
[TestFixture]
public class PersistenceUt
{
    private const string FilePath = "awesomefile.txt";

    [Test]
    public void Persistence_SaveAndLoadWorld_Stream_ObjectsAreEqual()
    {
        var world = new LearningWorld("Name", "Shortname", "Authors", "Language",
            "Description", "Goals");
        var space = new LearningSpace("Name", "Shortname", "Authors", "Description", "Goals");
        var element = new LearningElement("le", "la", "li", "le", "lu", "ll", "lll");
        world.LearningSpaces.Add(space);
        world.LearningElements.Add(element);
        
        using var stream = new MemoryStream();
        var saveHandler = CreateTestableFileSaveHandler<LearningWorld>();
        
        saveHandler.SaveToStream(world, stream);
        stream.Position = 0;
        var restoredWorld = saveHandler.LoadFromStream(stream);
        
        PropertyValuesAreEqual(restoredWorld, world);
    }
    
    [Test]
    public void Persistence_SaveAndLoadSpace_Stream_ObjectsAreEqual()
    {
        var space = new LearningSpace("Name", "Shortname", "Authors", "Description", "Goals");
        var element = new LearningElement("le", "la", "li", "le", "lu", "ll", "lll");
        space.LearningElements.Add(element);
        
        using var stream = new MemoryStream();
        var saveHandler = CreateTestableFileSaveHandler<LearningSpace>();
        
        saveHandler.SaveToStream(space, stream);
        stream.Position = 0;
        var restoredSpace = saveHandler.LoadFromStream(stream);
        
        PropertyValuesAreEqual(restoredSpace, space);
    }
    
    [Test]
    public void Persistence_SaveAndLoadElement_Stream_ObjectsAreEqual()
    {
        var element = new LearningElement("le", "la", "li", "le", "lu", "ll", "lll");
        
        using var stream = new MemoryStream();
        var saveHandler = CreateTestableFileSaveHandler<LearningElement>();
        
        saveHandler.SaveToStream(element, stream);
        stream.Position = 0;
        var restoredElement = saveHandler.LoadFromStream(stream);

        PropertyValuesAreEqual(restoredElement, element);
    }

    [Test]
    public void Persistence_SaveAndLoadWorld_File_ObjectsAreEqual()
    {
        var world = new LearningWorld("Name", "Shortname", "Authors", "Language",
            "Description", "Goals");
        var space = new LearningSpace("Name", "Shortname", "Authors", "Description", "Goals");
        var element = new LearningElement("le", "la", "li", "le", "lu", "ll", "lll");
        world.LearningSpaces.Add(space);
        world.LearningElements.Add(element);
        var mockFileSystem = new MockFileSystem();

        var saveHandler = CreateTestableFileSaveHandler<LearningWorld>(fileSystem:mockFileSystem);
        
        saveHandler.SaveToDisk(world, FilePath);
        var restoredWorld = saveHandler.LoadFromDisk(FilePath);
        
        PropertyValuesAreEqual(restoredWorld, world);
    }
    
    [Test]
    public void Persistence_SaveAndLoadSpace_File_ObjectsAreEqual()
    {
        var space = new LearningSpace("Name", "Shortname", "Authors", "Description", "Goals");
        var element = new LearningElement("le", "la", "li", "le", "lu", "ll", "lll");
        space.LearningElements.Add(element);
        var mockFileSystem = new MockFileSystem();
        
        var saveHandler = CreateTestableFileSaveHandler<LearningSpace>(fileSystem:mockFileSystem);
        
        saveHandler.SaveToDisk(space, FilePath);
        var restoredSpace = saveHandler.LoadFromDisk(FilePath);
        
        PropertyValuesAreEqual(restoredSpace, space);
    }
    
    [Test]
    public void Persistence_SaveAndLoadElement_File_ObjectsAreEqual()
    {
        var element = new LearningElement("le", "la", "li", "le", "lu", "ll", "lll");
        var mockFileSystem = new MockFileSystem();

        var saveHandler = CreateTestableFileSaveHandler<LearningElement>(fileSystem:mockFileSystem);
        
        saveHandler.SaveToDisk(element, FilePath);
        var restoredElement = saveHandler.LoadFromDisk(FilePath);

        PropertyValuesAreEqual(restoredElement, element);
    }
    
    private FileSaveHandler<T> CreateTestableFileSaveHandler<T>(ILogger<FileSaveHandler<T>>? logger = null, IFileSystem? fileSystem = null) where T : class
    {
        logger ??= Substitute.For<ILogger<FileSaveHandler<T>>>();
        return fileSystem == null ? new FileSaveHandler<T>(logger) : new FileSaveHandler<T>(logger, fileSystem);
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

            if (actualValue is IList actualList && expectedValue is IList expectedList)
                AssertListsAreEqual(property, actualList, expectedList);
            else if (!Equals(expectedValue, actualValue)) 
                Assert.Fail($"Property {property.DeclaringType?.Name}.{property.Name} does not match. Expected: {expectedValue} but was: {actualValue}");
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