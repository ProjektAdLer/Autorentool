using Generator.ATF;
using Generator.ATF.AdaptivityElement;
using Generator.XmlClasses;
using Generator.XmlClasses.Entities.Gradebook.xml;
using Generator.XmlClasses.Entities.Groups.xml;
using Generator.XmlClasses.Entities.MoodleBackup.xml;
using Generator.XmlClasses.Entities.Outcomes.xml;
using Generator.XmlClasses.Entities.Questions.xml;
using Generator.XmlClasses.Entities.Roles.xml;
using Generator.XmlClasses.Entities.Scales.xml;
using GeneratorTest.Xsd;
using NSubstitute;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses;

[TestFixture]
public class XmlBackupFactoryUt
{
    [Test]
    public void XmlBackupFactory_CreateXmlBackupFactory_AllMethodsCalled()
    {
        // Arrange
        var mockReadAtf = Substitute.For<IReadAtf>();

        var mockGradeItems = Substitute.For<IGradebookXmlGradeItems>();
        var mockGradeCategories = Substitute.For<IGradebookXmlGradeCategories>();
        var mockGradeSetting = new GradebookXmlGradeSetting();
        var mockGradeSettings = Substitute.For<IGradebookXmlGradeSettings>();
        var mockGradebook = Substitute.For<IGradebookXmlGradebook>();
        var mockContextId = 12345;

        var currentTime = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();

        var mockGradeItem = new GradebookXmlGradeItem
        {
            Timecreated = currentTime,
            Timemodified = currentTime,
        };

        var mockGradeCategory = new GradebookXmlGradeCategory
        {
            Timecreated = currentTime,
            Timemodified = currentTime,
        };

        var mockGroups = Substitute.For<IGroupsXmlGroups>();
        var mockGroupingsList = new GroupsXmlGroupingsList();

        var mockDetail = new MoodleBackupXmlDetail();
        var mockDetails = Substitute.For<IMoodleBackupXmlDetails>();
        var mockSetting = Substitute.For<IMoodleBackupXmlSetting>();
        Substitute.For<IMoodleBackupXmlSettings>();
        var mockContents = Substitute.For<IMoodleBackupXmlContents>();
        var mockInformation = Substitute.For<IMoodleBackupXmlInformation>();
        var mockMoodleBackup = Substitute.For<IMoodleBackupXmlMoodleBackup>();
        var mockAktivities = Substitute.For<IMoodleBackupXmlActivities>();
        var mockSections = Substitute.For<IMoodleBackupXmlSections>();
        var mockCourse = Substitute.For<IMoodleBackupXmlCourse>();
        var mockLearningWorld = new LearningWorldJson("world", "",
            new List<ITopicJson>(), new List<ILearningSpaceJson>(), new List<IElementJson>());

        var mockLearningElement = new LearningElementJson(1, "", "", "", "h5p", "", 2, 2, "");
        List<ILearningElementJson> learningElementJsons = new() { mockLearningElement };

        var mockAdaptivityElements = new List<IAdaptivityElementJson>();

        List<IElementJson> elementJsons = new() { mockLearningElement };

        var mockLearningSpaceContent = new List<int?> { mockLearningElement.ElementId };
        var mockLearningSpace = new LearningSpaceJson(1, "", "", mockLearningSpaceContent, 0, "", "");
        List<ILearningSpaceJson> learningSpacesJsons = new() { mockLearningSpace };

        var mockAtfDocumentJson = new LearningElementJson(2, "", "", "", "", "json", 0, 8, "");
        var mockSpaceElementJson = new LearningElementJson(3, "", "", "", "space", "", 9, 3, "");

        mockReadAtf.GetLearningWorld().Returns(mockLearningWorld);
        mockReadAtf.GetH5PElementsList().Returns(learningElementJsons);
        mockReadAtf.GetSpaceList().Returns(learningSpacesJsons);
        mockReadAtf.GetResourceElementList().Returns(new List<ILearningElementJson> { mockAtfDocumentJson });
        mockReadAtf.GetAdaptivityElementsList().Returns(mockAdaptivityElements);

        learningElementJsons.Add(mockAtfDocumentJson);
        learningElementJsons.Add(mockSpaceElementJson);
        mockReadAtf.GetElementsOrderedList().Returns(elementJsons);
        mockReadAtf.GetBaseLearningElementsList().Returns(new List<IBaseLearningElementJson>());

        var mockOutcomes = Substitute.For<IOutcomesXmlOutcomesDefinition>();

        var mockQuestion = Substitute.For<IQuestionsXmlQuestionsCategories>();

        var mockScales = Substitute.For<IScalesXmlScalesDefinition>();

        var mockRolesDefinition = Substitute.For<IRolesXmlRolesDefinition>();
        var mockRole = new RolesXmlRole();

        var systemUnderTest = new XmlBackupFactory(mockReadAtf, mockContextId, mockGradeItem, mockGradeItems,
            mockGradeCategory,
            mockGradeCategories, mockGradeSetting, mockGradeSettings, mockGradebook, mockGroupingsList, mockGroups,
            mockDetail, mockDetails, mockAktivities, null, mockSections, mockCourse, mockContents, mockSetting,
            mockInformation, mockMoodleBackup, mockOutcomes, mockQuestion, mockRole, mockRolesDefinition, mockScales);

        // Act
        systemUnderTest.CreateXmlBackupFactory();


        // Assert
        Assert.Multiple(() =>
        {
            systemUnderTest.GradebookXmlGradebook.Received().Serialize();
            systemUnderTest.GroupsXmlGroups.Received().Serialize();
            systemUnderTest.MoodleBackupXmlMoodleBackup.Received().Serialize();
            systemUnderTest.OutcomesXmlOutcomesDefinition.Received().Serialize();
            systemUnderTest.QuestionsXmlQuestionsCategories.Received().Serialize();
            systemUnderTest.RolesXmlRolesDefinition.Received().Serialize();
            systemUnderTest.ScalesXmlScalesDefinition.Received().Serialize();
        });
    }

