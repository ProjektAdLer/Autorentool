using System.Collections.Generic;
using AuthoringTool.DataAccess.DSL;
using AuthoringTool.DataAccess.XmlClasses;
using AuthoringTool.DataAccess.XmlClasses.Entities;
using NSubstitute;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses;

[TestFixture]
public class XmlBackupFactoryUt
{
    
    [Test]
    public void XmlBackupFactory_Constructor_AllPropertiesSet()
    {
        //Arrange
        
        //Act
        var mockReadDsl = Substitute.For<IReadDSL>();
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
    public void CreateGradebookXml_Default_ParametersSetAndSerialized()
    {
        //Arrange
        var mockReadDsl = Substitute.For<IReadDSL>();
        var mockGradebookXmlGradebookItem = Substitute.For<IGradebookXmlGradeItem>();
        var mockGradebookXmlGradebookItems = Substitute.For<IGradebookXmlGradeItems>();
        var mockGradebookXmlGradebookCategory = Substitute.For<IGradebookXmlGradeCategory>();
        var mockGradebookXmlGradebookCategories = Substitute.For<IGradebookXmlGradeCategories>();
        var mockGradebookXmlGradebookSetting = Substitute.For<IGradebookXmlGradeSetting>();
        var mockGradebookXmlGradebookSettings = Substitute.For<IGradebookXmlGradeSettings>();
        var mockGradebookXmlGradebook = Substitute.For<IGradebookXmlGradebook>();
        
        var xmlBackupFactory = new XmlBackupFactory(mockReadDsl, mockGradebookXmlGradebookItem,
            mockGradebookXmlGradebookItems, mockGradebookXmlGradebookCategory, mockGradebookXmlGradebookCategories,
            mockGradebookXmlGradebookSetting, mockGradebookXmlGradebookSettings,mockGradebookXmlGradebook);
        
        //Act
        xmlBackupFactory.CreateGradebookXml();

        //Assert
        mockGradebookXmlGradebookSetting.Received().SetParameters(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        mockGradebookXmlGradebookSettings.Received().SetParameters(Arg.Any<GradebookXmlGradeSetting>());
        mockGradebookXmlGradebook.Received().SetParameters(Arg.Any<string>(), Arg.Any<GradebookXmlGradeCategories?>(), 
            Arg.Any<GradebookXmlGradeItems?>(), Arg.Any<string>(), Arg.Any<GradebookXmlGradeSettings?>());
        mockGradebookXmlGradebook.Received().Serialize();
    }
    
    [Test]
    public void CreateGroupsXml_Default_ParametersSetAndSerialized()
    {
        //Arrange
        var mockReadDsl = Substitute.For<IReadDSL>();
        var mockIdentifier = Substitute.For<IdentifierJson>();
        var mockLearningWorld = new LearningWorldJson();
        mockLearningWorld.identifier = mockIdentifier;

        mockReadDsl.GetLearningWorld().Returns(mockLearningWorld);
        var mockGroupsXmlGroupingsList = Substitute.For<IGroupsXmlGroupingsList>();
        var mockGroupsXmlGroups = Substitute.For<IGroupsXmlGroups>();
        
        var xmlBackupFactory = new XmlBackupFactory(mockReadDsl, null, null, 
            null, null, null, 
            null,null, mockGroupsXmlGroupingsList, mockGroupsXmlGroups);
        
        //Act
        xmlBackupFactory.CreateGroupsXml();

        //Assert
        mockGroupsXmlGroupingsList.Received().SetParameters(Arg.Any<string>());
        mockGroupsXmlGroups.Received().SetParameters(Arg.Any<GroupsXmlGroupingsList>());
        mockGroupsXmlGroups.Received().Serialize();
    }
    
    [Test]
    public void CreateMoodleBackupXml_Default_ParametersSetAndSerialized()
    {
        //Arrange
        var mockReadDsl = Substitute.For<IReadDSL>();
        var mockIdentifier = Substitute.For<IdentifierJson>();
        var mockLearningWorld = new LearningWorldJson();
        mockLearningWorld.identifier = mockIdentifier;

        var mockLearningElement = new LearningElementJson();
        mockIdentifier.type = "name";
        mockIdentifier.value = "Element_1";

        mockLearningElement.id = 1;
        mockLearningElement.identifier = mockIdentifier;
        mockLearningElement.elementType = "H5P";
        List<LearningElementJson> learningElementJsons = new List<LearningElementJson>();
        learningElementJsons.Add(mockLearningElement);

        mockReadDsl.GetLearningWorld().Returns(mockLearningWorld);
        mockReadDsl.GetH5PElementsList().Returns(learningElementJsons);
        
        var mockMoodleBackupXmlDetail = Substitute.For<IMoodleBackupXmlDetail>();
        var mockMoodleBackupXmlDetails = Substitute.For<IMoodleBackupXmlDetails>();
        var mockMoodleBackupXmlActivities = Substitute.For<IMoodleBackupXmlActivities>();
        var mockMoodleBackupXmlActivity = Substitute.For<IMoodleBackupXmlActivity>();
        var mockMoodleBackupXmlSection = Substitute.For<IMoodleBackupXmlSection>();
        var mockMoodleBackupXmlSections = Substitute.For<IMoodleBackupXmlSections>();
        var mockMoodleBackupXmlCourse = Substitute.For<IMoodleBackupXmlCourse>();
        var mockMoodleBackupXmlContents = Substitute.For<IMoodleBackupXmlContents>();
        var mockMoodleBackupXmlSetting = Substitute.For<IMoodleBackupXmlSetting>();
        var mockMoodleBackupXmlSettings = Substitute.For<IMoodleBackupXmlSettings>();
        var mockMoodleBackupXmlInformation = Substitute.For<IMoodleBackupXmlInformation>();
        var mockMoodleBackupXmlMoodleBackup = Substitute.For<IMoodleBackupXmlMoodleBackup>();
        
        var xmlBackupFactory = new XmlBackupFactory(mockReadDsl, null, null, 
            null, null, null,null,
            null, null, null, mockMoodleBackupXmlDetail, 
            mockMoodleBackupXmlDetails, mockMoodleBackupXmlActivities, mockMoodleBackupXmlActivity, 
            mockMoodleBackupXmlSection, mockMoodleBackupXmlSections, mockMoodleBackupXmlCourse, 
            mockMoodleBackupXmlContents, mockMoodleBackupXmlSetting, mockMoodleBackupXmlSettings, mockMoodleBackupXmlInformation, mockMoodleBackupXmlMoodleBackup);
        
        //Act
        xmlBackupFactory.CreateMoodleBackupXml();

        //Assert
        mockMoodleBackupXmlDetail.Received().SetParameters(Arg.Any<string>(), Arg.Any<string>(), 
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        mockMoodleBackupXmlDetails.Received().SetParameters(Arg.Any<MoodleBackupXmlDetail>());
        mockMoodleBackupXmlSections.Received().SetParameters(Arg.Any<List<MoodleBackupXmlSection>>());
        mockMoodleBackupXmlCourse.Received().SetParameters(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        mockMoodleBackupXmlContents.Received().SetParameters(Arg.Any<MoodleBackupXmlActivities?>(), 
            Arg.Any<MoodleBackupXmlSections?>(), Arg.Any<MoodleBackupXmlCourse?>());
        mockMoodleBackupXmlSetting.Received().SetParametersSetting(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        mockMoodleBackupXmlSettings.Received().SetParameters(Arg.Any<List<MoodleBackupXmlSetting>>());
        mockMoodleBackupXmlSettings.Received().SetParameters(Arg.Any<List<MoodleBackupXmlSetting>>());
        mockMoodleBackupXmlInformation.Received().SetParameters(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), 
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), 
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), 
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), 
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<MoodleBackupXmlDetails>(),
            Arg.Any<MoodleBackupXmlContents>(), Arg.Any<MoodleBackupXmlSettings>());
        mockMoodleBackupXmlMoodleBackup.Received().SetParameters(Arg.Any<MoodleBackupXmlInformation>());
        mockMoodleBackupXmlMoodleBackup.Received().Serialize();
    }
    
    [Test]
    public void CreateOutcomesXml_Default_ParametersSetAndSerialized()
    {
        //Arrange
        var mockReadDsl = Substitute.For<IReadDSL>();
        var mockOutcomesOutcomesDefinition = Substitute.For<IOutcomesXmlOutcomesDefinition>();
        
        var xmlBackupFactory = new XmlBackupFactory(mockReadDsl, null, null, null,
            null, null, null,null,
            null, null, null, null, null, 
            null, null, null, null, 
            null, null, null, null, null,
            mockOutcomesOutcomesDefinition);
        
        //Act
        xmlBackupFactory.CreateOutcomesXml();

        //Assert
        mockOutcomesOutcomesDefinition.Received().SetParameters();
        mockOutcomesOutcomesDefinition.Received().Serialize();
    }
    
    [Test]
    public void CreateQuestionsXml_Default_ParametersSetAndSerialized()
    {
        //Arrange
        var mockReadDsl = Substitute.For<IReadDSL>();
        var mockQuestionsQuestionsCategories = Substitute.For<IQuestionsXmlQuestionsCategories>();
        
        var xmlBackupFactory = new XmlBackupFactory(mockReadDsl, null, null, 
            null, null, null, null,
            null, null, null, null, 
            null, null, null, null, 
            null, null, null, null, 
            null, null, null, null,
            mockQuestionsQuestionsCategories);
        
        //Act
        xmlBackupFactory.CreateQuestionsXml();

        //Assert
        mockQuestionsQuestionsCategories.Received().SetParameters();
        mockQuestionsQuestionsCategories.Received().Serialize();
    }
    
    [Test]
    public void CreateRolesXml_Default_ParametersSetAndSerialized()
    {
        //Arrange
        var mockReadDsl = Substitute.For<IReadDSL>();
        var mockrolesXmlRole = Substitute.For<IRolesXmlRole>();
        var mockrolesXmlRolesDefinition = Substitute.For<IRolesXmlRolesDefinition>();
        
        var xmlBackupFactory = new XmlBackupFactory(mockReadDsl, null, null, 
            null, null, null, 
            null,null, null, null, 
            null, null, null, null, 
            null, null, null, null, 
            null, null, null, null, 
            null, null, mockrolesXmlRole, mockrolesXmlRolesDefinition);
        
        //Act
        xmlBackupFactory.CreateRolesXml();

        //Assert
        mockrolesXmlRole.Received().SetParameters(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        mockrolesXmlRolesDefinition.Received().SetParameters(Arg.Any<RolesXmlRole>());
        mockrolesXmlRolesDefinition.Received().Serialize();
    }
    
    [Test]
    public void CreateScalesXml_Default_ParametersSetAndSerialized()
    {
        //Arrange
        var mockReadDsl = Substitute.For<IReadDSL>();
        var mockScalesXmlScalesDefinition = Substitute.For<IScalesXmlScalesDefinition>();
        
        var xmlBackupFactory = new XmlBackupFactory(mockReadDsl, null, null, 
            null, null, null, null,null,
            null, null, null, null, null, 
            null, null, null, null, 
            null, null, null, null, null,
            null, null, null, null, mockScalesXmlScalesDefinition);
        
        //Act
        xmlBackupFactory.CreateScalesXml();

        //Assert
        mockScalesXmlScalesDefinition.Received().SetParameters();
        mockScalesXmlScalesDefinition.Received().Serialize();
    }
    
    public XmlBackupFactory CreateStandardXmlBackupFactory()
    {
        return new XmlBackupFactory(Arg.Any<ReadDSL>());
    }

    /*
    public XmlBackupFactory CreateTestableXmlBackupFactory(IReadDSL readDsl ,IGradebookXmlGradeSetting gradebookXmlGradeSetting = null, IGradebookXmlGradeSettings gradebookXmlGradeSettings = null,IGradebookXmlGradebook gradebookXmlGradebook = null,
        IGroupsXmlGroupingsList groupsXmlGroupingsList = null, IGroupsXmlGroups groupsXmlGroups = null, 
        IMoodleBackupXmlDetail moodleBackupXmlDetail = null, IMoodleBackupXmlDetails moodleBackupXmlDetails = null, IMoodleBackupXmlSection moodleBackupXmlSection = null,
        IMoodleBackupXmlSections moodleBackupXmlSections = null, IMoodleBackupXmlCourse moodleBackupXmlCourse = null, IMoodleBackupXmlContents moodleBackupXmlContents = null,
        IMoodleBackupXmlSetting moodleBackupXmlSetting = null, IMoodleBackupXmlSettings moodleBackupXmlSettings = null, IMoodleBackupXmlInformation moodleBackupXmlInformation = null,
        IMoodleBackupXmlMoodleBackup moodleBackupXmlMoodleBackup = null, IOutcomesXmlOutcomesDefinition outcomesXmlOutcomesDefinition = null,
        IQuestionsXmlQuestionsCategories questionsXmlQuestionsCategories = null, IRolesXmlRole rolesXmlRole = null, 
        IRolesXmlRolesDefinition rolesXmlRolesDefinition = null, IScalesXmlScalesDefinition scalesXmlScalesDefinition = null)
    {
       
        gradebookXmlGradeSetting ??= Substitute.For<IGradebookXmlGradeSetting>();
        gradebookXmlGradeSettings ??= Substitute.For<IGradebookXmlGradeSettings>();
        gradebookXmlGradebook ??= Substitute.For<IGradebookXmlGradebook>();

        groupsXmlGroupingsList ??= Substitute.For<IGroupsXmlGroupingsList>();
        groupsXmlGroups ??= Substitute.For<IGroupsXmlGroups>();

        moodleBackupXmlDetail ??= Substitute.For<IMoodleBackupXmlDetail>();
        moodleBackupXmlDetails ??= Substitute.For<IMoodleBackupXmlDetails>();
        moodleBackupXmlSection ??= Substitute.For<IMoodleBackupXmlSection>();
        moodleBackupXmlSections ??= Substitute.For<IMoodleBackupXmlSections>();
        moodleBackupXmlCourse ??= Substitute.For<IMoodleBackupXmlCourse>();
        moodleBackupXmlContents ??= Substitute.For<IMoodleBackupXmlContents>();
        moodleBackupXmlSetting ??= Substitute.For<IMoodleBackupXmlSetting>();
        moodleBackupXmlSettings ??= Substitute.For<IMoodleBackupXmlSettings>();
        moodleBackupXmlInformation ??= Substitute.For<IMoodleBackupXmlInformation>();
        moodleBackupXmlMoodleBackup ??= Substitute.For<IMoodleBackupXmlMoodleBackup>();

        outcomesXmlOutcomesDefinition ??= Substitute.For<IOutcomesXmlOutcomesDefinition>();

        questionsXmlQuestionsCategories ??= Substitute.For<IQuestionsXmlQuestionsCategories>();

        rolesXmlRole ??= Substitute.For<IRolesXmlRole>();
        rolesXmlRolesDefinition ??= Substitute.For<IRolesXmlRolesDefinition>();

        scalesXmlScalesDefinition ??= Substitute.For<IScalesXmlScalesDefinition>();
        
        

        return new XmlBackupFactory(readDsl, gradebookXmlGradeSetting, gradebookXmlGradeSettings, gradebookXmlGradebook, 
            groupsXmlGroupingsList, groupsXmlGroups, moodleBackupXmlDetail, moodleBackupXmlDetails, moodleBackupXmlSection,
            moodleBackupXmlSections, moodleBackupXmlCourse, moodleBackupXmlContents, moodleBackupXmlSetting, moodleBackupXmlSettings,
            moodleBackupXmlInformation, moodleBackupXmlMoodleBackup, outcomesXmlOutcomesDefinition, questionsXmlQuestionsCategories,
            rolesXmlRole, rolesXmlRolesDefinition,scalesXmlScalesDefinition);
    }*/
    
}