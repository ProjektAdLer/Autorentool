using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.DSL;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.DataAccess.XmlClasses;
using AuthoringTool.DataAccess.XmlClasses.Entities;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses;

[TestFixture]
public class XmlBackupFactoryUt
{
/*
    [Test]
    public void XmlBackupFactory_CreateXmlBackupFactory_AllMethodsCalled()
    {
        // Arrange
        var mockReadDsl = Substitute.For<IReadDSL>();
        var xmlBackupFactory = new XmlBackupFactory(mockReadDsl);
        
        var mockFileSystem = new MockFileSystem();
        var backupFileGen = new BackupFileGenerator(mockFileSystem);
        backupFileGen.CreateBackupFolders();
        var curWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        
        // Act
        xmlBackupFactory.CreateXmlBackupFactory();
        
        var pathXmlFileGradebook = Path.Join(curWorkDir, "XMLFilesForExport", "gradebook.xml");
        var pathXmlFileGroups = Path.Join(curWorkDir, "XMLFilesForExport", "groups.xml");
        var pathXmlFileOutcomes = Path.Join(curWorkDir, "XMLFilesForExport", "outcomes.xml");
        var pathXmlFileQuestions = Path.Join(curWorkDir, "XMLFilesForExport", "questions.xml");
        var pathXmlFileRoles = Path.Join(curWorkDir, "XMLFilesForExport", "roles.xml");
        var pathXmlFileScales = Path.Join(curWorkDir, "XMLFilesForExport", "scales.xml");
        var pathXmlFileMoodleBackup = Path.Join(curWorkDir, "XMLFilesForExport", "moodle_backup.xml");
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.IsTrue(mockFileSystem.File.Exists(pathXmlFileGradebook));
            Assert.IsTrue(mockFileSystem.File.Exists(pathXmlFileGroups));
            Assert.IsTrue(mockFileSystem.File.Exists(pathXmlFileOutcomes));
            Assert.IsTrue(mockFileSystem.File.Exists(pathXmlFileQuestions));
            Assert.IsTrue(mockFileSystem.File.Exists(pathXmlFileRoles));
            Assert.IsTrue(mockFileSystem.File.Exists(pathXmlFileScales));
            Assert.IsTrue(mockFileSystem.File.Exists(pathXmlFileMoodleBackup));
        });
        
    }
    
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
        var mockFileSystem = new MockFileSystem();
        var backupFileGen = new BackupFileGenerator(mockFileSystem);
        backupFileGen.CreateBackupFolders();
        var curWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        
        var mockReadDsl = Substitute.For<IReadDSL>();
        var xmlBackupFactory = new XmlBackupFactory(mockReadDsl);
        
        //Act
        xmlBackupFactory.CreateGradebookXml();
        var pathXmlFileGradebook = Path.Join(curWorkDir, "XMLFilesForExport", "gradebook.xml");

        //Assert
        Assert.Multiple(() =>
        {
            xmlBackupFactory.GradebookXmlGradebookSettings.GradeSetting = Arg.Any<GradebookXmlGradeSetting>();
            xmlBackupFactory.GradebookXmlGradebook.GradeSettings = Arg.Any<GradebookXmlGradeSettings>();
            xmlBackupFactory.GradebookXmlGradebook.GradeCategories = Arg.Any<GradebookXmlGradeCategories>();
            xmlBackupFactory.GradebookXmlGradebook.GradeItems = Arg.Any<GradebookXmlGradeItems>();
        
            Assert.IsTrue(mockFileSystem.File.Exists(pathXmlFileGradebook));
        });
    }
    
    [Test]
    public void CreateGroupsXml_Default_ParametersSetAndSerialized()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        var backupFileGen = new BackupFileGenerator(mockFileSystem);
        backupFileGen.CreateBackupFolders();
        var curWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        
        var mockReadDsl = Substitute.For<IReadDSL>();
        var xmlBackupFactory = new XmlBackupFactory(mockReadDsl);
        
        //Act
        xmlBackupFactory.CreateGroupsXml();
        var pathXmlFileGroups = Path.Join(curWorkDir, "XMLFilesForExport", "groups.xml");
        
        //Assert
        Assert.Multiple(() =>
        {
            xmlBackupFactory.GroupsXmlGroups.GroupingsList = Arg.Any<GroupsXmlGroupingsList>();
            Assert.IsTrue(mockFileSystem.File.Exists(pathXmlFileGroups));
        });
    }
    
    [Test]
    public void CreateMoodleBackupXml_Default_ParametersSetAndSerialized()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        var backupFileGen = new BackupFileGenerator(mockFileSystem);
        backupFileGen.CreateBackupFolders();
        var curWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        
        var mockReadDsl = Substitute.For<IReadDSL>();
        var mockIdentifier = new IdentifierJson();
        var mockLearningWorld = new LearningWorldJson();
        var mockDslDocument = new List<LearningElementJson>();
        
        mockLearningWorld.identifier = mockIdentifier;
        mockIdentifier.type = "name";
        mockIdentifier.value = "Element_1";

        var mockLearningElement = new LearningElementJson();
        mockLearningElement.id = 1;
        mockLearningElement.identifier = mockIdentifier;
        mockLearningElement.elementType = "h5p";
        List<LearningElementJson> learningElementJsons = new List<LearningElementJson>();
        learningElementJsons.Add(mockLearningElement);

        var mockDslDocumentJson = new LearningElementJson();
        mockDslDocumentJson.id = 2;
        mockDslDocumentJson.identifier = mockIdentifier;
        mockDslDocument.Add(mockDslDocumentJson);

        mockReadDsl.GetLearningWorld().Returns(mockLearningWorld);
        mockReadDsl.GetH5PElementsList().Returns(learningElementJsons);
        mockReadDsl.GetDslDocumentList().Returns(mockDslDocument);
        
        var xmlBackupFactory = new XmlBackupFactory(mockReadDsl);
        
        //Act
        xmlBackupFactory.CreateMoodleBackupXml();
        var pathXmlFileMoodleBackup = Path.Join(curWorkDir, "XMLFilesForExport", "moodle_backup.xml");

        //Assert
        Assert.Multiple(() =>
        {   
            Assert.That(xmlBackupFactory.MoodleBackupXmlCourse.Title, Is.EqualTo(mockIdentifier.value.ToString()));
            
            Assert.That(xmlBackupFactory.MoodleBackupXmlSettingFilename.Name, Is.EqualTo("filename"));
            Assert.That(xmlBackupFactory.MoodleBackupXmlSettingFilename.Value, Is.EqualTo("C#_AuthoringTool_Created_Backup.mbz"));
            Assert.That(xmlBackupFactory.MoodleBackupXmlSettingImscc11.Name, Is.EqualTo("imscc11"));
            Assert.That(xmlBackupFactory.MoodleBackupXmlSettingUsers.Name, Is.EqualTo("users"));
            Assert.That(xmlBackupFactory.MoodleBackupXmlSettingAnonymize.Name, Is.EqualTo("anonymize"));
            Assert.That(xmlBackupFactory.MoodleBackupXmlSettingRoleAssignments.Name, Is.EqualTo("role_assignments"));
            Assert.That(xmlBackupFactory.MoodleBackupXmlSettingActivities.Name, Is.EqualTo("activities"));
            Assert.That(xmlBackupFactory.MoodleBackupXmlSettingActivities.Value, Is.EqualTo("1"));
            Assert.That(xmlBackupFactory.MoodleBackupXmlSettingBlocks.Name, Is.EqualTo("blocks"));
            Assert.That(xmlBackupFactory.MoodleBackupXmlSettingFiles.Name, Is.EqualTo("files"));
            Assert.That(xmlBackupFactory.MoodleBackupXmlSettingFiles.Value, Is.EqualTo("1"));
            Assert.That(xmlBackupFactory.MoodleBackupXmlSettingFilters.Name, Is.EqualTo("filters"));
            Assert.That(xmlBackupFactory.MoodleBackupXmlSettingComments.Name, Is.EqualTo("comments"));
            Assert.That(xmlBackupFactory.MoodleBackupXmlSettingBadges.Name, Is.EqualTo("badges"));
            Assert.That(xmlBackupFactory.MoodleBackupXmlSettingCalendarevents.Name, Is.EqualTo("calendarevents"));
            Assert.That(xmlBackupFactory.MoodleBackupXmlSettingUserscompletion.Name, Is.EqualTo("userscompletion"));
            Assert.That(xmlBackupFactory.MoodleBackupXmlSettingLogs.Name, Is.EqualTo("logs"));
            Assert.That(xmlBackupFactory.MoodleBackupXmlSettingGradeHistories.Name, Is.EqualTo("grade_histories"));
            Assert.That(xmlBackupFactory.MoodleBackupXmlSettingQuestionbank.Name, Is.EqualTo("questionbank"));
            Assert.That(xmlBackupFactory.MoodleBackupXmlSettingGroups.Name, Is.EqualTo("groups"));
            Assert.That(xmlBackupFactory.MoodleBackupXmlSettingCompetencies.Name, Is.EqualTo("competencies"));
            Assert.That(xmlBackupFactory.MoodleBackupXmlSettingCustomfield.Name, Is.EqualTo("customfield"));
            Assert.That(xmlBackupFactory.MoodleBackupXmlSettingContentbankcontent.Name, Is.EqualTo("contentbankcontent"));
            Assert.That(xmlBackupFactory.MoodleBackupXmlSettingLegacyfiles.Name, Is.EqualTo("legacyfiles"));
            Assert.That(xmlBackupFactory.MoodleBackupXmlSettingLogs.Name, Is.EqualTo("logs"));
            
            Assert.That(xmlBackupFactory.moodleBackupXmlActivityList[0].ModuleId, Is.EqualTo(mockDslDocumentJson.id.ToString()));
            Assert.That(xmlBackupFactory.moodleBackupXmlActivityList[0].ModuleName, Is.EqualTo("resource"));
            Assert.That(xmlBackupFactory.moodleBackupXmlActivityList[0].Title, Is.EqualTo(mockIdentifier.value.ToString()));
            Assert.That(xmlBackupFactory.moodleBackupXmlActivityList[0].Directory, Is.EqualTo("activities/" + "resource" + "_" +  mockDslDocumentJson.id.ToString()));
            
            Assert.That(xmlBackupFactory.moodleBackupXmlSectionList[0].Directory, Is.EqualTo("sections/section_" +  mockDslDocumentJson.id.ToString()));

            Assert.That(xmlBackupFactory.moodleBackupXmlSettingList[0].Level, Is.EqualTo("section")); 
            Assert.That(xmlBackupFactory.moodleBackupXmlSettingList[0].Name, Is.EqualTo("section_" + mockDslDocumentJson.id.ToString() + "_included"));    
            Assert.That(xmlBackupFactory.moodleBackupXmlSettingList[0].Value, Is.EqualTo("1")); 
            Assert.That(xmlBackupFactory.moodleBackupXmlSettingList[0].Section, Is.EqualTo("section_" + mockDslDocumentJson.id.ToString())); 
            
            Assert.That(xmlBackupFactory.MoodleBackupXmlInformation.OriginalCourseFullname, Is.EqualTo(mockIdentifier.value.ToString()));
            Assert.That(xmlBackupFactory.MoodleBackupXmlInformation.OriginalCourseShortname, Is.EqualTo(mockIdentifier.value.ToString()));
            Assert.That(xmlBackupFactory.MoodleBackupXmlInformation.Details, Is.EqualTo(xmlBackupFactory.MoodleBackupXmlDetails));
            Assert.That(xmlBackupFactory.MoodleBackupXmlInformation.Contents, Is.EqualTo(xmlBackupFactory.MoodleBackupXmlContents));
            Assert.That(xmlBackupFactory.MoodleBackupXmlInformation.Settings, Is.EqualTo(xmlBackupFactory.MoodleBackupXmlSettings));
            
            Assert.That(xmlBackupFactory.MoodleBackupXmlMoodleBackup.Information, Is.EqualTo(xmlBackupFactory.MoodleBackupXmlInformation));
            
            Assert.That(mockFileSystem.FileExists(pathXmlFileMoodleBackup), Is.True);
        });
        

    }*/
    
