using System.IO.Abstractions.TestingHelpers;
using Generator.DSL;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using PersistEntities;
using PersistEntities.LearningContent;

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
        
        var inboundObject1 = new LearningSpacePe("1", "", "",
            1, null, positionX: 0, positionY: 0, inBoundObjects: null, outBoundObjects: null);
        var inboundObject2 = new LearningSpacePe("2", "", "",
            1, null, positionX: 0, positionY: 0, inBoundObjects: null, outBoundObjects: null);
        var inboundObject3 = new LearningSpacePe("3", "", "",
            1, null, positionX: 0, positionY: 0, inBoundObjects: null, outBoundObjects: null);
        var inboundObject4 = new LearningSpacePe("4", "", "",
            1, null, positionX: 0, positionY: 0, inBoundObjects: null, outBoundObjects: null);
        var inboundObject5 = new LearningSpacePe("5", "", "",
            1, null, positionX: 0, positionY: 0, inBoundObjects: null, outBoundObjects: null);
       
        var listLearningSpaces = new List<LearningSpacePe>
        {
            inboundObject1,
            inboundObject2,
            inboundObject3,
            inboundObject4,
            inboundObject5
        };

        int incrementId = 1;
        foreach (var space in listLearningSpaces)
        {
            systemUnderTest.DictionarySpaceIdToGuid.Add(incrementId, space.Id);
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
    public void CreateDSL_SearchDuplicateLearningElementNames_DuplicatesFoundAndNamesChanged()
    {
        //Arrange
        var mockElement1 = new LearningElementPe("Same Name Element", null, 
            "", "",  LearningElementDifficultyEnumPe.Easy);
        var mockElement2 = new LearningElementPe("Another Element", null, 
            "", "",  LearningElementDifficultyEnumPe.Easy);
        var mockElement3 = new LearningElementPe("Same Name Element", null, 
            "", "",  LearningElementDifficultyEnumPe.Easy);
        var mockElement4 = new LearningElementPe("Same Name Element", null, 
            "", "",  LearningElementDifficultyEnumPe.Easy);
        var mockElement5 = new LearningElementPe("Same Name Element", null, 
            "", "",  LearningElementDifficultyEnumPe.Easy);

        var mockLearningElements1 = new Dictionary<int, ILearningElementPe>()
        {
            {
                0,
                mockElement1
            },
            {
                1,
                mockElement2
            }
        };
        var mockLearningSpaceLayout1 = new LearningSpaceLayoutPe(mockLearningElements1, FloorPlanEnumPe.Rectangle2X3);
        var mockLearningElements2 = new Dictionary<int, ILearningElementPe>()
        {
            {
                0,
                mockElement3
            }
        };
        var mockLearningSpaceLayout2 = new LearningSpaceLayoutPe(mockLearningElements2, FloorPlanEnumPe.Rectangle2X3);
        var mockLearningElements3 = new Dictionary<int, ILearningElementPe>()
        {
            {
                0,
                mockElement4
            },
            {
                1,
                mockElement5
            }
        };
        var mockLearningSpaceLayout3 = new LearningSpaceLayoutPe(mockLearningElements3, FloorPlanEnumPe.Rectangle2X3);

        var mockSpace1 = new LearningSpacePe("Space1", "", "", 1,
            mockLearningSpaceLayout1);
        var mockSpace2 = new LearningSpacePe("Space2", "", "", 1,
            mockLearningSpaceLayout2);
        var mockSpace3 = new LearningSpacePe("Space3", "", "", 1,
            mockLearningSpaceLayout3);
        
        
        var mockSpaces = new List<LearningSpacePe> {mockSpace1, mockSpace2, mockSpace3};
        
        var mockFileSystem = new MockFileSystem();
        var mockLogger = Substitute.For<ILogger<CreateDsl>>();

        //Act
        var systemUnderTest = new CreateDsl(mockFileSystem, mockLogger);
        var learningSpaceList = systemUnderTest.SearchDuplicateLearningElementNames(mockSpaces);

        //Assert
        Assert.Multiple(()=>{ 
            Assert.That(mockElement1.Name, Is.EqualTo("Same Name Element(1)"));
            Assert.That(mockElement2.Name, Is.EqualTo("Another Element"));
            Assert.That(mockElement3.Name, Is.EqualTo("Same Name Element(2)"));
            Assert.That(mockElement4.Name, Is.EqualTo("Same Name Element(3)"));
            Assert.That(mockElement5.Name, Is.EqualTo("Same Name Element(4)"));
            Assert.That(learningSpaceList.Count, Is.EqualTo(3));
            Assert.That(learningSpaceList[0].LearningSpaceLayout.ContainedLearningElements.Count, Is.EqualTo(2));
            Assert.That(learningSpaceList[1].LearningSpaceLayout.ContainedLearningElements.Count, Is.EqualTo(1));
            Assert.That(learningSpaceList[2].LearningSpaceLayout.ContainedLearningElements.Count, Is.EqualTo(2));
        });
    }
    
    [Test]
    public void CreateDSL_WriteLearningWorld_DSLDocumentWritten()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        mockFileSystem.AddFile("/foo/bar.txt", new MockFileData("barbaz"));
        mockFileSystem.AddFile("/foo/foo.txt", new MockFileData("foo"));
        var curWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        mockFileSystem.AddDirectory(Path.Join(curWorkDir, "XMLFilesForExport"));
        mockFileSystem.AddFile(curWorkDir + "\\XMLFilesForExport\\LearningWorld.xml", new MockFileData(""));
        var mockLogger = Substitute.For<ILogger<CreateDsl>>();
        
        const string name = "asdf";
        const string shortname = "jkl;";
        const string authors = "ben and jerry";
        const string language = "german";
        const string description = "very cool element";
        const string goals = "learn very many things";
        const string savePath = "C:\\foo\\bar";
        
        var content1 = new FileContentPe("FileName", "h5p", "/foo/bar.txt");
        var content2 = new FileContentPe("FileName", "png", "/foo/bar.txt");
        var content3 = new LinkContentPe("LinkName", "http://www.google.com");
        var content4 = new FileContentPe("FileName", "txt", "/foo/foo.txt");
        var content5 = new FileContentPe("FileName", "pdf", "/foo/foo.txt");

        var ele1 = new LearningElementPe("a",content1, "pupup", "g", 
            LearningElementDifficultyEnumPe.Easy, workload: 17, points: 2, positionX: 23);
        var ele2 = new LearningElementPe("b",content2, "pupup", "g", 
            LearningElementDifficultyEnumPe.Easy, workload: 17, points: 2, positionX: 23);
        var ele3 = new LearningElementPe("c", content3,"pupup", "g", 
            LearningElementDifficultyEnumPe.Easy, workload: 17, points: 2, positionX: 23);
        var ele4 = new LearningElementPe("d",content4,"pupup", "g", 
            LearningElementDifficultyEnumPe.Easy, workload: 17, points: 2, positionX: 23);
        var ele5 = new LearningElementPe("e",content5,"pupup", "g", 
            LearningElementDifficultyEnumPe.Easy, workload: 17, points: 2, positionX: 23);
var topic1 = new TopicPe("topic1");
        var topic2 = new TopicPe("topic2");

        var space1 = new LearningSpacePe("ff", "ff", "ff", 5,
            null, positionX: 0, positionY: 0, inBoundObjects: new List<IObjectInPathWayPe>(),
            outBoundObjects: new List<IObjectInPathWayPe>(), topic1)
        {
            LearningSpaceLayout =
            {
                LearningElements = new Dictionary<int, ILearningElementPe>()
                {
                    {
                        0,
                        ele1
                    },
                    {
                        1,
                        ele2
                    },
                    {
                        2,
                        ele3
                    },
                    {
                        3,
                        ele4
                    },
                    {
                        4,
                        ele5
                    }
                }
            }
        };
        var space2 = new LearningSpacePe("ff2", "ff", "ff", 5, 
            null, positionX: 0, positionY: 0, inBoundObjects: new List<IObjectInPathWayPe>(), outBoundObjects: new List<IObjectInPathWayPe>(), topic1);
        var space3 = new LearningSpacePe("ff", "ff", "ff", 5, 
            null, positionX: 0, positionY: 0, inBoundObjects: new List<IObjectInPathWayPe>(), outBoundObjects: new List<IObjectInPathWayPe>(), topic2);
        var condition1 = new PathWayConditionPe(ConditionEnumPe.And, 0, 0, 
            new List<IObjectInPathWayPe>{space1, space2}, null);
        space1.OutBoundObjects = new List<IObjectInPathWayPe>() {condition1};
        space2.InBoundObjects = new List<IObjectInPathWayPe>() {condition1};
        space2.OutBoundObjects = new List<IObjectInPathWayPe>() {space3};
        space3.InBoundObjects = new List<IObjectInPathWayPe>() {space2};
        var learningSpaces = new List<LearningSpacePe> { space1, space2, space3 };
        var topics = new List<TopicPe>() {topic1, topic2};
        

        var learningWorld = new LearningWorldPe(name, shortname, authors, language, description, goals, savePath,
             learningSpaces, topics:topics);

        var systemUnderTest = new CreateDsl(mockFileSystem, mockLogger);
        
        //Every Element except Content with "url" is added to the comparison list.
        var learningElementsSpace1 = new List<LearningElementPe> { ele1, ele2, ele4, ele5 };
        var learningElementsSpace2 = new List<LearningElementPe>();
        
        //Act
        systemUnderTest.WriteLearningWorld(learningWorld);
        
        //Assert
        var pathXmlFile = Path.Join(curWorkDir, "XMLFilesForExport", "DSL_Document.json");
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Uuid, Is.Not.Null);
            Assert.That(systemUnderTest.LearningWorldJson, Is.Not.Null);
        });
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.LearningWorldJson!.LmsElementIdentifier.Value, Is.EqualTo(learningWorld.Name));
            Assert.That(systemUnderTest.ElementsWithFileContent, Is.EquivalentTo(learningElementsSpace1));
            Assert.That(systemUnderTest.ListLearningSpaces, Is.EquivalentTo(learningSpaces));
            Assert.That(systemUnderTest.ListTopics, Is.EquivalentTo(topics));
            Assert.That(systemUnderTest.LearningWorldJson.Spaces[0].RequiredSpacesToEnter,
                Is.EqualTo(""));
            Assert.That(systemUnderTest.LearningWorldJson.Spaces[1].RequiredSpacesToEnter,
                Is.EqualTo("(1)^(2)"));
        });
        Assert.Multiple(() =>
        {
            Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True);
        });
    }
    
    [Test]
     public void CreateDSL_WriteLearningWorld_UnsupportedTypeExceptionThrown()
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
        const string savePath = "C:\\Users\\Ben\\Desktop\\test";
        
        var content1 = new FileContentPe("FileName", "mp3", "/foo/bar.txt");

        var ele1 = new LearningElementPe("a", content1, "pupup", "g",
            LearningElementDifficultyEnumPe.Easy, workload: 17, points: 2, positionX: 23);

        var space1 = new LearningSpacePe("ff", "ff", "ff", 5,
            null, positionX: 0, positionY: 0, inBoundObjects: new List<IObjectInPathWayPe>(),
            outBoundObjects: new List<IObjectInPathWayPe>())
        {
            LearningSpaceLayout =
            {
                LearningElements = new Dictionary<int, ILearningElementPe>
                {
                    {
                        0,
                        ele1
                    }
                }
            }
        };
        var learningSpaces = new List<LearningSpacePe> { space1 };

        var learningWorld = new LearningWorldPe(name, shortname, authors, language, description, goals, savePath, learningSpaces);

        var systemUnderTest = new CreateDsl(mockFileSystem, mockLogger);

        //Act
        try
        {
            systemUnderTest.WriteLearningWorld(learningWorld); 
            Assert.Fail("Learning Content Exception was not thrown");
        }
        catch (Exception e)
        {
            //Assert
            Assert.That(e.Message, Is.EqualTo("The given LearningContent Type is not supported - in CreateDsl."));
        }

    }
    
}