using System;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Reflection;
using AuthoringTool;
using AutoMapper;
using BusinessLogic.API;
using BusinessLogic.Entities.LearningContent.FileContent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NUnit.Framework;
using Shared;
using Shared.Configuration;

namespace IntegrationTest.VersionCompatibility;

[TestFixture]
public class VersionCompatibilityIt
{
    [SetUp]
    public void Setup()
    {
        var serviceCollection = new ServiceCollection();
        Startup.ConfigureDataAccess(serviceCollection);
        Startup.ConfigureAutoMapper(serviceCollection);
        Startup.ConfigureMyLearningWorlds(serviceCollection);
        _fileSystem = new MockFileSystem();
        serviceCollection.AddSingleton<IFileSystem>(_fileSystem);
        serviceCollection.AddSingleton(Substitute.For<IApplicationConfiguration>());
        serviceCollection.AddLogging(conf =>
        {
            conf.ClearProviders();
            conf.AddProvider(NullLoggerProvider.Instance);
        });
        _serviceProvider = serviceCollection.BuildServiceProvider();
        _mapper = _serviceProvider.GetService<IMapper>() ?? throw new InvalidOperationException();
        _dataAccess = _serviceProvider.GetService<IDataAccess>() ?? throw new InvalidOperationException();
    }

    [TearDown]
    public void TearDown()
    {
        _serviceProvider.Dispose();
    }

    private MockFileSystem _fileSystem;
    private IMapper _mapper;
    private IDataAccess _dataAccess;
    private ServiceProvider _serviceProvider;

    [Test]
    // ANF-ID: [ASE2]
    public void Version_1_1_0_WorldFile_LoadsCorrectly()
    {
        var asmb = Assembly.GetExecutingAssembly();
        using var stream = asmb.GetManifestResourceStream("IntegrationTest.VersionCompatibility.testworld110.awf");
        Assert.That(stream, Is.Not.Null, "stream was null");
        var world = _dataAccess.LoadLearningWorld(stream);
        Assert.Multiple(() =>
        {
            Assert.That(world.Name, Is.EqualTo("testworld110"));
            Assert.That(world.LearningSpaces, Has.Count.EqualTo(3));
        });
        var space1 = world.LearningSpaces.ElementAt(0);
        var space2 = world.LearningSpaces.ElementAt(1);
        var space3 = world.LearningSpaces.ElementAt(2);
        Assert.Multiple(() =>
        {
            Assert.That(space1.Name, Is.EqualTo("sp1"));
            Assert.That(space1.RequiredPoints, Is.EqualTo(1));
            Assert.That(space2.Name, Is.EqualTo("sp2"));
            Assert.That(space2.RequiredPoints, Is.EqualTo(2));
            Assert.That(space3.Name, Is.EqualTo("sp3"));
            Assert.That(space3.RequiredPoints, Is.EqualTo(3));
        });
        Assert.That(space1.LearningSpaceLayout.LearningElements, Has.Count.EqualTo(1));
        Assert.That(space1.LearningSpaceLayout.LearningElements.TryGetValue(0, out var element1), Is.True);
        Assert.Multiple(() =>
        {
            Assert.That(element1!.Name, Is.EqualTo("ele1"));
            Assert.That(element1.LearningContent, Is.TypeOf<FileContent>());
            Assert.That(element1.LearningContent.Name, Is.EqualTo("regex.txt"));
            Assert.That(element1.Points, Is.EqualTo(123));
            Assert.That(element1.Workload, Is.EqualTo(10));
            Assert.That(element1.Difficulty, Is.EqualTo(LearningElementDifficultyEnum.Medium));
        });
        Assert.That(space2.LearningSpaceLayout.LearningElements, Has.Count.EqualTo(1));
        Assert.That(space2.LearningSpaceLayout.LearningElements.TryGetValue(1, out var element2), Is.True);
        Assert.Multiple(() =>
        {
            Assert.That(element2!.Name, Is.EqualTo("ele2"));
            Assert.That(element2.LearningContent, Is.TypeOf<FileContent>());
            Assert.That(element2.LearningContent.Name, Is.EqualTo("entities_adaptivity.jpeg"));
            Assert.That(element2.Points, Is.EqualTo(100));
            Assert.That(element2.Workload, Is.EqualTo(20));
            Assert.That(element2.Difficulty, Is.EqualTo(LearningElementDifficultyEnum.Easy));
        });
        Assert.That(space3.LearningSpaceLayout.LearningElements, Has.Count.EqualTo(1));
        Assert.That(space3.LearningSpaceLayout.LearningElements.TryGetValue(9, out var element3), Is.True);
        Assert.Multiple(() =>
        {
            Assert.That(element3!.Name, Is.EqualTo("ele3"));
            Assert.That(element3.LearningContent, Is.TypeOf<FileContent>());
            Assert.That(element3.LearningContent.Name, Is.EqualTo("whisky_liste.txt"));
            Assert.That(element3.Points, Is.EqualTo(1));
            Assert.That(element3.Workload, Is.EqualTo(1));
        });
    }
}