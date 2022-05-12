using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using AuthoringTool.BusinessLogic.API;
using AuthoringTool.Entities;
using AuthoringTool.PresentationLogic.EntityMapping;
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
            new AuthoringTool.Entities.LearningElement("foo", "foo", "foo", "foo",
                "foo", null, "foo", "foo", "foo");
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
        var elementVm = new LearningElementViewModel("ba", "ba", null, "ba", "ba",
            null, "ba", "ba", "ba");
        elementMapper.ToViewModel(Arg.Any<ILearningElement>()).Returns(elementVm);
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
        //TODO: implement
    }

    private ToolboxEntriesProvider CreateTestableToolboxEntriesProvider(ILogger<ToolboxEntriesProvider>? logger = null,
        IBusinessLogic? businessLogic = null, IEntityMapping? entityMapping = null, IFileSystem? fileSystem = null)
    {
        logger ??= Substitute.For<ILogger<ToolboxEntriesProvider>>();
        businessLogic ??= Substitute.For<IBusinessLogic>();
        entityMapping ??= Substitute.For<IEntityMapping>();
        fileSystem ??= Substitute.For<IFileSystem>();
        
        return new ToolboxEntriesProvider(logger, businessLogic, entityMapping, fileSystem);
    }
}