using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.MyLearningWorlds;
using Presentation.PresentationLogic.SelectedViewModels;
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
        var workspacePresenter = Substitute.For<IAuthoringToolWorkspacePresenter>();
        workspacePresenter.AuthoringToolWorkspaceVm.Returns(workspaceViewModel);
        var fileSystem = Substitute.For<IFileSystem>();
        var logger = Substitute.For<ILogger<MyLearningWorldsProvider>>();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var errorService = Substitute.For<IErrorService>();

        var systemUnderTest = new MyLearningWorldsProvider(presentationLogic, workspacePresenter, fileSystem, logger,
            selectedViewModelsProvider, errorService);
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.PresentationLogic, Is.EqualTo(presentationLogic));
            Assert.That(systemUnderTest.WorkspacePresenter, Is.EqualTo(workspacePresenter));
            Assert.That(systemUnderTest.WorkspaceVm, Is.EqualTo(workspaceViewModel));
            Assert.That(systemUnderTest.FileSystem, Is.EqualTo(fileSystem));
            Assert.That(systemUnderTest.Logger, Is.EqualTo(logger));
            Assert.That(systemUnderTest.SelectedViewModelsProvider, Is.EqualTo(selectedViewModelsProvider));
            Assert.That(systemUnderTest.ErrorService, Is.EqualTo(errorService));
        });
    }

    [Test]
    public void GetLoadedLearningWorlds_ReturnsLoadedLearningWorlds()
    {
        var learningWorld1 = new LearningWorldViewModel("w1", "s", "a", "l", "d", "g");
        var learningWorld2 = new LearningWorldViewModel("w2", "s", "a", "l", "d", "g");
        var workspaceViewModel = Substitute.For<IAuthoringToolWorkspaceViewModel>();
        workspaceViewModel.LearningWorlds.Returns(new List<ILearningWorldViewModel> { learningWorld1, learningWorld2 });
        var workspacePresenter = Substitute.For<IAuthoringToolWorkspacePresenter>();
        workspacePresenter.AuthoringToolWorkspaceVm.Returns(workspaceViewModel);
        var systemUnderTest = CreateProviderForTesting(workspacePresenter: workspacePresenter);

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
        var savedLearningWorld1 = new SavedLearningWorldPath
            { Id = Guid.ParseExact("00000000-0000-0000-0000-000000000001", "D"), Name = "w1", Path = "p1" };
        var savedLearningWorld2 = new SavedLearningWorldPath
            { Id = Guid.ParseExact("00000000-0000-0000-0000-000000000002", "D"), Name = "w2", Path = "p2" };
        var presentationLogic = Substitute.For<IPresentationLogic>();
        presentationLogic.GetSavedLearningWorldPaths().Returns(new List<SavedLearningWorldPath>
            { savedLearningWorld1, savedLearningWorld2 });
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
        workspaceViewModel.LearningWorlds.Returns(new List<ILearningWorldViewModel> { learningWorld1 });
        var workspacePresenter = Substitute.For<IAuthoringToolWorkspacePresenter>();
        workspacePresenter.AuthoringToolWorkspaceVm.Returns(workspaceViewModel);
        var savedLearningWorld1 = new SavedLearningWorldPath { Id = learningWorld1.Id, Name = "w1", Path = "p1" };
        var savedLearningWorld2 = new SavedLearningWorldPath
            { Id = Guid.ParseExact("00000000-0000-0000-0000-000000000002", "D"), Name = "w2", Path = "p2" };
        var presentationLogic = Substitute.For<IPresentationLogic>();
        presentationLogic.GetSavedLearningWorldPaths().Returns(new List<SavedLearningWorldPath>
            { savedLearningWorld1, savedLearningWorld2 });
        var systemUnderTest =
            CreateProviderForTesting(presentationLogic: presentationLogic, workspacePresenter: workspacePresenter);

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
        var workspacePresenter = Substitute.For<IAuthoringToolWorkspacePresenter>();
        workspacePresenter.AuthoringToolWorkspaceVm.Returns(workspaceViewModel);
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        workspaceViewModel.LearningWorlds.Returns(new List<ILearningWorldViewModel> { learningWorld1 });
        var systemUnderTest = CreateProviderForTesting(workspacePresenter: workspacePresenter,
            selectedViewModelsProvider: selectedViewModelsProvider);

        var savedPaths = systemUnderTest.GetLoadedLearningWorlds();
        systemUnderTest.OpenLearningWorld(savedPaths.First());

        selectedViewModelsProvider.Received(1).SetLearningWorld(learningWorld1, null);
    }

    [Test]
    public void OpenLearningWorld_WorldIsSavedAndSaveFileExists_PresentationLogicIsCalled()
    {
        var workspaceVm = Substitute.For<IAuthoringToolWorkspaceViewModel>();
        var worldInWorkspace = new LearningWorldViewModel("n", "s", "a", "l", "d", "g");
        workspaceVm.LearningWorlds.Returns(new List<ILearningWorldViewModel> { worldInWorkspace });
        var workspacePresenter = Substitute.For<IAuthoringToolWorkspacePresenter>();
        workspacePresenter.AuthoringToolWorkspaceVm.Returns(workspaceVm);
        var savedLearningWorld1 = new SavedLearningWorldPath
            { Id = Guid.ParseExact("00000000-0000-0000-0000-000000000001", "D"), Name = "w1", Path = "p1" };
        var presentationLogic = Substitute.For<IPresentationLogic>();
        presentationLogic.GetSavedLearningWorldPaths()
            .Returns(new List<SavedLearningWorldPath> { savedLearningWorld1 });
        var fileSystem = Substitute.For<IFileSystem>();
        fileSystem.File.Exists(Arg.Any<string>()).Returns(true);
        var systemUnderTest = CreateProviderForTesting(presentationLogic: presentationLogic,
            workspacePresenter: workspacePresenter, fileSystem: fileSystem);

        var savedPaths = systemUnderTest.GetSavedLearningWorlds().ToList();
        systemUnderTest.OpenLearningWorld(savedPaths.First());

        presentationLogic.Received(1).LoadLearningWorldFromPath(workspaceVm, savedLearningWorld1.Path);
        presentationLogic.Received(1).UpdateIdOfSavedLearningWorldPath(savedPaths.First(), worldInWorkspace.Id);
    }

    [Test]
    public void OpenLearningWorld_WorldIsSavedButFileDoesNotExist_PresentationLogicIsNotCalled()
    {
        var savedLearningWorld1 = new SavedLearningWorldPath
            { Id = Guid.ParseExact("00000000-0000-0000-0000-000000000001", "D"), Name = "w1", Path = "p1" };
        var presentationLogic = Substitute.For<IPresentationLogic>();
        presentationLogic.GetSavedLearningWorldPaths()
            .Returns(new List<SavedLearningWorldPath> { savedLearningWorld1 });
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
        var savedLearningWorld1 = new SavedLearningWorldPath
            { Id = Guid.ParseExact("00000000-0000-0000-0000-000000000001", "D"), Name = "w1", Path = "p1" };
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var systemUnderTest = CreateProviderForTesting(presentationLogic: presentationLogic);

        Assert.Throws<ArgumentException>(() => systemUnderTest.OpenLearningWorld(savedLearningWorld1));
    }

    [Test]
    public void LoadSavedLearningWorld_CallsPresentationLogicGetWorldSavePath()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var systemUnderTest = CreateProviderForTesting(presentationLogic: presentationLogic);

        _ = systemUnderTest.LoadSavedLearningWorld();

        presentationLogic.Received(1).GetWorldSavePath();
    }

    [Test]
    public void LoadSavedLearningWorld_GetWorldSavePathThrowsOperationCanceledException_ReturnsFalse()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        presentationLogic.GetWorldSavePath().Throws(new OperationCanceledException());
        var systemUnderTest = CreateProviderForTesting(presentationLogic: presentationLogic);

        var result = systemUnderTest.LoadSavedLearningWorld();

        Assert.That(result.Result, Is.False);
    }

    [Test]
    public void LoadSavedLearningWorld_CallsPresentationLogicAddSavedLearningWorldPathByPathOnly()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        presentationLogic.GetWorldSavePath().Returns("path");
        var systemUnderTest = CreateProviderForTesting(presentationLogic: presentationLogic);

        _ = systemUnderTest.LoadSavedLearningWorld();

        presentationLogic.Received(1).AddSavedLearningWorldPathByPathOnly("path");
    }

    [Test]
    public void
        LoadSavedLearningWorld_LoadLearningWorldFromSaved_SavedPathExistsReturnsFalse_CallsPresentationLogicRemoveSavedLearningWorldPath()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        const string savePath = "path";
        presentationLogic.GetWorldSavePath().Returns(savePath);
        var savedLearningWorldPath = new SavedLearningWorldPath { Path = savePath };
        var fileSystem = Substitute.For<IFileSystem>();
        fileSystem.File.Exists(savePath).Returns(false);
        presentationLogic.AddSavedLearningWorldPathByPathOnly(savePath).Returns(savedLearningWorldPath);
        var systemUnderTest = CreateProviderForTesting(presentationLogic: presentationLogic, fileSystem: fileSystem);

        _ = systemUnderTest.LoadSavedLearningWorld();

        presentationLogic.Received(1).RemoveSavedLearningWorldPath(savedLearningWorldPath);
    }

    [Test]
    public void LoadSavedLearningWorld_LoadLearningWorldFromSaved_CallsPresentationLogicLoadLearningWorldFromPath()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        const string savePath = "path";
        presentationLogic.GetWorldSavePath().Returns(savePath);
        var savedLearningWorldPath = new SavedLearningWorldPath { Path = savePath };
        var fileSystem = Substitute.For<IFileSystem>();
        fileSystem.File.Exists(savePath).Returns(true);
        presentationLogic.AddSavedLearningWorldPathByPathOnly(savePath).Returns(savedLearningWorldPath);
        var workspacePresenter = Substitute.For<IAuthoringToolWorkspacePresenter>();
        var workspaceVm = Substitute.For<IAuthoringToolWorkspaceViewModel>();
        workspacePresenter.AuthoringToolWorkspaceVm.Returns(workspaceVm);
        var systemUnderTest = CreateProviderForTesting(presentationLogic: presentationLogic, fileSystem: fileSystem,
            workspacePresenter: workspacePresenter);

        _ = systemUnderTest.LoadSavedLearningWorld();

        presentationLogic.Received(1).LoadLearningWorldFromPath(workspaceVm, savePath);
    }

    [Test]
    public void
        LoadSavedLearningWorld_LoadLearningWorldFromSaved_CallsPresentationLogicUpdateIdOfSavedLearningWorldPath()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        const string savePath = "path";
        presentationLogic.GetWorldSavePath().Returns(savePath);
        var savedLearningWorldPath = new SavedLearningWorldPath { Path = savePath };
        var fileSystem = Substitute.For<IFileSystem>();
        fileSystem.File.Exists(savePath).Returns(true);
        presentationLogic.AddSavedLearningWorldPathByPathOnly(savePath).Returns(savedLearningWorldPath);
        var workspacePresenter = Substitute.For<IAuthoringToolWorkspacePresenter>();
        var workspaceVm = Substitute.For<IAuthoringToolWorkspaceViewModel>();
        var worldVm = Substitute.For<ILearningWorldViewModel>();
        var worldVmId = Guid.ParseExact("00000000-0000-0000-0000-000000000001", "D");
        worldVm.Id.Returns(worldVmId);
        workspaceVm.LearningWorlds.Returns(new List<ILearningWorldViewModel> { worldVm });
        workspacePresenter.AuthoringToolWorkspaceVm.Returns(workspaceVm);
        var systemUnderTest = CreateProviderForTesting(presentationLogic: presentationLogic, fileSystem: fileSystem,
            workspacePresenter: workspacePresenter);

        _ = systemUnderTest.LoadSavedLearningWorld();

        presentationLogic.Received(1).UpdateIdOfSavedLearningWorldPath(savedLearningWorldPath, worldVmId);
    }

    [Test]
    public void DeleteLearningWorld_CallsWorkspacePresenterDeleteLearningWorld()
    {
        var worldVmId = Guid.ParseExact("00000000-0000-0000-0000-000000000001", "D");
        var workspacePresenter = Substitute.For<IAuthoringToolWorkspacePresenter>();
        var workspaceVm = Substitute.For<IAuthoringToolWorkspaceViewModel>();
        var worldVm = Substitute.For<ILearningWorldViewModel>();
        worldVm.Id.Returns(worldVmId);
        workspaceVm.LearningWorlds.Returns(new List<ILearningWorldViewModel> { worldVm });
        workspacePresenter.AuthoringToolWorkspaceVm.Returns(workspaceVm);
        var savedLearningWorldPath = new SavedLearningWorldPath { Id = worldVmId };
        var systemUnderTest = CreateProviderForTesting(workspacePresenter: workspacePresenter);

        _ = systemUnderTest.DeleteLearningWorld(savedLearningWorldPath);

        workspacePresenter.Received(1).DeleteLearningWorld(worldVm);
    }

    private MyLearningWorldsProvider CreateProviderForTesting(IPresentationLogic? presentationLogic = null,
        IAuthoringToolWorkspacePresenter? workspacePresenter = null, IFileSystem? fileSystem = null,
        ISelectedViewModelsProvider? selectedViewModelsProvider = null,
        ILogger<MyLearningWorldsProvider>? logger = null, IErrorService? errorService = null)
    {
        presentationLogic ??= Substitute.For<IPresentationLogic>();
        workspacePresenter ??= Substitute.For<IAuthoringToolWorkspacePresenter>();
        fileSystem ??= Substitute.For<IFileSystem>();
        logger ??= Substitute.For<ILogger<MyLearningWorldsProvider>>();
        selectedViewModelsProvider ??= Substitute.For<ISelectedViewModelsProvider>();
        errorService ??= Substitute.For<IErrorService>();
        return new MyLearningWorldsProvider(presentationLogic, workspacePresenter, fileSystem, logger,
            selectedViewModelsProvider, errorService);
    }
}