using AuthoringTool.API.Configuration;
using NUnit.Framework;
using AuthoringTool.BusinessLogic.API;
using AuthoringTool.DataAccess.API;
using AuthoringTool.Entities;
using NSubstitute;

namespace AuthoringToolTest.BusinessLogic.API;

[TestFixture]
public class BusinessLogicUt
{
    [Test]
    public void BusinessLogic_StandardConstructor_AllPropertiesInitialized()
    {
        IAuthoringToolConfiguration mockConfiguration = Substitute.For<IAuthoringToolConfiguration>();
        IDataAccess mockDataAccess = Substitute.For<IDataAccess>();
        var systemUnderTest = CreateStandardBusinessLogic(mockConfiguration, mockDataAccess);

        Assert.AreEqual(mockConfiguration, systemUnderTest.Configuration);
        Assert.AreEqual(mockDataAccess, systemUnderTest.DataAccess);
    }

    [Test]
    public void BusinessLogic_ConstructBackup_CallsDataAccess()
    {
        IDataAccess mockDataAccess = Substitute.For<IDataAccess>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.ConstructBackup();

        mockDataAccess.Received().ConstructBackup();
    }

    [Test]
    public void BusinessLogic_SaveLearningWorld_CallsDataAccess()
    {
        IDataAccess mockDataAccess = Substitute.For<IDataAccess>();
        var learningWorld = new LearningWorld("fa", "a", "f", "f", "f", "f");

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.SaveLearningWorld(learningWorld, "foobar");

        mockDataAccess.Received().SaveLearningWorldToFile(learningWorld, "foobar");
    }

    [Test]
    public void BusinessLogic_LoadLearningWorld_CallsDataAccess()
    {
        IDataAccess mockDataAccess = Substitute.For<IDataAccess>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.LoadLearningWorld("foobar");

        mockDataAccess.Received().LoadLearningWorldFromFile("foobar");
    }

    [Test]
    public void BusinessLogic_LoadLearningWorld_ReturnsLearningWorld()
    {
        IDataAccess mockDataAccess = Substitute.For<IDataAccess>();
        var learningWorld = new LearningWorld("fa", "a", "f", "f", "f", "f");
        mockDataAccess.LoadLearningWorldFromFile("foobar").Returns(learningWorld);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var learningWorldActual = systemUnderTest.LoadLearningWorld("foobar");

        Assert.That(learningWorldActual, Is.EqualTo(learningWorld));
    }

    [Test]
    public void BusinessLogic_SaveLearningSpace_CallsDataAccess()
    {
        IDataAccess mockDataAccess = Substitute.For<IDataAccess>();
        var learningSpace = new LearningSpace("fa", "a", "f", "f", "f");

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.SaveLearningSpace(learningSpace, "foobar");

        mockDataAccess.Received().SaveLearningSpaceToFile(learningSpace, "foobar");
    }

    [Test]
    public void BusinessLogic_LoadLearningSpace_CallsDataAccess()
    {
        IDataAccess mockDataAccess = Substitute.For<IDataAccess>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.LoadLearningSpace("foobar");

        mockDataAccess.Received().LoadLearningSpaceFromFile("foobar");
    }

    [Test]
    public void BusinessLogic_LoadLearningSpace_ReturnsLearningSpace()
    {
        IDataAccess mockDataAccess = Substitute.For<IDataAccess>();
        var learningSpace = new LearningSpace("fa", "a", "f", "f", "f");
        mockDataAccess.LoadLearningSpaceFromFile("foobar").Returns(learningSpace);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var learningSpaceActual = systemUnderTest.LoadLearningSpace("foobar");

        Assert.That(learningSpaceActual, Is.EqualTo(learningSpace));
    }

    [Test]
    public void BusinessLogic_SaveLearningElement_CallsDataAccess()
    {
        IDataAccess mockDataAccess = Substitute.For<IDataAccess>();
        var learningElement = new LearningElement("fa", "a", "f", "f", "f", "f", "f");

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.SaveLearningElement(learningElement, "foobar");

        mockDataAccess.Received().SaveLearningElementToFile(learningElement, "foobar");
    }

    [Test]
    public void BusinessLogic_LoadLearningElement_CallsDataAccess()
    {
        IDataAccess mockDataAccess = Substitute.For<IDataAccess>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.LoadLearningElement("foobar");

        mockDataAccess.Received().LoadLearningElementFromFile("foobar");
    }

    [Test]
    public void BusinessLogic_LoadLearningElement_ReturnsLearningElement()
    {
        IDataAccess mockDataAccess = Substitute.For<IDataAccess>();
        var learningElement = new LearningElement("fa", "a", "f", "f", "f", "f", "f");
        mockDataAccess.LoadLearningElementFromFile("foobar").Returns(learningElement);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var learningElementActual = systemUnderTest.LoadLearningElement("foobar");

        Assert.That(learningElementActual, Is.EqualTo(learningElement));
    }

    private AuthoringTool.BusinessLogic.API.BusinessLogic CreateStandardBusinessLogic(
        IAuthoringToolConfiguration? fakeConfiguration = null,
        IDataAccess? fakeDataAccess = null)
    {
        fakeConfiguration ??= Substitute.For<IAuthoringToolConfiguration>();
        fakeDataAccess ??= Substitute.For<IDataAccess>();
        return new AuthoringTool.BusinessLogic.API.BusinessLogic(fakeConfiguration, fakeDataAccess);
    }
}