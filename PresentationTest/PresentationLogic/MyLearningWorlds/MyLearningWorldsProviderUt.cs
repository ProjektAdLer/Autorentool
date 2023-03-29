using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.MyLearningWorlds;
using Shared;

namespace PresentationTest.PresentationLogic.MyLearningWorlds;

[TestFixture]
public class MyLearningWorldsProviderUt
{
    [Test]
    public void Constructor_InitializesAllProperties()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var workspaceViewModel = Substitute.For<IAuthoringToolWorkspaceViewModel>();
        var fileSystem = Substitute.For<IFileSystem>();
        var logger = Substitute.For<ILogger<MyLearningWorldsProvider>>();

        var systemUnderTest = new MyLearningWorldsProvider(presentationLogic, workspaceViewModel, fileSystem, logger);
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.PresentationLogic, Is.EqualTo(presentationLogic));
            Assert.That(systemUnderTest.WorkspaceVm, Is.EqualTo(workspaceViewModel));
            Assert.That(systemUnderTest.FileSystem, Is.EqualTo(fileSystem));
            Assert.That(systemUnderTest.Logger, Is.EqualTo(logger));
        });
    }

    [Test]
    public void GetLoadedLearningWorlds_ReturnsLoadedLearningWorlds()
    {
        var learningWorld1 = new LearningWorldViewModel("w1", "s", "a", "l", "d", "g");
        var learningWorld2 = new LearningWorldViewModel("w2", "s", "a", "l", "d", "g");
        var workspaceViewModel = Substitute.For<IAuthoringToolWorkspaceViewModel>();
        workspaceViewModel.LearningWorlds.Returns(new List<LearningWorldViewModel>()
            {learningWorld1, learningWorld2});
        var systemUnderTest = CreateProviderForTesting(workspaceViewModel: workspaceViewModel);

        var result = systemUnderTest.GetLoadedLearningWorlds();
        var resultList = result.ToList();

        Assert.Multiple(() =>
        {
            Assert.That(resultList, Has.Count.EqualTo(2));
            Assert.That(resultList,
                Has.Exactly(1).Matches<SavedLearningWorldPath>(x =>
                    x.Id == learningWorld1.Id && x.Name == learningWorld1.Name && x.Path == ""));
            Assert.That(resultList,
                Has.Exactly(1).Matches<SavedLearningWorldPath>(x =>
                    x.Id == learningWorld2.Id && x.Name == learningWorld2.Name && x.Path == ""));
        });
    }

    [Test]
    public void GetSavedLearningWorlds_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var systemUnderTest = CreateProviderForTesting(presentationLogic: presentationLogic);

        systemUnderTest.GetSavedLearningWorlds();

        presentationLogic.Received(1).GetSavedLearningWorldPaths();
    }

    [Test]
    public void GetSavedLearningWorlds_ReturnsSavedLearningWorlds()
    {
        var savedLearningWorld1 = new SavedLearningWorldPath()
            {Id = Guid.ParseExact("00000000-0000-0000-0000-000000000001", "D"), Name = "w1", Path = "p1"};
        var savedLearningWorld2 = new SavedLearningWorldPath()
            {Id = Guid.ParseExact("00000000-0000-0000-0000-000000000002", "D"), Name = "w2", Path = "p2"};
        var presentationLogic = Substitute.For<IPresentationLogic>();
        presentationLogic.GetSavedLearningWorldPaths().Returns(new List<SavedLearningWorldPath>()
            {savedLearningWorld1, savedLearningWorld2});
        var systemUnderTest = CreateProviderForTesting(presentationLogic: presentationLogic);

        var result = systemUnderTest.GetSavedLearningWorlds();
        var resultList = result.ToList();

        Assert.Multiple(() =>
        {
            Assert.That(resultList, Has.Count.EqualTo(2));
            Assert.That(resultList,
                Has.Exactly(1).Matches<SavedLearningWorldPath>(x =>
                    x.Id == savedLearningWorld1.Id && x.Name == savedLearningWorld1.Name &&
                    x.Path == savedLearningWorld1.Path));
            Assert.That(resultList,
                Has.Exactly(1).Matches<SavedLearningWorldPath>(x =>
                    x.Id == savedLearningWorld2.Id && x.Name == savedLearningWorld2.Name &&
                    x.Path == savedLearningWorld2.Path));
        });
    }

    [Test]
    public void GetSavedLearningWorlds_ReturnsSavedLearningWorlds_ExceptLoadedLearningWorlds()
    {
        var learningWorld1 = new LearningWorldViewModel("w1", "s", "a", "l", "d", "g");
        var workspaceViewModel = Substitute.For<IAuthoringToolWorkspaceViewModel>();
        workspaceViewModel.LearningWorlds.Returns(new List<LearningWorldViewModel>()
            {learningWorld1});
        var savedLearningWorld1 = new SavedLearningWorldPath()
            {Id = learningWorld1.Id, Name = "w1", Path = "p1"};
        var savedLearningWorld2 = new SavedLearningWorldPath()
            {Id = Guid.ParseExact("00000000-0000-0000-0000-000000000002", "D"), Name = "w2", Path = "p2"};
        var presentationLogic = Substitute.For<IPresentationLogic>();
        presentationLogic.GetSavedLearningWorldPaths().Returns(new List<SavedLearningWorldPath>()
            {savedLearningWorld1, savedLearningWorld2});
        var systemUnderTest =
            CreateProviderForTesting(presentationLogic: presentationLogic, workspaceViewModel: workspaceViewModel);

        var result = systemUnderTest.GetSavedLearningWorlds();
        var resultList = result.ToList();

        Assert.Multiple(() =>
        {
            Assert.That(resultList, Has.Count.EqualTo(1));
            Assert.That(resultList,
                Has.Exactly(1).Matches<SavedLearningWorldPath>(x =>
                    x.Id == savedLearningWorld2.Id && x.Name == savedLearningWorld2.Name &&
                    x.Path == savedLearningWorld2.Path));
        });
    }

    [Test]
    public void OpenLearningWorld_WorldIsAlreadyLoaded_SelectedLearningWorldInWorkspaceViewModelIsSet()
    {
        var learningWorld1 = new LearningWorldViewModel("w1", "s", "a", "l", "d", "g");
        var workspaceViewModel = Substitute.For<IAuthoringToolWorkspaceViewModel>();
        workspaceViewModel.LearningWorlds.Returns(new List<LearningWorldViewModel>()
            {learningWorld1});
        var systemUnderTest = CreateProviderForTesting(workspaceViewModel: workspaceViewModel);

        var savedPaths = systemUnderTest.GetLoadedLearningWorlds();
        systemUnderTest.OpenLearningWorld(savedPaths.First());

        workspaceViewModel.Received(1).SelectedLearningWorld = learningWorld1;
    }

    [Test]
    public void OpenLearningWorld_WorldIsSavedAndSaveFileExists_PresentationLogicIsCalled()
    {
        var workspaceVm = Substitute.For<IAuthoringToolWorkspaceViewModel>();
        var worldInWorkspace = new LearningWorldViewModel("n", "s", "a", "l", "d", "g");
        workspaceVm.LearningWorlds.Returns(new List<LearningWorldViewModel>() {worldInWorkspace});
        var savedLearningWorld1 = new SavedLearningWorldPath()
            {Id = Guid.ParseExact("00000000-0000-0000-0000-000000000001", "D"), Name = "w1", Path = "p1"};
        var presentationLogic = Substitute.For<IPresentationLogic>();
        presentationLogic.GetSavedLearningWorldPaths().Returns(new List<SavedLearningWorldPath>()
            {savedLearningWorld1});
        var fileSystem = Substitute.For<IFileSystem>();
        fileSystem.File.Exists(Arg.Any<string>()).Returns(true);
        var systemUnderTest = CreateProviderForTesting(presentationLogic: presentationLogic,
            workspaceViewModel: workspaceVm, fileSystem: fileSystem);

        var savedPaths = systemUnderTest.GetSavedLearningWorlds().ToList();
        systemUnderTest.OpenLearningWorld(savedPaths.First());

        presentationLogic.Received(1).LoadLearningWorldFromPath(workspaceVm, savedLearningWorld1.Path);
        presentationLogic.Received(1).UpdateIdOfSavedLearningWorldPath(savedPaths.First(), worldInWorkspace.Id);
    }

    [Test]
    public void OpenLearningWorld_WorldIsSavedButFileDoesNotExist_PresentationLogicIsNotCalled()
    {
        var savedLearningWorld1 = new SavedLearningWorldPath()
            {Id = Guid.ParseExact("00000000-0000-0000-0000-000000000001", "D"), Name = "w1", Path = "p1"};
        var presentationLogic = Substitute.For<IPresentationLogic>();
        presentationLogic.GetSavedLearningWorldPaths().Returns(new List<SavedLearningWorldPath>()
            {savedLearningWorld1});
        var fileSystem = Substitute.For<IFileSystem>();
        fileSystem.File.Exists(Arg.Any<string>()).Returns(false);
        var systemUnderTest = CreateProviderForTesting(presentationLogic: presentationLogic, fileSystem: fileSystem);

        var savedPaths = systemUnderTest.GetSavedLearningWorlds().ToList();
        systemUnderTest.OpenLearningWorld(savedPaths.First());

        presentationLogic.DidNotReceive().UpdateIdOfSavedLearningWorldPath(savedPaths.First(), Arg.Any<Guid>());
        presentationLogic.Received(1).RemoveSavedLearningWorldPath(savedPaths.First());
    }

    [Test]
    public void OpenLearningWorld_WorldIsNotSavedAndNotLoaded_ThrowsException()
    {
        var savedLearningWorld1 = new SavedLearningWorldPath()
            {Id = Guid.ParseExact("00000000-0000-0000-0000-000000000001", "D"), Name = "w1", Path = "p1"};
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var systemUnderTest = CreateProviderForTesting(presentationLogic: presentationLogic);

        Assert.Throws<ArgumentException>(() => systemUnderTest.OpenLearningWorld(savedLearningWorld1));
    }

    private MyLearningWorldsProvider CreateProviderForTesting(IPresentationLogic? presentationLogic = null,
        IAuthoringToolWorkspaceViewModel? workspaceViewModel = null, IFileSystem? fileSystem = null,
        ILogger<MyLearningWorldsProvider>? logger = null)
    {
        presentationLogic ??= Substitute.For<IPresentationLogic>();
        workspaceViewModel ??= Substitute.For<IAuthoringToolWorkspaceViewModel>();
        fileSystem ??= Substitute.For<IFileSystem>();
        logger ??= Substitute.For<ILogger<MyLearningWorldsProvider>>();
        return new MyLearningWorldsProvider(presentationLogic, workspaceViewModel, fileSystem, logger);
    }
}