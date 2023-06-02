using System.IO.Abstractions.TestingHelpers;
using Generator.DSL;
using Generator.XmlClasses.Entities._activities.GradeHistory.xml;
using Generator.XmlClasses.Entities._activities.Grades.xml;
using Generator.XmlClasses.Entities._activities.Inforef.xml;
using Generator.XmlClasses.Entities._activities.Label.xml;
using Generator.XmlClasses.Entities._activities.Module.xml;
using Generator.XmlClasses.Entities._activities.Roles.xml;
using Generator.XmlClasses.XmlFileFactories;
using NSubstitute;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.XmlFileFactories;

[TestFixture]
public class XmlLabelFactoryUt
{
    [Test]
    public void XmlLabelFactory_StandardConstructor_AllPropertiesSet()
    {
        // Arrange
        var mockReadDsl = Substitute.For<IReadDsl>();
        var mockFileSystem = new MockFileSystem();
        var mockGradeItem = Substitute.For<IActivitiesGradesXmlGradeItem>();
        var mockGradeItems = Substitute.For<IActivitiesGradesXmlGradeItems>();
        var mockGradebook = Substitute.For<IActivitiesGradesXmlActivityGradebook>();
        var mockLabel = Substitute.For<IActivitiesLabelXmlLabel>();
        var mockLabelActivity = Substitute.For<IActivitiesLabelXmlActivity>();
        var mockRoles = Substitute.For<IActivitiesRolesXmlRoles>();
        var mockModule = Substitute.For<IActivitiesModuleXmlModule>();
        var mockGradeHistory = Substitute.For<IActivitiesGradeHistoryXmlGradeHistory>();
        var mockInforefFileref = Substitute.For<IActivitiesInforefXmlFileref>();
        var mockInforefGradeItem = Substitute.For<ActivitiesInforefXmlGradeItem>();
        var mockInforefGradeItemref = Substitute.For<IActivitiesInforefXmlGradeItemref>();
        var mockInforefInforef = Substitute.For<IActivitiesInforefXmlInforef>();

        // Act
        var systemUnderTest = new XmlLabelFactory(mockReadDsl, mockFileSystem, mockGradeItem, mockGradeItems, mockGradebook, 
            mockLabel, mockLabelActivity, mockRoles, mockModule, mockGradeHistory, 
            mockInforefFileref, mockInforefGradeItem, mockInforefGradeItemref, mockInforefInforef);


        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.ReadDsl, Is.EqualTo(mockReadDsl));
            Assert.That(systemUnderTest.LabelId, Is.EqualTo(""));
            Assert.That(systemUnderTest.LabelName, Is.EqualTo(""));
            Assert.That(systemUnderTest.LabelParentSpaceId, Is.EqualTo(""));
            Assert.That(systemUnderTest.CurrentTime, Is.Not.Empty);
            
