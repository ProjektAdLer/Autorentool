using System.IO.Abstractions.TestingHelpers;
using Generator.DSL;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using PersistEntities;

namespace GeneratorTest.DSL;

[TestFixture]
public class CreateDslUt
{
    [Test]
    public void CreateDSL_DefineLogicalExpression_RequirementDefined()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        var mockLogger = Substitute.For<ILogger<CreateDsl>>();
        var systemUnderTest = new CreateDsl(mockFileSystem, mockLogger);
        
        var inboundObject1 = new SpacePe("1", "1", "dimi", "", "",
            1, null, 0, 0, null, null);
        var inboundObject2 = new SpacePe("2", "1", "dimi", "", "",
            1, null, 0, 0, null, null);
        var inboundObject3 = new SpacePe("3", "1", "dimi", "", "",
            1, null, 0, 0, null, null);
        var inboundObject4 = new SpacePe("4", "1", "dimi", "", "",
            1, null, 0, 0, null, null);
        var inboundObject5 = new SpacePe("5", "1", "dimi", "", "",
            1, null, 0, 0, null, null);
       
        var listSpaces = new List<SpacePe>
        {
            inboundObject1,
            inboundObject2,
            inboundObject3,
            inboundObject4,
            inboundObject5
        };

        int incrementId = 1;
        foreach (var space in listSpaces)
        {
            systemUnderTest.IdDictionary.Add(incrementId, space.Id);
            incrementId++;
        }
        
        var inboundObjectList1 = new List<IObjectInPathWayPe>
        {
            inboundObject3,
            inboundObject4
        };
        var inboundObject6 = new PathWayConditionPe(ConditionEnumPe.And, 0, 0, inboundObjectList1, 
            null);
        
        var inboundObjectList2 = new List<IObjectInPathWayPe>
        {
            inboundObject5
        };
        var inboundObject7 = new PathWayConditionPe(ConditionEnumPe.Or, 0, 0, inboundObjectList2, 
            null);
        
        
        var inboundObjects = new List<IObjectInPathWayPe>
        {
            //Space = 1
            inboundObject1,
            
            //Space = 2
            inboundObject2,
            
            //List (3 & 4)
            inboundObject6,
            
            //List (5)
            inboundObject7
        };
        
        var pathwayConditionPe = new PathWayConditionPe(ConditionEnumPe.Or, 0, 0, inboundObjects, null);
        
