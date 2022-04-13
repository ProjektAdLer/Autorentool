using System;
using AuthoringTool.DataAccess.XmlClasses;
using NSubstitute;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.WorldExport;

[TestFixture]
public class XmlBackupFactoryUt
{
    
    [Test]
    public void XmlBackupFactory_Constructor_AllPropertiesSet()
    {
        //Arrange
        
        //Act
        var xmlBackupFactory = CreateStandardXmlBackupFactory();

        //Assert
        Assert.That(xmlBackupFactory.FilesXmlFiles, Is.Not.Null);
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
        Assert.That(xmlBackupFactory.MoodleBackupXmlSettingSection_160_Included, Is.Not.Null);
        Assert.That(xmlBackupFactory.MoodleBackupXmlSettingSection_160_userinfo, Is.Not.Null);
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
    public void CreateFilesXml_Default_ParametersSetAndSerialized()
    {
        //Arrange
        var mockFilesXmlFiles = Substitute.For<IFilesXmlFiles>();
        var xmlBackupFactory = CreateTestableXmlBackupFactory(mockFilesXmlFiles);
        
        //Act
        xmlBackupFactory.CreateFilesXml();
        
        //Assert
        mockFilesXmlFiles.Received().SetParameters();
        mockFilesXmlFiles.Received().Serialize();
    }
    
    [Test]
    public void CreateGradebookXml_Default_ParametersSetAndSerialized()
    {
        //Arrange
        var mockGradebookXmlGradebookSetting = Substitute.For<IGradebookXmlGradeSetting>();
        var mockGradebookXmlGradebookSettings = Substitute.For<IGradebookXmlGradeSettings>();
        var mockGradebookXmlGradebook = Substitute.For<IGradebookXmlGradebook>();
        
        var xmlBackupFactory = CreateTestableXmlBackupFactory(null,mockGradebookXmlGradebookSetting, mockGradebookXmlGradebookSettings,mockGradebookXmlGradebook);
        
        //Act
        xmlBackupFactory.CreateGradebookXml();

        //Assert
        mockGradebookXmlGradebookSetting.Received().SetParameters(Arg.Any<string>(), Arg.Any<string>());
        mockGradebookXmlGradebookSettings.Received().SetParameters(Arg.Any<GradebookXmlGradeSetting>());
        mockGradebookXmlGradebook.Received().SetParameters(Arg.Any<GradebookXmlGradeSettings>());
        mockGradebookXmlGradebook.Received().Serialize();
    }
    
    [Test]
    public void CreateGroupsXml_Default_ParametersSetAndSerialized()
    {
        //Arrange
        var mockGroupsXmlGroupingsList = Substitute.For<IGroupsXmlGroupingsList>();
        var mockGroupsXmlGroups = Substitute.For<IGroupsXmlGroups>();
        
        var xmlBackupFactory = CreateTestableXmlBackupFactory(null,null, null,null,
            mockGroupsXmlGroupingsList, mockGroupsXmlGroups);
        
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
        var mockMoodleBackupXmlDetail = Substitute.For<IMoodleBackupXmlDetail>();
        var mockMoodleBackupXmlDetails = Substitute.For<IMoodleBackupXmlDetails>();
        var mockMoodleBackupXmlSection = Substitute.For<IMoodleBackupXmlSection>();
        var mockMoodleBackupXmlSections = Substitute.For<IMoodleBackupXmlSections>();
        var mockMoodleBackupXmlCourse = Substitute.For<IMoodleBackupXmlCourse>();
        var mockMoodleBackupXmlContents = Substitute.For<IMoodleBackupXmlContents>();
        var mockMoodleBackupXmlSetting = Substitute.For<IMoodleBackupXmlSetting>();
        var mockMoodleBackupXmlSettings = Substitute.For<IMoodleBackupXmlSettings>();
        var mockMoodleBackupXmlInformation = Substitute.For<IMoodleBackupXmlInformation>();
        var mockMoodleBackupXmlMoodleBackup = Substitute.For<IMoodleBackupXmlMoodleBackup>();
        
        var xmlBackupFactory = CreateTestableXmlBackupFactory(null,null, null,null,
            null, null, mockMoodleBackupXmlDetail, mockMoodleBackupXmlDetails, mockMoodleBackupXmlSection, 
            mockMoodleBackupXmlSections, mockMoodleBackupXmlCourse, mockMoodleBackupXmlContents, mockMoodleBackupXmlSetting, 
            mockMoodleBackupXmlSettings, mockMoodleBackupXmlInformation, mockMoodleBackupXmlMoodleBackup);
        
        //Act
        xmlBackupFactory.CreateMoodleBackupXml();

        //Assert
        mockMoodleBackupXmlDetail.Received().SetParameters(Arg.Any<string>());
        mockMoodleBackupXmlDetails.Received().SetParameters(Arg.Any<MoodleBackupXmlDetail>());
        mockMoodleBackupXmlSection.Received().SetParameters(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        mockMoodleBackupXmlSections.Received().SetParameters(Arg.Any<MoodleBackupXmlSection>());
        mockMoodleBackupXmlCourse.Received().SetParameters(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        mockMoodleBackupXmlContents.Received().SetParameters(Arg.Any<MoodleBackupXmlSections>(), Arg.Any<MoodleBackupXmlCourse>());
        mockMoodleBackupXmlSetting.Received().SetParametersShort(Arg.Any<string>(), Arg.Any<string>());
        mockMoodleBackupXmlSetting.Received().SetParametersFull(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        mockMoodleBackupXmlSettings.Received().SetParameters();
        mockMoodleBackupXmlSettings.Received().FillSettings(Arg.Any<MoodleBackupXmlSetting>());
        mockMoodleBackupXmlInformation.Received().SetParameters(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), 
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), 
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<MoodleBackupXmlDetails>(), 
            Arg.Any<MoodleBackupXmlContents>(), Arg.Any<MoodleBackupXmlSettings>());
        mockMoodleBackupXmlMoodleBackup.Received().SetParameters(Arg.Any<MoodleBackupXmlInformation>());
        mockMoodleBackupXmlMoodleBackup.Received().Serialize();
    }
    
    [Test]
    public void CreateOutcomesXml_Default_ParametersSetAndSerialized()
    {
        //Arrange
        var mockOutcomesOutcomesDefinition = Substitute.For<IOutcomesXmlOutcomesDefinition>();
        
        var xmlBackupFactory = CreateTestableXmlBackupFactory(null,null, null,null,
            null, null, null, null, null, 
            null, null, null, null, 
            null, null, null, mockOutcomesOutcomesDefinition);
        
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
        var mockQuestionsQuestionsCategories = Substitute.For<IQuestionsXmlQuestionsCategories>();
        
        var xmlBackupFactory = CreateTestableXmlBackupFactory(null,null, null,null,
            null, null, null, null, null, 
            null, null, null, null, 
            null, null, null, null, mockQuestionsQuestionsCategories);
        
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
        var mockrolesXmlRole = Substitute.For<IRolesXmlRole>();
        var mockrolesXmlRolesDefinition = Substitute.For<IRolesXmlRolesDefinition>();
        
        var xmlBackupFactory = CreateTestableXmlBackupFactory(null,null, null,null,
            null, null, null, null, null, 
            null, null, null, null, 
            null, null, null, null, null,
            mockrolesXmlRole, mockrolesXmlRolesDefinition);
        
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
        var mockScalesXmlScalesDefinition = Substitute.For<IScalesXmlScalesDefinition>();
        
        var xmlBackupFactory = CreateTestableXmlBackupFactory(null,null, null,null,
            null, null, null, null, null, 
            null, null, null, null, 
            null, null, null, null, null,
            null, null, mockScalesXmlScalesDefinition);
        
        //Act
        xmlBackupFactory.CreateScalesXml();

        //Assert
        mockScalesXmlScalesDefinition.Received().SetParameters();
        mockScalesXmlScalesDefinition.Received().Serialize();
    }
    
    public XmlBackupFactory CreateStandardXmlBackupFactory()
    {
        return new XmlBackupFactory();
    }

    public XmlBackupFactory CreateTestableXmlBackupFactory(IFilesXmlFiles filesXmlFiles = null, 
        IGradebookXmlGradeSetting gradebookXmlGradeSetting = null, IGradebookXmlGradeSettings gradebookXmlGradeSettings = null,IGradebookXmlGradebook gradebookXmlGradebook = null,
        IGroupsXmlGroupingsList groupsXmlGroupingsList = null, IGroupsXmlGroups groupsXmlGroups = null, 
        IMoodleBackupXmlDetail moodleBackupXmlDetail = null, IMoodleBackupXmlDetails moodleBackupXmlDetails = null, IMoodleBackupXmlSection moodleBackupXmlSection = null,
        IMoodleBackupXmlSections moodleBackupXmlSections = null, IMoodleBackupXmlCourse moodleBackupXmlCourse = null, IMoodleBackupXmlContents moodleBackupXmlContents = null,
        IMoodleBackupXmlSetting moodleBackupXmlSetting = null, IMoodleBackupXmlSettings moodleBackupXmlSettings = null, IMoodleBackupXmlInformation moodleBackupXmlInformation = null,
        IMoodleBackupXmlMoodleBackup moodleBackupXmlMoodleBackup = null, IOutcomesXmlOutcomesDefinition outcomesXmlOutcomesDefinition = null,
        IQuestionsXmlQuestionsCategories questionsXmlQuestionsCategories = null, IRolesXmlRole rolesXmlRole = null, 
        IRolesXmlRolesDefinition rolesXmlRolesDefinition = null, IScalesXmlScalesDefinition scalesXmlScalesDefinition = null)
    {
        filesXmlFiles ??= Substitute.For<IFilesXmlFiles>();
        
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

        return new XmlBackupFactory(filesXmlFiles, gradebookXmlGradeSetting, gradebookXmlGradeSettings, gradebookXmlGradebook, 
            groupsXmlGroupingsList, groupsXmlGroups, moodleBackupXmlDetail, moodleBackupXmlDetails, moodleBackupXmlSection,
            moodleBackupXmlSections, moodleBackupXmlCourse, moodleBackupXmlContents, moodleBackupXmlSetting, moodleBackupXmlSettings,
            moodleBackupXmlInformation, moodleBackupXmlMoodleBackup, outcomesXmlOutcomesDefinition, questionsXmlQuestionsCategories,
            rolesXmlRole, rolesXmlRolesDefinition,scalesXmlScalesDefinition);
    }
    
}