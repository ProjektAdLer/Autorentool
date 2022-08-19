using System;
using System.IO;
using AuthoringToolLib.API.Configuration;
using AuthoringToolLib.BusinessLogic.API;
using AuthoringToolLib.DataAccess.API;
using AuthoringToolLib.Entities;
using AuthoringToolLib.PresentationLogic.LearningElement;
using AutoMapper;
using ElectronWrapper;
using Generator.PersistEntities;
using NSubstitute;
using NUnit.Framework;

namespace AuthoringToolLibTest.BusinessLogic.API;

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
        var learningWorldPe = new LearningWorldPe("fa", "a", "f", "f", "f", "f");
        var mockDataAccess = Substitute.For<IDataAccess>();
        var mockMapper = Substitute.For<IMapper>();
        mockMapper.Map<LearningWorldPe>(learningWorld).Returns(learningWorldPe);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess, mapper: mockMapper);

        systemUnderTest.SaveLearningWorld(learningWorld, "foobar");

        mockDataAccess.Received().SaveLearningWorldToFile(learningWorldPe, "foobar");
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
        var learningWorld = new LearningWorld("fa", "a", "f", "f", "f", "f");
        var learningWorldPe = new LearningWorldPe("fa", "a", "f", "f", "f", "f");
        var mockMapper = Substitute.For<IMapper>();
        mockMapper.Map<LearningWorld>(learningWorldPe).Returns(learningWorld);
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.LoadLearningWorldFromFile("foobar").Returns(learningWorldPe);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess, mapper: mockMapper);

        var learningWorldActual = systemUnderTest.LoadLearningWorld("foobar");

        Assert.That(learningWorldActual, Is.EqualTo(learningWorld));
    }

    [Test]
    public void SaveLearningSpace_CallsDataAccess()
    {
        var learningSpace = new LearningSpace("fa", "a", "f", "f", "f");
        var learningSpacePe = new LearningSpacePe("fa", "a", "f", "f", "f");
        var mockMapper = Substitute.For<IMapper>();
        mockMapper.Map<LearningSpacePe>(learningSpace).Returns(learningSpacePe);
        var mockDataAccess = Substitute.For<IDataAccess>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess, mapper: mockMapper);

        systemUnderTest.SaveLearningSpace(learningSpace, "foobar");

        mockDataAccess.Received().SaveLearningSpaceToFile(learningSpacePe, "foobar");
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
        var learningSpace = new LearningSpace("fa", "a", "f", "f", "f");
        var learningSpacePe = new LearningSpacePe("fa", "a", "f", "f", "f");
        var mockMapper = Substitute.For<IMapper>();
        mockMapper.Map<LearningSpace>(learningSpacePe).Returns(learningSpace);
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.LoadLearningSpaceFromFile("foobar").Returns(learningSpacePe);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess, mapper: mockMapper);

        var learningSpaceActual = systemUnderTest.LoadLearningSpace("foobar");

        Assert.That(learningSpaceActual, Is.EqualTo(learningSpace));
    }

    [Test]
    public void SaveLearningElement_CallsDataAccess()
    {
        var content = new LearningContent("a", "b", Array.Empty<byte>());
        var learningElement = new LearningElement("fa", "f", "f", content, "f",
            "f", "f", LearningElementDifficultyEnum.Easy);
        var contentPe = new LearningContentPe("a", "b", Array.Empty<byte>());
        var learningElementPe = new LearningElementPe("fa", "f", "f", contentPe, "f",
            "f", "f", LearningElementDifficultyEnumPe.Easy);
        var mockMapper = Substitute.For<IMapper>();
        mockMapper.Map<LearningElementPe>(learningElement).Returns(learningElementPe);
        var mockDataAccess = Substitute.For<IDataAccess>();

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess, mapper: mockMapper);

        systemUnderTest.SaveLearningElement(learningElement, "foobar");

        mockDataAccess.Received().SaveLearningElementToFile(learningElementPe, "foobar");
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
        var content = new LearningContent("a", "b", Array.Empty<byte>());
        var learningElement = new LearningElement("fa", "a", "f", content, "f", "f",
            "f", LearningElementDifficultyEnum.Easy);
        var contentPe = new LearningContentPe("a", "b", Array.Empty<byte>());
        var learningElementPe = new LearningElementPe("fa", "a", "f", contentPe, "f", "f", "f",
            LearningElementDifficultyEnumPe.Easy);
        var mockMapper = Substitute.For<IMapper>();
        mockMapper.Map<LearningElement>(learningElementPe).Returns(learningElement);
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.LoadLearningElementFromFile("foobar").Returns(learningElementPe);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess, mapper: mockMapper);

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
        var learningContent = new LearningContent("fa", "a", new byte[] {0x01, 0x02, 0x03});
        var learningContentPe = new LearningContentPe("fa", "a", new byte[] {0x01, 0x02, 0x03});
        var mockMapper = Substitute.For<IMapper>();
        mockMapper.Map<LearningContent>(learningContentPe).Returns(learningContent);
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.LoadLearningContentFromFile("foobar").Returns(learningContentPe);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess, mapper: mockMapper);

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
        var learningWorld = new LearningWorld("fa", "a", "f", "f", "f", "f");
        var learningWorldPe = new LearningWorldPe("fa", "a", "f", "f", "f", "f");
        var mockMapper = Substitute.For<IMapper>();
        mockMapper.Map<LearningWorld>(learningWorldPe).Returns(learningWorld);
        var stream = Substitute.For<Stream>();
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.LoadLearningWorldFromStream(stream).Returns(learningWorldPe);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess, mapper: mockMapper);

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
        var learningSpace = new LearningSpace("fa", "a", "f", "f", "f");
        var learningSpacePe = new LearningSpacePe("fa", "a", "f", "f", "f");
        var mockMapper = Substitute.For<IMapper>();
        mockMapper.Map<LearningSpace>(learningSpacePe).Returns(learningSpace);
        var stream = Substitute.For<Stream>();
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.LoadLearningSpaceFromStream(stream).Returns(learningSpacePe);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess, mapper: mockMapper);

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
        var content = new LearningContent("a", "b", Array.Empty<byte>());
        var learningElement = new LearningElement("fa", "a", "f", content, "f", "f",
            "f", LearningElementDifficultyEnum.Easy);
        var contentPe = new LearningContentPe("a", "b", Array.Empty<byte>());
        var learningElementPe = new LearningElementPe("fa", "a", "f", contentPe, "f", "f", "f",
            LearningElementDifficultyEnumPe.Easy);
        var mockMapper = Substitute.For<IMapper>();
        mockMapper.Map<LearningElement>(learningElementPe).Returns(learningElement);
        var stream = Substitute.For<Stream>();
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.LoadLearningElementFromStream(stream).Returns(learningElementPe);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess, mapper: mockMapper);

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
        var learningContent = new LearningContent("filename", "extension", Array.Empty<byte>());
        var learningContentPe = new LearningContentPe("filename", "extension", Array.Empty<byte>());
        var mockMapper = Substitute.For<IMapper>();
        mockMapper.Map<LearningContent>(learningContentPe).Returns(learningContent);
        var stream = Substitute.For<Stream>();
        var mockDataAccess = Substitute.For<IDataAccess>();
        mockDataAccess.LoadLearningContentFromStream("filename.extension", stream).Returns(learningContentPe);

        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess, mapper: mockMapper);

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

    private AuthoringToolLib.BusinessLogic.API.BusinessLogic CreateStandardBusinessLogic(
        IAuthoringToolConfiguration? fakeConfiguration = null,
        IDataAccess? fakeDataAccess = null,
        IHybridSupportWrapper? fakeHybridSupportWrapper = null,
        IMapper? mapper = null,
        IWorldGenerator? worldGenerator = null)
    {
        fakeConfiguration ??= Substitute.For<IAuthoringToolConfiguration>();
        fakeDataAccess ??= Substitute.For<IDataAccess>();
        fakeHybridSupportWrapper ??= Substitute.For<IHybridSupportWrapper>();
        mapper ??= Substitute.For<IMapper>();
        worldGenerator ??= Substitute.For<IWorldGenerator>();
        
        return new AuthoringToolLib.BusinessLogic.API.BusinessLogic(fakeConfiguration, fakeDataAccess,
            fakeHybridSupportWrapper, mapper, worldGenerator);
    }
}