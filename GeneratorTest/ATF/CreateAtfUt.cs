using System.Globalization;
using System.IO.Abstractions.TestingHelpers;
using Generator.ATF;
using Generator.ATF.AdaptivityElement;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using PersistEntities;
using PersistEntities.LearningContent;
using PersistEntities.LearningContent.Action;
using PersistEntities.LearningContent.Question;
using PersistEntities.LearningContent.Trigger;
using PersistEntities.LearningOutcome;
using Shared;
using Shared.Adaptivity;
using Shared.Configuration;
using Shared.LearningOutcomes;
using Shared.Theme;
using TestHelpers;

namespace GeneratorTest.ATF;

[TestFixture]
public class CreateAtfUt
{
    private static IEnumerable<FloorPlanEnum> FloorPlanEnumValues =>
        Enum.GetValues(typeof(FloorPlanEnum)).Cast<FloorPlanEnum>();

    [Test]
    // ANF-ID: [GHO01, GHO02]
    public void GenerateAndExportLearningWorldJson_DefineLogicalExpression_RequirementDefined()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        var mockLogger = Substitute.For<ILogger<CreateAtf>>();
        var systemUnderTest = new CreateAtf(mockFileSystem, mockLogger);

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
    // ANF-ID: [GHO01, GHO02]
    public void GenerateAndExportLearningWorldJson_SearchDuplicateLearningElementNames_DuplicatesFoundAndNamesChanged()
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
        var mockLearningSpaceLayout3 = PersistEntityProvider.GetLearningSpaceLayout(
            learningElements: mockLearningElements3,
            floorPlan: FloorPlanEnum.R_20X30_8L);

        var mockSpace1 =
            PersistEntityProvider.GetLearningSpace(name: "Space1", learningSpaceLayout: mockLearningSpaceLayout1);
        var mockSpace2 =
            PersistEntityProvider.GetLearningSpace(name: "Space2", learningSpaceLayout: mockLearningSpaceLayout2);
        var mockSpace3 =
            PersistEntityProvider.GetLearningSpace(name: "Space3", learningSpaceLayout: mockLearningSpaceLayout3);


        var mockSpaces = new List<LearningSpacePe> { mockSpace1, mockSpace2, mockSpace3 };

        //Act
        var learningSpaceList = CreateAtf.IncrementDuplicateLearningElementNames(mockSpaces);

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
    // ANF-ID: [GHO01, GHO02]
    public void GenerateAndExportLearningWorldJson_WriteLearningWorld_ATFDocumentWritten()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        mockFileSystem.AddFile("/foo/bar.txt", new MockFileData("barbaz"));
        mockFileSystem.AddFile("/foo/foo.txt", new MockFileData("foo"));
        var mockLogger = Substitute.For<ILogger<CreateAtf>>();

        const string name = "asdf";
        const string shortname = "jkl;";
        const string authors = "ben and jerry";
        const string language = "german";
        const string description = "very cool element";
        const string goals = "learn very many things";
        const string evaluationLink = "http://www.projekt-alder.eu";
        const string enrolmentKey = "1234";
        const string storyStart = "story start";
        const string storyEnd = "story end";
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
        var content6 = PersistEntityProvider.GetFileContent(name: "Stadtteile AB", type: "pdf",
            filepath: "/foo/foo.txt");
        var content7 = PersistEntityProvider.GetFileContent(name: "primitive", type: "h5p",
            filepath: "/foo/bar.txt", primitiveH5p: true);
        var adaptivityContent1 = PersistEntityProvider.GetAdaptivityContent();
        var introStoryContent = PersistEntityProvider.GetStoryContent();
        var outroStoryContent =
            PersistEntityProvider.GetStoryContent(story: new List<string>() { "Outro", "Story", "Text" });

        var ele1 = PersistEntityProvider.GetLearningElement(name: "ele1", content: content1);
        var ele2 = PersistEntityProvider.GetLearningElement(name: "ele2", content: content2);
        var ele3 = PersistEntityProvider.GetLearningElement(name: "ele3", content: content3);
        var ele4 = PersistEntityProvider.GetLearningElement(name: "ele4", content: content4);
        var ele5 = PersistEntityProvider.GetLearningElement(name: "ele5", content: content5);
        var ele6 = PersistEntityProvider.GetLearningElement(name: "ele6", content: adaptivityContent1);
        var introEle = PersistEntityProvider.GetLearningElement(name: "StoryEle1", content: introStoryContent);
        var outroEle = PersistEntityProvider.GetLearningElement(name: "StoryEle2", content: outroStoryContent);
        var ele7 = PersistEntityProvider.GetLearningElement(name: "primitive", content: content7);
        var topic1 = PersistEntityProvider.GetTopic(name: "topic1");
        var topic2 = PersistEntityProvider.GetTopic(name: "topic2");

        var choice1 = new ChoicePe("Ja");
        var choice2 = new ChoicePe("Nein");
        var choice3 = new ChoicePe("Österreicher Kolonie");
        var choice4 = new ChoicePe("Obernauer Kolonie");
        var choice5 = new ChoicePe("Nilkheimer Kolonie");
        var choice6 = new ChoicePe("Schweinheimer Kolonie");