    [Test]
    public void XmlBackupFactory_Constructor_AllPropertiesSet()
    {
        var mockContextId = 12345;
        var mockReadAtf = Substitute.For<IReadAtf>();

        var learningWorldJson = new LearningWorldJson("world", "",
            new List<ITopicJson> { new TopicJson(1, "Topic", new List<int> { 1 }) }, new List<ILearningSpaceJson>
            {
                new LearningSpaceJson(1,
                    "", "space", new List<int?> { 1 }, 0, "", "")
            },
            new List<IElementJson>
            {
                new LearningElementJson(1, "", "", "", "", "h5p",
                    0, 2, "")
            });
        mockReadAtf.GetResourceElementList().Returns(new List<ILearningElementJson>());
        mockReadAtf.GetLearningWorld().Returns(learningWorldJson);
        mockReadAtf.GetH5PElementsList().Returns(new List<ILearningElementJson>());

        var xmlBackupFactory = new XmlBackupFactory(mockReadAtf, mockContextId);

        //Assert
        Assert.That(xmlBackupFactory.GradebookXmlGradebookSetting, Is.Not.Null);
        Assert.That(xmlBackupFactory.GradebookXmlGradebookSettings, Is.Not.Null);
        Assert.That(xmlBackupFactory.GradebookXmlGradebook, Is.Not.Null);
        Assert.That(xmlBackupFactory.GroupsXmlGroupingsList, Is.Not.Null);
        Assert.That(xmlBackupFactory.GroupsXmlGroups, Is.Not.Null);
        Assert.That(xmlBackupFactory.MoodleBackupXmlDetail, Is.Not.Null);
        Assert.That(xmlBackupFactory.MoodleBackupXmlDetails, Is.Not.Null);
        Assert.That(xmlBackupFactory.MoodleBackupXmlSection, Is.Not.Null);
        Assert.That(xmlBackupFactory.MoodleBackupXmlSections, Is.Not.Null);
        Assert.That(xmlBackupFactory.MoodleBackupXmlCourse, Is.Not.Null);
        Assert.That(xmlBackupFactory.MoodleBackupXmlContents, Is.Not.Null);
        Assert.That(xmlBackupFactory.MoodleBackupXmlSettingFilename, Is.Not.Null);
        Assert.That(xmlBackupFactory.MoodleBackupXmlSettingImscc11, Is.Not.Null);
        Assert.That(xmlBackupFactory.MoodleBackupXmlSettingUsers, Is.Not.Null);
        Assert.That(xmlBackupFactory.MoodleBackupXmlSettingAnonymize, Is.Not.Null);
        Assert.That(xmlBackupFactory.MoodleBackupXmlSettingRoleAssignments, Is.Not.Null);
        Assert.That(xmlBackupFactory.MoodleBackupXmlSettingActivities, Is.Not.Null);
        Assert.That(xmlBackupFactory.MoodleBackupXmlSettingBlocks, Is.Not.Null);
        Assert.That(xmlBackupFactory.MoodleBackupXmlSettingFiles, Is.Not.Null);
        Assert.That(xmlBackupFactory.MoodleBackupXmlSettingFilters, Is.Not.Null);
        Assert.That(xmlBackupFactory.MoodleBackupXmlSettingComments, Is.Not.Null);
        Assert.That(xmlBackupFactory.MoodleBackupXmlSettingBadges, Is.Not.Null);
        Assert.That(xmlBackupFactory.MoodleBackupXmlSettingCalendarevents, Is.Not.Null);
        Assert.That(xmlBackupFactory.MoodleBackupXmlSettingUserscompletion, Is.Not.Null);
        Assert.That(xmlBackupFactory.MoodleBackupXmlSettingLogs, Is.Not.Null);
        Assert.That(xmlBackupFactory.MoodleBackupXmlSettingGradeHistories, Is.Not.Null);
        Assert.That(xmlBackupFactory.MoodleBackupXmlSettingQuestionbank, Is.Not.Null);
        Assert.That(xmlBackupFactory.MoodleBackupXmlSettingGroups, Is.Not.Null);
        Assert.That(xmlBackupFactory.MoodleBackupXmlSettingCompetencies, Is.Not.Null);
        Assert.That(xmlBackupFactory.MoodleBackupXmlSettingCustomfield, Is.Not.Null);
        Assert.That(xmlBackupFactory.MoodleBackupXmlSettingContentbankcontent, Is.Not.Null);
        Assert.That(xmlBackupFactory.MoodleBackupXmlSettingLegacyfiles, Is.Not.Null);
        Assert.That(xmlBackupFactory.MoodleBackupXmlSettings, Is.Not.Null);
        Assert.That(xmlBackupFactory.MoodleBackupXmlInformation, Is.Not.Null);
        Assert.That(xmlBackupFactory.MoodleBackupXmlMoodleBackup, Is.Not.Null);
        Assert.That(xmlBackupFactory.OutcomesXmlOutcomesDefinition, Is.Not.Null);
        Assert.That(xmlBackupFactory.QuestionsXmlQuestionsCategories, Is.Not.Null);
        Assert.That(xmlBackupFactory.ScalesXmlScalesDefinition, Is.Not.Null);
        Assert.That(xmlBackupFactory.RolesXmlRole, Is.Not.Null);
        Assert.That(xmlBackupFactory.RolesXmlRolesDefinition, Is.Not.Null);
    }