    [Test]
    public void CreateOutcomesXml_Serializes()
    {
        //Arrange
        var mockReadDsl = Substitute.For<IReadDSL>();
        var systemUnderTest = Substitute.For<IOutcomesXmlOutcomesDefinition>();
        var xmlBackupFactory = new XmlBackupFactory(mockReadDsl, outcomesXmlOutcomesDefinition: systemUnderTest);
        
        //Act
        xmlBackupFactory.CreateOutcomesXml();

        //Assert
        systemUnderTest.Received().Serialize();
    }
    
    [Test]
    public void CreateQuestionsXml_Serializes()
    {
        //Arrange
        var mockReadDsl = Substitute.For<IReadDSL>();
        var systemUnderTest = Substitute.For<IQuestionsXmlQuestionsCategories>();
        var xmlBackupFactory = new XmlBackupFactory(mockReadDsl, questionsXmlQuestionsCategories: systemUnderTest);
        
        //Act
        xmlBackupFactory.CreateQuestionsXml();

        //Assert
        systemUnderTest.Received().Serialize();
    }
    
    [Test]
    public void CreateRolesXml_SetsRoleInRoleDefinition_AndSerializes()
    {
        //Arrange
        var mockReadDsl = Substitute.For<IReadDSL>();
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
        var mockReadDsl = Substitute.For<IReadDSL>();
        var systemUnderTest = Substitute.For<IScalesXmlScalesDefinition>();
        var xmlBackupFactory = new XmlBackupFactory(mockReadDsl, scalesXmlScalesDefinition: systemUnderTest);
        
        //Act
        xmlBackupFactory.CreateScalesXml();
        
        //Assert
        systemUnderTest.Received().Serialize();
    }
    
}