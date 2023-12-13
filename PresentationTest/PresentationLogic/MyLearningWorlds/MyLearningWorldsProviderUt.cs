using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.MyLearningWorlds;
using Presentation.PresentationLogic.SelectedViewModels;
using IFileInfo = System.IO.Abstractions.IFileInfo;

namespace PresentationTest.PresentationLogic.MyLearningWorlds;

[TestFixture]
public class MyLearningWorldsProviderUt
{
    [Test]
    public void Constructor_InitializesAllProperties()
    {
        var logger = Substitute.For<ILogger<MyLearningWorldsProvider>>();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var workspaceViewModel = Substitute.For<IAuthoringToolWorkspaceViewModel>();
        var workspacePresenter = Substitute.For<IAuthoringToolWorkspacePresenter>();
        workspacePresenter.AuthoringToolWorkspaceVm.Returns(workspaceViewModel);
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();

        var systemUnderTest =
            CreateProviderForTesting(logger, presentationLogic, workspacePresenter, selectedViewModelsProvider);

        Assert.That(systemUnderTest.Logger, Is.EqualTo(logger));
        Assert.That(systemUnderTest.PresentationLogic, Is.EqualTo(presentationLogic));
        Assert.That(systemUnderTest.WorkspacePresenter, Is.EqualTo(workspacePresenter));
        Assert.That(systemUnderTest.SelectedViewModelsProvider, Is.EqualTo(selectedViewModelsProvider));
        Assert.That(systemUnderTest.WorkspaceVm, Is.EqualTo(workspaceViewModel));
    }

    [Test]
    public void ReloadLearningWorldsInWorkspace_LoadsAllWorlds_ExceptAlreadyLoadedOnes()
    {
        var workspacePresenter = Substitute.For<IAuthoringToolWorkspacePresenter>();
        var workspaceViewModel = Substitute.For<IAuthoringToolWorkspaceViewModel>();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        workspacePresenter.AuthoringToolWorkspaceVm.Returns(workspaceViewModel);
        var loadedWorlds = new[]
        {
            Substitute.For<ILearningWorldViewModel>(),
            Substitute.For<ILearningWorldViewModel>(),
            Substitute.For<ILearningWorldViewModel>(),
        };
        workspaceViewModel.LearningWorlds.Returns(loadedWorlds);
        var fileinfos = new[]
        {
            Substitute.For<IFileInfo>(),
            Substitute.For<IFileInfo>(),
            Substitute.For<IFileInfo>(),
            Substitute.For<IFileInfo>(),
        };
        fileinfos[0].FullName.Returns("path1");
        fileinfos[1].FullName.Returns("path2");
        fileinfos[2].FullName.Returns("path3");
        fileinfos[3].FullName.Returns("path4");
        presentationLogic.GetSavedLearningWorldPaths().Returns(fileinfos);
        presentationLogic.GetFileInfoForLearningWorld(loadedWorlds[0]).Returns(fileinfos[0]);
        presentationLogic.GetFileInfoForLearningWorld(loadedWorlds[1]).Returns(fileinfos[1]);
        presentationLogic.GetFileInfoForLearningWorld(loadedWorlds[2]).Returns(fileinfos[2]);

        var systemUnderTest = CreateProviderForTesting(Substitute.For<ILogger<MyLearningWorldsProvider>>(),
            presentationLogic, workspacePresenter, Substitute.For<ISelectedViewModelsProvider>());

        systemUnderTest.ReloadLearningWorldsInWorkspace();

        presentationLogic.Received(1).LoadLearningWorldFromPath(workspaceViewModel, fileinfos[3].FullName, false);
        presentationLogic.DidNotReceive().LoadLearningWorldFromPath(Arg.Any<IAuthoringToolWorkspaceViewModel>(),
            fileinfos[0].FullName, Arg.Any<bool>());
        presentationLogic.DidNotReceive().LoadLearningWorldFromPath(Arg.Any<IAuthoringToolWorkspaceViewModel>(),
            fileinfos[1].FullName, Arg.Any<bool>());
        presentationLogic.DidNotReceive().LoadLearningWorldFromPath(Arg.Any<IAuthoringToolWorkspaceViewModel>(),
            fileinfos[2].FullName, Arg.Any<bool>());
    }

    private MyLearningWorldsProvider CreateProviderForTesting(ILogger<MyLearningWorldsProvider> logger,
        IPresentationLogic presentationLogic, IAuthoringToolWorkspacePresenter workspacePresenter,
        ISelectedViewModelsProvider selectedViewModelsProvider)
    {
        return new MyLearningWorldsProvider(logger, presentationLogic, workspacePresenter, selectedViewModelsProvider);
    }
}