        var rule1 = new AdaptivityRulePe(new CorrectnessTriggerPe(AnswerResult.Incorrect),
            new CommentActionPe("Bla bla bla"));
        var rule2 = new AdaptivityRulePe(new CorrectnessTriggerPe(AnswerResult.Incorrect),
            PersistEntityProvider.GetElementReferenceAction(ele1.Id));
        var rule3 = new AdaptivityRulePe(new CorrectnessTriggerPe(AnswerResult.Incorrect),
            PersistEntityProvider.GetContentReferenceAction(content6));

        var question1 =
            new MultipleChoiceSingleResponseQuestionPe(0,
                new List<ChoicePe> { choice1, choice2 }, "Die Stadt Aschaffenburg hat insgesamt 10 Stadtteile", choice1,
                QuestionDifficulty.Easy, new List<IAdaptivityRulePe> { rule1 });
        var question2 = new MultipleChoiceMultipleResponseQuestionPe(0,
            new List<ChoicePe> { choice3, choice4, choice5, choice6 }, new List<ChoicePe> { choice3, choice4 },
            new List<IAdaptivityRulePe> { rule2 }, "Welche der folgenden Stadtteile gehören zu Aschaffenburg?",
            QuestionDifficulty.Medium);
        var question3 = new MultipleChoiceSingleResponseQuestionPe(0,
            new List<ChoicePe> { choice1, choice2 }, "Schwere Frage 1", choice2, QuestionDifficulty.Hard,
            new List<IAdaptivityRulePe> { rule3 });

        var task1 = new AdaptivityTaskPe(new List<IAdaptivityQuestionPe> { question1, question2 },
            QuestionDifficulty.Medium, "Stadtteile AB");
        var task2 = new AdaptivityTaskPe(new List<IAdaptivityQuestionPe> { question3 },
            null, "Schwere Frage 1");

        var adaptivityContent2 = new AdaptivityContentPe("Abschlussquiz zur Stadt Aschaffenburg",
            new List<IAdaptivityTaskPe> { task1, task2 });

        var ele8 = PersistEntityProvider.GetLearningElement(name: "ele8", content: adaptivityContent2);

        var manualLearningOutcome = new ManualLearningOutcomePe("Outcome");
        var structuredLearningOutcome1 = new StructuredLearningOutcomePe(TaxonomyLevel.Level1, "whatDe", "wherebyDe",
            "whatForDe", "verbOfVisibilityDe", new CultureInfo("de-DE"));
        var structuredLearningOutcome2 = new StructuredLearningOutcomePe(TaxonomyLevel.Level2, "whatEn", "wherebyEn",
            "whatForEn",
            "verbOfVisibilityEn", new CultureInfo("en-DE"));

