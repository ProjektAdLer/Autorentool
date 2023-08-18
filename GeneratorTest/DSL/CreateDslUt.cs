using System.IO.Abstractions.TestingHelpers;
using Generator.DSL;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using PersistEntities;
using Shared;
using TestHelpers;

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

        var inboundObject1 = PersistEntityProvider.GetLearningSpace("1");
        var inboundObject2 = PersistEntityProvider.GetLearningSpace("2");
        var inboundObject3 = PersistEntityProvider.GetLearningSpace("3");
        var inboundObject4 = PersistEntityProvider.GetLearningSpace("4");
        var inboundObject5 = PersistEntityProvider.GetLearningSpace("5");

        var listLearningSpaces = new List<LearningSpacePe>
        {
            inboundObject1,
            inboundObject2,
            inboundObject3,
            inboundObject4,
            inboundObject5
        };

        var incrementId = 1;
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
        var inboundObject6 = new PathWayConditionPe(ConditionEnum.And, 0, 0, inboundObjectList1);

        var inboundObjectList2 = new List<IObjectInPathWayPe>
        {
            inboundObject5
        };
        var inboundObject7 = new PathWayConditionPe(ConditionEnum.Or, 0, 0, inboundObjectList2);

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

        var pathwayConditionPe = new PathWayConditionPe(ConditionEnum.Or, 0, 0, inboundObjects);

        //Act
        var stringUnderTest = systemUnderTest.DefineLogicalExpression(pathwayConditionPe);

        //Assert
        Assert.Multiple(() => { Assert.That(stringUnderTest, Is.EqualTo("(1)v(2)v((3)^(4))v(5)")); });
    }


    [Test]
    public void CreateDSL_SearchDuplicateLearningElementNames_DuplicatesFoundAndNamesChanged()
    {
        //Arrange
        var mockElement1 = PersistEntityProvider.GetLearningElement(name: "Same Name Element");
        var mockElement2 = PersistEntityProvider.GetLearningElement(name: "Another Element");
        var mockElement3 = PersistEntityProvider.GetLearningElement(name: "Same Name Element");
        var mockElement4 = PersistEntityProvider.GetLearningElement(name: "Same Name Element");
        var mockElement5 = PersistEntityProvider.GetLearningElement(name: "Same Name Element");

        var mockLearningElements1 = new Dictionary<int, ILearningElementPe>
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
        var mockLearningSpaceLayout1 =
            PersistEntityProvider.GetLearningSpaceLayout(learningElements: mockLearningElements1,
                floorPlan: FloorPlanEnum.R_20X30_8L);
        var mockLearningElements2 = new Dictionary<int, ILearningElementPe>
        {
            {
                0,
                mockElement3
            }
        };
        var mockLearningSpaceLayout2 =
            PersistEntityProvider.GetLearningSpaceLayout(learningElements: mockLearningElements2,
                floorPlan: FloorPlanEnum.R_20X30_8L);
        var mockLearningElements3 = new Dictionary<int, ILearningElementPe>
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
        var mockLearningSpaceLayout3 = new LearningSpaceLayoutPe(mockLearningElements3, FloorPlanEnum.R_20X30_8L);

        var mockSpace1 =
            PersistEntityProvider.GetLearningSpace(name: "Space1", learningSpaceLayout: mockLearningSpaceLayout1);
        var mockSpace2 =
            PersistEntityProvider.GetLearningSpace(name: "Space2", learningSpaceLayout: mockLearningSpaceLayout2);
        var mockSpace3 =
            PersistEntityProvider.GetLearningSpace(name: "Space3", learningSpaceLayout: mockLearningSpaceLayout3);


        var mockSpaces = new List<LearningSpacePe> { mockSpace1, mockSpace2, mockSpace3 };

        var mockFileSystem = new MockFileSystem();
        var mockLogger = Substitute.For<ILogger<CreateDsl>>();

        //Act
        var systemUnderTest = new CreateDsl(mockFileSystem, mockLogger);
        var learningSpaceList = systemUnderTest.SearchDuplicateLearningElementNames(mockSpaces);

        //Assert
        Assert.Multiple(() =>
        {
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
        const string evaluationLink = "http://www.projekt-alder.eu";
        const string savePath = "C:\\foo\\bar";

        var content1 = PersistEntityProvider.GetFileContent(name: "FileName", type: "h5p",
            filepath: "/foo/bar.txt");
        var content2 = PersistEntityProvider.GetFileContent(name: "FileName", type: "png",
            filepath: "/foo/bar.txt");
        var content3 = PersistEntityProvider.GetLinkContent(name: "LinkName", link: "http://www.google.com");
        var content4 = PersistEntityProvider.GetFileContent(name: "FileName", type: "txt",
            filepath: "/foo/foo.txt");
        var content5 = PersistEntityProvider.GetFileContent(name: "FileName", type: "pdf",
            filepath: "/foo/foo.txt");

        var ele1 = PersistEntityProvider.GetLearningElement(name: "a", content: content1);
        var ele2 = PersistEntityProvider.GetLearningElement(name: "b", content: content2);
        var ele3 = PersistEntityProvider.GetLearningElement(name: "c", content: content3);
        var ele4 = PersistEntityProvider.GetLearningElement(name: "d", content: content4);
        var ele5 = PersistEntityProvider.GetLearningElement(name: "e", content: content5);
        var topic1 = PersistEntityProvider.GetTopic(name: "topic1");
        var topic2 = PersistEntityProvider.GetTopic(name: "topic2");

        var space1 = new LearningSpacePe("a", "ff", "ff", 5, Theme.Campus,
            null, positionX: 0, positionY: 0, inBoundObjects: new List<IObjectInPathWayPe>(),
            outBoundObjects: new List<IObjectInPathWayPe>(), topic1)
        {
            LearningSpaceLayout =
            {
                LearningElements = new Dictionary<int, ILearningElementPe>
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
        var space2 = new LearningSpacePe("b", "ff", "ff", 5, Theme.Campus,
            null, positionX: 0, positionY: 0, inBoundObjects: new List<IObjectInPathWayPe>(),
            outBoundObjects: new List<IObjectInPathWayPe>(), topic1);
        var space3 = new LearningSpacePe("c", "ff", "ff", 5, Theme.Campus,
            new LearningSpaceLayoutPe(new Dictionary<int, ILearningElementPe>(),
                FloorPlanEnum.R_20X30_8L), positionX: 0, positionY: 0, inBoundObjects: new List<IObjectInPathWayPe>(),
            outBoundObjects: new List<IObjectInPathWayPe>(), topic2);
        var space4 = new LearningSpacePe("d", "ff", "ff", 5, Theme.Campus,
            new LearningSpaceLayoutPe(new Dictionary<int, ILearningElementPe>(),
                FloorPlanEnum.L_32X31_10L), positionX: 0, positionY: 0, inBoundObjects: new List<IObjectInPathWayPe>(),
            outBoundObjects: new List<IObjectInPathWayPe>(), topic2);

        var condition1 = new PathWayConditionPe(ConditionEnum.And, 0, 0,
            new List<IObjectInPathWayPe>());

        space1.OutBoundObjects = new List<IObjectInPathWayPe> { space2 };
        space2.InBoundObjects = new List<IObjectInPathWayPe> { space1 };

        space2.OutBoundObjects = new List<IObjectInPathWayPe> { condition1 };
        condition1.InBoundObjects = new List<IObjectInPathWayPe> { space2, space3 };

        space3.OutBoundObjects = new List<IObjectInPathWayPe> { condition1 };

        condition1.OutBoundObjects = new List<IObjectInPathWayPe> { space4 };
        space4.InBoundObjects = new List<IObjectInPathWayPe> { condition1 };

        var learningSpaces = new List<LearningSpacePe> { space1, space2, space3, space4 };
        var conditions = new List<PathWayConditionPe> { condition1 };
        var topics = new List<TopicPe> { topic1, topic2 };


        var learningWorld = new LearningWorldPe(name, shortname, authors, language, description, goals, evaluationLink,
            savePath,
            learningSpaces, conditions, topics: topics);

        var systemUnderTest = new CreateDsl(mockFileSystem, mockLogger);

        //Every Element except Content with "url" is added to the comparison list.
        var learningElementsSpace1 = new List<LearningElementPe> { ele1, ele2, ele4, ele5 };

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
            Assert.That(systemUnderTest.LearningWorldJson.WorldName, Is.EqualTo(learningWorld.Name));
            Assert.That(systemUnderTest.ElementsWithFileContent, Is.EquivalentTo(learningElementsSpace1));
            Assert.That(systemUnderTest.LearningWorldJson.Spaces[0].SpaceName, Is.EqualTo(space1.Name));
            Assert.That(systemUnderTest.LearningWorldJson.Spaces[1].SpaceName, Is.EqualTo(space3.Name));
            Assert.That(systemUnderTest.LearningWorldJson.Spaces[2].SpaceName, Is.EqualTo(space2.Name));
            Assert.That(systemUnderTest.LearningWorldJson.Spaces[3].SpaceName, Is.EqualTo(space4.Name));
            Assert.That(systemUnderTest.ListTopics, Is.EquivalentTo(topics));
            Assert.That(systemUnderTest.LearningWorldJson.Spaces[0].RequiredSpacesToEnter,
                Is.EqualTo(""));
            Assert.That(systemUnderTest.LearningWorldJson.Spaces[1].RequiredSpacesToEnter,
                Is.EqualTo(""));
            Assert.That(systemUnderTest.LearningWorldJson.Spaces[2].RequiredSpacesToEnter,
                Is.EqualTo("1"));
            Assert.That(systemUnderTest.LearningWorldJson.Spaces[3].RequiredSpacesToEnter,
                Is.EqualTo("(3)^(2)"));
            Assert.That(systemUnderTest.LearningWorldJson.EvaluationLink, Is.EqualTo(evaluationLink));
        });
        Assert.Multiple(() => { Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True); });
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
        const string evaluationLink = "https://www.projekt-alder.eu";
        const string savePath = "C:\\Users\\Ben\\Desktop\\test";

        var content1 = PersistEntityProvider.GetFileContent("FileName", "mp3", "/foo/bar.txt");

        var ele1 = PersistEntityProvider.GetLearningElement(name: "a", content: content1);

        var space1 = new LearningSpacePe("ff", "ff", "ff", 5, Theme.Campus, positionX: 0, positionY: 0,
            inBoundObjects: new List<IObjectInPathWayPe>(),
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

        var learningWorld = new LearningWorldPe(name, shortname, authors, language, description, goals, evaluationLink,
            savePath,
            learningSpaces);

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
            Assert.That(e.Message,
                Is.EqualTo(
                    "The given LearningContent Type of element a is not supported (Parameter 'LearningContent')"));
        }
    }

    [Test]
    public void WriteLearningWorld_UnsupportedFloorPlanNameExceptionThrown()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        mockFileSystem.AddFile("/foo/foo.txt", new MockFileData("foo"));
        var mockLogger = Substitute.For<ILogger<CreateDsl>>();

        const string name = "space1";
        const string shortname = "s1";
        const string authors = "ben and jerry";
        const string language = "german";
        const string description = "very cool element";
        const string goals = "learn very many things";
        const string evaluationLink = "https://www.projekt-alder.eu";
        const string savePath = "C:\\Users\\Ben\\Desktop\\test";

        var space1 = new LearningSpacePe("ff", "ff", "ff", 5, Theme.Campus, positionX: 0, positionY: 0,
            inBoundObjects: new List<IObjectInPathWayPe>(),
            outBoundObjects: new List<IObjectInPathWayPe>())
        {
            LearningSpaceLayout =
            {
                FloorPlanName = (FloorPlanEnum)999 // ungültiger FloorPlanName
            }
        };

        var learningSpaces = new List<LearningSpacePe> { space1 };
        var learningWorld = new LearningWorldPe(name, shortname, authors, language, description, goals, evaluationLink,
            savePath,
            learningSpaces);

        var systemUnderTest = new CreateDsl(mockFileSystem, mockLogger);

        //Act
        try
        {
            systemUnderTest.WriteLearningWorld(learningWorld);
            Assert.Fail("FloorPlanName Exception was not thrown");
        }
        catch (Exception e)
        {
            //Assert
            Assert.That(e.Message,
                Is.EqualTo("The FloorPlanName 999 of space ff is not supported (Parameter 'FloorPlanName')"));
        }
    }

    [Test]
    public void WriteLearningWorld_InvalidLearningContentTypeExceptionThrown()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        mockFileSystem.AddFile("/foo/foo.txt", new MockFileData("foo"));
        var mockLogger = Substitute.For<ILogger<CreateDsl>>();

        const string name = "ele1";
        const string shortname = "e1";
        const string authors = "ben and jerry";
        const string language = "german";
        const string description = "very cool element";
        const string goals = "learn very many things";
        const string evaluationLink = "https://www.projekt-alder.eu";
        const string savePath = "C:\\Users\\Ben\\Desktop\\test";

        var ele1 = PersistEntityProvider.GetLearningElement(name: "a", content: null);

        var space1 = new LearningSpacePe("ff", "ff", "ff", 5, Theme.Campus, positionX: 0, positionY: 0,
            inBoundObjects: new List<IObjectInPathWayPe>(),
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
        var learningWorld = new LearningWorldPe(name, shortname, authors, language, description, goals, evaluationLink,
            savePath,
            learningSpaces);

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
            Assert.That(e.Message,
                Is.EqualTo(
                    "The given LearningContent of element a is either FileContent or LinkContent (Parameter 'LearningContent')"));
        }
    }
}