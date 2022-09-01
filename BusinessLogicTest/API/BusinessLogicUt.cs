using AutoMapper;
using BusinessLogic.API;
using BusinessLogic.Entities;
using NSubstitute;
using NUnit.Framework;
using PersistEntities;
using Shared;
using Shared.Configuration;

namespace BusinessLogicTest.API;

[TestFixture]
public class BusinessLogicUt
{
    [Test]
    public void StandardConstructor_AllPropertiesInitialized()
    {
        var mockConfiguration = Substitute.For<IAuthoringToolConfiguration>();
        var mockDataAccess = Substitute.For<IDataAccess>();
        var mockGenerator = Substitute.For<IWorldGenerator>();
        
        var systemUnderTest = CreateStandardBusinessLogic(mockConfiguration, mockDataAccess);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Configuration, Is.EqualTo(mockConfiguration));
            Assert.That(systemUnderTest.DataAccess, Is.EqualTo(mockDataAccess));
        });
    }

    [Test]
    public void ConstructBackup_CallsWorldGenerator()
    {
        var mockWorldGenerator = Substitute.For<IWorldGenerator>();

        var systemUnderTest = CreateStandardBusinessLogic(worldGenerator: mockWorldGenerator);

        systemUnderTest.ConstructBackup(null!, "foobar");

        mockWorldGenerator.Received().ConstructBackup(null!, "foobar");
    }

    [Test]
    public void SaveLearningWorld_CallsDataAccess()
    {
        var learningWorld = new LearningWorld("fa", "a", "f", "f", "f", "f");
        var mockDataAccess = Substitute.For<IDataAccess>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.SaveLearningWorld(learningWorld, "foobar");

        mockDataAccess.Received().SaveLearningWorldToFile(learningWorld, "foobar");
    }

    [Test]
    public void LoadLearningWorld_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.LoadLearningWorld("foobar");

        mockDataAccess.Received().LoadLearningWorld("foobar");
    }

    [Test]
    public void LoadLearningWorld_ReturnsLearningWorld()
    {
        var learningWorld = new LearningWorld("fa", "a", "f", "f", "f", "f");
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.LoadLearningWorld("foobar").Returns(learningWorld);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var learningWorldActual = systemUnderTest.LoadLearningWorld("foobar");

        Assert.That(learningWorldActual, Is.EqualTo(learningWorld));
    }

    [Test]
    public void SaveLearningSpace_CallsDataAccess()
    {
        var learningSpace = new LearningSpace("fa", "a", "f", "f", "f");
        var mockDataAccess = Substitute.For<IDataAccess>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.SaveLearningSpace(learningSpace, "foobar");

        mockDataAccess.Received().SaveLearningSpaceToFile(learningSpace, "foobar");
    }

    [Test]
    public void LoadLearningSpace_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.LoadLearningSpace("foobar");

        mockDataAccess.Received().LoadLearningSpace("foobar");
    }

    [Test]
    public void LoadLearningSpace_ReturnsLearningSpace()
    {
        var learningSpace = new LearningSpace("fa", "a", "f", "f", "f");
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.LoadLearningSpace("foobar").Returns(learningSpace);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var learningSpaceActual = systemUnderTest.LoadLearningSpace("foobar");

        Assert.That(learningSpaceActual, Is.EqualTo(learningSpace));
    }

    [Test]
    public void SaveLearningElement_CallsDataAccess()
    {
        var content = new LearningContent("a", "b", Array.Empty<byte>());
        var learningElement = new LearningElement("fa", "f", content, "f",
            "f", "f", LearningElementDifficultyEnum.Easy, null);
        var mockDataAccess = Substitute.For<IDataAccess>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.SaveLearningElement(learningElement, "foobar");

        mockDataAccess.Received().SaveLearningElementToFile(learningElement, "foobar");
    }

    [Test]
    public void LoadLearningElement_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.LoadLearningElement("foobar");

        mockDataAccess.Received().LoadLearningElement("foobar");
    }

    [Test]
    public void LoadLearningElement_ReturnsLearningElement()
    {
        var content = new LearningContent("a", "b", Array.Empty<byte>());
        var learningElement = new LearningElement("fa", "a", content, "f", "f",
            "f", LearningElementDifficultyEnum.Easy, null);
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.LoadLearningElement("foobar").Returns(learningElement);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var learningElementActual = systemUnderTest.LoadLearningElement("foobar");

        Assert.That(learningElementActual, Is.EqualTo(learningElement));
    }

    [Test]
    public void LoadLearningContent_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.LoadLearningContent("foobar");

        mockDataAccess.Received().LoadLearningContent("foobar");
    }

    [Test]
    public void LoadLearningContent_ReturnsLearningElement()
    {
        var learningContent = new LearningContent("fa", "a", new byte[] {0x01, 0x02, 0x03});
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.LoadLearningContent("foobar").Returns(learningContent);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var learningElementActual = systemUnderTest.LoadLearningContent("foobar");

        Assert.That(learningElementActual, Is.EqualTo(learningContent));
    }

    [Test]
    public void LoadLearningWorldFromStream_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var stream = Substitute.For<Stream>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.LoadLearningWorld(stream);

        mockDataAccess.Received().LoadLearningWorld(stream);
    }

    [Test]
    public void LoadLearningWorldFromStream_ReturnsLearningWorld()
    {
        var learningWorld = new LearningWorld("fa", "a", "f", "f", "f", "f");
        var stream = Substitute.For<Stream>();
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.LoadLearningWorld(stream).Returns(learningWorld);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var learningWorldActual = systemUnderTest.LoadLearningWorld(stream);

        Assert.That(learningWorldActual, Is.EqualTo(learningWorld));
    }

    [Test]
    public void LoadLearningSpaceFromStream_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var stream = Substitute.For<Stream>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.LoadLearningSpace(stream);

        mockDataAccess.Received().LoadLearningSpace(stream);
    }

    [Test]
    public void LoadLearningSpaceFromStream_ReturnsLearningSpace()
    {
        var learningSpace = new LearningSpace("fa", "a", "f", "f", "f");
        var stream = Substitute.For<Stream>();
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.LoadLearningSpace(stream).Returns(learningSpace);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var learningSpaceActual = systemUnderTest.LoadLearningSpace(stream);

        Assert.That(learningSpaceActual, Is.EqualTo(learningSpace));
    }

    [Test]
    public void LoadLearningElementFromStream_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var stream = Substitute.For<Stream>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.LoadLearningElement(stream);

        mockDataAccess.Received().LoadLearningElement(stream);
    }

    [Test]
    public void LoadLearningElementFromStream_ReturnsLearningElement()
    {
        var content = new LearningContent("a", "b", Array.Empty<byte>());
        var learningElement = new LearningElement("fa", "a", content, "f", "f",
            "f", LearningElementDifficultyEnum.Easy, null);
        var stream = Substitute.For<Stream>();
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.LoadLearningElement(stream).Returns(learningElement);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var learningElementActual = systemUnderTest.LoadLearningElement(stream);

        Assert.That(learningElementActual, Is.EqualTo(learningElement));
    }

    [Test]
    public void LoadLearningContentFromStream_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var stream = Substitute.For<Stream>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.LoadLearningContent("filename.extension", stream);

        mockDataAccess.Received().LoadLearningContent("filename.extension", stream);
    }

    [Test]
    public void LoadLearningContentFromStream_ReturnsLearningElement()
    {
        var learningContent = new LearningContent("filename", "extension", Array.Empty<byte>());
        var stream = Substitute.For<Stream>();
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.LoadLearningContent("filename.extension", stream).Returns(learningContent);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var learningElementActual = systemUnderTest.LoadLearningContent("filename.extension", stream);

        Assert.That(learningElementActual, Is.EqualTo(learningContent));
    }

    [Test]
    public void FindSuitableNewSavePath_CallsDataAccess()
    {
        var dataAccess = Substitute.For<IDataAccess>();
        var systemUnderTest = CreateStandardBusinessLogic(fakeDataAccess: dataAccess);

        systemUnderTest.FindSuitableNewSavePath("foo", "bar", "baz");

        dataAccess.Received().FindSuitableNewSavePath("foo", "bar", "baz");
    }

    private BusinessLogic.API.BusinessLogic CreateStandardBusinessLogic(
        IAuthoringToolConfiguration? fakeConfiguration = null,
        IDataAccess? fakeDataAccess = null,
        IWorldGenerator? worldGenerator = null)
    {
        fakeConfiguration ??= Substitute.For<IAuthoringToolConfiguration>();
        fakeDataAccess ??= Substitute.For<IDataAccess>();
        worldGenerator ??= Substitute.For<IWorldGenerator>();
        
        return new BusinessLogic.API.BusinessLogic(fakeConfiguration, fakeDataAccess, worldGenerator);
    }
}