        var space1 = new LearningSpacePe("a", "ff", 5, Theme.CampusAschaffenburg,
            PersistEntityProvider.GetLearningOutcomeCollection(new List<ILearningOutcomePe>()
                { structuredLearningOutcome1 }),
            positionX: 0, positionY: 0, inBoundObjects: new List<IObjectInPathWayPe>(),
            outBoundObjects: new List<IObjectInPathWayPe>(), assignedTopic: topic1)
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
                        3,
                        ele2
                    }
                },
                FloorPlanName = FloorPlanEnum.R_20X20_6L,
                StoryElements = new Dictionary<int, ILearningElementPe>
                {
                    {
                        0,
                        introEle
                    },
                    {
                        1,
                        outroEle
                    }
                }
            }
        };
        var space2 = new LearningSpacePe("b", "ff", 5, Theme.CampusAschaffenburg, positionX: 0, positionY: 0, inBoundObjects: new List<IObjectInPathWayPe>(),
            outBoundObjects: new List<IObjectInPathWayPe>(), assignedTopic: null)
        {
            LearningSpaceLayout =
            {
                LearningElements = new Dictionary<int, ILearningElementPe>()
                {
                    {
                        2,
                        ele3
                    },
                    {
                        3,
                        ele4
                    },
                    {
                        5,
                        ele6
                    },
                    {
                        7,
                        ele7
                    }
                },
                FloorPlanName = FloorPlanEnum.R_20X30_8L
            }
        };
        var space3 = new LearningSpacePe("c", "ff", 5, Theme.CampusAschaffenburg, positionX: 0, positionY: 0, inBoundObjects: new List<IObjectInPathWayPe>(),
            outBoundObjects: new List<IObjectInPathWayPe>(), assignedTopic: topic2)
        {
            LearningSpaceLayout =
            {
                LearningElements = new Dictionary<int, ILearningElementPe>()
                {
                    {
                        4,
                        ele5
                    },
                    {
                        5,
                        ele8
                    }
                },
                FloorPlanName = FloorPlanEnum.L_32X31_10L
            }
        };
        var space4 = new LearningSpacePe("d", "ff", 5, Theme.CampusAschaffenburg,
            PersistEntityProvider.GetLearningOutcomeCollection(new List<ILearningOutcomePe>()
                { structuredLearningOutcome2, manualLearningOutcome }),
            PersistEntityProvider.GetLearningSpaceLayout(learningElements: new Dictionary<int, ILearningElementPe>(),
                floorPlan:
                FloorPlanEnum.L_32X31_10L), positionX: 0, positionY: 0, inBoundObjects: new List<IObjectInPathWayPe>(),
            outBoundObjects: new List<IObjectInPathWayPe>(), assignedTopic: topic2);

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
            enrolmentKey, storyStart, storyEnd,
            savePath,
            learningSpaces, conditions, topics: topics);

        var systemUnderTest = new CreateAtf(mockFileSystem, mockLogger);

        //Every Element except Content with "url" is added to the comparison list.
        var listFileContent = new List<(FileContentPe, string)>
        {
            ((FileContentPe)ele1.LearningContent, ele1.Name),
            ((FileContentPe)ele2.LearningContent, ele2.Name),
            ((FileContentPe)ele5.LearningContent, ele5.Name),
            (content6, content6.Name),
            ((FileContentPe)ele4.LearningContent, ele4.Name),
            (content7, content7.Name)
        };

        //Act
        systemUnderTest.GenerateAndExportLearningWorldJson(learningWorld);

        //Assert
        var pathXmlFile = Path.Join(ApplicationPaths.BackupFolder, "XMLFilesForExport", "ATF_Document.json");
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Uuid, Is.Not.Null);
            Assert.That(systemUnderTest.LearningWorldJson, Is.Not.Null);
        });
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.LearningWorldJson.WorldName, Is.EqualTo(learningWorld.Name));
            Assert.That(systemUnderTest.ListFileContent, Is.EquivalentTo(listFileContent));
            Assert.That(systemUnderTest.LearningWorldJson.Topics.Count, Is.EqualTo(2));
            Assert.That(systemUnderTest.LearningWorldJson.Topics[0].TopicName, Is.EqualTo(topic1.Name));
            Assert.That(systemUnderTest.LearningWorldJson.Topics[0].TopicId, Is.EqualTo(1));
            Assert.That(systemUnderTest.LearningWorldJson.Topics[0].TopicContents, Is.EqualTo(new List<int>() { 1 }));
            Assert.That(systemUnderTest.LearningWorldJson.Topics[1].TopicName, Is.EqualTo(topic2.Name));
            Assert.That(systemUnderTest.LearningWorldJson.Topics[1].TopicId, Is.EqualTo(2));
            Assert.That(systemUnderTest.LearningWorldJson.Topics[1].TopicContents,
                Is.EqualTo(new List<int>() { 2, 4 }));
            Assert.That(systemUnderTest.LearningWorldJson.Spaces.Count, Is.EqualTo(4));
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
            Assert.That(systemUnderTest.LearningWorldJson.Spaces[0].SpaceGoals[0],
                Is.EqualTo(structuredLearningOutcome1.GetOutcome()));
            Assert.That(systemUnderTest.LearningWorldJson.Spaces[3].SpaceGoals[0],
                Is.EqualTo(structuredLearningOutcome2.GetOutcome()));
            Assert.That(systemUnderTest.LearningWorldJson.Spaces[3].SpaceGoals[1],
                Is.EqualTo(manualLearningOutcome.GetOutcome()));
            Assert.That(systemUnderTest.LearningWorldJson.EvaluationLink, Is.EqualTo(evaluationLink));
            Assert.That(systemUnderTest.LearningWorldJson.EnrolmentKey, Is.EqualTo(enrolmentKey));
            Assert.That(systemUnderTest.LearningWorldJson.Elements.Count, Is.EqualTo(9));

            Assert.That(systemUnderTest.LearningWorldJson.Spaces[0].SpaceStory.IntroStory, Is.Not.Null);
            Assert.That(systemUnderTest.LearningWorldJson.Spaces[0].SpaceStory.IntroStory?.StoryTexts,
                Is.EqualTo(introStoryContent.StoryText.ToArray()));
            Assert.That(systemUnderTest.LearningWorldJson.Spaces[0].SpaceStory.IntroStory?.ElementModel,
                Is.EqualTo(ElementModel.l_h5p_slotmachine_1.ToString()));
            Assert.That(systemUnderTest.LearningWorldJson.Spaces[0].SpaceStory.OutroStory, Is.Not.Null);
            Assert.That(systemUnderTest.LearningWorldJson.Spaces[0].SpaceStory.OutroStory?.StoryTexts,
                Is.EqualTo(outroStoryContent.StoryText.ToArray()));
            Assert.That(systemUnderTest.LearningWorldJson.Spaces[0].SpaceStory.OutroStory?.ElementModel,
                Is.EqualTo(ElementModel.l_h5p_slotmachine_1.ToString()));

            Assert.That(systemUnderTest.LearningWorldJson.Elements[0].ElementName, Is.EqualTo(ele1.Name));
            Assert.That(systemUnderTest.LearningWorldJson.Elements[0].ElementId, Is.EqualTo(1));
            Assert.That(systemUnderTest.LearningWorldJson.Elements[0].ElementUUID, Is.EqualTo(ele1.Id.ToString()));
            Assert.That(systemUnderTest.LearningWorldJson.Elements[0].ElementFileType, Is.EqualTo("h5p"));
            Assert.That(systemUnderTest.LearningWorldJson.Elements[0].ElementCategory, Is.EqualTo("h5p"));
            Assert.That(((LearningElementJson)systemUnderTest.LearningWorldJson.Elements[0]).ElementMaxScore,
                Is.EqualTo(ele1.Points));
            Assert.That(((LearningElementJson)systemUnderTest.LearningWorldJson.Elements[0]).ElementDescription,
                Is.EqualTo(ele1.Description));
            Assert.That(((LearningElementJson)systemUnderTest.LearningWorldJson.Elements[0]).ElementGoals,
                Is.EqualTo(ele1.Goals.Split("\n")));
            Assert.That(((LearningElementJson)systemUnderTest.LearningWorldJson.Elements[0]).ElementModel,
                Is.EqualTo(ele1.ElementModel.ToString()));
            Assert.That(((LearningElementJson)systemUnderTest.LearningWorldJson.Elements[0]).LearningSpaceParentId,
                Is.EqualTo(systemUnderTest.LearningWorldJson.Spaces[0].SpaceId));

            Assert.That(systemUnderTest.LearningWorldJson.Elements[1].ElementName, Is.EqualTo(ele2.Name));
            Assert.That(systemUnderTest.LearningWorldJson.Elements[1].ElementId, Is.EqualTo(2));
            Assert.That(systemUnderTest.LearningWorldJson.Elements[1].ElementUUID, Is.EqualTo(ele2.Id.ToString()));
            Assert.That(systemUnderTest.LearningWorldJson.Elements[1].ElementFileType, Is.EqualTo("png"));
            Assert.That(systemUnderTest.LearningWorldJson.Elements[1].ElementCategory, Is.EqualTo("image"));
            Assert.That(((LearningElementJson)systemUnderTest.LearningWorldJson.Elements[1]).ElementMaxScore,
                Is.EqualTo(ele2.Points));
            Assert.That(((LearningElementJson)systemUnderTest.LearningWorldJson.Elements[1]).ElementDescription,
                Is.EqualTo(ele2.Description));
            Assert.That(((LearningElementJson)systemUnderTest.LearningWorldJson.Elements[1]).ElementGoals,
                Is.EqualTo(ele2.Goals.Split("\n")));
            Assert.That(((LearningElementJson)systemUnderTest.LearningWorldJson.Elements[1]).ElementModel,
                Is.EqualTo(ele2.ElementModel.ToString()));
            Assert.That(((LearningElementJson)systemUnderTest.LearningWorldJson.Elements[1]).LearningSpaceParentId,
                Is.EqualTo(systemUnderTest.LearningWorldJson.Spaces[0].SpaceId));

            Assert.That(systemUnderTest.LearningWorldJson.Elements[2].ElementName, Is.EqualTo(ele5.Name));
            Assert.That(systemUnderTest.LearningWorldJson.Elements[2].ElementId, Is.EqualTo(3));
            Assert.That(systemUnderTest.LearningWorldJson.Elements[2].ElementUUID, Is.EqualTo(ele5.Id.ToString()));
            Assert.That(systemUnderTest.LearningWorldJson.Elements[2].ElementFileType, Is.EqualTo("pdf"));
            Assert.That(systemUnderTest.LearningWorldJson.Elements[2].ElementCategory, Is.EqualTo("pdf"));
            Assert.That(((LearningElementJson)systemUnderTest.LearningWorldJson.Elements[2]).ElementMaxScore,
                Is.EqualTo(ele5.Points));
            Assert.That(((LearningElementJson)systemUnderTest.LearningWorldJson.Elements[2]).ElementDescription,
                Is.EqualTo(ele5.Description));
            Assert.That(((LearningElementJson)systemUnderTest.LearningWorldJson.Elements[2]).ElementGoals,
                Is.EqualTo(ele5.Goals.Split("\n")));
            Assert.That(((LearningElementJson)systemUnderTest.LearningWorldJson.Elements[2]).ElementModel,
                Is.EqualTo(ele5.ElementModel.ToString()));
            Assert.That(((LearningElementJson)systemUnderTest.LearningWorldJson.Elements[2]).LearningSpaceParentId,
                Is.EqualTo(systemUnderTest.LearningWorldJson.Spaces[1].SpaceId));

            //AdaptivityElement with ContentReferenceAction
            Assert.That(systemUnderTest.LearningWorldJson.Elements[3].ElementName, Is.EqualTo(ele8.Name));
            Assert.That(systemUnderTest.LearningWorldJson.Elements[3].ElementId, Is.EqualTo(4));
            Assert.That(systemUnderTest.LearningWorldJson.Elements[3].ElementUUID, Is.EqualTo(ele8.Id.ToString()));
            Assert.That(systemUnderTest.LearningWorldJson.Elements[3].ElementFileType, Is.EqualTo("adaptivity"));
            Assert.That(systemUnderTest.LearningWorldJson.Elements[3].ElementCategory, Is.EqualTo("adaptivity"));
            Assert.That(((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).ElementMaxScore,
                Is.EqualTo(ele8.Points));
            Assert.That(((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).ElementDescription,
                Is.EqualTo(ele8.Description));
            Assert.That(((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).ElementGoals,
                Is.EqualTo(ele8.Goals.Split("\n")));
            Assert.That(((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).ElementModel,
                Is.EqualTo(ele8.ElementModel.ToString()));
            Assert.That(((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).LearningSpaceParentId,
                Is.EqualTo(systemUnderTest.LearningWorldJson.Spaces[1].SpaceId));

            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent.IntroText,
                Is.EqualTo(adaptivityContent2.Name));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[0].TaskTitle, Is.EqualTo(task1.Name));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[0].TaskId, Is.EqualTo(1));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[0].TaskUUID, Is.EqualTo(task1.Id.ToString()));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[0].Optional, Is.EqualTo(false));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[0].RequiredDifficulty, Is.EqualTo(100));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[0].AdaptivityQuestions[0].QuestionType, Is.EqualTo(ResponseType.singleResponse));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[0].AdaptivityQuestions[0].QuestionId, Is.EqualTo(1));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[0].AdaptivityQuestions[0].QuestionUUID, Is.EqualTo(question1.Id.ToString()));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[0].AdaptivityQuestions[0].QuestionDifficulty, Is.EqualTo(0));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[0].AdaptivityQuestions[0].QuestionText, Is.EqualTo(question1.Text));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[0].AdaptivityQuestions[0].AdaptivityRules[0].TriggerId, Is.EqualTo(1));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[0].AdaptivityQuestions[0].AdaptivityRules[0].TriggerCondition,
                Is.EqualTo("incorrect"));
            Assert.That(
                ((CommentActionJson)((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3])
                    .AdaptivityContent.AdaptivityTasks[0].AdaptivityQuestions[0].AdaptivityRules[0].AdaptivityAction)
                .CommentText, Is.EqualTo("Bla bla bla"));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[0].AdaptivityQuestions[0].Choices[0].AnswerText, Is.EqualTo("Ja"));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[0].AdaptivityQuestions[0].Choices[0].IsCorrect, Is.EqualTo(true));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[0].AdaptivityQuestions[0].Choices[1].AnswerText, Is.EqualTo("Nein"));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[0].AdaptivityQuestions[0].Choices[1].IsCorrect, Is.EqualTo(false));

            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[0].AdaptivityQuestions[1].QuestionType, Is.EqualTo(ResponseType.multipleResponse));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[0].AdaptivityQuestions[1].QuestionId, Is.EqualTo(2));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[0].AdaptivityQuestions[1].QuestionUUID, Is.EqualTo(question2.Id.ToString()));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[0].AdaptivityQuestions[1].QuestionDifficulty, Is.EqualTo(100));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[0].AdaptivityQuestions[1].QuestionText, Is.EqualTo(question2.Text));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[0].AdaptivityQuestions[1].AdaptivityRules[0].TriggerId, Is.EqualTo(1));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[0].AdaptivityQuestions[1].AdaptivityRules[0].TriggerCondition,
                Is.EqualTo("incorrect"));
            Assert.That(
                ((ElementReferenceActionJson)((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3])
                    .AdaptivityContent.AdaptivityTasks[0].AdaptivityQuestions[1].AdaptivityRules[0].AdaptivityAction)
                .ElementId, Is.EqualTo(1));

            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[0].AdaptivityQuestions[1].Choices[0].AnswerText, Is.EqualTo("Österreicher Kolonie"));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[0].AdaptivityQuestions[1].Choices[0].IsCorrect, Is.EqualTo(true));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[0].AdaptivityQuestions[1].Choices[1].AnswerText, Is.EqualTo("Obernauer Kolonie"));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[0].AdaptivityQuestions[1].Choices[1].IsCorrect, Is.EqualTo(true));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[0].AdaptivityQuestions[1].Choices[2].AnswerText, Is.EqualTo("Nilkheimer Kolonie"));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[0].AdaptivityQuestions[1].Choices[2].IsCorrect, Is.EqualTo(false));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[0].AdaptivityQuestions[1].Choices[3].AnswerText, Is.EqualTo("Schweinheimer Kolonie"));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[0].AdaptivityQuestions[1].Choices[3].IsCorrect, Is.EqualTo(false));

            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[1].TaskTitle, Is.EqualTo(task2.Name));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[1].TaskId, Is.EqualTo(2));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[1].TaskUUID, Is.EqualTo(task2.Id.ToString()));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[1].Optional, Is.EqualTo(true));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[1].RequiredDifficulty, Is.EqualTo(null));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[1].AdaptivityQuestions[0].QuestionType, Is.EqualTo(ResponseType.singleResponse));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[1].AdaptivityQuestions[0].QuestionId, Is.EqualTo(3));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[1].AdaptivityQuestions[0].QuestionUUID, Is.EqualTo(question3.Id.ToString()));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[1].AdaptivityQuestions[0].QuestionDifficulty, Is.EqualTo(200));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[1].AdaptivityQuestions[0].QuestionText, Is.EqualTo(question3.Text));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[1].AdaptivityQuestions[0].AdaptivityRules[0].TriggerId, Is.EqualTo(1));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[1].AdaptivityQuestions[0].AdaptivityRules[0].TriggerCondition,
                Is.EqualTo("incorrect"));
            Assert.That(
                ((ContentReferenceActionJson)((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3])
                    .AdaptivityContent.AdaptivityTasks[1].AdaptivityQuestions[0].AdaptivityRules[0].AdaptivityAction)
                .ElementId, Is.EqualTo(5));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[1].AdaptivityQuestions[0].Choices[0].AnswerText, Is.EqualTo("Ja"));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[1].AdaptivityQuestions[0].Choices[0].IsCorrect, Is.EqualTo(false));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[1].AdaptivityQuestions[0].Choices[1].AnswerText, Is.EqualTo("Nein"));
            Assert.That(
                ((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[3]).AdaptivityContent
                .AdaptivityTasks[1].AdaptivityQuestions[0].Choices[1].IsCorrect, Is.EqualTo(true));

            //Base Learning Element
            Assert.That(systemUnderTest.LearningWorldJson.Elements[4].ElementName, Is.EqualTo(content6.Name));
            Assert.That(systemUnderTest.LearningWorldJson.Elements[4].ElementId, Is.EqualTo(5));
            Assert.That(systemUnderTest.LearningWorldJson.Elements[4].ElementUUID,
                Is.EqualTo(rule3.Action.Id.ToString()));
            Assert.That(systemUnderTest.LearningWorldJson.Elements[4].ElementFileType, Is.EqualTo("pdf"));
            Assert.That(systemUnderTest.LearningWorldJson.Elements[4].ElementCategory, Is.EqualTo("pdf"));

            Assert.That(systemUnderTest.LearningWorldJson.Elements[5].ElementName, Is.EqualTo(ele3.Name));
            Assert.That(systemUnderTest.LearningWorldJson.Elements[5].ElementId, Is.EqualTo(6));
            Assert.That(systemUnderTest.LearningWorldJson.Elements[5].ElementUUID, Is.EqualTo(ele3.Id.ToString()));
            Assert.That(systemUnderTest.LearningWorldJson.Elements[5].ElementFileType, Is.EqualTo("url"));
            Assert.That(systemUnderTest.LearningWorldJson.Elements[5].ElementCategory, Is.EqualTo("video"));
            Assert.That(((LearningElementJson)systemUnderTest.LearningWorldJson.Elements[5]).ElementMaxScore,
                Is.EqualTo(ele3.Points));
            Assert.That(((LearningElementJson)systemUnderTest.LearningWorldJson.Elements[5]).ElementDescription,
                Is.EqualTo(ele3.Description));
            Assert.That(((LearningElementJson)systemUnderTest.LearningWorldJson.Elements[5]).ElementGoals,
                Is.EqualTo(ele3.Goals.Split("\n")));
            Assert.That(((LearningElementJson)systemUnderTest.LearningWorldJson.Elements[5]).ElementModel,
                Is.EqualTo(ele3.ElementModel.ToString()));
            Assert.That(((LearningElementJson)systemUnderTest.LearningWorldJson.Elements[5]).LearningSpaceParentId,
                Is.EqualTo(systemUnderTest.LearningWorldJson.Spaces[2].SpaceId));

            Assert.That(systemUnderTest.LearningWorldJson.Elements[6].ElementName, Is.EqualTo(ele4.Name));
            Assert.That(systemUnderTest.LearningWorldJson.Elements[6].ElementId, Is.EqualTo(7));
            Assert.That(systemUnderTest.LearningWorldJson.Elements[6].ElementUUID, Is.EqualTo(ele4.Id.ToString()));
            Assert.That(systemUnderTest.LearningWorldJson.Elements[6].ElementFileType, Is.EqualTo("txt"));
            Assert.That(systemUnderTest.LearningWorldJson.Elements[6].ElementCategory, Is.EqualTo("text"));
            Assert.That(((LearningElementJson)systemUnderTest.LearningWorldJson.Elements[6]).ElementMaxScore,
                Is.EqualTo(ele4.Points));
            Assert.That(((LearningElementJson)systemUnderTest.LearningWorldJson.Elements[6]).ElementDescription,
                Is.EqualTo(ele4.Description));
            Assert.That(((LearningElementJson)systemUnderTest.LearningWorldJson.Elements[6]).ElementGoals,
                Is.EqualTo(ele4.Goals.Split("\n")));
            Assert.That(((LearningElementJson)systemUnderTest.LearningWorldJson.Elements[6]).ElementModel,
                Is.EqualTo(ele4.ElementModel.ToString()));
            Assert.That(((LearningElementJson)systemUnderTest.LearningWorldJson.Elements[6]).LearningSpaceParentId,
                Is.EqualTo(systemUnderTest.LearningWorldJson.Spaces[2].SpaceId));

            Assert.That(systemUnderTest.LearningWorldJson.Elements[7].ElementName, Is.EqualTo(ele6.Name));
            Assert.That(systemUnderTest.LearningWorldJson.Elements[7].ElementId, Is.EqualTo(8));
            Assert.That(systemUnderTest.LearningWorldJson.Elements[7].ElementUUID, Is.EqualTo(ele6.Id.ToString()));
            Assert.That(systemUnderTest.LearningWorldJson.Elements[7].ElementFileType, Is.EqualTo("adaptivity"));
            Assert.That(systemUnderTest.LearningWorldJson.Elements[7].ElementCategory, Is.EqualTo("adaptivity"));
            Assert.That(((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[7]).ElementMaxScore,
                Is.EqualTo(ele6.Points));
            Assert.That(((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[7]).ElementDescription,
                Is.EqualTo(ele6.Description));
            Assert.That(((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[7]).ElementGoals,
                Is.EqualTo(ele6.Goals.Split("\n")));
            Assert.That(((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[7]).ElementModel,
                Is.EqualTo(ele6.ElementModel.ToString()));
            Assert.That(((IAdaptivityElementJson)systemUnderTest.LearningWorldJson.Elements[7]).LearningSpaceParentId,
                Is.EqualTo(systemUnderTest.LearningWorldJson.Spaces[2].SpaceId));

            Assert.That(systemUnderTest.LearningWorldJson.Elements[8].ElementName, Is.EqualTo(ele7.Name));
            Assert.That(systemUnderTest.LearningWorldJson.Elements[8].ElementId, Is.EqualTo(9));
            Assert.That(systemUnderTest.LearningWorldJson.Elements[8].ElementUUID, Is.EqualTo(ele7.Id.ToString()));
            Assert.That(systemUnderTest.LearningWorldJson.Elements[8].ElementFileType, Is.EqualTo("h5p"));
            Assert.That(systemUnderTest.LearningWorldJson.Elements[8].ElementCategory, Is.EqualTo("primitiveH5P"));
            Assert.That(((LearningElementJson)systemUnderTest.LearningWorldJson.Elements[8]).ElementMaxScore,
                Is.EqualTo(ele7.Points));
            Assert.That(((LearningElementJson)systemUnderTest.LearningWorldJson.Elements[8]).ElementDescription,
                Is.EqualTo(ele7.Description));
            Assert.That(((LearningElementJson)systemUnderTest.LearningWorldJson.Elements[8]).ElementGoals,
                Is.EqualTo(ele7.Goals.Split("\n")));
            Assert.That(((LearningElementJson)systemUnderTest.LearningWorldJson.Elements[8]).ElementModel,
                Is.EqualTo(ele7.ElementModel.ToString()));
            Assert.That(((LearningElementJson)systemUnderTest.LearningWorldJson.Elements[8]).LearningSpaceParentId,
                Is.EqualTo(systemUnderTest.LearningWorldJson.Spaces[2].SpaceId));
            
            Assert.That(systemUnderTest.LearningWorldJson.FrameStory.FrameStoryIntro, Is.EqualTo(storyStart));
            Assert.That(systemUnderTest.LearningWorldJson.FrameStory.FrameStoryOutro, Is.EqualTo(storyEnd));
        });
        Assert.Multiple(() => { Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True); });
    }

    [Test]
    // ANF-ID: [GHO01, GHO02]
    public void GenerateAndExportLearningWorldJson_WriteLearningWorld_UnsupportedTypeExceptionThrown()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        mockFileSystem.AddFile("/foo/foo.txt", new MockFileData("foo"));
        var mockLogger = Substitute.For<ILogger<CreateAtf>>();

        const string name = "asdf";
        const string shortname = "jkl;";
        const string authors = "ben and jerry";
        const string language = "german";
        const string description = "very cool element";
        const string goals = "learn very many things";
        const string evaluationLink = "https://www.projekt-alder.eu";
        const string enrolmentKey = "1234";
        const string storyStart = "story start";
        const string storyEnd = "story end";
        const string savePath = "C:\\Users\\Ben\\Desktop\\test";

        var content1 = PersistEntityProvider.GetFileContent("FileName", "mp3", "/foo/bar.txt");

        var ele1 = PersistEntityProvider.GetLearningElement(name: "a", content: content1);

        var space1 = new LearningSpacePe("ff", "ff", 5, Theme.CampusAschaffenburg, positionX: 0, positionY: 0,
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
            enrolmentKey, storyStart, storyEnd,
            savePath,
            learningSpaces);

        var systemUnderTest = new CreateAtf(mockFileSystem, mockLogger);

        //Act
        try
        {
            systemUnderTest.GenerateAndExportLearningWorldJson(learningWorld);
            Assert.Fail("Learning Content Exception was not thrown");
        }
        catch (Exception e)
        {
            //Assert
            Assert.That(e.Message,
                Is.EqualTo(
                    "The given LearningContent Type of file FileName is not supported (Parameter 'Type')"));
        }
    }

    [Test]
    // ANF-ID: [GHO01, GHO02]
    public void GenerateAndExportLearningWorldJson_UnsupportedFloorPlanNameExceptionThrown()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        mockFileSystem.AddFile("/foo/foo.txt", new MockFileData("foo"));
        var mockLogger = Substitute.For<ILogger<CreateAtf>>();

        const string name = "space1";
        const string shortname = "s1";
        const string authors = "ben and jerry";
        const string language = "german";
        const string description = "very cool element";
        const string goals = "learn very many things";
        const string evaluationLink = "https://www.projekt-alder.eu";
        const string enrolmentKey = "1234";
        const string storyStart = "story start";
        const string storyEnd = "story end";
        const string savePath = "C:\\Users\\Ben\\Desktop\\test";

        var space1 = new LearningSpacePe("ff", "ff", 5, Theme.CampusAschaffenburg, positionX: 0, positionY: 0,
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
            enrolmentKey, storyStart, storyEnd,
            savePath,
            learningSpaces);

        var systemUnderTest = new CreateAtf(mockFileSystem, mockLogger);

        //Act
        try
        {
            systemUnderTest.GenerateAndExportLearningWorldJson(learningWorld);
            Assert.Fail("FloorPlanName Exception was not thrown");
        }
        catch (ArgumentOutOfRangeException e)
        {
            //Assert
            Assert.That(e.Message,
                Is.EqualTo("The FloorPlanName 999 of space ff is not supported (Parameter 'FloorPlanName')"));
        }
    }

    [Test]
    [TestCaseSource(nameof(FloorPlanEnumValues))]
    // ANF-ID: [GHO01, GHO02]
    public void GenerateAndExportLearningWorldJson_SupportedFloorPlanName_NoExceptionThrown(FloorPlanEnum floorPlan)
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        mockFileSystem.AddFile("/foo/foo.txt", new MockFileData("foo"));
        var mockLogger = Substitute.For<ILogger<CreateAtf>>();

        const string name = "space1";
        const string shortname = "s1";
        const string authors = "ben and jerry";
        const string language = "german";
        const string description = "very cool element";
        const string goals = "learn very many things";
        const string evaluationLink = "https://www.projekt-alder.eu";
        const string enrolmentKey = "1234";
        const string storyStart = "story start";
        const string storyEnd = "story end";
        const string savePath = "C:\\Users\\Ben\\Desktop\\test";

        var space1 = new LearningSpacePe("ff", "ff", 5, Theme.CampusAschaffenburg, positionX: 0, positionY: 0,
            inBoundObjects: new List<IObjectInPathWayPe>(),
            outBoundObjects: new List<IObjectInPathWayPe>())
        {
            LearningSpaceLayout =
            {
                FloorPlanName = floorPlan // ungültiger FloorPlanName
            }
        };

        var learningSpaces = new List<LearningSpacePe> { space1 };
        var learningWorld = new LearningWorldPe(name, shortname, authors, language, description, goals, evaluationLink,
            enrolmentKey, storyStart, storyEnd,
            savePath,
            learningSpaces);

        var systemUnderTest = new CreateAtf(mockFileSystem, mockLogger);

        //Act

        Assert.DoesNotThrow(() => systemUnderTest.GenerateAndExportLearningWorldJson(learningWorld));
    }

    [Test]
    // ANF-ID: [GHO01, GHO02]
    public void GenerateAndExportLearningWorldJson_InvalidLearningContentTypeExceptionThrown()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        mockFileSystem.AddFile("/foo/foo.txt", new MockFileData("foo"));
        var mockLogger = Substitute.For<ILogger<CreateAtf>>();

        const string name = "ele1";
        const string shortname = "e1";
        const string authors = "ben and jerry";
        const string language = "german";
        const string description = "very cool element";
        const string goals = "learn very many things";
        const string evaluationLink = "https://www.projekt-alder.eu";
        const string enrolmentKey = "1234";
        const string storyStart = "story start";
        const string storyEnd = "story end";
        const string savePath = "C:\\Users\\Ben\\Desktop\\test";

        var ele1 = PersistEntityProvider.GetLearningElement(name: "a", content: null);

        var space1 = new LearningSpacePe("ff", "ff", 5, Theme.CampusAschaffenburg, positionX: 0, positionY: 0,
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
            enrolmentKey, storyStart, storyEnd,
            savePath,
            learningSpaces);

        var systemUnderTest = new CreateAtf(mockFileSystem, mockLogger);

        //Act
        try
        {
            systemUnderTest.GenerateAndExportLearningWorldJson(learningWorld);
            Assert.Fail("Learning Content Exception was not thrown");
        }
        catch (Exception e)
        {
            //Assert
            Assert.That(e.Message,
                Is.EqualTo(
                    "The given LearningContent of element a is neither FileContent, LinkContent or AdaptivityContent (Parameter 'LearningContent')"));
        }
    }
}