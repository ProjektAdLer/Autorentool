using AuthoringTool.API.Configuration;
using NUnit.Framework;
using AuthoringTool.DataAccess.API;
using AuthoringTool.Entities;
using ElectronWrapper;
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
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Configuration, Is.EqualTo(mockConfiguration));
            Assert.That(systemUnderTest.DataAccess, Is.EqualTo(mockDataAccess));
        });
    }

    [Test]
    public void BusinessLogic_ConstructBackup_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.ConstructBackup();

        mockDataAccess.Received().ConstructBackup();
    }

    [Test]
    public void BusinessLogic_SaveLearningWorld_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var learningWorld = new LearningWorld("fa", "a", "f", "f", "f", "f");

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.SaveLearningWorld(learningWorld, "foobar");

        mockDataAccess.Received().SaveLearningWorldToFile(learningWorld, "foobar");
    }

    [Test]
    public void BusinessLogic_LoadLearningWorld_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.LoadLearningWorld("foobar");

        mockDataAccess.Received().LoadLearningWorldFromFile("foobar");
    }

    [Test]
    public void BusinessLogic_LoadLearningWorld_ReturnsLearningWorld()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var learningWorld = new LearningWorld("fa", "a", "f", "f", "f", "f");
        mockDataAccess.LoadLearningWorldFromFile("foobar").Returns(learningWorld);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var learningWorldActual = systemUnderTest.LoadLearningWorld("foobar");

        Assert.That(learningWorldActual, Is.EqualTo(learningWorld));
    }

    [Test]
    public void BusinessLogic_SaveLearningSpace_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var learningSpace = new LearningSpace("fa", "a", "f", "f", "f");

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.SaveLearningSpace(learningSpace, "foobar");

        mockDataAccess.Received().SaveLearningSpaceToFile(learningSpace, "foobar");
    }

    [Test]
    public void BusinessLogic_LoadLearningSpace_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.LoadLearningSpace("foobar");

        mockDataAccess.Received().LoadLearningSpaceFromFile("foobar");
    }

    [Test]
    public void BusinessLogic_LoadLearningSpace_ReturnsLearningSpace()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var learningSpace = new LearningSpace("fa", "a", "f", "f", "f");
        mockDataAccess.LoadLearningSpaceFromFile("foobar").Returns(learningSpace);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var learningSpaceActual = systemUnderTest.LoadLearningSpace("foobar");

        Assert.That(learningSpaceActual, Is.EqualTo(learningSpace));
    }

    [Test]
    public void BusinessLogic_SaveLearningElement_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var learningElement = new LearningElement("fa", "f", "f", "f", "f", "f", "f", "f");

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.SaveLearningElement(learningElement, "foobar");

        mockDataAccess.Received().SaveLearningElementToFile(learningElement, "foobar");
    }

    [Test]
    public void BusinessLogic_LoadLearningElement_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.LoadLearningElement("foobar");

        mockDataAccess.Received().LoadLearningElementFromFile("foobar");
    }

    [Test]
    public void BusinessLogic_LoadLearningElement_ReturnsLearningElement()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var learningElement = new LearningElement("fa","a", "f", "f", "f", "f", "f", "f");
        mockDataAccess.LoadLearningElementFromFile("foobar").Returns(learningElement);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var learningElementActual = systemUnderTest.LoadLearningElement("foobar");

        Assert.That(learningElementActual, Is.EqualTo(learningElement));
    }

    private AuthoringTool.BusinessLogic.API.BusinessLogic CreateStandardBusinessLogic(
        IAuthoringToolConfiguration? fakeConfiguration = null,
        IDataAccess? fakeDataAccess = null,
        IHybridSupportWrapper? fakeHybridSupportWrapper = null)
    {
        fakeConfiguration ??= Substitute.For<IAuthoringToolConfiguration>();
        fakeDataAccess ??= Substitute.For<IDataAccess>();
        fakeHybridSupportWrapper ??= Substitute.For<IHybridSupportWrapper>();
        return new AuthoringTool.BusinessLogic.API.BusinessLogic(fakeConfiguration, fakeDataAccess,
            fakeHybridSupportWrapper);
    }
}