    [Test]
    public void CreateGradebookXml_GradeItemsGradeCategoriesGradeSettingGradeSettingsGradebook_AndSerialized()
    {
        //Arrange
        var mockReadAtf = Substitute.For<IReadAtf>();
        var mockContextId = 12345;

        var learningWorldJson = new LearningWorldJson("world", "",
            new List<ITopicJson> { new TopicJson(1, "Topic", new List<int> { 1 }) }, new List<ILearningSpaceJson>
            {
                new LearningSpaceJson(1,
                    "", "space", new List<int?> { 1 }, 0, "", "")
            },
            new List<IElementJson>
            {
                new LearningElementJson(1, "", "", "", "", "h5p",
                    0, 2, "", "")
            });
        mockReadAtf.GetResourceElementList().Returns(new List<ILearningElementJson>());
        mockReadAtf.GetLearningWorld().Returns(learningWorldJson);
        mockReadAtf.GetH5PElementsList().Returns(new List<ILearningElementJson>());
        mockReadAtf.GetBaseLearningElementsList().Returns(new List<IBaseLearningElementJson>());

        var mockGradeItems = Substitute.For<IGradebookXmlGradeItems>();
        var mockGradeCategories = Substitute.For<IGradebookXmlGradeCategories>();
        var mockGradeSetting = new GradebookXmlGradeSetting();
        var mockGradeSettings = Substitute.For<IGradebookXmlGradeSettings>();
        var mockGradebook = Substitute.For<IGradebookXmlGradebook>();

        var currentTime = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();

        var mockGradeItem = new GradebookXmlGradeItem
        {
            Timecreated = currentTime,
            Timemodified = currentTime,
        };

        var mockGradeCategory = new GradebookXmlGradeCategory
        {
            Timecreated = currentTime,
            Timemodified = currentTime,
        };

        var systemUnderTest = new XmlBackupFactory(mockReadAtf, mockContextId, gradebookXmlGradeItems: mockGradeItems,
            gradebookXmlGradebookSetting: mockGradeSetting, gradebookXmlGradeItem: mockGradeItem,
            gradebookXmlGradeCategory: mockGradeCategory, gradebookXmlGradeCategories: mockGradeCategories,
            gradebookXmlGradebookSettings: mockGradeSettings, gradebookXmlGradebook: mockGradebook);


        //Act
        systemUnderTest.CreateGradebookXml();

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.GradebookXmlGradeItem, Is.EqualTo(mockGradeItem));
            Assert.That(systemUnderTest.GradebookXmlGradeItems, Is.EqualTo(mockGradeItems));
            Assert.That(systemUnderTest.GradebookXmlGradebookSetting, Is.EqualTo(mockGradeSetting));
            Assert.That(systemUnderTest.GradebookXmlGradeCategories, Is.EqualTo(mockGradeCategories));
            Assert.That(systemUnderTest.GradebookXmlGradebookSettings, Is.EqualTo(mockGradeSettings));
            Assert.That(systemUnderTest.GradebookXmlGradeCategory, Is.EqualTo(mockGradeCategory));
            systemUnderTest.GradebookXmlGradebook.Received().Serialize();
        });
    }

    [Test]
    public void CreateGroupsXml_SetsGroupsGroupingsList_AndSerialized()
    {
        //Arrange
        var mockReadAtf = Substitute.For<IReadAtf>();
        var mockContextId = 12345;

        var learningWorldJson = new LearningWorldJson("", "world",
            new List<ITopicJson> { new TopicJson(1, "Topic", new List<int> { 1 }) }, new List<ILearningSpaceJson>
            {
                new LearningSpaceJson(1,
                    "", "space", new List<int?> { 1 }, 0, "", "")
            },
            new List<IElementJson>
            {
                new LearningElementJson(1, "", "", "", "", "h5p",
                    0, 2, "")
            });
        mockReadAtf.GetResourceElementList().Returns(new List<ILearningElementJson>());
        mockReadAtf.GetLearningWorld().Returns(learningWorldJson);
        mockReadAtf.GetH5PElementsList().Returns(new List<ILearningElementJson>());
        var mockGroups = Substitute.For<IGroupsXmlGroups>();
        var mockGroupingsList = new GroupsXmlGroupingsList();
        var systemUnderTest = new XmlBackupFactory(mockReadAtf, mockContextId, groupsXmlGroups: mockGroups,
            groupsXmlGroupingsList: mockGroupingsList);

        //Act
        systemUnderTest.CreateGroupsXml();

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.GroupsXmlGroups.GroupingsList, Is.EqualTo(mockGroupingsList));
            systemUnderTest.GroupsXmlGroups.Received().Serialize();
        });
    }


    [Test]
    public void
        CreateMoodleBackupXml_SetsDetailDetailsSettingSettingsContentsInformationMoodleBackupActivitiesSectionsCourse_AndSerialized()
    {
        //Arrange
        var mockReadAtf = Substitute.For<IReadAtf>();
        var mockDetail = new MoodleBackupXmlDetail();
        var mockDetails = Substitute.For<IMoodleBackupXmlDetails>();
        var mockSetting = Substitute.For<IMoodleBackupXmlSetting>();
        Substitute.For<IMoodleBackupXmlSettings>();
        var mockContents = Substitute.For<IMoodleBackupXmlContents>();
        var mockInformation = Substitute.For<IMoodleBackupXmlInformation>();
        var mockMoodleBackup = Substitute.For<IMoodleBackupXmlMoodleBackup>();
        var mockAktivities = Substitute.For<IMoodleBackupXmlActivities>();
        var mockSections = Substitute.For<IMoodleBackupXmlSections>();
        var mockCourse = Substitute.For<IMoodleBackupXmlCourse>();
        var mockContextId = 12345;

        var mockLearningWorld = new LearningWorldJson("world", "",
            new List<ITopicJson>(), new List<ILearningSpaceJson>(), new List<IElementJson>());

        var mockLearningElement1 = new LearningElementJson(1, "", "element1", "", "", "h5p", 0, 2, "");
        var mockLearningElement2 = new LearningElementJson(2, "", "element2", "", "", "url", 0, 2, "");
        var adaptivityElementJson1 = new AdaptivityElementJson(3,
            "", "element1", "adaptivity", "adaptivity", 2, 5, "",
            new AdaptivityContentJson("introText1", new List<IAdaptivityTaskJson>()
            {
                new AdaptivityTaskJson(1, "", "task1", false, 100, new List<IAdaptivityQuestionJson>()
                {
                    new AdaptivityQuestionJson(ResponseType.singleResponse, 1, "", 100, "question1",
                        new List<IAdaptivityRuleJson>()
                        {
                            new AdaptivityRuleJson(1, "incorrect", new CommentActionJson("Falsche Antwort1")),
                        },
                        new List<IChoiceJson>(new IChoiceJson[]
                            { new ChoiceJson("choice11", true), new ChoiceJson("choice12", false) })),
                })
            }));
        var adaptivityElements = new List<IAdaptivityElementJson> { adaptivityElementJson1 };
        var baseElements = new List<IBaseLearningElementJson>()
            { new BaseLearningElementJson(4, "", "element4", "", "h5p", "h5p") };

        List<ILearningElementJson> learningElementJsons = new()
        {
            mockLearningElement1,
            mockLearningElement2
        };

        List<IElementJson> elementJsons = new()
        {
            mockLearningElement1,
            mockLearningElement2,
            adaptivityElementJson1,
            baseElements[0]
        };

        var mockLearningSpaceContent = new List<int?> { mockLearningElement1.ElementId };
        var mockLearningSpace = new LearningSpaceJson(1, "", "", mockLearningSpaceContent, 0, "", "");
        List<ILearningSpaceJson> learningSpacesJsons = new() { mockLearningSpace };


        var mockAtfDocumentJson = new LearningElementJson(2, "", "", "", "", "json", 0, 2, "");

        mockReadAtf.GetLearningWorld().Returns(mockLearningWorld);
        mockReadAtf.GetH5PElementsList().Returns(learningElementJsons);
        mockReadAtf.GetSpaceList().Returns(learningSpacesJsons);
        mockReadAtf.GetResourceElementList().Returns(new List<ILearningElementJson> { mockAtfDocumentJson });
        mockReadAtf.GetAdaptivityElementsList().Returns(adaptivityElements);
        mockReadAtf.GetBaseLearningElementsList().Returns(baseElements);

        learningElementJsons.Add(mockAtfDocumentJson);

        mockReadAtf.GetElementsOrderedList().Returns(elementJsons);

        var systemUnderTest = new XmlBackupFactory(mockReadAtf, mockContextId, moodleBackupXmlDetail: mockDetail,
            moodleBackupXmlDetails: mockDetails,
            moodleBackupXmlSetting: mockSetting, moodleBackupXmlContents: mockContents,
            moodleBackupXmlInformation: mockInformation, moodleBackupXmlMoodleBackup: mockMoodleBackup,
            moodleBackupXmlActivities: mockAktivities,
            moodleBackupXmlSections: mockSections, moodleBackupXmlCourse: mockCourse);

        //Act
        systemUnderTest.CreateMoodleBackupXml();

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.MoodleBackupXmlDetails.Detail, Is.EqualTo(mockDetail));
            Assert.That(systemUnderTest.MoodleBackupXmlCourse.Title, Is.EqualTo("world"));
            Assert.That(systemUnderTest.MoodleBackupXmlSettingLegacyfiles.Name, Is.EqualTo("legacyfiles"));
            Assert.That(systemUnderTest.MoodleBackupXmlSettingFiles.Value, Is.EqualTo("1"));
            Assert.That(systemUnderTest.MoodleBackupXmlActivityList[0].ModuleName, Is.EqualTo("h5pactivity"));
            Assert.That(systemUnderTest.MoodleBackupXmlActivityList[1].ModuleId,
                Is.EqualTo(mockLearningElement2.ElementId.ToString()));
            Assert.That(systemUnderTest.MoodleBackupXmlActivityList[1].ModuleName, Is.EqualTo("url"));
            Assert.That(systemUnderTest.MoodleBackupXmlActivityList[1].Title, Is.EqualTo("element2"));
            Assert.That(systemUnderTest.MoodleBackupXmlActivityList[1].Directory,
                Is.EqualTo("activities/url_" + mockAtfDocumentJson.ElementId));

            Assert.That(systemUnderTest.MoodleBackupXmlSettingList[2].Level, Is.EqualTo("activity"));
            Assert.That(systemUnderTest.MoodleBackupXmlSettingList[2].Name,
                Is.EqualTo("url_" + mockAtfDocumentJson.ElementId + "_included"));
            Assert.That(systemUnderTest.MoodleBackupXmlSettingList[2].Value, Is.EqualTo("1"));

            Assert.That(systemUnderTest.MoodleBackupXmlSettingList[3].Level, Is.EqualTo("activity"));
            Assert.That(systemUnderTest.MoodleBackupXmlSettingList[3].Name,
                Is.EqualTo("url_" + mockAtfDocumentJson.ElementId + "_userinfo"));
            Assert.That(systemUnderTest.MoodleBackupXmlSettingList[3].Value, Is.EqualTo("0"));

            Assert.That(systemUnderTest.MoodleBackupXmlSettingList[4].Level, Is.EqualTo("activity"));
            Assert.That(systemUnderTest.MoodleBackupXmlSettingList[4].Name,
                Is.EqualTo("adleradaptivity_" + adaptivityElementJson1.ElementId + "_included"));
            Assert.That(systemUnderTest.MoodleBackupXmlSettingList[4].Value, Is.EqualTo("1"));

            Assert.That(systemUnderTest.MoodleBackupXmlSettingList[5].Level, Is.EqualTo("activity"));
            Assert.That(systemUnderTest.MoodleBackupXmlSettingList[5].Name,
                Is.EqualTo("adleradaptivity_" + adaptivityElementJson1.ElementId + "_userinfo"));
            Assert.That(systemUnderTest.MoodleBackupXmlSettingList[5].Value, Is.EqualTo("0"));

            Assert.That(systemUnderTest.MoodleBackupXmlSettingList[6].Level, Is.EqualTo("activity"));
            Assert.That(systemUnderTest.MoodleBackupXmlSettingList[6].Name,
                Is.EqualTo("h5pactivity_" + baseElements[0].ElementId + "_included"));
            Assert.That(systemUnderTest.MoodleBackupXmlSettingList[6].Value, Is.EqualTo("1"));

            Assert.That(systemUnderTest.MoodleBackupXmlInformation, Is.EqualTo(mockInformation));
            systemUnderTest.MoodleBackupXmlMoodleBackup.Received().Serialize();
        });
    }

    [Test]
    public void CreateOutcomesXml_Serializes()
    {
        //Arrange
        var xsdFileProvider = new XsdFileProvider();
        var mockReadAtf = Substitute.For<IReadAtf>();
        var mockContextId = 12345;
        var learningWorldJson = new LearningWorldJson("", "world",
            new List<ITopicJson> { new TopicJson(1, "Topic", new List<int> { 1 }) }, new List<ILearningSpaceJson>
            {
                new LearningSpaceJson(1,
                    "", "space", new List<int?> { 1 }, 0, "", "")
            },
            new List<IElementJson>
            {
                new LearningElementJson(1, "", "", "", "", "h5p",
                    0, 2, "")
            });
        mockReadAtf.GetResourceElementList().Returns(new List<ILearningElementJson>());
        mockReadAtf.GetLearningWorld().Returns(learningWorldJson);
        mockReadAtf.GetH5PElementsList().Returns(new List<ILearningElementJson>());

        var mockOutcomes = Substitute.For<IOutcomesXmlOutcomesDefinition>();
        var systemUnderTest =
            new XmlBackupFactory(mockReadAtf, mockContextId, outcomesXmlOutcomesDefinition: mockOutcomes);

        //Act
        systemUnderTest.CreateOutcomesXml();

        //Assert
        systemUnderTest.OutcomesXmlOutcomesDefinition.Received().Serialize();
    }

    [Test]
    public void CreateQuestionsXml_Serializes()
    {
        //Arrange
        var mockContextId = 12345;
        var mockReadAtf = Substitute.For<IReadAtf>();
        var learningWorldJson = new LearningWorldJson("", "world",
            new List<ITopicJson> { new TopicJson(1, "Topic", new List<int> { 1 }) }, new List<ILearningSpaceJson>
            {
                new LearningSpaceJson(1,
                    "", "space", new List<int?> { 1 }, 0, "", "")
            },
            new List<IElementJson>
            {
                new LearningElementJson(1, "", "", "", "", "h5p",
                    0, 2, "")
            });

        var adaptivityElementJson1 = new AdaptivityElementJson(1,
            "", "element1", "adaptivity", "adaptivity", 2, 5, "",
            new AdaptivityContentJson("introText1", new List<IAdaptivityTaskJson>()
            {
                new AdaptivityTaskJson(1, "", "task1", false, 100, new List<IAdaptivityQuestionJson>()
                {
                    new AdaptivityQuestionJson(ResponseType.singleResponse, 1, "", 100, "question1",
                        new List<IAdaptivityRuleJson>()
                        {
                            new AdaptivityRuleJson(1, "incorrect", new CommentActionJson("Falsche Antwort1")),
                            new AdaptivityRuleJson(2, "incorrect", new ContentReferenceActionJson(3, "hinText1"))
                        },
                        new List<IChoiceJson>(new IChoiceJson[]
                            { new ChoiceJson("choice11", true), new ChoiceJson("choice12", false) })),
                    new AdaptivityQuestionJson(ResponseType.multipleResponse, 2, "", 200, "question2",
                        new List<IAdaptivityRuleJson>()
                        {
                            new AdaptivityRuleJson(3, "incorrect", new ElementReferenceActionJson(4, "hinText2")),
                            new AdaptivityRuleJson(4, "incorrect", new ContentReferenceActionJson(3, "hinText3"))
                        },
                        new List<IChoiceJson>(new IChoiceJson[]
                        {
                            new ChoiceJson("choice121", true), new ChoiceJson("choiceText122", false),
                            new ChoiceJson("choice123", true)
                        }))
                })
            }));

        var adaptivityElementJson2 = new AdaptivityElementJson(2,
            "", "element2", "adaptivity", "adaptivity", 2, 5, "",
            new AdaptivityContentJson("introText2", new List<IAdaptivityTaskJson>()
            {
                new AdaptivityTaskJson(1, "", "task2", false, 100, new List<IAdaptivityQuestionJson>()
                {
                    new AdaptivityQuestionJson(ResponseType.singleResponse, 3, "", 100, "questionText2",
                        new List<IAdaptivityRuleJson>()
                        {
                            new AdaptivityRuleJson(2, "incorrect", new ElementReferenceActionJson(3, null))
                        },
                        new List<IChoiceJson>(new IChoiceJson[]
                            { new ChoiceJson("choiceText1", true), new ChoiceJson("choiceText2", false) }))
                })
            }));

        var baseElementJson = new BaseLearningElementJson(3, "", "element6", "", "h5p", "h5p");

        var learningElementJson4 = new LearningElementJson(4,
            "", "element4", "", "", "label", 1, 4, "");

        var mockAdaptivityElements = new List<IAdaptivityElementJson>()
            { adaptivityElementJson1, adaptivityElementJson2 };
        ;

        var mockSpace = new LearningSpaceJson(1, "", "space1", new List<int?> { 1, 2, 3, 4 }, 0, "", "");

        mockReadAtf.GetResourceElementList().Returns(new List<ILearningElementJson>());
        mockReadAtf.GetLearningWorld().Returns(learningWorldJson);
        mockReadAtf.GetH5PElementsList().Returns(new List<ILearningElementJson>());
        mockReadAtf.GetAdaptivityElementsList().Returns(mockAdaptivityElements);
        mockReadAtf.GetBaseLearningElementsList().Returns(new List<IBaseLearningElementJson>() { baseElementJson });
        mockReadAtf.GetElementsOrderedList().Returns(new List<IElementJson>()
            { mockAdaptivityElements[0], mockAdaptivityElements[1], baseElementJson, learningElementJson4 });
        mockReadAtf.GetSpaceList().Returns(new List<ILearningSpaceJson>() { mockSpace });

        var mockQuestion = Substitute.For<IQuestionsXmlQuestionsCategories>();
        var mockQuestionCategories = new List<QuestionsXmlQuestionsCategory>();
        mockQuestion.QuestionCategory.Returns(mockQuestionCategories);
        var systemUnderTest =
            new XmlBackupFactory(mockReadAtf, mockContextId, questionsXmlQuestionsCategories: mockQuestion);

        //Act
        systemUnderTest.CreateQuestionsXml();

        //Assert
        systemUnderTest.QuestionsXmlQuestionsCategories.Received().Serialize();
        Assert.That(systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory.Count, Is.EqualTo(2));
        Assert.That(systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[0].Name, Is.EqualTo("top"));
        Assert.That(systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[0].ContextId,
            Is.EqualTo(mockContextId.ToString()));
        Assert.That(systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[0].ContextLevel, Is.EqualTo("50"));
        Assert.That(systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[0].ContextInstanceId,
            Is.EqualTo("1"));
        Assert.That(systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[0].Id, Is.EqualTo("3"));
        Assert.That(systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[0].Parent, Is.EqualTo("0"));
        Assert.That(systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[0].SortOrder, Is.EqualTo("0"));
        Assert.That(systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[0].IdNumber,
            Is.EqualTo("$@NULL@$"));
        Assert.That(systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[0].Info, Is.EqualTo(""));
        Assert.That(systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[0].InfoFormat, Is.EqualTo("0"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[0].QuestionBankEntries.QuestionBankEntries,
            Has.Count.EqualTo(0));

        Assert.That(systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].Name,
            Is.EqualTo("Default for name"));
        Assert.That(systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].ContextId,
            Is.EqualTo(mockContextId.ToString()));
        Assert.That(systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].ContextLevel, Is.EqualTo("50"));
        Assert.That(systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].ContextInstanceId,
            Is.EqualTo("1"));
        Assert.That(systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].Id, Is.EqualTo("4"));
        Assert.That(systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].Parent, Is.EqualTo("3"));
        Assert.That(systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].SortOrder, Is.EqualTo("0"));
        Assert.That(systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].IdNumber,
            Is.EqualTo("$@NULL@$"));
        Assert.That(systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].Info, Is.EqualTo(""));
        Assert.That(systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].InfoFormat, Is.EqualTo("0"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries.QuestionBankEntries,
            Has.Count.EqualTo(3));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[0].Id, Is.EqualTo("1"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[0].QuestionCategoryId, Is.EqualTo("4"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[0].IdNumber,
            Is.EqualTo(adaptivityElementJson1.AdaptivityContent.AdaptivityTasks[0].AdaptivityQuestions[0]
                .QuestionUUID));

        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[0].QuestionVersion.QuestionVersions, Has.Count.EqualTo(1));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[0].QuestionVersion.QuestionVersions[0].Id, Is.EqualTo("1"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[0].QuestionVersion.QuestionVersions[0].Version, Is.EqualTo("1"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[0].QuestionVersion.QuestionVersions[0].Status, Is.EqualTo("ready"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[0].QuestionVersion.QuestionVersions[0].Questions.Question, Has.Count.EqualTo(1));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[0].QuestionVersion.QuestionVersions[0].Questions.Question[0].Id, Is.EqualTo("1"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[0].QuestionVersion.QuestionVersions[0].Questions.Question[0].Parent,
            Is.EqualTo("0"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[0].QuestionVersion.QuestionVersions[0].Questions.Question[0].Name,
            Is.EqualTo("question1"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[0].QuestionVersion.QuestionVersions[0].Questions.Question[0].QuestionText,
            Is.EqualTo(adaptivityElementJson1.AdaptivityContent.AdaptivityTasks[0].AdaptivityQuestions[0]
                .QuestionText));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[0].QuestionVersion.QuestionVersions[0].Questions.Question[0].QuestionTextFormat,
            Is.EqualTo("1"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[0].QuestionVersion.QuestionVersions[0].Questions.Question[0].GeneralFeedback,
            Is.EqualTo(""));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[0].QuestionVersion.QuestionVersions[0].Questions.Question[0].GeneralFeedbackFormat,
            Is.EqualTo("1"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[0].QuestionVersion.QuestionVersions[0].Questions.Question[0].DefaultMark,
            Is.EqualTo("1.0000000"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[0].QuestionVersion.QuestionVersions[0].Questions.Question[0].Penalty,
            Is.EqualTo("0.3333333"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[0].QuestionVersion.QuestionVersions[0].Questions.Question[0].QType,
            Is.EqualTo("multichoice"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[0].QuestionVersion.QuestionVersions[0].Questions.Question[0].Length,
            Is.EqualTo("1"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[0].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Answers.Answer, Has.Count.EqualTo(2));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[0].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Answers.Answer[0].Id, Is.EqualTo("1"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[0].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Answers.Answer[0].Answertext,
            Is.EqualTo(adaptivityElementJson1.AdaptivityContent.AdaptivityTasks[0].AdaptivityQuestions[0].Choices[0]
                .AnswerText));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[0].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Answers.Answer[0].Answerformat, Is.EqualTo("1"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[0].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Answers.Answer[0].Fraction, Is.EqualTo("1.0000000"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[0].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Answers.Answer[0].Feedback, Is.EqualTo(""));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[0].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Answers.Answer[0].Feedbackformat, Is.EqualTo("1"));

        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[0].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Answers.Answer[1].Id, Is.EqualTo("2"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[0].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Answers.Answer[1].Answertext,
            Is.EqualTo(adaptivityElementJson1.AdaptivityContent.AdaptivityTasks[0].AdaptivityQuestions[0].Choices[1]
                .AnswerText));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[0].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Answers.Answer[1].Fraction, Is.EqualTo("-1.0000000"));

        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[0].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Multichoice.Id, Is.EqualTo("1"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[0].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Multichoice.Layout, Is.EqualTo("0"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[0].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Multichoice.Single, Is.EqualTo("1"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[0].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Multichoice.Correctfeedback, Is.EqualTo("Diese Antwort ist korrekt."));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[0].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Multichoice.Partiallycorrectfeedback,
            Is.EqualTo(
                "Diese Antwort ist falsch. <br>Hinweis: Falsche Antwort1<br>Schaue dir noch mal das Lernelement element6 an. <br>hinText1<br>"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[0].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Multichoice.Incorrectfeedback,
            Is.EqualTo(
                "Diese Antwort ist falsch. <br>Hinweis: Falsche Antwort1<br>Schaue dir noch mal das Lernelement element6 an. <br>hinText1<br>"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[0].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Multichoice.Shuffleanswers, Is.EqualTo("0"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[0].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Multichoice.Answernumbering, Is.EqualTo("abc"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[0].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Multichoice.Shownumcorrect, Is.EqualTo("1"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[0].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Multichoice.Showstandardinstruction, Is.EqualTo("0"));


        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[1].Id, Is.EqualTo("2"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[1].QuestionCategoryId, Is.EqualTo("4"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[1].IdNumber,
            Is.EqualTo(adaptivityElementJson1.AdaptivityContent.AdaptivityTasks[0].AdaptivityQuestions[1]
                .QuestionUUID));

        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[1].QuestionVersion.QuestionVersions[0].Id, Is.EqualTo("2"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[1].QuestionVersion.QuestionVersions[0].Questions.Question, Has.Count.EqualTo(1));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[1].QuestionVersion.QuestionVersions[0].Questions.Question[0].Id, Is.EqualTo("2"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[1].QuestionVersion.QuestionVersions[0].Questions.Question[0].QuestionText,
            Is.EqualTo(adaptivityElementJson1.AdaptivityContent.AdaptivityTasks[0].AdaptivityQuestions[1]
                .QuestionText));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[1].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Answers.Answer, Has.Count.EqualTo(3));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[1].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Answers.Answer[0].Id, Is.EqualTo("3"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[1].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Answers.Answer[0].Answertext,
            Is.EqualTo(adaptivityElementJson1.AdaptivityContent.AdaptivityTasks[0].AdaptivityQuestions[1].Choices[0]
                .AnswerText));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[1].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Answers.Answer[0].Fraction, Is.EqualTo("0.5000000"));

        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[1].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Answers.Answer[1].Id, Is.EqualTo("4"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[1].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Answers.Answer[1].Answertext,
            Is.EqualTo(adaptivityElementJson1.AdaptivityContent.AdaptivityTasks[0].AdaptivityQuestions[1].Choices[1]
                .AnswerText));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[1].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Answers.Answer[1].Fraction, Is.EqualTo("-1.0000000"));

        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[1].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Answers.Answer[2].Id, Is.EqualTo("5"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[1].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Answers.Answer[2].Answertext,
            Is.EqualTo(adaptivityElementJson1.AdaptivityContent.AdaptivityTasks[0].AdaptivityQuestions[1].Choices[2]
                .AnswerText));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[1].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Answers.Answer[2].Fraction, Is.EqualTo("0.5000000"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[1].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Multichoice.Partiallycorrectfeedback,
            Is.EqualTo(
                "Diese Antwort ist falsch. <br>Hinweis: Schaue dir noch mal das Lernelement element4 in Raum \"space1\" an. <br>hinText2<br>Schaue dir noch mal das Lernelement element6 an. <br>hinText3<br>"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[1].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Multichoice.Incorrectfeedback,
            Is.EqualTo(
                "Diese Antwort ist falsch. <br>Hinweis: Schaue dir noch mal das Lernelement element4 in Raum \"space1\" an. <br>hinText2<br>Schaue dir noch mal das Lernelement element6 an. <br>hinText3<br>"));

        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[2].Id, Is.EqualTo("3"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[2].QuestionCategoryId, Is.EqualTo("4"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[2].IdNumber,
            Is.EqualTo(adaptivityElementJson2.AdaptivityContent.AdaptivityTasks[0].AdaptivityQuestions[0]
                .QuestionUUID));

        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[2].QuestionVersion.QuestionVersions[0].Id, Is.EqualTo("3"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[2].QuestionVersion.QuestionVersions[0].Questions.Question, Has.Count.EqualTo(1));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[2].QuestionVersion.QuestionVersions[0].Questions.Question[0].Id, Is.EqualTo("3"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[2].QuestionVersion.QuestionVersions[0].Questions.Question[0].QuestionText,
            Is.EqualTo(adaptivityElementJson2.AdaptivityContent.AdaptivityTasks[0].AdaptivityQuestions[0]
                .QuestionText));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[2].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Answers.Answer, Has.Count.EqualTo(2));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[2].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Answers.Answer[0].Id, Is.EqualTo("6"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[2].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Answers.Answer[0].Answertext,
            Is.EqualTo(adaptivityElementJson2.AdaptivityContent.AdaptivityTasks[0].AdaptivityQuestions[0].Choices[0]
                .AnswerText));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[2].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Answers.Answer[0].Fraction, Is.EqualTo("1.0000000"));

        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[2].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Answers.Answer[1].Id, Is.EqualTo("7"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[2].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Answers.Answer[1].Answertext,
            Is.EqualTo(adaptivityElementJson2.AdaptivityContent.AdaptivityTasks[0].AdaptivityQuestions[0].Choices[1]
                .AnswerText));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[2].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Answers.Answer[1].Fraction, Is.EqualTo("-1.0000000"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[2].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Multichoice.Partiallycorrectfeedback,
            Is.EqualTo(
                "Diese Antwort ist falsch. <br>Hinweis: Schaue dir noch mal das Lernelement element6 in Raum \"space1\" an. <br>"));
        Assert.That(
            systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory[1].QuestionBankEntries
                .QuestionBankEntries[2].QuestionVersion.QuestionVersions[0].Questions.Question[0]
                .PluginQTypeMultichoiceQuestion.Multichoice.Incorrectfeedback,
            Is.EqualTo(
                "Diese Antwort ist falsch. <br>Hinweis: Schaue dir noch mal das Lernelement element6 in Raum \"space1\" an. <br>"));

        var questionsCategories = new QuestionsXmlQuestionsCategories
        {
            QuestionCategory = systemUnderTest.QuestionsXmlQuestionsCategories.QuestionCategory
        };

        var serializedXml = XmlSerializerHelper.SerializeObjectToXmlString(questionsCategories);

        var questionXmlXsd = XsdFileProvider.QuestionXmlXsd;

        var isValid = XmlSerializerHelper.ValidateXmlAgainstXsd(serializedXml, questionXmlXsd);
        Assert.That(isValid, Is.True);
    }

    [Test]
    public void CreateRolesXml_SetsRoleInRoleDefinition_AndSerializes()
    {
        //Arrange
        var mockReadAtf = Substitute.For<IReadAtf>();
        var mockContextId = 12345;
        var learningWorldJson = new LearningWorldJson("", "world",
            new List<ITopicJson> { new TopicJson(1, "Topic", new List<int> { 1 }) }, new List<ILearningSpaceJson>
            {
                new LearningSpaceJson(1,
                    "", "space", new List<int?> { 1 }, 0, "", "")
            },
            new List<IElementJson>
            {
                new LearningElementJson(1, "", "", "", "", "h5p",
                    0, 2, "")
            });
        mockReadAtf.GetResourceElementList().Returns(new List<ILearningElementJson>());
        mockReadAtf.GetLearningWorld().Returns(learningWorldJson);
        mockReadAtf.GetH5PElementsList().Returns(new List<ILearningElementJson>());

        var mockRolesDefinition = Substitute.For<IRolesXmlRolesDefinition>();
        var mockRole = new RolesXmlRole();
        var systemUnderTest = new XmlBackupFactory(mockReadAtf, mockContextId, rolesXmlRole: mockRole,
            rolesXmlRolesDefinition: mockRolesDefinition);

        //Act
        systemUnderTest.CreateRolesXml();

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.RolesXmlRolesDefinition.Role, Is.EqualTo(mockRole));
            mockRolesDefinition.Received().Serialize();
        });
    }

    [Test]
    public void CreateScalesXml_Serializes()
    {
        //Arrange
        var mockReadAtf = Substitute.For<IReadAtf>();
        var mockContextId = 12345;
        var learningWorldJson = new LearningWorldJson("", "world",
            new List<ITopicJson> { new TopicJson(1, "Topic", new List<int> { 1 }) }, new List<ILearningSpaceJson>
            {
                new LearningSpaceJson(1,
                    "", "space", new List<int?> { 1 }, 0, "", "")
            },
            new List<IElementJson>
            {
                new LearningElementJson(1, "", "", "", "", "h5p",
                    0, 2, "")
            });
        mockReadAtf.GetResourceElementList().Returns(new List<ILearningElementJson>());
        mockReadAtf.GetLearningWorld().Returns(learningWorldJson);
        mockReadAtf.GetH5PElementsList().Returns(new List<ILearningElementJson>());

        var mockScales = Substitute.For<IScalesXmlScalesDefinition>();
        var systemUnderTest = new XmlBackupFactory(mockReadAtf, mockContextId, scalesXmlScalesDefinition: mockScales);

        //Act
        systemUnderTest.CreateScalesXml();

        //Assert
        systemUnderTest.ScalesXmlScalesDefinition.Received().Serialize();
    }
}