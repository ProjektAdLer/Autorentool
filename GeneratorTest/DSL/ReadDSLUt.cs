using System.IO.Abstractions.TestingHelpers;
using Generator.DSL;
using NUnit.Framework;

namespace GeneratorTest.DSL;

[TestFixture]
public class ReadDslUt
{
    
    [Test]
    public void ReadDSL_ReadWorld_DSLDocumentRead()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();

        var identifierWorldJson = new IdentifierJson("name", "World");

        var identifierSpaceJson1 = new IdentifierJson("name", "Space_1");
  
        var identifierSpaceJson2 = new IdentifierJson("name", "Space_2");
    
        var identifierElementJson1 = new IdentifierJson("name", "Element_1");
   
        var identifierElementJson2 = new IdentifierJson("name", "DSL Dokument");

        var elementValueJson1 = new ElementValueJson("points", "0");

        var elementValueJson2 = new ElementValueJson("points", "0");

        var elementValueList1 = new List<ElementValueJson>(){elementValueJson1};
        var elementValueList2 = new List<ElementValueJson>(){elementValueJson2};
        
        var worldContentJson = new List<int>(){1,2};
        
        var topicsJson = new TopicJson();
        var topicsList = new List<TopicJson>(){topicsJson};

        var spacesJson1 = new SpaceJson(1, identifierSpaceJson1, 
            new List<int> {1, 2}, 0, 0);
 
        var spacesJson2 = new SpaceJson(2, identifierSpaceJson2, 
            new List<int>(), 0, 0);

        var spacesList = new List<SpaceJson>(){spacesJson1, spacesJson2};

        var elementJson1 = new ElementJson(1,
            identifierElementJson1, "", "", "h5p",1, elementValueList1);
        
        var elementJson2 = new ElementJson(2,
            identifierElementJson2, "", "", "json",1, elementValueList2);
        
        var elementJson3 = new ElementJson(3,
            identifierElementJson2, "", "", "url",1, elementValueList2);
        
        var elementJson4 = new ElementJson(4,
            identifierElementJson2, "", "", "label",1, elementValueList2);

        var elementList = new List<ElementJson>(){elementJson1, elementJson2, elementJson3, elementJson4};
        
        var worldJson = new WorldJson("uuid", identifierWorldJson, worldContentJson, 
            topicsList, spacesList, elementList, "World Description", "World Goals");

        var rootJson = new DocumentRootJson(worldJson);
        

        //Act
        var systemUnderTest = new ReadDsl(mockFileSystem);
        systemUnderTest.ReadWorld("dslPath", rootJson);

        var listSpace = systemUnderTest.GetSectionList();
        var resourceList = systemUnderTest.GetResourceList();

        //Assert
        var getWorldJson = systemUnderTest.GetWorld();
        var getH5PElementsList = systemUnderTest.GetH5PElementsList();
        var getSpacesAndElementsList = systemUnderTest.GetSpacesAndElementsOrderedList();
        var getLabelsList = systemUnderTest.GetLabelsList();
        var getUrlList = systemUnderTest.GetUrlList();

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.ListH5PElements, Is.Not.Null);
            Assert.That(worldJson, Is.Not.Null);
            Assert.That(systemUnderTest.ListH5PElements, Has.Count.EqualTo(1));
            Assert.That(getWorldJson.Elements, Is.Not.Null);
            Assert.That(getWorldJson.Spaces, Is.Not.Null);
            
            Assert.That(getWorldJson.Elements, Has.Count.EqualTo(elementList.Count));
            Assert.That(getWorldJson.Spaces, Has.Count.EqualTo(spacesList.Count));
            Assert.That(resourceList, Has.Count.EqualTo(1));
            Assert.That(getH5PElementsList, Has.Count.EqualTo(1));
            
            //Spaces + Elements + World Description & Goals (As they are created as Labels in Moodle)
            Assert.That(getSpacesAndElementsList, Has.Count.EqualTo(5));
            Assert.That(getLabelsList, Has.Count.EqualTo(1));
            
            //Because there are no Topics in the AuthoringTool, every  space is added to Topic 0
            Assert.That(listSpace.Count, Is.EqualTo(1));
            
            //Currently all Elements with Element-Type "mp4" are added to the list of Urls
            Assert.That(getUrlList, Has.Count.EqualTo(1));
        });
    }

    [Test]
    public void ReadDSL_ReadWorld_DSLDocumentReadWithEmptyWorldDescriptionAndGoals()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();

        var identifierWorldJson = new IdentifierJson("name", "World");
        var topicsJson = new TopicJson();
        var topicsList = new List<TopicJson>(){topicsJson};
        
     
        var worldJson = new WorldJson("uuid", identifierWorldJson, 
            new List<int>(), topicsList, new List<SpaceJson>(), 
            new List<ElementJson>(), "", "");

        var rootJson = new DocumentRootJson(worldJson);
        

        //Act
        var systemUnderTest = new ReadDsl(mockFileSystem);
        systemUnderTest.ReadWorld("dslPath", rootJson);

        var listSpace = systemUnderTest.GetSectionList();

        //Assert
        var getWorldJson = systemUnderTest.GetWorld();
        var getSpacesAndElementsList = systemUnderTest.GetSpacesAndElementsOrderedList();

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.ListH5PElements, Is.Not.Null);
            Assert.That(worldJson, Is.Not.Null);
            Assert.That(getWorldJson.Elements, Is.Not.Null);
            Assert.That(getWorldJson.Spaces, Is.Not.Null);

            //If Description and Goals are empty, they are not added to the list of Labels
            //But 
            Assert.That(getSpacesAndElementsList, Has.Count.EqualTo(0));

            //There is always at least 1 Section (Topic 0) in a Moodle Course
            Assert.That(listSpace.Count, Is.EqualTo(1));

        });
    }

    
}