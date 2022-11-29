using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using AutoMapper;
using BusinessLogic.API;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.Toolbox;
using Shared;

namespace PresentationTest.PresentationLogic.Toolbox;

[TestFixture]
public class ToolboxEntriesProviderUt
{
    [Test]
    public void ToolboxEntriesProvider_DefaultConstructor_ParametersSet()
    {
        var logger = Substitute.For<ILogger<ToolboxEntriesProvider>>();
        var businessLogic = Substitute.For<IBusinessLogic>();
        var mockMapper = Substitute.For<IMapper>();
        var fileSystem = new MockFileSystem();

        var systemUnderTest = CreateTestableToolboxEntriesProvider(logger, businessLogic,
            mockMapper, fileSystem);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Logger, Is.EqualTo(logger));
            Assert.That(systemUnderTest.BusinessLogic, Is.EqualTo(businessLogic));
            Assert.That(systemUnderTest.Mapper, Is.EqualTo(mockMapper));
            Assert.That(systemUnderTest.FileSystem, Is.EqualTo(fileSystem));
        });
    }

    [Test]
    public void ToolboxEntriesProvider_ConstructorWithoutFilesystem_SetsFilesystem()
    {
        var logger = Substitute.For<ILogger<ToolboxEntriesProvider>>();
        var businessLogic = Substitute.For<IBusinessLogic>();
        var mockMapper = Substitute.For<IMapper>();
        
        var systemUnderTest = new ToolboxEntriesProvider(logger, businessLogic, mockMapper);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Logger, Is.EqualTo(logger));
            Assert.That(systemUnderTest.BusinessLogic, Is.EqualTo(businessLogic));
            Assert.That(systemUnderTest.Mapper, Is.EqualTo(mockMapper));
            Assert.That(systemUnderTest.FileSystem, Is.Not.Null);
        });
        Assert.That(systemUnderTest.FileSystem, Is.TypeOf<FileSystem>());
    }

    [Test]
    public void IToolboxEntriesProvider_Entries_ReturnsCorrectEntries()
    {
        var basePath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "AdLerAuthoring", "Toolbox");
        var worldPath = Path.Join(basePath, "asdf.awf");
        var spacePath = Path.Join(basePath, "foobar.asf");
        var elementPath = Path.Join(basePath, "boobar.aef");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            {worldPath, new MockFileData("world")},
            {spacePath, new MockFileData("space")},
            {elementPath, new MockFileData("element")},
        });
        
        var businessLogic = Substitute.For<IBusinessLogic>();
        var lwEntity = new BusinessLogic.Entities.LearningWorld("world", "foo", "foo", "foo", "foo", "foo");
        var lsEntity = new BusinessLogic.Entities.LearningSpace("space", "foo", "foo", "foo", "foo", 5);
        var leEntity =
            new BusinessLogic.Entities.LearningElement("foo", "foo", null!, "url","foo",
                "foo", "foo", LearningElementDifficultyEnum.Easy);
        businessLogic.LoadLearningWorld(Arg.Any<string>()).Returns(lwEntity);
        businessLogic.LoadLearningSpace(Arg.Any<string>()).Returns(lsEntity);
        businessLogic.LoadLearningElement(Arg.Any<string>()).Returns(leEntity);

        var worldVm = new LearningWorldViewModel("ba", "ba", "ba", "ba", "ba", "ba");
        var spaceVm = new LearningSpaceViewModel("ba", "ba", "ba", "ba", "ba");
        var elementVm = new LearningElementViewModel("ba", "ba", null!, "url","ba",
            "ba", "ba", LearningElementDifficultyEnum.Easy);
        var mockMapper = Substitute.For<IMapper>();
        mockMapper.Map<LearningWorldViewModel>(Arg.Any<ILearningWorld>()).Returns(worldVm);
        mockMapper.Map<LearningSpaceViewModel>(Arg.Any<ILearningSpace>()).Returns(spaceVm);
        mockMapper.Map<LearningElementViewModel>(Arg.Any<BusinessLogic.Entities.LearningElement>()).Returns(elementVm);
        

        IToolboxEntriesProvider systemUnderTest =
            CreateTestableToolboxEntriesProvider(null, businessLogic, mockMapper, fileSystem);

        var entries = systemUnderTest.Entries.ToArray();
        
        Assert.That(entries.Count, Is.EqualTo(3));
        Assert.That(entries, Contains.Item(worldVm));
        Assert.That(entries, Contains.Item(spaceVm));
        Assert.That(entries, Contains.Item(elementVm));

        businessLogic.Received().LoadLearningWorld(worldPath);
        businessLogic.Received().LoadLearningSpace(spacePath);
        businessLogic.Received().LoadLearningElement(elementPath);

        mockMapper.Received().Map<LearningWorldViewModel>(lwEntity);
        mockMapper.Received().Map<LearningSpaceViewModel>(lsEntity);
        mockMapper.Received().Map<LearningElementViewModel>(leEntity);
    }

    [Test]
    public void IToolboxEntriesProviderMutable_AddEntry_AddsToEntriesAndSavesFile()
    {
        var businessLogic = Substitute.For<IBusinessLogic>();
        var targetFolder = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AdLerAuthoring",
            "Toolbox");
        const string worldPath = "/foo/bar/world.awf";
        const string spacePath = "/foo/bar/space.asf";
        const string elementPath = "/foo/bar/element.aef";
        businessLogic.FindSuitableNewSavePath(targetFolder, "world", "awf").Returns(worldPath);
        businessLogic.FindSuitableNewSavePath(targetFolder, "space", "asf").Returns(spacePath);
        businessLogic.FindSuitableNewSavePath(targetFolder, "element", "aef").Returns(elementPath);
        
        var mockMapper = Substitute.For<IMapper>();

        var lwViewModel = new LearningWorldViewModel("world", "foo", "foo", "foo", "foo", "foo");
        var lsViewModel = new LearningSpaceViewModel("space", "foo", "foo", "foo", "foo");
        var leViewModel = new LearningElementViewModel("element", "foo", null!, "url","foo", "foo", "foo",LearningElementDifficultyEnum.Easy);
        
        var lwEntity = new BusinessLogic.Entities.LearningWorld("world", "foo", "foo", "foo", "foo", "foo");
        var lsEntity = new BusinessLogic.Entities.LearningSpace("space", "foo", "foo", "foo", "foo", 5);
        var leEntity =
            new BusinessLogic.Entities.LearningElement("element", "foo", null!, "url","foo",
                "foo", "foo", LearningElementDifficultyEnum.Easy);
        mockMapper.Map<BusinessLogic.Entities.LearningWorld>(Arg.Any<ILearningWorldViewModel>()).Returns(lwEntity);
        mockMapper.Map<BusinessLogic.Entities.LearningSpace>(Arg.Any<ILearningSpaceViewModel>()).Returns(lsEntity);
        mockMapper.Map<BusinessLogic.Entities.LearningElement>(Arg.Any<LearningElementViewModel>()).Returns(leEntity);

        IToolboxEntriesProviderModifiable systemUnderTest =
            CreateTestableToolboxEntriesProvider(businessLogic: businessLogic, mapper: mockMapper);
        
        Assert.That(systemUnderTest.Entries, Is.Empty);
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.AddEntry(lwViewModel), Is.True);
            Assert.That(systemUnderTest.Entries, Contains.Item(lwViewModel));
        });
        mockMapper.Received().Map<BusinessLogic.Entities.LearningWorld>(lwViewModel);
        businessLogic.Received().FindSuitableNewSavePath(targetFolder, "world", "awf");
        businessLogic.Received().SaveLearningWorld(lwEntity, worldPath);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.AddEntry(lsViewModel), Is.True);
            Assert.That(systemUnderTest.Entries, Contains.Item(lsViewModel));
        });
        mockMapper.Received().Map<BusinessLogic.Entities.LearningSpace>(lsViewModel);
        businessLogic.Received().FindSuitableNewSavePath(targetFolder, "space", "asf");
        businessLogic.Received().SaveLearningSpace(lsEntity, spacePath);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.AddEntry(leViewModel), Is.True);
            Assert.That(systemUnderTest.Entries, Contains.Item(leViewModel));
        });
        mockMapper.Received().Map<BusinessLogic.Entities.LearningElement>(leViewModel);
        businessLogic.Received().FindSuitableNewSavePath(targetFolder, "element", "aef");
        businessLogic.Received().SaveLearningElement(leEntity, elementPath);
    }

    [Test]
    public void IToolboxEntriesProviderMutable_AddEntry_SkipsDuplicateEntries()
    {
        var basePath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "AdLerAuthoring", "Toolbox");
        var worldPath = Path.Join(basePath, "asdf.awf");
        var spacePath = Path.Join(basePath, "foobar.asf");
        var elementPath = Path.Join(basePath, "boobar.aef");
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            {worldPath, new MockFileData("world")},
            {spacePath, new MockFileData("space")},
            {elementPath, new MockFileData("element")},
        });
        var businessLogic = Substitute.For<IBusinessLogic>();
        var lwEntity = new BusinessLogic.Entities.LearningWorld("world", "foo", "foo", "foo", "foo", "foo");
        var lsEntity = new BusinessLogic.Entities.LearningSpace("space", "foo", "foo", "foo", "foo", 5);
        var leEntity =
            new BusinessLogic.Entities.LearningElement("element", "foo", null!,"url","foo",
                "foo", null!, LearningElementDifficultyEnum.Easy);
        businessLogic.LoadLearningWorld(Arg.Any<string>()).Returns(lwEntity);
        businessLogic.LoadLearningSpace(Arg.Any<string>()).Returns(lsEntity);
        businessLogic.LoadLearningElement(Arg.Any<string>()).Returns(leEntity);

        var worldVm = new LearningWorldViewModel("world", "ba", "ba", "ba", "ba", "ba");
        var spaceVm = new LearningSpaceViewModel("space", "ba", "ba", "ba", "ba");
        var elementVm = new LearningElementViewModel("element", "ba", null!, "url","ba", "ba",
            "foo",LearningElementDifficultyEnum.Easy);
        var mockMapper = Substitute.For<IMapper>();
        mockMapper.Map<LearningWorldViewModel>(Arg.Any<ILearningWorld>()).Returns(worldVm);
        mockMapper.Map<LearningSpaceViewModel>(Arg.Any<ILearningSpace>()).Returns(spaceVm);
        mockMapper.Map<LearningElementViewModel>(Arg.Any<BusinessLogic.Entities.LearningElement>()).Returns(elementVm);

        IToolboxEntriesProviderModifiable systemUnderTest = CreateTestableToolboxEntriesProvider(
            mapper: mockMapper,
            businessLogic: businessLogic, fileSystem: fileSystem);

        var entries = systemUnderTest.Entries.ToArray();
        
        Assert.That(entries.Count, Is.EqualTo(3));
        Assert.That(entries, Contains.Item(worldVm));
        Assert.That(entries, Contains.Item(spaceVm));
        Assert.That(entries, Contains.Item(elementVm));
        
        Assert.That(systemUnderTest.AddEntry(worldVm), Is.False);
        Assert.That(entries.Count, Is.EqualTo(3));
        Assert.That(entries, Contains.Item(worldVm));
        Assert.That(entries, Contains.Item(spaceVm));
        Assert.That(entries, Contains.Item(elementVm));
        
        Assert.That(systemUnderTest.AddEntry(spaceVm), Is.False);
        Assert.That(entries.Count, Is.EqualTo(3));
        Assert.That(entries, Contains.Item(worldVm));
        Assert.That(entries, Contains.Item(spaceVm));
        Assert.That(entries, Contains.Item(elementVm));
        
        Assert.That(systemUnderTest.AddEntry(elementVm), Is.False);
        Assert.That(entries.Count, Is.EqualTo(3));
        Assert.That(entries, Contains.Item(worldVm));
        Assert.That(entries, Contains.Item(spaceVm));
        Assert.That(entries, Contains.Item(elementVm));
    }

    [Test]
    public void IToolboxEntriesProviderModifiable_AddEntry_ReturnsFalse()
    {
        var logger = Substitute.For<ILogger<ToolboxEntriesProvider>>();
        var learningObject = Substitute.For<IDisplayableLearningObject>();
        var systemUnderTest = CreateTestableToolboxEntriesProvider(logger);
        
        Assert.That(systemUnderTest.AddEntry(learningObject), Is.False);
    }

    private ToolboxEntriesProvider CreateTestableToolboxEntriesProvider(ILogger<ToolboxEntriesProvider>? logger = null,
        IBusinessLogic? businessLogic = null, IMapper? mapper = null, IFileSystem? fileSystem = null)
    {
        logger ??= Substitute.For<ILogger<ToolboxEntriesProvider>>();
        businessLogic ??= Substitute.For<IBusinessLogic>();
        mapper ??= Substitute.For<IMapper>();
        fileSystem ??= new MockFileSystem();
        
        return new ToolboxEntriesProvider(logger, businessLogic, mapper, fileSystem);
    }
}