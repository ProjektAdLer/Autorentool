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

namespace GeneratorTest.XmlClasses;

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
        Substitute.For<IMoodleBackupXmlSettings>();
        var mockContents = Substitute.For<IMoodleBackupXmlContents>();
        var mockInformation = Substitute.For<IMoodleBackupXmlInformation>();
        var mockMoodleBackup = Substitute.For<IMoodleBackupXmlMoodleBackup>();
        var mockAktivities = Substitute.For<IMoodleBackupXmlActivities>();
        var mockSections = Substitute.For<IMoodleBackupXmlSections>();
        var mockCourse = Substitute.For<IMoodleBackupXmlCourse>();
        var mockElementValueList = new List<ElementValueJson>{new ElementValueJson("points","0")};
        var mockIdentifier = new IdentifierJson("name", "Identifier_Value");
        var mockWorld = new WorldJson("Uuid", mockIdentifier, new List<int>(),
            new List<TopicJson>(), new List<SpaceJson>(), new List<ElementJson>());
        
        mockWorld.Identifier = mockIdentifier;

        var mockElement = new ElementJson(1, mockIdentifier, "", "", "h5p",0, mockElementValueList);
        List<ElementJson> elementJsons = new List<ElementJson>();
        elementJsons.Add(mockElement);
        
        var mockSpaceContent = new List<int>();
        mockSpaceContent.Add(mockElement.Id);
        var mockSpace = new SpaceJson(1, mockIdentifier, mockSpaceContent, 0, 0);
        List<SpaceJson> spacesJsons = new List<SpaceJson>();
        spacesJsons.Add(mockSpace);

        var mockDslDocumentJson = new ElementJson(2, mockIdentifier, "", "", "json",0, mockElementValueList);
        var mockSpaceElementJson = new ElementJson(3, mockIdentifier, "", "", "space",0, mockElementValueList);

        mockReadDsl.GetWorld().Returns(mockWorld);
        mockReadDsl.GetH5PElementsList().Returns(elementJsons);
        mockReadDsl.GetSectionList().Returns(spacesJsons);
        mockReadDsl.GetResourceList().Returns(new List<ElementJson>(){mockDslDocumentJson});
        
        elementJsons.Add(mockDslDocumentJson);
        elementJsons.Add(mockSpaceElementJson);
        mockReadDsl.GetSpacesAndElementsOrderedList().Returns(elementJsons);
        
        var mockOutcomes = Substitute.For<IOutcomesXmlOutcomesDefinition>();
        
        var mockQuestion = Substitute.For<IQuestionsXmlQuestionsCategories>();
        
        var mockScales = Substitute.For<IScalesXmlScalesDefinition>();

        var mockRolesDefinition = Substitute.For<IRolesXmlRolesDefinition>();
        var mockRole = new RolesXmlRole();

        var systemUnderTest = new XmlBackupFactory(mockReadDsl, mockGradeItem, mockGradeItems, mockGradeCategory,
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
        //Arrange
        
        //Act
        var mockReadDsl = Substitute.For<IReadDsl>();
        
        var worldJson = new WorldJson("1", new IdentifierJson("1", "1"), new List<int>(){1},
            new List<TopicJson>() {new TopicJson()}, new List<SpaceJson>{new SpaceJson(1, 
                new IdentifierJson("1","1"), new List<int>(){1}, 0, 0)}, 
            new List<ElementJson>{new (1, new IdentifierJson("1","1"), "", "", "h5p", 
                0, new List<ElementValueJson>{new ("Points", "0")})});
        mockReadDsl.GetResourceList().Returns(new List<ElementJson>());
        mockReadDsl.GetWorld().Returns(worldJson);
        mockReadDsl.GetH5PElementsList().Returns(new List<ElementJson>());
        
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

        var worldJson = new WorldJson("1", new IdentifierJson("1", "1"), new List<int>(){1},
            new List<TopicJson>() {new TopicJson()}, new List<SpaceJson>{new SpaceJson(1, 
                new IdentifierJson("1","1"), new List<int>(){1}, 0, 0)}, 
            new List<ElementJson>{new (1, new IdentifierJson("1","1"), "", "", "h5p", 
                0, new List<ElementValueJson>{new ("Points", "0")})});
        mockReadDsl.GetResourceList().Returns(new List<ElementJson>());
        mockReadDsl.GetWorld().Returns(worldJson);
        mockReadDsl.GetH5PElementsList().Returns(new List<ElementJson>());
        
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
            gradebookXmlGradebookSetting: mockGradeSetting, gradebookXmlGradeItem: mockGradeItem, 
            gradebookXmlGradeCategory: mockGradeCategory,gradebookXmlGradeCategories: mockGradeCategories, 
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
        var mockReadDsl = Substitute.For<IReadDsl>();
        
        var worldJson = new WorldJson("1", new IdentifierJson("1", "1"), new List<int>(){1},
            new List<TopicJson>() {new TopicJson()}, new List<SpaceJson>{new SpaceJson(1, 
                new IdentifierJson("1","1"), new List<int>(){1}, 0, 0)}, 
            new List<ElementJson>{new (1, new IdentifierJson("1","1"), "", "", "h5p", 
                0, new List<ElementValueJson>{new ("Points", "0")})});
        mockReadDsl.GetResourceList().Returns(new List<ElementJson>());
        mockReadDsl.GetWorld().Returns(worldJson);
        mockReadDsl.GetH5PElementsList().Returns(new List<ElementJson>());
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
    public void CreateMoodleBackupXml_SetsDetailDetailsSettingSettingsContentsInformationMoodleBackupActivitiesSectionsCourse_AndSerialized()
    {
        //Arrange
        var mockReadDsl = Substitute.For<IReadDsl>();
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
        var mockIdentifier = new IdentifierJson( "name", "Identifier_Value");
        var mockElementValueList = new List<ElementValueJson>{new ElementValueJson("points","0")};
       
        
        var mockWorld = new WorldJson("Uuid", mockIdentifier, new List<int>(),
            new List<TopicJson>(), new List<SpaceJson>(), new List<ElementJson>() );

        var mockElement1 = new ElementJson(1, mockIdentifier, "", "", "h5p",0, mockElementValueList);
        var mockElement2 = new ElementJson(2, mockIdentifier, "", "", "url",0, mockElementValueList);
        List<ElementJson> elementJsons = new List<ElementJson>();
        elementJsons.Add(mockElement1);
        elementJsons.Add(mockElement2);
        
        var mockSpaceContent = new List<int>();
        mockSpaceContent.Add(mockElement1.Id);
        var mockSpace = new SpaceJson(1, mockIdentifier, mockSpaceContent, 0, 0);
        List<SpaceJson> spacesJsons = new List<SpaceJson>();
        spacesJsons.Add(mockSpace);

        
        var mockDslDocumentJson = new ElementJson(2, mockIdentifier, "", "", "json",0, mockElementValueList);
        var mockSpaceElementJson = new ElementJson(3, mockIdentifier, "", "", "space",0, mockElementValueList);
        
        mockReadDsl.GetWorld().Returns(mockWorld);
        mockReadDsl.GetH5PElementsList().Returns(elementJsons);
        mockReadDsl.GetSectionList().Returns(spacesJsons);
        mockReadDsl.GetResourceList().Returns(new List<ElementJson>(){mockDslDocumentJson});
        
        elementJsons.Add(mockDslDocumentJson);
        elementJsons.Add(mockSpaceElementJson);
        
        mockReadDsl.GetSpacesAndElementsOrderedList().Returns(elementJsons);
        
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
            Assert.That(systemUnderTest.MoodleBackupXmlActivityList[0].ModuleName, Is.EqualTo("h5pactivity"));
            Assert.That(systemUnderTest.MoodleBackupXmlActivityList[1].ModuleId, Is.EqualTo(mockElement2.Id.ToString()));
            Assert.That(systemUnderTest.MoodleBackupXmlActivityList[1].ModuleName, Is.EqualTo("url"));
            Assert.That(systemUnderTest.MoodleBackupXmlActivityList[1].Title, Is.EqualTo(mockIdentifier.Value));
            Assert.That(systemUnderTest.MoodleBackupXmlActivityList[1].Directory, Is.EqualTo("activities/url_" + mockDslDocumentJson.Id.ToString()));
            
            Assert.That(systemUnderTest.MoodleBackupXmlActivityList[3].ModuleId, Is.EqualTo(mockSpaceElementJson.Id.ToString()));
            Assert.That(systemUnderTest.MoodleBackupXmlActivityList[3].ModuleName, Is.EqualTo("label"));
            Assert.That(systemUnderTest.MoodleBackupXmlActivityList[3].Title, Is.EqualTo(mockIdentifier.Value));
            Assert.That(systemUnderTest.MoodleBackupXmlActivityList[3].Directory, Is.EqualTo("activities/label_" + mockSpaceElementJson.Id.ToString()));
            
            Assert.That(systemUnderTest.MoodleBackupXmlSettingList[2].Level, Is.EqualTo("activity"));
            Assert.That(systemUnderTest.MoodleBackupXmlSettingList[2].Name, Is.EqualTo("url_" + mockDslDocumentJson.Id.ToString() + "_included"));
            Assert.That(systemUnderTest.MoodleBackupXmlSettingList[2].Value, Is.EqualTo("1"));
            
            Assert.That(systemUnderTest.MoodleBackupXmlSettingList[8].Section, Is.EqualTo("section_" + mockSpace.SpaceId.ToString()));
            
            Assert.That(systemUnderTest.MoodleBackupXmlInformation, Is.EqualTo(mockInformation));
            systemUnderTest.MoodleBackupXmlMoodleBackup.Received().Serialize();
        });
        

    }
    
    [Test]
    public void CreateOutcomesXml_Serializes()
    {
        //Arrange
        var mockReadDsl = Substitute.For<IReadDsl>();
        var worldJson = new WorldJson("1", new IdentifierJson("1", "1"), new List<int>(){1},
            new List<TopicJson>() {new TopicJson()}, new List<SpaceJson>{new SpaceJson(1, 
                new IdentifierJson("1","1"), new List<int>(){1}, 0, 0)}, 
            new List<ElementJson>{new (1, new IdentifierJson("1","1"), "", "", "h5p", 
                0, new List<ElementValueJson>{new ("Points", "0")})});
        mockReadDsl.GetResourceList().Returns(new List<ElementJson>());
        mockReadDsl.GetWorld().Returns(worldJson);
        mockReadDsl.GetH5PElementsList().Returns(new List<ElementJson>());
        
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
        var worldJson = new WorldJson("1", new IdentifierJson("1", "1"), new List<int>(){1},
            new List<TopicJson>() {new TopicJson()}, new List<SpaceJson>{new SpaceJson(1, 
                new IdentifierJson("1","1"), new List<int>(){1}, 0, 0)}, 
            new List<ElementJson>{new (1, new IdentifierJson("1","1"), "", "", "h5p", 
                0, new List<ElementValueJson>{new ("Points", "0")})});
        mockReadDsl.GetResourceList().Returns(new List<ElementJson>());
        mockReadDsl.GetWorld().Returns(worldJson);
        mockReadDsl.GetH5PElementsList().Returns(new List<ElementJson>());
        
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
        var worldJson = new WorldJson("1", new IdentifierJson("1", "1"), new List<int>(){1},
            new List<TopicJson>() {new TopicJson()}, new List<SpaceJson>{new SpaceJson(1, 
                new IdentifierJson("1","1"), new List<int>(){1}, 0, 0)}, 
            new List<ElementJson>{new (1, new IdentifierJson("1","1"), "", "", "h5p", 
                0, new List<ElementValueJson>{new ("Points", "0")})});
        mockReadDsl.GetResourceList().Returns(new List<ElementJson>());
        mockReadDsl.GetWorld().Returns(worldJson);
        mockReadDsl.GetH5PElementsList().Returns(new List<ElementJson>());
        
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
        var worldJson = new WorldJson("1", new IdentifierJson("1", "1"), new List<int>(){1},
            new List<TopicJson>() {new TopicJson()}, new List<SpaceJson>{new SpaceJson(1, 
                new IdentifierJson("1","1"), new List<int>(){1}, 0, 0)}, 
            new List<ElementJson>{new (1, new IdentifierJson("1","1"), "", "", "h5p", 
                0, new List<ElementValueJson>{new ("Points", "0")})});
        mockReadDsl.GetResourceList().Returns(new List<ElementJson>());
        mockReadDsl.GetWorld().Returns(worldJson);
        mockReadDsl.GetH5PElementsList().Returns(new List<ElementJson>());
        
        var mockScales = Substitute.For<IScalesXmlScalesDefinition>();
        var systemUnderTest = new XmlBackupFactory(mockReadDsl, scalesXmlScalesDefinition: mockScales);
        
        //Act
        systemUnderTest.CreateScalesXml();
        
        //Assert
        systemUnderTest.ScalesXmlScalesDefinition.Received().Serialize();
    }
    
}