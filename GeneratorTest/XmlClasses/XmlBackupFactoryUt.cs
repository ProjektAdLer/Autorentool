using Generator.DSL;
using Generator.XmlClasses;
using Generator.XmlClasses.Entities.Gradebook.xml;
using Generator.XmlClasses.Entities.Groups.xml;
using Generator.XmlClasses.Entities.MoodleBackup.xml;
using Generator.XmlClasses.Entities.Outcomes.xml;
using Generator.XmlClasses.Entities.Questions.xml;
using Generator.XmlClasses.Entities.Roles.xml;
using Generator.XmlClasses.Entities.Scales.xml;
using NSubstitute;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses;

[TestFixture]
public class XmlBackupFactoryUt
{

    [Test]
    public void XmlBackupFactory_CreateXmlBackupFactory_AllMethodsCalled()
    {
        // Arrange
        var mockReadDsl = Substitute.For<IReadDsl>();

        var mockGradeItems = Substitute.For<IGradebookXmlGradeItems>();
        var mockGradeCategories = Substitute.For<IGradebookXmlGradeCategories>();
        var mockGradeSetting = new GradebookXmlGradeSetting();
        var mockGradeSettings = Substitute.For<IGradebookXmlGradeSettings>();
        var mockGradebook = Substitute.For<IGradebookXmlGradebook>();
        
        var currentTime = DateTimeOffset.Now.ToUnixTimeSeconds().ToString(); 
        
        var mockGradeItem = new GradebookXmlGradeItem()
        {
            Timecreated = currentTime,
            Timemodified = currentTime,
        };

        var mockGradeCategory = new GradebookXmlGradeCategory()
        {
            Timecreated = currentTime,
            Timemodified = currentTime,
        };
        
        var mockGroups = Substitute.For<IGroupsXmlGroups>();
        var mockGroupingsList = new GroupsXmlGroupingsList();
        
        var mockDetail = new MoodleBackupXmlDetail();
        var mockDetails = Substitute.For<IMoodleBackupXmlDetails>();
        var mockSetting = Substitute.For<IMoodleBackupXmlSetting>();
        var mockSettings = Substitute.For<IMoodleBackupXmlSettings>();
        var mockContents = Substitute.For<IMoodleBackupXmlContents>();
        var mockInformation = Substitute.For<IMoodleBackupXmlInformation>();
        var mockMoodleBackup = Substitute.For<IMoodleBackupXmlMoodleBackup>();
        var mockAktivities = Substitute.For<IMoodleBackupXmlActivities>();
        var mockSections = Substitute.For<IMoodleBackupXmlSections>();
        var mockCourse = Substitute.For<IMoodleBackupXmlCourse>();
        var mockIdentifier = new IdentifierJson("name", "Element_1");
        var mockLearningWorld = new LearningWorldJson("Uuid", mockIdentifier, new List<int>(),
            new List<TopicJson>(), new List<LearningSpaceJson>(), new List<LearningElementJson>());
        var mockDslDocument = new List<LearningElementJson>();
        
        mockLearningWorld.Identifier = mockIdentifier;

        var mockLearningElement = new LearningElementJson(1, mockIdentifier, "h5p");
        List<LearningElementJson> learningElementJsons = new List<LearningElementJson>();
        learningElementJsons.Add(mockLearningElement);

        var mockDslDocumentJson = new LearningElementJson(2, mockIdentifier, "json");
        mockDslDocument.Add(mockDslDocumentJson);

        mockReadDsl.GetLearningWorld().Returns(mockLearningWorld);
        mockReadDsl.GetH5PElementsList().Returns(learningElementJsons);
        mockReadDsl.GetDslDocumentList().Returns(mockDslDocument);
        
        var mockOutcomes = Substitute.For<IOutcomesXmlOutcomesDefinition>();
        
        var mockQuestion = Substitute.For<IQuestionsXmlQuestionsCategories>();
        
        var mockScales = Substitute.For<IScalesXmlScalesDefinition>();

        var mockRolesDefinition = Substitute.For<IRolesXmlRolesDefinition>();
        var mockRole = new RolesXmlRole();

        var systemUnderTest = new XmlBackupFactory(mockReadDsl, mockGradeItem, mockGradeItems, mockGradeCategory,
            mockGradeCategories,
            mockGradeSetting, mockGradeSettings, mockGradebook, mockGroupingsList, mockGroups, mockDetail, mockDetails,
            mockAktivities, null, mockSections, mockCourse, mockContents, mockSetting,
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
        //Arrange
        
        //Act
        var mockReadDsl = Substitute.For<IReadDsl>();
        var xmlBackupFactory = new XmlBackupFactory(mockReadDsl);

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
        var mockReadDsl = Substitute.For<IReadDsl>();
        
        var mockGradeItems = Substitute.For<IGradebookXmlGradeItems>();
        var mockGradeCategories = Substitute.For<IGradebookXmlGradeCategories>();
        var mockGradeSetting = new GradebookXmlGradeSetting();
        var mockGradeSettings = Substitute.For<IGradebookXmlGradeSettings>();
        var mockGradebook = Substitute.For<IGradebookXmlGradebook>();
        
        var currentTime = DateTimeOffset.Now.ToUnixTimeSeconds().ToString(); 
        
        var mockGradeItem = new GradebookXmlGradeItem()
        {
            Timecreated = currentTime,
            Timemodified = currentTime,
        };

        var mockGradeCategory = new GradebookXmlGradeCategory()
        {
            Timecreated = currentTime,
            Timemodified = currentTime,
        };

        var systemUnderTest = new XmlBackupFactory(mockReadDsl, gradebookXmlGradeItems: mockGradeItems,
            gradebookXmlGradebookSetting: mockGradeSetting,
            gradebookXmlGradeItem: mockGradeItem, gradebookXmlGradeCategory: mockGradeCategory,gradebookXmlGradeCategories:
            mockGradeCategories, gradebookXmlGradebookSettings: mockGradeSettings, gradebookXmlGradebook: mockGradebook);
        

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
        var mockReadDsl = Substitute.For<IReadDsl>();
        var mockGroups = Substitute.For<IGroupsXmlGroups>();
        var mockGroupingsList = new GroupsXmlGroupingsList();
        var systemUnderTest = new XmlBackupFactory(mockReadDsl, groupsXmlGroups: mockGroups, groupsXmlGroupingsList: mockGroupingsList);
        
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
    public void CreateMoodleBackupXml_SetsDetailDetailsSettingSettingsContentsInformationMoodleBackupAktivitiesSectionsCourse_AndSerialized()
    {
        //Arrange
        var mockReadDsl = Substitute.For<IReadDsl>();
        var mockDetail = new MoodleBackupXmlDetail();
        var mockDetails = Substitute.For<IMoodleBackupXmlDetails>();
        var mockSetting = Substitute.For<IMoodleBackupXmlSetting>();
        var mockSettings = Substitute.For<IMoodleBackupXmlSettings>();
        var mockContents = Substitute.For<IMoodleBackupXmlContents>();
        var mockInformation = Substitute.For<IMoodleBackupXmlInformation>();
        var mockMoodleBackup = Substitute.For<IMoodleBackupXmlMoodleBackup>();
        var mockAktivities = Substitute.For<IMoodleBackupXmlActivities>();
        var mockSections = Substitute.For<IMoodleBackupXmlSections>();
        var mockCourse = Substitute.For<IMoodleBackupXmlCourse>();
        var mockIdentifier = new IdentifierJson( "name", "Element_1");
       
        var mockDslDocument = new List<LearningElementJson>();
        
        var mockLearningWorld = new LearningWorldJson("Uuid", mockIdentifier, new List<int>(),
            new List<TopicJson>(), new List<LearningSpaceJson>(), new List<LearningElementJson>() );

        var mockLearningElement = new LearningElementJson(1, mockIdentifier, "h5p");
        List<LearningElementJson> learningElementJsons = new List<LearningElementJson>();
        learningElementJsons.Add(mockLearningElement);

        var mockDslDocumentJson = new LearningElementJson(2, mockIdentifier, "json");
        mockDslDocument.Add(mockDslDocumentJson);

        mockReadDsl.GetLearningWorld().Returns(mockLearningWorld);
        mockReadDsl.GetH5PElementsList().Returns(learningElementJsons);
        mockReadDsl.GetDslDocumentList().Returns(mockDslDocument);
        
        var systemUnderTest = new XmlBackupFactory(mockReadDsl, moodleBackupXmlDetail: mockDetail, moodleBackupXmlDetails: mockDetails,
            moodleBackupXmlSetting: mockSetting, moodleBackupXmlContents: mockContents,
            moodleBackupXmlInformation: mockInformation, moodleBackupXmlMoodleBackup: mockMoodleBackup, moodleBackupXmlActivities: mockAktivities,
            moodleBackupXmlSections: mockSections, moodleBackupXmlCourse: mockCourse);
        
        //Act
        systemUnderTest.CreateMoodleBackupXml();
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.MoodleBackupXmlDetails.Detail, Is.EqualTo(mockDetail));
            Assert.That(systemUnderTest.MoodleBackupXmlCourse.Title, Is.EqualTo(mockIdentifier.Value));
            Assert.That(systemUnderTest.MoodleBackupXmlSettingLegacyfiles.Name, Is.EqualTo("legacyfiles"));
            Assert.That(systemUnderTest.MoodleBackupXmlSettingFiles.Value, Is.EqualTo("1"));
            Assert.That(systemUnderTest.MoodleBackupXmlActivityList[0].ModuleId, Is.EqualTo(mockDslDocumentJson.Id.ToString()));
            Assert.That(systemUnderTest.MoodleBackupXmlActivityList[0].ModuleName, Is.EqualTo("resource"));
            Assert.That(systemUnderTest.MoodleBackupXmlActivityList[0].Title, Is.EqualTo(mockIdentifier.Value));
            Assert.That(systemUnderTest.MoodleBackupXmlActivityList[0].Directory, Is.EqualTo("activities/resource_" + mockDslDocumentJson.Id.ToString()));
            Assert.That(systemUnderTest.MoodleBackupXmlSettingList[0].Level, Is.EqualTo("section"));
            Assert.That(systemUnderTest.MoodleBackupXmlSettingList[0].Name, Is.EqualTo("section_" + mockDslDocumentJson.Id.ToString() + "_included"));
            Assert.That(systemUnderTest.MoodleBackupXmlSettingList[0].Value, Is.EqualTo("1"));
            Assert.That(systemUnderTest.MoodleBackupXmlSettingList[1].Section, Is.EqualTo("section_" + mockDslDocumentJson.Id.ToString()));
            Assert.That(systemUnderTest.MoodleBackupXmlActivityList[1].ModuleName, Is.EqualTo("h5pactivity"));
            Assert.That(systemUnderTest.MoodleBackupXmlInformation, Is.EqualTo(mockInformation));
            systemUnderTest.MoodleBackupXmlMoodleBackup.Received().Serialize();
        });
        

    }
    
    [Test]
    public void CreateOutcomesXml_Serializes()
    {
        //Arrange
        var mockReadDsl = Substitute.For<IReadDsl>();
        var mockOutcomes = Substitute.For<IOutcomesXmlOutcomesDefinition>();
        var systemUnderTest = new XmlBackupFactory(mockReadDsl, outcomesXmlOutcomesDefinition: mockOutcomes);
        
        //Act
        systemUnderTest.CreateOutcomesXml();

        //Assert
        systemUnderTest.OutcomesXmlOutcomesDefinition.Received().Serialize();
    }
    
    [Test]
    public void CreateQuestionsXml_Serializes()
    {
        //Arrange
        var mockReadDsl = Substitute.For<IReadDsl>();
        var mockQuestion = Substitute.For<IQuestionsXmlQuestionsCategories>();
        var systemUnderTest = new XmlBackupFactory(mockReadDsl, questionsXmlQuestionsCategories: mockQuestion);
        
        //Act
        systemUnderTest.CreateQuestionsXml();

        //Assert
        systemUnderTest.QuestionsXmlQuestionsCategories.Received().Serialize();
    }
    
    [Test]
    public void CreateRolesXml_SetsRoleInRoleDefinition_AndSerializes()
    {
        //Arrange
        var mockReadDsl = Substitute.For<IReadDsl>();
        var mockRolesDefinition = Substitute.For<IRolesXmlRolesDefinition>();
        var mockRole = new RolesXmlRole();
        var systemUnderTest = new XmlBackupFactory(mockReadDsl, rolesXmlRole: mockRole, rolesXmlRolesDefinition:mockRolesDefinition);
        
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
        var mockReadDsl = Substitute.For<IReadDsl>();
        var mockScales = Substitute.For<IScalesXmlScalesDefinition>();
        var systemUnderTest = new XmlBackupFactory(mockReadDsl, scalesXmlScalesDefinition: mockScales);
        
        //Act
        systemUnderTest.CreateScalesXml();
        
        //Assert
        systemUnderTest.ScalesXmlScalesDefinition.Received().Serialize();
    }
    
}