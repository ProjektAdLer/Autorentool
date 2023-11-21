using System.IO.Abstractions.TestingHelpers;
using Generator.ATF;
using Generator.ATF.AdaptivityElement;
using Generator.WorldExport;
using Generator.XmlClasses.Entities._activities.Adleradaptivity.xml;
using Generator.XmlClasses.Entities._activities.GradeHistory.xml;
using Generator.XmlClasses.Entities._activities.Grades.xml;
using Generator.XmlClasses.Entities._activities.Inforef.xml;
using Generator.XmlClasses.Entities._activities.Module.xml;
using Generator.XmlClasses.Entities._activities.Roles.xml;
using Generator.XmlClasses.XmlFileFactories;
using GeneratorTest.Xsd;
using NSubstitute;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.XmlFileFactories;

[TestFixture]
public class XmlAdaptivityFactoryUt
{
    [Test]
    public void Constructor_AllParametersSet()
    {
        var mockReadAtf = Substitute.For<IReadAtf>();
        var mockFileSystem = new MockFileSystem();

        var systemUnderTest = new XmlAdaptivityFactory(mockReadAtf, mockFileSystem);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.ReadAtf, Is.EqualTo(mockReadAtf));
            Assert.That(systemUnderTest.ActivitiesGradesXmlGradeItem, Is.Not.Null);
            Assert.That(systemUnderTest.ActivitiesGradesXmlGradeItems, Is.Not.Null);
            Assert.That(systemUnderTest.ActivitiesGradesXmlActivityGradebook, Is.Not.Null);
            Assert.That(systemUnderTest.ActivitiesRolesXmlRoles, Is.Not.Null);
            Assert.That(systemUnderTest.ActivitiesModuleXmlModule, Is.Not.Null);
            Assert.That(systemUnderTest.ActivitiesGradeHistoryXmlGradeHistory, Is.Not.Null);
            Assert.That(systemUnderTest.ActivitiesInforefXmlFileref, Is.Not.Null);
            Assert.That(systemUnderTest.ActivitiesInforefXmlGradeItem, Is.Not.Null);
            Assert.That(systemUnderTest.ActivitiesInforefXmlGradeItemref, Is.Not.Null);
            Assert.That(systemUnderTest.ActivitiesInforefXmlInforef, Is.Not.Null);
            Assert.That(systemUnderTest.ActivitiesAdleradaptivityXmlActivity, Is.Not.Null);
            Assert.That(systemUnderTest.AdaptivityElementId, Is.EqualTo("0"));
            Assert.That(systemUnderTest.AdaptivityElementName, Is.EqualTo(""));
            Assert.That(systemUnderTest.AdaptivityElementParentSpaceId, Is.EqualTo(""));
            Assert.That(systemUnderTest.AdaptivityElementPoints, Is.EqualTo(0));
            Assert.That(systemUnderTest.AdaptivityElementUuid, Is.EqualTo(""));
            Assert.That(systemUnderTest.CurrentTime, Is.Not.Empty);
            Assert.That(systemUnderTest.ListAdaptivityElements, Is.Not.Null);
            Assert.That(systemUnderTest.FileSystem, Is.Not.Null);
        });
    }

    [Test]
    public void CreateXmlAdaptivityFactory_SetsParametersAndSerializes()
    {
        var mockReadAtf = Substitute.For<IReadAtf>();
        var mockFileSystem = new MockFileSystem();
        var mockGradesGradebook = Substitute.For<IActivitiesGradesXmlActivityGradebook>();
        var mockRoles = Substitute.For<IActivitiesRolesXmlRoles>();
        var mockModule = Substitute.For<IActivitiesModuleXmlModule>();
        var mockGradeHistory = Substitute.For<IActivitiesGradeHistoryXmlGradeHistory>();
        var mockInforef = Substitute.For<IActivitiesInforefXmlInforef>();

        var mockAdlerPluginModule = Substitute.For<ActivitiesModuleXmlPluginLocalAdlerModule>();

        var systemUnderTest = new XmlAdaptivityFactory(mockReadAtf, mockFileSystem, null, null, mockGradesGradebook,
            null, mockRoles, mockModule, mockGradeHistory, null, null, null, mockInforef);
        systemUnderTest.ActivitiesModuleXmlModule.PluginLocalAdlerModule = mockAdlerPluginModule;

        var adaptivityElementJson1 = new AdaptivityElementJson(1,
            "9876", "element1", "adaptivity", "adaptivity", 2, 5, "",
            new AdaptivityContentJson("introText1", new List<IAdaptivityTaskJson>()
            {
                new AdaptivityTaskJson(1, "1234", "task1", false, 100, new List<IAdaptivityQuestionJson>()
                {
                    new AdaptivityQuestionJson(ResponseType.singleResponse, 1, "", 100, "question1",
                        new List<IAdaptivityRuleJson>()
                        {
                            new AdaptivityRuleJson(1, "incorrect", new CommentActionJson("Falsche Antwort1")),
                        },
                        new List<IChoiceJson>(new IChoiceJson[]
                            { new ChoiceJson("choice11", true), new ChoiceJson("choice12", false) })),
                }),
                new AdaptivityTaskJson(2, "2345", "task2", true, 000, new List<IAdaptivityQuestionJson>()
                {
                    new AdaptivityQuestionJson(ResponseType.multipleResponse, 2, "", 100, "question2",
                        new List<IAdaptivityRuleJson>()
                        {
                            new AdaptivityRuleJson(1, "incorrect", new CommentActionJson("Falsche Antwort2")),
                        },
                        new List<IChoiceJson>(new IChoiceJson[]
                        {
                            new ChoiceJson("choice21", true), new ChoiceJson("choice22", false),
                            new ChoiceJson("choice23", true)
                        })),
                    new AdaptivityQuestionJson(ResponseType.singleResponse, 3, "", 200, "question3",
                        new List<IAdaptivityRuleJson>()
                        {
                            new AdaptivityRuleJson(1, "incorrect", new CommentActionJson("Falsche Antwort3")),
                        },
                        new List<IChoiceJson>(new IChoiceJson[]
                            { new ChoiceJson("choice31", true), new ChoiceJson("choice32", false) }))
                })
            }), "adaptivityDescription", new[] { "goals1", "goals2" });

        var adaptivityElementsList = new List<IAdaptivityElementJson> { adaptivityElementJson1 };
        mockReadAtf.GetAdaptivityElementsList().Returns(adaptivityElementsList);

        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        systemUnderTest.CreateXmlAdaptivityFactory();

        Assert.That(systemUnderTest.AdaptivityElementId, Is.EqualTo(adaptivityElementJson1.ElementId.ToString()));
        Assert.That(systemUnderTest.AdaptivityElementName, Is.EqualTo(adaptivityElementJson1.ElementName));
        Assert.That(systemUnderTest.AdaptivityElementUuid, Is.EqualTo(adaptivityElementJson1.ElementUUID));
        Assert.That(systemUnderTest.AdaptivityElementParentSpaceId,
            Is.EqualTo(adaptivityElementJson1.LearningSpaceParentId.ToString()));
        Assert.That(systemUnderTest.AdaptivityElementPoints, Is.EqualTo(adaptivityElementJson1.ElementMaxScore));

        mockGradesGradebook.Received().Serialize("adleradaptivity", "1");
        Assert.That(systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Id, Is.EqualTo("1"));
        Assert.That(systemUnderTest.ActivitiesAdleradaptivityXmlActivity.ModuleId, Is.EqualTo("1"));
        Assert.That(systemUnderTest.ActivitiesAdleradaptivityXmlActivity.ModuleName, Is.EqualTo("adleradaptivity"));
        Assert.That(systemUnderTest.ActivitiesAdleradaptivityXmlActivity.ContextId, Is.EqualTo("0"));
        Assert.That(systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Adleradaptivity, Is.Not.Null);
        Assert.That(systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Adleradaptivity.Id, Is.EqualTo("1"));
        Assert.That(systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Adleradaptivity.Name, Is.EqualTo("element1"));
        Assert.That(systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Adleradaptivity.Intro,
            Is.EqualTo("<h5>Description:</h5> <p>adaptivityDescription</p><h5>Goals:</h5> <p>goals1<br>goals2</p>"));
        Assert.That(systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Adleradaptivity.IntroFormat, Is.EqualTo("1"));
        Assert.That(systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Adleradaptivity.TimeModified,
            Is.Not.Empty);
        Assert.That(systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Adleradaptivity.Tasks, Is.Not.Null);
        Assert.That(systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Adleradaptivity.Tasks.Tasks, Is.Not.Null);
        Assert.That(systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Adleradaptivity.Tasks.Tasks.Count,
            Is.EqualTo(2));
        Assert.That(systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Adleradaptivity.Tasks.Tasks[0].Id,
            Is.EqualTo("1"));
        Assert.That(systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Adleradaptivity.Tasks.Tasks[0].Title,
            Is.EqualTo("task1"));
        Assert.That(systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Adleradaptivity.Tasks.Tasks[0].Uuid,
            Is.EqualTo("1234"));
        Assert.That(
            systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Adleradaptivity.Tasks.Tasks[0].RequiredDifficulty,
            Is.EqualTo("100"));
        Assert.That(systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Adleradaptivity.Tasks.Tasks[0].Questions,
            Is.Not.Null);
        Assert.That(
            systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Adleradaptivity.Tasks.Tasks[0].Questions.Questions,
            Is.Not.Null);
        Assert.That(
            systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Adleradaptivity.Tasks.Tasks[0].Questions.Questions
                .Count, Is.EqualTo(1));
        Assert.That(
            systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Adleradaptivity.Tasks.Tasks[0].Questions.Questions[0]
                .Id, Is.EqualTo("1"));
        Assert.That(
            systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Adleradaptivity.Tasks.Tasks[0].Questions.Questions[0]
                .Difficulty, Is.EqualTo("100"));
        Assert.That(
            systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Adleradaptivity.Tasks.Tasks[0].Questions.Questions[0]
                .QuestionReference.Id, Is.EqualTo("1"));
        Assert.That(
            systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Adleradaptivity.Tasks.Tasks[0].Questions.Questions[0]
                .QuestionReference.UsingContextId, Is.EqualTo("0"));
        Assert.That(
            systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Adleradaptivity.Tasks.Tasks[0].Questions.Questions[0]
                .QuestionReference.Component, Is.EqualTo("mod_adleradaptivity"));
        Assert.That(
            systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Adleradaptivity.Tasks.Tasks[0].Questions.Questions[0]
                .QuestionReference.QuestionArea, Is.EqualTo("question"));
        Assert.That(
            systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Adleradaptivity.Tasks.Tasks[0].Questions.Questions[0]
                .QuestionReference.QuestionBankEntryId, Is.EqualTo("1"));
        Assert.That(
            systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Adleradaptivity.Tasks.Tasks[0].Questions.Questions[0]
                .QuestionReference.Version, Is.EqualTo("1"));
        Assert.That(systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Adleradaptivity.Tasks.Tasks[1].Id,
            Is.EqualTo("2"));
        Assert.That(systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Adleradaptivity.Tasks.Tasks[1].Title,
            Is.EqualTo("task2"));
        Assert.That(systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Adleradaptivity.Tasks.Tasks[1].Uuid,
            Is.EqualTo("2345"));
        Assert.That(
            systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Adleradaptivity.Tasks.Tasks[1].RequiredDifficulty,
            Is.EqualTo("$@NULL@$"));
        Assert.That(systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Adleradaptivity.Tasks.Tasks[1].Questions,
            Is.Not.Null);
        Assert.That(
            systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Adleradaptivity.Tasks.Tasks[1].Questions.Questions,
            Is.Not.Null);
        Assert.That(
            systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Adleradaptivity.Tasks.Tasks[1].Questions.Questions
                .Count, Is.EqualTo(2));
        Assert.That(
            systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Adleradaptivity.Tasks.Tasks[1].Questions.Questions[0]
                .Id, Is.EqualTo("2"));
        Assert.That(
            systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Adleradaptivity.Tasks.Tasks[1].Questions.Questions[0]
                .Difficulty, Is.EqualTo("100"));
        Assert.That(
            systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Adleradaptivity.Tasks.Tasks[1].Questions.Questions[0]
                .QuestionReference.Id, Is.EqualTo("2"));
        Assert.That(
            systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Adleradaptivity.Tasks.Tasks[1].Questions.Questions[0]
                .QuestionReference.QuestionBankEntryId, Is.EqualTo("2"));
        Assert.That(
            systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Adleradaptivity.Tasks.Tasks[1].Questions.Questions[1]
                .Id, Is.EqualTo("3"));
        Assert.That(
            systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Adleradaptivity.Tasks.Tasks[1].Questions.Questions[1]
                .Difficulty, Is.EqualTo("200"));
        Assert.That(
            systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Adleradaptivity.Tasks.Tasks[1].Questions.Questions[1]
                .QuestionReference.Id, Is.EqualTo("3"));
        Assert.That(
            systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Adleradaptivity.Tasks.Tasks[1].Questions.Questions[1]
                .QuestionReference.QuestionBankEntryId, Is.EqualTo("3"));

        mockRoles.Received().Serialize("adleradaptivity", "1");

        var activitiesAdleradaptivityXmlActivity = new ActivitiesAdleradaptivityXmlActivity();

        activitiesAdleradaptivityXmlActivity.Adleradaptivity =
            systemUnderTest.ActivitiesAdleradaptivityXmlActivity.Adleradaptivity;

        var serializedXml = XmlSerializerHelper.SerializeObjectToXmlString(activitiesAdleradaptivityXmlActivity);

        var adleractivityXmlXsd = XsdFileProvider.AdleradaptivityXmlXsd;

        var isValid = XmlSerializerHelper.ValidateXmlAgainstXsd(serializedXml, adleractivityXmlXsd);

        Assert.That(isValid, Is.True);

        Assert.That(systemUnderTest.ActivitiesModuleXmlModule.ModuleName, Is.EqualTo("adleradaptivity"));
        Assert.That(systemUnderTest.ActivitiesModuleXmlModule.ShowDescription, Is.EqualTo("1"));
        Assert.That(systemUnderTest.ActivitiesModuleXmlModule.Indent, Is.EqualTo("1"));
        Assert.That(systemUnderTest.ActivitiesModuleXmlModule.SectionId, Is.EqualTo("2"));
        Assert.That(systemUnderTest.ActivitiesModuleXmlModule.SectionNumber, Is.EqualTo("2"));
        Assert.That(systemUnderTest.ActivitiesModuleXmlModule.Added, Is.Not.Null);
        Assert.That(systemUnderTest.ActivitiesModuleXmlModule.Id, Is.EqualTo("1"));
        Assert.That(systemUnderTest.ActivitiesModuleXmlModule.Completion, Is.EqualTo("2"));
        Assert.That(systemUnderTest.ActivitiesModuleXmlModule.PluginLocalAdlerModule.AdlerModule, Is.Not.Null);
        Assert.That(systemUnderTest.ActivitiesModuleXmlModule.PluginLocalAdlerModule.AdlerModule!.ScoreMax,
            Is.EqualTo("5.00000"));
        Assert.That(systemUnderTest.ActivitiesModuleXmlModule.PluginLocalAdlerModule.AdlerModule!.Uuid,
            Is.EqualTo("9876"));
        mockModule.Received().Serialize("adleradaptivity", "1");

        mockGradeHistory.Received().Serialize("adleradaptivity", "1");

        mockInforef.Received().Serialize("adleradaptivity", "1");
    }
}