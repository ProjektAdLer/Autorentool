using System.Collections;
using System.IO;
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
public class PersistenceCt
{
    [Test]
    public void Persistence_SaveAndLoadWorld_ObjectsAreEqual()
    {
        var world = new LearningWorld("Name", "Shortname", "Authors", "Language",
            "Description", "Goals");
        var space = new LearningSpace();
        var element = new LearningElement();
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
    public void Persistence_SaveAndLoadSpace_ObjectsAreEqual()
    {
        var space = new LearningSpace();
        var element = new LearningElement();
        space.LearningElements.Add(element);
        
        using var stream = new MemoryStream();
        var saveHandler = CreateTestableFileSaveHandler<LearningSpace>();
        
        saveHandler.SaveToStream(space, stream);
        stream.Position = 0;
        var restoredSpace = saveHandler.LoadFromStream(stream);
        
        PropertyValuesAreEqual(restoredSpace, space);
    }
    
    [Test]
    public void Persistence_SaveAndLoadElement_ObjectsAreEqual()
    {
        var element = new LearningElement();
        
        using var stream = new MemoryStream();
        var saveHandler = CreateTestableFileSaveHandler<LearningElement>();
        
        saveHandler.SaveToStream(element, stream);
        stream.Position = 0;
        var restoredElement = saveHandler.LoadFromStream(stream);

        PropertyValuesAreEqual(restoredElement, element);
    }

    private FileSaveHandler<T> CreateTestableFileSaveHandler<T>(ILogger<FileSaveHandler<T>>? logger = null) where T : class
    {
        logger ??= Substitute.For<ILogger<FileSaveHandler<T>>>();
        return new FileSaveHandler<T>(logger);
    }

    private void PropertyValuesAreEqual<T>(T actual, T expected) where T : class
    {
        var properties = expected.GetType().GetProperties();
        foreach (var property in properties)
        {
            var expectedValue = property.GetValue(expected, null);
            var actualValue = property.GetValue(actual, null);

            if (actualValue is IList list)
                AssertListsAreEqual(property, list, (IList)expectedValue);
            else if (!Equals(expectedValue, actualValue)) 
                Assert.Fail($"Property {property.DeclaringType.Name}.{property.Name} does not match. Expected: {expectedValue} but was: {actualValue}");
        }
    }
    private void AssertListsAreEqual(PropertyInfo property, IList actualList, IList expectedList)
    { 
        if (actualList.Count != expectedList.Count)
            Assert.Fail($"Property {property.PropertyType.Name}.{property.Name} does not match. Expected IList containing {expectedList.Count} elements but was IList containing {actualList.Count} elements");
 
        for (var i = 0; i < actualList.Count; i++)
            if (!actualList[i].GetType().IsValueType)
            {
                PropertyValuesAreEqual(actualList[i], expectedList[i]);
            }
            else if (!Equals(actualList[i], expectedList[i]))
                Assert.Fail("Property {0}.{1} does not match. Expected IList with element {1} equals to {2} but was IList with element {1} equals to {3}", property.PropertyType.Name, property.Name, expectedList[i], actualList[i]);
    }
}