        //Act
        var stringUnderTest = systemUnderTest.DefineLogicalExpression(pathwayConditionPe);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(stringUnderTest, Is.EqualTo("(1)v(2)v((3)^(4))v(5)"));
        });

    }
    

    [Test]
    public void CreateDSL_SearchDuplicateElementNames_DuplicatesFoundAndNamesChanged()
    {
        //Arrange
        var mockElement1 = new ElementPe("Same Name Element", "el", null, "", 
            "", "", "", PersistEntities.ElementDifficultyEnumPe.Easy);
        var mockElement2 = new ElementPe("Another Element", "el", null, "", 
            "", "", "", PersistEntities.ElementDifficultyEnumPe.Easy);
        var mockElement3 = new ElementPe("Same Name Element", "el", null, "", 
            "", "", "", PersistEntities.ElementDifficultyEnumPe.Easy);
        var mockElement4 = new ElementPe("Same Name Element", "el", null, "", 
            "", "", "", PersistEntities.ElementDifficultyEnumPe.Easy);
        var mockElement5 = new ElementPe("Same Name Element", "el", null, "", 
            "", "", "", PersistEntities.ElementDifficultyEnumPe.Easy);
        
        var mockElements1 = new IElementPe?[] {mockElement1, mockElement2};
        var mockSpaceLayout1 = new SpaceLayoutPe(mockElements1, FloorPlanEnumPe.Rectangle2X3);
        var mockElements2 = new IElementPe?[] {mockElement3};
        var mockSpaceLayout2 = new SpaceLayoutPe(mockElements2, FloorPlanEnumPe.Rectangle2X3);
        var mockElements3 = new IElementPe?[] {mockElement4, mockElement5};
        var mockSpaceLayout3 = new SpaceLayoutPe(mockElements3, FloorPlanEnumPe.Rectangle2X3);

        var mockSpace1 = new SpacePe("Space1", "sp", null, "", "", 1,
            mockSpaceLayout1);
        var mockSpace2 = new SpacePe("Space2", "sp", null, "", "", 1,
            mockSpaceLayout2);
        var mockSpace3 = new SpacePe("Space3", "sp", null, "", "", 1,
            mockSpaceLayout3);
        
        
        var mockSpaces = new List<SpacePe> {mockSpace1, mockSpace2, mockSpace3};
        
        var mockFileSystem = new MockFileSystem();
        var mockLogger = Substitute.For<ILogger<CreateDsl>>();

        //Act
        var systemUnderTest = new CreateDsl(mockFileSystem, mockLogger);
        var spaceList = systemUnderTest.SearchDuplicateElementNames(mockSpaces);

        //Assert
        Assert.Multiple(()=>{ 
            Assert.That(mockElement1.Name, Is.EqualTo("Same Name Element(1)"));
            Assert.That(mockElement2.Name, Is.EqualTo("Another Element"));
            Assert.That(mockElement3.Name, Is.EqualTo("Same Name Element(2)"));
            Assert.That(mockElement4.Name, Is.EqualTo("Same Name Element(3)"));
            Assert.That(mockElement5.Name, Is.EqualTo("Same Name Element(4)"));
            Assert.That(spaceList.Count, Is.EqualTo(3));
            Assert.That(spaceList[0].SpaceLayout.ContainedElements.Count, Is.EqualTo(2));
            Assert.That(spaceList[1].SpaceLayout.ContainedElements.Count, Is.EqualTo(1));
            Assert.That(spaceList[2].SpaceLayout.ContainedElements.Count, Is.EqualTo(2));
        });
    }
    
    [Test]
    public void CreateDSL_WriteWorld_DSLDocumentWritten()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        mockFileSystem.AddFile("/foo/bar.txt", new MockFileData("barbaz"));
        mockFileSystem.AddFile("/foo/foo.txt", new MockFileData("foo"));
        var curWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        mockFileSystem.AddDirectory(Path.Join(curWorkDir, "XMLFilesForExport"));
        mockFileSystem.AddFile(curWorkDir + "\\XMLFilesForExport\\World.xml", new MockFileData(""));
        var mockLogger = Substitute.For<ILogger<CreateDsl>>();
        
        const string name = "asdf";
        const string shortname = "jkl;";
        const string authors = "ben and jerry";
        const string language = "german";
        const string description = "very cool element";
        const string goals = "learn very many things";
        
        var content1 = new ContentPe("FileName", "h5p", "/foo/bar.txt");
        var content2 = new ContentPe("FileName", "png", "/foo/bar.txt");
        var content3 = new ContentPe("FileName", "url", "/foo/bar.txt");
        var content4 = new ContentPe("FileName", "txt", "/foo/foo.txt");
        var content5 = new ContentPe("FileName", "pdf", "/foo/foo.txt");

        var ele1 = new ElementPe("a", "b",content1, "", "pupup", "g","h", 
            ElementDifficultyEnumPe.Easy, 17, 2, 23);
        var ele2 = new ElementPe("b", "b",content2, "", "pupup", "g","h", 
            ElementDifficultyEnumPe.Easy, 17, 2, 23);
        var ele3 = new ElementPe("c", "b", content3, "","pupup", "g","h", 
            ElementDifficultyEnumPe.Easy, 17, 2, 23);
        var ele4 = new ElementPe("d", "b",content4, "","pupup", "g","h", 
            ElementDifficultyEnumPe.Easy, 17, 2, 23);
        var ele5 = new ElementPe("e", "b",content5, "","pupup", "g","h", 
            ElementDifficultyEnumPe.Easy, 17, 2, 23);
        

        var space1 = new SpacePe("ff", "ff", "ff", "ff", "ff", 5, 
            null, 0, 0, new List<IObjectInPathWayPe>(), 
            new List<IObjectInPathWayPe>());
        space1.SpaceLayout.Elements = new IElementPe[] {ele1, ele2, ele3, ele4, ele5};
        var space2 = new SpacePe("ff2", "ff", "ff", "ff", "ff", 5, 
            null, 0, 0, new List<IObjectInPathWayPe>(), new List<IObjectInPathWayPe>());
        var space3 = new SpacePe("ff", "ff", "ff", "ff", "ff", 5, 
            null, 0, 0, new List<IObjectInPathWayPe>(), new List<IObjectInPathWayPe>());
        var condition1 = new PathWayConditionPe(ConditionEnumPe.And, 0, 0, 
            new List<IObjectInPathWayPe>{space1, space2}, null);
        space1.OutBoundObjects = new List<IObjectInPathWayPe>() {condition1};
        space2.InBoundObjects = new List<IObjectInPathWayPe>() {condition1};
        space2.OutBoundObjects = new List<IObjectInPathWayPe>() {space3};
        space3.InBoundObjects = new List<IObjectInPathWayPe>() {space2};
        var spaces = new List<SpacePe> { space1, space2, space3 };
        

        var world = new WorldPe(name, shortname, authors, language, description, goals,
             spaces);

        var systemUnderTest = new CreateDsl(mockFileSystem, mockLogger);
        
        //Every Element except Content with "url" is added to the comparison list.
        var elementsSpace1 = new List<ElementPe> { ele1, ele2, ele4, ele5 };
        var elementsSpace2 = new List<ElementPe>();
        
        var elementsForComparison = new List<List<ElementPe>> {elementsSpace1, elementsSpace2};
        

        //Act
        systemUnderTest.WriteWorld(world);
        
        //Assert
        var pathXmlFile = Path.Join(curWorkDir, "XMLFilesForExport", "DSL_Document.json");
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Uuid, Is.Not.Null);
            Assert.That(systemUnderTest.WorldJson, Is.Not.Null);
        });
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.WorldJson!.Identifier.Value, Is.EqualTo(name));
            Assert.That(systemUnderTest.ListElementsWithContents, Is.EquivalentTo(elementsSpace1));
            Assert.That(systemUnderTest.ListSpaces, Is.EquivalentTo(spaces));
            Assert.That(systemUnderTest.WorldJson.Spaces[0].Requirements,
                Is.EqualTo(""));
            Assert.That(systemUnderTest.WorldJson.Spaces[1].Requirements,
                Is.EqualTo("(1)^(2)"));
        });
        Assert.Multiple(() =>
        {
            Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True);
        });
    }
    
    [Test]
     public void CreateDSL_WriteWorld_UnsupportedTypeExceptionThrown()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        mockFileSystem.AddFile("/foo/foo.txt", new MockFileData("foo"));
        var mockLogger = Substitute.For<ILogger<CreateDsl>>();
        
        const string name = "asdf";
        const string shortname = "jkl;";
        const string authors = "ben and jerry";
        const string language = "german";
        const string description = "very cool element";
        const string goals = "learn very many things";
        
        var content1 = new ContentPe("FileName", "mp3", "/foo/bar.txt");

        var ele1 = new ElementPe("a", "b",content1, "", "pupup", "g","h", 
            ElementDifficultyEnumPe.Easy, 17, 2, 23);

        var space1 = new SpacePe("ff", "ff", "ff", "ff", "ff", 5, 
            null, 0, 0, new List<IObjectInPathWayPe>(), 
            new List<IObjectInPathWayPe>());
        space1.SpaceLayout.Elements = new IElementPe[] {ele1};
        var spaces = new List<SpacePe> { space1 };

        var world = new WorldPe(name, shortname, authors, language, description, goals,
             spaces);

        var systemUnderTest = new CreateDsl(mockFileSystem, mockLogger);

        //Act
        try
        {
            systemUnderTest.WriteWorld(world); 
            Assert.Fail("Content Exception was not thrown");
        }
        catch (Exception e)
        {
            //Assert
            Assert.That(e.Message, Is.EqualTo("The given Content Type is not supported - in CreateDsl."));
        }

    }
    
}