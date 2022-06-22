using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using AuthoringTool.BusinessLogic.API;
using AuthoringTool.Entities;
using AuthoringTool.PresentationLogic;
using AuthoringTool.PresentationLogic.EntityMapping;
using AuthoringTool.PresentationLogic.EntityMapping.LearningElementMapper;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningSpace;
using AuthoringTool.PresentationLogic.LearningWorld;
using AuthoringTool.PresentationLogic.Toolbox;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace AuthoringToolTest.PresentationLogic.Toolbox;

[TestFixture]
public class ToolboxEntriesProviderUt
{
    [Test]
    public void ToolboxEntriesProvider_DefaultConstructor_ParametersSet()
    {
        var logger = Substitute.For<ILogger<ToolboxEntriesProvider>>();
        var businessLogic = Substitute.For<IBusinessLogic>();
        var entityMapping = Substitute.For<IEntityMapping>();
        var fileSystem = new MockFileSystem();

        var systemUnderTest = CreateTestableToolboxEntriesProvider(logger, businessLogic,
            entityMapping, fileSystem);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Logger, Is.EqualTo(logger));
            Assert.That(systemUnderTest.BusinessLogic, Is.EqualTo(businessLogic));
            Assert.That(systemUnderTest.EntityMapping, Is.EqualTo(entityMapping));
            Assert.That(systemUnderTest.FileSystem, Is.EqualTo(fileSystem));
        });
    }

    [Test]
    public void ToolboxEntriesProvider_ConstructorWithoutFilesystem_SetsFilesystem()
    {
        var logger = Substitute.For<ILogger<ToolboxEntriesProvider>>();
        var businessLogic = Substitute.For<IBusinessLogic>();
        var entityMapping = Substitute.For<IEntityMapping>();
        
        var systemUnderTest = new ToolboxEntriesProvider(logger, businessLogic, entityMapping);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Logger, Is.EqualTo(logger));
            Assert.That(systemUnderTest.BusinessLogic, Is.EqualTo(businessLogic));
            Assert.That(systemUnderTest.EntityMapping, Is.EqualTo(entityMapping));
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
        var lwEntity = new AuthoringTool.Entities.LearningWorld("foo", "foo", "foo", "foo", "foo", "foo");
        var lsEntity = new AuthoringTool.Entities.LearningSpace("foo", "foo", "foo", "foo", "foo");
        var leEntity =
            new AuthoringTool.Entities.LearningElement("foo", "foo", "foo", null, "foo",
                "foo", "foo",LearningElementDifficultyEnum.Easy);
        businessLogic.LoadLearningWorld(Arg.Any<string>()).Returns(lwEntity);
        businessLogic.LoadLearningSpace(Arg.Any<string>()).Returns(lsEntity);
        businessLogic.LoadLearningElement(Arg.Any<string>()).Returns(leEntity);

        var worldMapper = Substitute.For<ILearningWorldMapper>();
        var worldVm = new LearningWorldViewModel("ba", "ba", "ba", "ba", "ba", "ba");
        worldMapper.ToViewModel(Arg.Any<ILearningWorld>()).Returns(worldVm);
        var spaceMapper = Substitute.For<ILearningSpaceMapper>();
        var spaceVm = new LearningSpaceViewModel("ba", "ba", "ba", "ba", "ba");
        spaceMapper.ToViewModel(Arg.Any<ILearningSpace>()).Returns(spaceVm);
        var elementMapper = Substitute.For<ILearningElementMapper>();
        var elementVm = new LearningElementViewModel("ba", "ba", null, null, "ba",
            "ba", "ba",LearningElementDifficultyEnum.Easy);
        elementMapper.ToViewModel(Arg.Any<AuthoringTool.Entities.LearningElement>()).Returns(elementVm);
        var entityMapping =
            new AuthoringTool.PresentationLogic.EntityMapping.EntityMapping(worldMapper, spaceMapper, elementMapper);

        IToolboxEntriesProvider systemUnderTest =
            CreateTestableToolboxEntriesProvider(null, businessLogic, entityMapping, fileSystem);

        var entries = systemUnderTest.Entries.ToArray();
        
        Assert.That(entries.Count, Is.EqualTo(3));
        Assert.That(entries, Contains.Item(worldVm));
        Assert.That(entries, Contains.Item(spaceVm));
        Assert.That(entries, Contains.Item(elementVm));

        businessLogic.Received().LoadLearningWorld(worldPath);
        businessLogic.Received().LoadLearningSpace(spacePath);
        businessLogic.Received().LoadLearningElement(elementPath);

        worldMapper.Received().ToViewModel(lwEntity);
        spaceMapper.Received().ToViewModel(lsEntity);
        elementMapper.Received().ToViewModel(leEntity);
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
        
        var worldMapper = Substitute.For<ILearningWorldMapper>();
        var spaceMapper = Substitute.For<ILearningSpaceMapper>();
        var elementMapper = Substitute.For<ILearningElementMapper>();
        var entityMapping =
            new AuthoringTool.PresentationLogic.EntityMapping.EntityMapping(worldMapper, spaceMapper, elementMapper);

        var lwViewModel = new LearningWorldViewModel("world", "foo", "foo", "foo", "foo", "foo");
        var lsViewModel = new LearningSpaceViewModel("space", "foo", "foo", "foo", "foo");
        var leViewModel = new LearningElementViewModel("element", "foo", null, null, "foo", "foo", "foo",LearningElementDifficultyEnum.Easy);
        
        var lwEntity = new AuthoringTool.Entities.LearningWorld("world", "foo", "foo", "foo", "foo", "foo");
        var lsEntity = new AuthoringTool.Entities.LearningSpace("space", "foo", "foo", "foo", "foo");
        var leEntity =
            new AuthoringTool.Entities.LearningElement("element", "foo", "foo", null, "foo",
                "foo", "foo",LearningElementDifficultyEnum.Easy);
        worldMapper.ToEntity(lwViewModel).Returns(lwEntity);
        spaceMapper.ToEntity(lsViewModel).Returns(lsEntity);
        elementMapper.ToEntity(leViewModel).Returns(leEntity);

        IToolboxEntriesProviderModifiable systemUnderTest =
            CreateTestableToolboxEntriesProvider(businessLogic: businessLogic, entityMapping: entityMapping);
        
        Assert.That(systemUnderTest.Entries, Is.Empty);
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.AddEntry(lwViewModel), Is.True);
            Assert.That(systemUnderTest.Entries, Contains.Item(lwViewModel));
        });
        worldMapper.Received().ToEntity(lwViewModel);
        businessLogic.Received().FindSuitableNewSavePath(targetFolder, "world", "awf");
        businessLogic.Received().SaveLearningWorld(lwEntity, worldPath);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.AddEntry(lsViewModel), Is.True);
            Assert.That(systemUnderTest.Entries, Contains.Item(lsViewModel));
        });
        spaceMapper.Received().ToEntity(lsViewModel);
        businessLogic.Received().FindSuitableNewSavePath(targetFolder, "space", "asf");
        businessLogic.Received().SaveLearningSpace(lsEntity, spacePath);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.AddEntry(leViewModel), Is.True);
            Assert.That(systemUnderTest.Entries, Contains.Item(leViewModel));
        });
        elementMapper.Received().ToEntity(leViewModel);
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
        var lwEntity = new AuthoringTool.Entities.LearningWorld("world", "foo", "foo", "foo", "foo", "foo");
        var lsEntity = new AuthoringTool.Entities.LearningSpace("space", "foo", "foo", "foo", "foo");
        var leEntity =
            new AuthoringTool.Entities.LearningElement("element", "foo", "foo", null,"foo",
                "foo", null,LearningElementDifficultyEnum.Easy);
        businessLogic.LoadLearningWorld(Arg.Any<string>()).Returns(lwEntity);
        businessLogic.LoadLearningSpace(Arg.Any<string>()).Returns(lsEntity);
        businessLogic.LoadLearningElement(Arg.Any<string>()).Returns(leEntity);

        var worldMapper = Substitute.For<ILearningWorldMapper>();
        var worldVm = new LearningWorldViewModel("world", "ba", "ba", "ba", "ba", "ba");
        worldMapper.ToViewModel(Arg.Any<ILearningWorld>()).Returns(worldVm);
        var spaceMapper = Substitute.For<ILearningSpaceMapper>();
        var spaceVm = new LearningSpaceViewModel("space", "ba", "ba", "ba", "ba");
        spaceMapper.ToViewModel(Arg.Any<ILearningSpace>()).Returns(spaceVm);
        var elementMapper = Substitute.For<ILearningElementMapper>();
        var elementVm = new LearningElementViewModel("element", "ba", null, null, "ba", "ba",
            "foo",LearningElementDifficultyEnum.Easy);
        elementMapper.ToViewModel(Arg.Any<AuthoringTool.Entities.LearningElement>()).Returns(elementVm);
        var entityMapping =
            new AuthoringTool.PresentationLogic.EntityMapping.EntityMapping(worldMapper, spaceMapper, elementMapper);

        IToolboxEntriesProviderModifiable systemUnderTest = CreateTestableToolboxEntriesProvider(
            entityMapping: entityMapping,
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
        IBusinessLogic? businessLogic = null, IEntityMapping? entityMapping = null, IFileSystem? fileSystem = null)
    {
        logger ??= Substitute.For<ILogger<ToolboxEntriesProvider>>();
        businessLogic ??= Substitute.For<IBusinessLogic>();
        entityMapping ??= Substitute.For<IEntityMapping>();
        fileSystem ??= new MockFileSystem();
        
        return new ToolboxEntriesProvider(logger, businessLogic, entityMapping, fileSystem);
    }
}