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
        mockMoodleBackupXmlInformation.Received().SetParameters(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), 
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), 
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<MoodleBackupXmlDetails>(), 
            Arg.Any<MoodleBackupXmlContents>(), Arg.Any<MoodleBackupXmlSettings>());
        mockMoodleBackupXmlMoodleBackup.Received().SetParameters(Arg.Any<MoodleBackupXmlInformation>());
        mockMoodleBackupXmlMoodleBackup.Received().Serialize();
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
        IMoodleBackupXmlMoodleBackup moodleBackupXmlMoodleBackup = null)
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

        return new XmlBackupFactory(filesXmlFiles, gradebookXmlGradeSetting, gradebookXmlGradeSettings, gradebookXmlGradebook, 
            groupsXmlGroupingsList, groupsXmlGroups, moodleBackupXmlDetail, moodleBackupXmlDetails, moodleBackupXmlSection,
            moodleBackupXmlSections, moodleBackupXmlCourse, moodleBackupXmlContents, moodleBackupXmlSetting, moodleBackupXmlSettings,
            moodleBackupXmlInformation, moodleBackupXmlMoodleBackup);
    }
    
}