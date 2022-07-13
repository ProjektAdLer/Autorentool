using System;
using System.IO;
using AuthoringTool.API.Configuration;
using NUnit.Framework;
using AuthoringTool.DataAccess.API;
using AuthoringTool.Entities;
using AuthoringTool.PresentationLogic.LearningElement;
using ElectronWrapper;
using NSubstitute;

namespace AuthoringToolTest.BusinessLogic.API;

[TestFixture]
public class BusinessLogicUt
{
    [Test]
    public void StandardConstructor_AllPropertiesInitialized()
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
    public void ConstructBackup_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.ConstructBackup(null!, "foobar");

        mockDataAccess.Received().ConstructBackup(null!, "foobar");
    }

    [Test]
    public void SaveLearningWorld_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var learningWorld = new LearningWorld("fa", "a", "f", "f", "f", "f");

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

        mockDataAccess.Received().LoadLearningWorldFromFile("foobar");
    }

    [Test]
    public void LoadLearningWorld_ReturnsLearningWorld()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var learningWorld = new LearningWorld("fa", "a", "f", "f", "f", "f");
        mockDataAccess.LoadLearningWorldFromFile("foobar").Returns(learningWorld);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var learningWorldActual = systemUnderTest.LoadLearningWorld("foobar");

        Assert.That(learningWorldActual, Is.EqualTo(learningWorld));
    }

    [Test]
    public void SaveLearningSpace_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var learningSpace = new LearningSpace("fa", "a", "f", "f", "f");

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

        mockDataAccess.Received().LoadLearningSpaceFromFile("foobar");
    }

    [Test]
    public void LoadLearningSpace_ReturnsLearningSpace()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var learningSpace = new LearningSpace("fa", "a", "f", "f", "f");
        mockDataAccess.LoadLearningSpaceFromFile("foobar").Returns(learningSpace);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var learningSpaceActual = systemUnderTest.LoadLearningSpace("foobar");

        Assert.That(learningSpaceActual, Is.EqualTo(learningSpace));
    }

    [Test]
    public void SaveLearningElement_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var content = new LearningContent("a", "b", Array.Empty<byte>());
        var learningElement = new LearningElement("fa", "f", "f", content, "f",
            "f", "f", LearningElementDifficultyEnum.Easy);

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

        mockDataAccess.Received().LoadLearningElementFromFile("foobar");
    }

    [Test]
    public void LoadLearningElement_ReturnsLearningElement()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var content = new LearningContent("a", "b", Array.Empty<byte>());
        var learningElement = new LearningElement("fa", "a", "f", content, "f", "f",
            "f", LearningElementDifficultyEnum.Easy);
        mockDataAccess.LoadLearningElementFromFile("foobar").Returns(learningElement);

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

        mockDataAccess.Received().LoadLearningContentFromFile("foobar");
    }

    [Test]
    public void LoadLearningContent_ReturnsLearningElement()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var learningContent = new LearningContent("fa", "a", new byte[] {0x01, 0x02, 0x03});
        mockDataAccess.LoadLearningContentFromFile("foobar").Returns(learningContent);

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

        systemUnderTest.LoadLearningWorldFromStream(stream);

        mockDataAccess.Received().LoadLearningWorldFromStream(stream);
    }

    [Test]
    public void LoadLearningWorldFromStream_ReturnsLearningWorld()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var learningWorld = new LearningWorld("fa", "a", "f", "f", "f", "f");
        var stream = Substitute.For<Stream>();
        mockDataAccess.LoadLearningWorldFromStream(stream).Returns(learningWorld);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var learningWorldActual = systemUnderTest.LoadLearningWorldFromStream(stream);

        Assert.That(learningWorldActual, Is.EqualTo(learningWorld));
    }

    [Test]
    public void LoadLearningSpaceFromStream_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var stream = Substitute.For<Stream>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.LoadLearningSpaceFromStream(stream);

        mockDataAccess.Received().LoadLearningSpaceFromStream(stream);
    }

    [Test]
    public void LoadLearningSpaceFromStream_ReturnsLearningSpace()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var learningSpace = new LearningSpace("fa", "a", "f", "f", "f");
        var stream = Substitute.For<Stream>();
        mockDataAccess.LoadLearningSpaceFromStream(stream).Returns(learningSpace);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var learningSpaceActual = systemUnderTest.LoadLearningSpaceFromStream(stream);

        Assert.That(learningSpaceActual, Is.EqualTo(learningSpace));
    }

    [Test]
    public void LoadLearningElementFromStream_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var stream = Substitute.For<Stream>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.LoadLearningElementFromStream(stream);

        mockDataAccess.Received().LoadLearningElementFromStream(stream);
    }

    [Test]
    public void LoadLearningElementFromStream_ReturnsLearningElement()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var content = new LearningContent("a", "b", Array.Empty<byte>());
        var learningElement = new LearningElement("fa", "a", "f", content, "f", "f",
            "f", LearningElementDifficultyEnum.Easy);
        var stream = Substitute.For<Stream>();
        mockDataAccess.LoadLearningElementFromStream(stream).Returns(learningElement);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var learningElementActual = systemUnderTest.LoadLearningElementFromStream(stream);

        Assert.That(learningElementActual, Is.EqualTo(learningElement));
    }

    [Test]
    public void LoadLearningContentFromStream_CallsDataAccess()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var stream = Substitute.For<Stream>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.LoadLearningContentFromStream("filename.extension", stream);

        mockDataAccess.Received().LoadLearningContentFromStream("filename.extension", stream);
    }

    [Test]
    public void LoadLearningContentFromStream_ReturnsLearningElement()
    {
        var mockDataAccess = Substitute.For<IDataAccess>();
        var content = new LearningContent("a", "b", Array.Empty<byte>());
        var learningContent = new LearningContent("filename", "extension", Array.Empty<byte>());
        var stream = Substitute.For<Stream>();
        mockDataAccess.LoadLearningContentFromStream("filename.extension", stream).Returns(learningContent);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var learningElementActual = systemUnderTest.LoadLearningContentFromStream("filename.extension", stream);

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