            Assert.That(systemUnderTest.ActivitiesGradesXmlGradeItem, Is.EqualTo(mockGradeItem));
            Assert.That(systemUnderTest.ActivitiesGradesXmlGradeItems, Is.EqualTo(mockGradeItems));
            Assert.That(systemUnderTest.ActivitiesGradesXmlActivityGradebook, Is.EqualTo(mockGradebook));
            Assert.That(systemUnderTest.ActivitiesLabelXmlLabel, Is.EqualTo(mockLabel));
            Assert.That(systemUnderTest.ActivitiesLabelXmlActivity, Is.EqualTo(mockLabelActivity));
            Assert.That(systemUnderTest.ActivitiesRolesXmlRoles, Is.EqualTo(mockRoles));
            Assert.That(systemUnderTest.ActivitiesModuleXmlModule, Is.EqualTo(mockModule));
            Assert.That(systemUnderTest.ActivitiesGradeHistoryXmlGradeHistory, Is.EqualTo(mockGradeHistory));
            Assert.That(systemUnderTest.ActivitiesInforefXmlFileref, Is.EqualTo(mockInforefFileref));
            Assert.That(systemUnderTest.ActivitiesInforefXmlGradeItem, Is.EqualTo(mockInforefGradeItem));
            Assert.That(systemUnderTest.ActivitiesInforefXmlGradeItemref, Is.EqualTo(mockInforefGradeItemref));
            Assert.That(systemUnderTest.ActivitiesInforefXmlInforef, Is.EqualTo(mockInforefInforef));
        });

    }

    [Test]
    public void XmlLabelFactory_CreateLabelFactory_ListSetMethodCalled()
    {
        // Arrange
        var mockReadDsl = Substitute.For<IReadDsl>();
        var mockFileSystem = new MockFileSystem();
        var mockGradeItem = Substitute.For<IActivitiesGradesXmlGradeItem>();
        var mockGradeItems = Substitute.For<IActivitiesGradesXmlGradeItems>();
        var mockGradebook = Substitute.For<IActivitiesGradesXmlActivityGradebook>();
        var mockLabel = Substitute.For<IActivitiesLabelXmlLabel>();
        var mockLabelActivity = Substitute.For<IActivitiesLabelXmlActivity>();
        var mockRoles = Substitute.For<IActivitiesRolesXmlRoles>();
        var mockModule = Substitute.For<IActivitiesModuleXmlModule>();
        var mockGradeHistory = Substitute.For<IActivitiesGradeHistoryXmlGradeHistory>();
        var mockInforefFileref = Substitute.For<IActivitiesInforefXmlFileref>();
        var mockInforefGradeItem = Substitute.For<ActivitiesInforefXmlGradeItem>();
        var mockInforefGradeItemref = Substitute.For<IActivitiesInforefXmlGradeItemref>();
        var mockInforefInforef = Substitute.For<IActivitiesInforefXmlInforef>();

        var systemUnderTest = new XmlLabelFactory(mockReadDsl, mockFileSystem, mockGradeItem, mockGradeItems, mockGradebook, 
            mockLabel, mockLabelActivity, mockRoles, mockModule, mockGradeHistory, 
            mockInforefFileref, mockInforefGradeItem, mockInforefGradeItemref, mockInforefInforef);

        var mockElementJson = new LearningElementJson(1, "", "element1","", "", "h5p", 1, 2,"");
        var mockLabelsElementJson = new LearningElementJson(2, "", "element2", "", "mp4","mp4", 1, 2,"");
        
        var elementJsonList = new List<LearningElementJson> {mockElementJson};
        var labelJsonList = new List<LearningElementJson> {mockLabelsElementJson};

        mockReadDsl.GetElementsOrderedList().Returns(elementJsonList);
        mockReadDsl.GetLabelsList().Returns(labelJsonList);

        // Act
        systemUnderTest.CreateLabelFactory();

        // Assert
        Assert.Multiple(() =>
        {
            mockReadDsl.Received().GetElementsOrderedList();
            Assert.That(systemUnderTest.LabelId, Is.EqualTo("1"));
            Assert.That(systemUnderTest.LabelName, Is.EqualTo("element1"));
            Assert.That(systemUnderTest.LabelParentSpaceId, Is.EqualTo("1"));
        });
    }

    [Test]
    public void XmlLabelFactory_CreateLabelFactory_AllPropertiesSetAndSerialized()
    {
        // Arrange
        var mockReadDsl = Substitute.For<IReadDsl>();
        var mockFileSystem = new MockFileSystem();
        var mockGradeItem = Substitute.For<IActivitiesGradesXmlGradeItem>();
        var mockGradeItems = Substitute.For<IActivitiesGradesXmlGradeItems>();
        var mockGradebook = Substitute.For<IActivitiesGradesXmlActivityGradebook>();
        var mockLabel = Substitute.For<IActivitiesLabelXmlLabel>();
        var mockLabelActivity = Substitute.For<IActivitiesLabelXmlActivity>();
        var mockRoles = Substitute.For<IActivitiesRolesXmlRoles>();
        var mockModule = Substitute.For<IActivitiesModuleXmlModule>();
        mockModule.PluginLocalAdlerModule = Substitute.For<ActivitiesModuleXmlPluginLocalAdlerModule>();
        var mockGradeHistory = Substitute.For<IActivitiesGradeHistoryXmlGradeHistory>();
        var mockInforefFileref = Substitute.For<IActivitiesInforefXmlFileref>();
        var mockInforefGradeItem = Substitute.For<ActivitiesInforefXmlGradeItem>();
        var mockInforefGradeItemref = Substitute.For<IActivitiesInforefXmlGradeItemref>();
        var mockInforefInforef = Substitute.For<IActivitiesInforefXmlInforef>();

        var systemUnderTest = new XmlLabelFactory(mockReadDsl, mockFileSystem, mockGradeItem, mockGradeItems, mockGradebook, 
            mockLabel, mockLabelActivity, mockRoles, mockModule, mockGradeHistory, 
            mockInforefFileref, mockInforefGradeItem, mockInforefGradeItemref, mockInforefInforef);
        
        var mockLabelsElementJson = new LearningElementJson(2, "", "",
            "","World Attributes", "World Attributes", 1, 0, "", "World Description",  new []{"World Goals"});

        var mockElementJson = new LearningElementJson(1, "", "", 
            "", "", "h5p", 0, 7,"");

        var spaceJsonList = new List<LearningElementJson> {mockElementJson};
        var labelJsonList = new List<LearningElementJson> {mockLabelsElementJson};

        mockReadDsl.GetElementsOrderedList().Returns(spaceJsonList);
        mockReadDsl.GetLabelsList().Returns(labelJsonList);

        // Act
        systemUnderTest.CreateLabelFactory();
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.ActivitiesGradesXmlGradeItem, Is.EqualTo(mockGradeItem));
            Assert.That(systemUnderTest.ActivitiesGradesXmlGradeItems, Is.EqualTo(mockGradeItems));
            Assert.That(systemUnderTest.ActivitiesGradesXmlActivityGradebook, Is.EqualTo(mockGradebook));
            systemUnderTest.ActivitiesGradesXmlActivityGradebook.Received().Serialize("label", "2");
            
            Assert.That(systemUnderTest.ActivitiesLabelXmlLabel, Is.EqualTo(mockLabel));
            Assert.That(systemUnderTest.ActivitiesLabelXmlLabel.Name, Is.EqualTo("<h5>Description:</h5> <p>World Description</p><h5>Goals:</h5> <p>World Goals</p>"));
            Assert.That(systemUnderTest.ActivitiesLabelXmlLabel.Id, Is.EqualTo("2"));
            Assert.That(systemUnderTest.ActivitiesLabelXmlLabel.Intro, Is.EqualTo("<h5>Description:</h5> <p>World Description</p><h5>Goals:</h5> <p>World Goals</p>"));
            Assert.That(systemUnderTest.ActivitiesLabelXmlLabel.Timemodified, Is.EqualTo(systemUnderTest.CurrentTime));
            Assert.That(systemUnderTest.ActivitiesLabelXmlActivity.Id, Is.EqualTo("2"));
            Assert.That(systemUnderTest.ActivitiesLabelXmlActivity.ModuleId, Is.EqualTo("2"));
            Assert.That(systemUnderTest.ActivitiesLabelXmlActivity.ContextId, Is.EqualTo("2"));
            Assert.That(systemUnderTest.ActivitiesLabelXmlActivity, Is.EqualTo(mockLabelActivity));
            systemUnderTest.ActivitiesLabelXmlActivity.Received().Serialize("label", "2");
            
            Assert.That(systemUnderTest.ActivitiesRolesXmlRoles, Is.EqualTo(mockRoles));
            systemUnderTest.ActivitiesRolesXmlRoles.Received().Serialize("label", "2");
            
            Assert.That(systemUnderTest.ActivitiesModuleXmlModule, Is.EqualTo(mockModule));
            Assert.That(systemUnderTest.ActivitiesModuleXmlModule.ModuleName, Is.EqualTo("label"));
            Assert.That(systemUnderTest.ActivitiesModuleXmlModule.SectionId, Is.EqualTo("1"));
            Assert.That(systemUnderTest.ActivitiesModuleXmlModule.SectionNumber, Is.EqualTo("1"));
            Assert.That(systemUnderTest.ActivitiesModuleXmlModule.Added, Is.EqualTo(systemUnderTest.CurrentTime));
            Assert.That(systemUnderTest.ActivitiesModuleXmlModule.Id, Is.EqualTo("2"));
            systemUnderTest.ActivitiesModuleXmlModule.Received().Serialize("label","2");
            
            Assert.That(systemUnderTest.ActivitiesGradeHistoryXmlGradeHistory, Is.EqualTo(mockGradeHistory));
            systemUnderTest.ActivitiesGradeHistoryXmlGradeHistory.Received().Serialize("label", "2");
            
            Assert.That(systemUnderTest.ActivitiesInforefXmlFileref, Is.EqualTo(mockInforefFileref));
            Assert.That(systemUnderTest.ActivitiesInforefXmlGradeItem, Is.EqualTo(mockInforefGradeItem));
            Assert.That(systemUnderTest.ActivitiesInforefXmlGradeItemref, Is.EqualTo(mockInforefGradeItemref));
            Assert.That(systemUnderTest.ActivitiesInforefXmlInforef, Is.EqualTo(mockInforefInforef));
            systemUnderTest.ActivitiesInforefXmlInforef.Received().Serialize("label", "2");
        });
        
        
    }
}